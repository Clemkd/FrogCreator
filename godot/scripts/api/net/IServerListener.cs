using FrogCreator.Api.Net.Socket;

namespace FrogCreator.Api.Net;

public interface IServerListener
{
    void OnStartUp(FrogServerSocket server);
}
