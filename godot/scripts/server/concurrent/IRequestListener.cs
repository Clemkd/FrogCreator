using FrogCreator.Api.Net;

namespace FrogCreator.Server.Concurrent;

public interface IRequestListener
{
    void OnRequestExecutionFinished(Packet result);
}
