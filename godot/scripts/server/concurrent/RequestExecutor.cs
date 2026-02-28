using System.Collections.Concurrent;
using System.Text.Json;
using FrogCreator.Api.Net;
using FrogCreator.Api.Net.Socket;

namespace FrogCreator.Server.Concurrent;

public class RequestExecutor
{
    private FrogServerSocket _server;
    private BlockingCollection<FrogTask> _queue;
    private Thread? _thread;

    public RequestExecutor(FrogServerSocket server, BlockingCollection<FrogTask> queue)
    {
        _server = server;
        _queue = queue;
    }

    public void Start()
    {
        _thread = new Thread(Run) { IsBackground = true };
        _thread.Start();
    }

    private void Run()
    {
        while (_server.IsRunning())
        {
            try
            {
                // Prend une tâche à réaliser
                FrogTask task = _queue.Take();
                // Extrait la requête associée
                Packet packet = task.GetPacket();

                var obj = new Dictionary<string, object>();
                PacketType resultType = PacketType.NONE;

                // Recherche et execution synchronisée de la requete
                switch (packet.GetPacketType())
                {
                    case PacketType.CONNECT:
                        Console.WriteLine("CONNECT : Do something with DB : " + packet.GetSerializedObject());
                        // if account OK
                        obj["result"] = true;
                        obj["token"] = "GENERATEDTOKEN1234567890";
                        // else
                        // obj["result"] = false;
                        resultType = PacketType.CONNECT_RESULT;
                        break;
                    default:
                        Console.WriteLine("Default request received...");
                        break;
                }

                Packet packetResult = new Packet(resultType, JsonSerializer.Serialize(obj)); // Resultat de l'execution

                // Extrait le callback de la requête
                IRequestListener? callback = task.GetCallback();
                callback?.OnRequestExecutionFinished(packetResult);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }
    }
}
