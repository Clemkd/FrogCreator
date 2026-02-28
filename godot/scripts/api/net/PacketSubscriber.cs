using FrogCreator.Api.Utils;

namespace FrogCreator.Api.Net;

/// <summary>
/// Aiguilleur de packets internet de jeu.
/// Classe permettant à des entités de souscrire à des notification.
/// Ces notification sont réalisées pour chaque reception de packets de
/// jeu et sont distribuées spécifiquement et de façon optimisé au entitées
/// ayant demandé de les recevoirs.
/// </summary>
public class PacketSubscriber
{
    private readonly Dictionary<PacketType, List<IPacketListener>> _listeners;
    private readonly object _lock = new object();

    public PacketSubscriber()
    {
        _listeners = new Dictionary<PacketType, List<IPacketListener>>();
    }

    public void Subscribe(PacketType type, IPacketListener packetHandler)
    {
        lock (_lock)
        {
            if (!_listeners.ContainsKey(type))
                _listeners[type] = new List<IPacketListener>();

            _listeners[type].Add(packetHandler);
        }
    }

    public void Unsubscribe(PacketType type, IPacketListener packetHandler)
    {
        lock (_lock)
        {
            if (_listeners.ContainsKey(type))
            {
                var list = _listeners[type];
                list.Remove(packetHandler);
                if (list.Count == 0)
                    _listeners.Remove(type);
            }
        }
    }

    public void PushPacket(Packet packet)
    {
        lock (_lock)
        {
            if (_listeners.ContainsKey(packet.GetPacketType()))
            {
                var list = _listeners[packet.GetPacketType()];
                int size = list.Count;
                for (int i = 0; i < size; i++)
                {
                    list[i].OnPacketReceived(packet);
                }
            }
        }
    }
}
