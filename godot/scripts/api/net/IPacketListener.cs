namespace FrogCreator.Api.Net;

public interface IPacketListener
{
    void OnPacketReceived(Packet packet);
}
