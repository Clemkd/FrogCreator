using System.Net.Sockets;

namespace FrogCreator.Api.Net;

public interface IClientListener
{
    void OnClientAccept(TcpClient client);
}
