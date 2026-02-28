using FrogCreator.Api.Entities;
using FrogCreator.Api.System.Objects;

namespace FrogCreator.Api.System.Components;

public abstract class AbstractComponent : IUpdatable
{
    private Entity? _parent;

    public Entity? GetParent() { return _parent; }
    public void SetParent(Entity parent) { _parent = parent; }
    public abstract string GetKey();
    public abstract void Update(float delta);
}
