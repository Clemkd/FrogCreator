using FrogCreator.Api.Utils;

namespace FrogCreator.Api.System.Events;

public interface IGameEventListener
{
    void EventReceived(GameEvent gameEvent);
}
