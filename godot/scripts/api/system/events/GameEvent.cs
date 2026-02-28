namespace FrogCreator.Api.System.Events;

public class GameEvent
{
    private GameEventType _eventType;
    private object[] _parameters;

    public GameEvent(GameEventType eventType, params object[] parameters)
    {
        _eventType = eventType;
        _parameters = parameters;
    }

    public GameEventType GetEventType() { return _eventType; }
    public object[] GetParameters() { return _parameters; }
}
