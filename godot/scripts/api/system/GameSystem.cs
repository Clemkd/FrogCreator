using FrogCreator.Api.Log;
using FrogCreator.Api.System.Events;
using FrogCreator.Api.Utils;

namespace FrogCreator.Api.System;

public class GameSystem
{
    // TODO : Revoir
    public static readonly GameSystem Instance = new GameSystem();

    private List<AbstractSystem> _systems;

    /// <summary>
    /// Liste des souscripteurs externes en écoute d'évènements entrants
    /// </summary>
    private List<IGameEventListener> _listeners;

    private GameSystem()
    {
        _systems = new List<AbstractSystem>();
        _listeners = new List<IGameEventListener>();
    }

    public List<AbstractSystem> GetSystems()
    {
        return _systems;
    }

    public void AddSystem(AbstractSystem system)
    {
        _systems.Add(system);
    }

    public void RemoveSystem(AbstractSystem system)
    {
        _systems.Remove(system);
    }

    public void AddEventListener(IGameEventListener listener)
    {
        _listeners.Add(listener);
    }

    public void RemoveEventListener(IGameEventListener listener)
    {
        _listeners.Remove(listener);
    }

    /// <summary>
    /// Ajoute un nouvel évènement dans le système
    /// </summary>
    /// <param name="gameEvent">Le nouvel évènement à prendre en charge</param>
    public void PushEvent(GameEvent gameEvent)
    {
        foreach (var system in _systems)
        {
            if (system.IsAcceptedEventType(gameEvent.GetEventType()))
            {
                system.PushEvent(gameEvent);
            }
        }

        /// Appel tous les souscripteurs en écoute d'évènements entrants
        foreach (var listener in _listeners)
        {
            try
            {
                listener.EventReceived(gameEvent);
            }
            catch (FrogException e)
            {
                Console.log.error(e);
            }
        }
    }
}
