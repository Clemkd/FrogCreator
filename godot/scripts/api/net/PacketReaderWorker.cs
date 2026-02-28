using FrogCreator.Api.Net.Socket;
using FrogCreator.Api.Utils;

namespace FrogCreator.Api.Net;

public class PacketReaderWorker
{
    private readonly List<IPacketListener> _listeners;
    private readonly FrogClientSocket _network;
    private readonly Thread _thread;

    public PacketReaderWorker(FrogClientSocket network)
    {
        _listeners = new List<IPacketListener>();
        _network = network;
        _thread = new Thread(Run) { IsBackground = true };
    }

    public void Start()
    {
        _thread.Start();
    }

    private void Run()
    {
        while (_network.IsRunning() && _network.IsConnected())
        {
            try
            {
                System.Console.WriteLine("Waiting for packets...");
                string? line = _network.GetReader()?.ReadLine();
                System.Console.WriteLine("Packet received : " + line);
                if (line != null)
                {
                    Packet responsePacket = Packet.GetPacket(line);
                    RaiseReceivedPacketEvent(responsePacket);
                }
            }
            catch (Exception e)
            {
                System.Console.Error.WriteLine(e);
            }
        }
    }

    private void RaiseReceivedPacketEvent(Packet packet)
    {
        IPacketListener[] snapshot;
        lock (_listeners)
        {
            snapshot = _listeners.ToArray();
        }
        for (int i = 0; i < snapshot.Length; i++)
        {
            snapshot[i].OnPacketReceived(packet);
        }
    }

    /// <summary>
    /// Ajoute un nouveau souscripteur.
    /// Le nouveau souscripteur sera notifié à chaque nouveau packet.
    /// </summary>
    /// <param name="listener">Le nouveau souscripteur à l'écoute des évènements</param>
    public void AddListener(IPacketListener listener)
    {
        lock (_listeners)
        {
            _listeners.Add(listener);
        }
    }

    /// <summary>
    /// Supprime un souscripteur.
    /// Le souscripteur ne recevra plus de notification d'évènements.
    /// </summary>
    /// <param name="listener">Le souscripteur à l'écoute des évènements</param>
    public void RemoveListener(IPacketListener listener)
    {
        lock (_listeners)
        {
            _listeners.Remove(listener);
        }
    }
}
