using System.Net.Sockets;
using System.Text.Json;
using FrogCreator.Api.Net;
using FrogCreator.Api.Net.Socket;
using FrogCreator.Api.Utils;

namespace FrogCreator.Server.Concurrent;

public class ClientWorker : IRequestListener
{
    private FrogServerSocket _server;
    private TcpClient _socket;
    private RequestManager _manager;
    private StreamReader _in;
    private StreamWriter _out;

    public ClientWorker(FrogServerSocket server, TcpClient socket, RequestManager manager)
    {
        _server = server;
        _socket = socket;
        _manager = manager;

        // Création des objets de communication
        NetworkStream stream = socket.GetStream();
        _out = new StreamWriter(stream) { AutoFlush = true };
        _in = new StreamReader(stream);
    }

    private bool IsProtocolVersionValid()
    {
        // Coupe la communication si la version du protocol de communication est invalide
        string? line = _in.ReadLine();
        if (line == null)
            throw new FrogException("Aucune donnée reçue");

        Packet firstPacket = Packet.GetPacket(line);

        if (firstPacket.GetPacketType() != PacketType.PROTOCOL_VERSION)
            throw new FrogException("Premier packet reçu incorrect");

        string packetJSON = firstPacket.GetSerializedObject();
        using JsonDocument doc = JsonDocument.Parse(packetJSON);
        string version = doc.RootElement.GetProperty("version").GetString() ?? string.Empty;

        bool result = version.Equals(FrogServerSocket.PROTOCOL_VERSION);

        var obj = new Dictionary<string, object> { { "result", result } };
        Packet packetResult = new Packet(PacketType.PROTOCOL_VERSION_RESULT, JsonSerializer.Serialize(obj));
        _out.WriteLine(packetResult.ToJSON());

        return result;
    }

    public void Run()
    {
        string reason = "";

        try
        {
            if (!IsProtocolVersionValid())
                throw new FrogException("Mauvaise version du protocol de communication");

            while (_socket.Connected && _server.IsRunning())
            {
                try
                {
                    string? str = _in.ReadLine();
                    if (str == null) break;
                    _manager.Submit(Packet.GetPacket(str), this);
                }
                catch (FrogException e)
                {
                    Console.Error.WriteLine(e);
                }
            }
        }
        catch (IOException)
        {
            reason = string.Format("(raison : {0})", "Fermeture de la communication par le client");
        }
        catch (FrogException e)
        {
            reason = string.Format("(raison : {0})", e.Message);
        }
        finally
        {
            try { _socket.Close(); }
            catch (Exception ex) { Console.Error.WriteLine(ex); }

            // Socket closed
            Console.WriteLine(string.Format("Client {0} déconnecté {1}", _socket.Client.RemoteEndPoint, reason));
        }
    }

    public void OnRequestExecutionFinished(Packet result)
    {
        // Envoi de la réponse au client
        _out.WriteLine(result.ToJSON());
    }
}
