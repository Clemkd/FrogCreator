using FrogCreator.Api.Environment.Map;
using FrogCreator.Api.Math;

namespace FrogCreator.Api.Ai.Pathfinder;

public abstract class Pathfinder
{
    public abstract MovementPath? GetPath(GameMap map, Vector2<int> absoluteBeginLocation, Vector2<int> absoluteEndLocation);
}
