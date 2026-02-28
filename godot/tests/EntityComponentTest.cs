using NUnit.Framework;
using FrogCreator.Api.Entities.Characters;
using FrogCreator.Api.System;
using FrogCreator.Api.System.Components;
using FrogCreator.Api.System.Events;
using FrogCreator.Api.Utils;

namespace FrogCreator.Tests;

[TestFixture]
public class EntityComponentTest
{
    [Test]
    public void AddComponentToEntity()
    {
        Player player = new Player();
        HealthComponent healthComponent = new HealthComponent(100);

        player.AddComponent(healthComponent);

        Assert.That(player.ContainsComponent(HealthComponent.COMPONENT_KEY), Is.True);
    }

    [Test]
    public void GetComponentFromEntity()
    {
        Player player = new Player();
        HealthComponent healthComponent = new HealthComponent(100);
        player.AddComponent(healthComponent);

        AbstractComponent retrieved = player.GetComponent(HealthComponent.COMPONENT_KEY);

        Assert.That(retrieved, Is.Not.Null);
        Assert.That(retrieved, Is.InstanceOf<HealthComponent>());
    }

    [Test]
    public void AddDuplicateComponentThrows()
    {
        Player player = new Player();
        HealthComponent healthComponent = new HealthComponent(100);
        player.AddComponent(healthComponent);

        Assert.Throws<FrogException>(() => player.AddComponent(new HealthComponent(50)));
    }

    [Test]
    public void GetMissingComponentThrows()
    {
        Player player = new Player();

        Assert.Throws<FrogException>(() => player.GetComponent("NonExistentKey"));
    }

    [Test]
    public void HealthComponentClamps()
    {
        HealthComponent hc = new HealthComponent(100);
        Assert.That(hc.GetHealth(), Is.EqualTo(100));

        hc.DecreaseHealth(30);
        Assert.That(hc.GetHealth(), Is.EqualTo(70));

        hc.IncreaseHealth(50);
        Assert.That(hc.GetHealth(), Is.EqualTo(100));

        hc.SetHealth(-10);
        Assert.That(hc.GetHealth(), Is.EqualTo(0));

        hc.SetHealth(200);
        Assert.That(hc.GetHealth(), Is.EqualTo(100));
    }

    [Test]
    public void HealthSystemHandlesDamages()
    {
        Player player = new Player();
        HealthComponent healthComponent = new HealthComponent(100);
        player.AddComponent(healthComponent);

        Player enemy = new Player();

        HealthSystem healthSystem = new HealthSystem();
        GameSystem.Instance.AddSystem(healthSystem);

        GameSystem.Instance.PushEvent(new GameEvent(GameEventType.DAMAGES, player, enemy, 25));
        healthSystem.Update(0.016f);

        Assert.That(healthComponent.GetHealth(), Is.EqualTo(75));

        GameSystem.Instance.RemoveSystem(healthSystem);
    }
}
