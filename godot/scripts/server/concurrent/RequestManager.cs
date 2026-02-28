using System.Collections.Concurrent;
using FrogCreator.Api.Net;
using FrogCreator.Api.Net.Socket;

namespace FrogCreator.Server.Concurrent;

public class RequestManager
{
    private BlockingCollection<FrogTask> _queue;
    private RequestExecutor _executor;

    public RequestManager(FrogServerSocket server)
    {
        _queue = new BlockingCollection<FrogTask>();
        _executor = new RequestExecutor(server, _queue);
    }

    public void Submit(Packet packet, IRequestListener? callback)
    {
        lock (_queue)
        {
            _queue.Add(new FrogTask(packet, callback));
        }
    }

    public void Start()
    {
        _executor.Start();
    }
}
