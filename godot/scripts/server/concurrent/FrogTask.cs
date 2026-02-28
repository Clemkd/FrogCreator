using FrogCreator.Api.Net;

namespace FrogCreator.Server.Concurrent;

public class FrogTask
{
    private Packet _packet;
    private IRequestListener? _callback;

    public FrogTask(Packet packet, IRequestListener? callback)
    {
        _packet = packet;
        _callback = callback;
    }

    public Packet GetPacket()
    {
        return _packet;
    }

    public IRequestListener? GetCallback()
    {
        return _callback;
    }
}
