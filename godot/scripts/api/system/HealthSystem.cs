using FrogCreator.Api.Entities;
using FrogCreator.Api.System.Components;
using FrogCreator.Api.System.Events;
using FrogCreator.Api.Utils;

namespace FrogCreator.Api.System;

public class HealthSystem : AbstractSystem
{
    public HealthSystem() : base(GameEventType.DAMAGES, GameEventType.HEAL)
    {
    }

    public override void Update(float delta)
    {
        // Something

        base.Update(delta);
    }

    public override void EventReceived(GameEvent gameEvent)
    {
        switch (gameEvent.GetEventType())
        {
            case GameEventType.DAMAGES:
                object[] parameters = gameEvent.GetParameters();
                Entity entity = (Entity)parameters[0];
                Entity enemy = (Entity)parameters[1];
                int damages = (int)parameters[2];

                HealthComponent component = (HealthComponent)entity.GetComponent(HealthComponent.COMPONENT_KEY);
                component.DecreaseHealth(damages);
                break;
            case GameEventType.HEAL:
                // TODO : Something
                break;
            default:
                throw new FrogException("Evenement non pris en charge atteint");
        }
    }
}
