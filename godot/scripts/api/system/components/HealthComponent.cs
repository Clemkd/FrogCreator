using FrogCreator.Api.System.Events;

namespace FrogCreator.Api.System.Components;

public class HealthComponent : AbstractComponent
{
    public static readonly string COMPONENT_KEY = "HealthComponentKey";
    private int _health;
    private int _maxHealth;

    public HealthComponent(int maxHealth)
    {
        _maxHealth = maxHealth;
        SetHealth(maxHealth);
    }

    public override void Update(float delta)
    {
        if (_health == 0)
            GameSystem.Instance.PushEvent(new GameEvent(GameEventType.DEATH, GetParent()!));
    }

    public int GetHealth() { return _health; }

    public void SetHealth(int health)
    {
        if (health < 0) _health = 0;
        else if (health > _maxHealth) _health = _maxHealth;
        else _health = health;
    }

    public void DecreaseHealth(int value) { SetHealth(GetHealth() - value); }
    public void IncreaseHealth(int value) { SetHealth(GetHealth() + value); }

    public override string GetKey() { return COMPONENT_KEY; }
}
