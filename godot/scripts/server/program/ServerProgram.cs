using System.Net.Sockets;
using FrogCreator.Api.Net;
using FrogCreator.Api.Net.Socket;
using FrogCreator.Api.Plugin;

namespace FrogCreator.Server.Program;

public class ServerProgram
{
    private static readonly int MAX_THREAD = 10;
    private static readonly int PORT = 5000;

    private static readonly string PLUGINS_FOLDER = "plugins";

    public static void Main(string[] args)
    {
        var pluginLoader = new FrogCreator.Server.Plugin.PluginLoader(PLUGINS_FOLDER);
        List<IPlugin> plugins = pluginLoader.GetPlugins();

        LoadPlugins(plugins);
        StartServerLoop();

        UnloadPlugins(plugins);
    }

    private static void LoadPlugins(List<IPlugin> plugins)
    {
        Console.WriteLine("Chargement des plugins...");

        for (int i = plugins.Count - 1; i >= 0; i--)
        {
            try
            {
                IPlugin plugin = plugins[i];
                Console.WriteLine($"[Plugin : {plugin.GetName()}] Auteur  : {plugin.GetAuthor()}; Version : {plugin.GetVersion()}");
                plugin.Load();
            }
            catch (Exception e)
            {
                // Suppression du mauvais plugin de la liste
                plugins.RemoveAt(i);
                Console.Error.WriteLine(e);
            }
        }
    }

    private static void UnloadPlugins(List<IPlugin> plugins)
    {
        // Décharge les plugins
        foreach (var plugin in plugins)
            plugin.Unload();
    }

    private static void StartServerLoop()
    {
        FrogServerSocket server = new FrogServerSocket();

        Console.WriteLine("Création du gestionnaire de requêtes...");
        // Création du gestionnaire de requêtes
        var requestManager = new FrogCreator.Server.Concurrent.RequestManager(server);

        server.SetServerListener(new ServerStartupListener(requestManager));
        server.SetClientListener(new ClientAcceptListener(server, requestManager));

        // Serveur d'écoute
        Console.WriteLine("Lancement du serveur d'écoute sur le port " + PORT);
        server.Start(PORT);
    }

    private class ServerStartupListener : IServerListener
    {
        private readonly FrogCreator.Server.Concurrent.RequestManager _requestManager;

        public ServerStartupListener(FrogCreator.Server.Concurrent.RequestManager requestManager)
        {
            _requestManager = requestManager;
        }

        public void OnStartUp(FrogServerSocket server)
        {
            // Lancement du request Manager au démarrage du serveur
            _requestManager.Start();
        }
    }

    private class ClientAcceptListener : IClientListener
    {
        private readonly FrogServerSocket _server;
        private readonly FrogCreator.Server.Concurrent.RequestManager _requestManager;

        public ClientAcceptListener(FrogServerSocket server, FrogCreator.Server.Concurrent.RequestManager requestManager)
        {
            _server = server;
            _requestManager = requestManager;
        }

        public void OnClientAccept(TcpClient client)
        {
            Console.WriteLine("Nouveau client " + client.Client.RemoteEndPoint);

            // Ajout du client stub dans un nouveau thread
            var worker = new FrogCreator.Server.Concurrent.ClientWorker(_server, client, _requestManager);
            Thread thread = new Thread(worker.Run) { IsBackground = true };
            thread.Start();
        }
    }
}
