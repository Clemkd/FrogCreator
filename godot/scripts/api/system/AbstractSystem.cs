using FrogCreator.Api.Log;
using FrogCreator.Api.System.Events;
using FrogCreator.Api.System.Objects;
using FrogCreator.Api.Utils;

namespace FrogCreator.Api.System;

public abstract class AbstractSystem : IUpdatable, IGameEventListener
{
    protected Stack<GameEvent> EventStack;
    protected HashSet<GameEventType> AcceptedEventsTypes;

    public AbstractSystem()
    {
        EventStack = new Stack<GameEvent>();
        AcceptedEventsTypes = new HashSet<GameEventType>();
    }

    protected AbstractSystem(params GameEventType[] eventsTypes) : this()
    {
        foreach (var type in eventsTypes)
            AcceptedEventsTypes.Add(type);
    }

    /// <summary>
    /// Ajoute un nouvel évènement au système
    /// </summary>
    /// <param name="gameEvent">Le nouvel évènement</param>
    public void PushEvent(GameEvent gameEvent)
    {
        EventStack.Push(gameEvent);
    }

    /// <summary>
    /// Permet de savoir si le système prend en charge le type d'évènement spécifié
    /// </summary>
    /// <param name="type">Le type d'évènement à tester</param>
    /// <returns>Vrai si le type d'évènement spécifié est pris en charge par le système, Faux dans le cas contraire</returns>
    public bool IsAcceptedEventType(GameEventType type)
    {
        return AcceptedEventsTypes.Contains(type);
    }

    public virtual void Update(float delta)
    {
        while (EventStack.Count > 0)
        {
            try
            {
                EventReceived(EventStack.Pop());
            }
            catch (FrogException e)
            {
                Console.log.error(e);
            }
        }
    }

    public abstract void EventReceived(GameEvent gameEvent);
}
