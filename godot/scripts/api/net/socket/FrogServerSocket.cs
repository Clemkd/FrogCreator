using System.Net;
using System.Net.Sockets;

namespace FrogCreator.Api.Net.Socket;

public class FrogServerSocket
{
    public static readonly string PROTOCOL_VERSION = "v0.0.0.1";

    private bool _isRunning;
    private IClientListener? _clientListener;
    private IServerListener? _serverListener;
    private readonly object _lock = new object();

    public FrogServerSocket()
    {
        _isRunning = false;
    }

    public void Start(int port)
    {
        TcpListener? server = null;
        try
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();
            _isRunning = true;
            RaiseServerStartUpEvent();

            while (IsRunning())
            {
                // Attente d'une connexion entrante
                TcpClient clientSocket = server.AcceptTcpClient();
                RaiseClientConnectedEvent(clientSocket);
            }
        }
        catch (SocketException ex) when (ex.SocketErrorCode == SocketError.AddressAlreadyInUse)
        {
            System.Console.Error.WriteLine("Impossible de lancer le serveur, le port est actuellement utilisé");
        }
        catch (Exception e)
        {
            System.Console.Error.WriteLine(e);
        }
        finally
        {
            _isRunning = false;
            server?.Stop();
        }
    }

    /// <summary>
    /// Obtient l'état du serveur
    /// </summary>
    /// <returns>True si le serveur est actif, False dans le cas contraire</returns>
    public bool IsRunning()
    {
        lock (_lock)
        {
            return _isRunning;
        }
    }

    /// <summary>
    /// Met à jour l'entité en écoute de nouveaux clients.
    /// Le souscripteur recevra des notifications à chaque nouvelle connexion cliente.
    /// </summary>
    /// <param name="listener">Le nouveau souscripteur</param>
    public void SetClientListener(IClientListener listener)
    {
        _clientListener = listener;
    }

    /// <summary>
    /// Met à jour l'entité en écoute du cycle de vie du serveur.
    /// Le souscripteur recevra des notifications à chaque évènement du cycle du serveur.
    /// </summary>
    /// <param name="listener">Le nouveau souscripteur</param>
    public void SetServerListener(IServerListener listener)
    {
        _serverListener = listener;
    }

    private void RaiseServerStartUpEvent()
    {
        _serverListener?.OnStartUp(this);
    }

    private void RaiseClientConnectedEvent(TcpClient client)
    {
        _clientListener?.OnClientAccept(client);
    }
}
