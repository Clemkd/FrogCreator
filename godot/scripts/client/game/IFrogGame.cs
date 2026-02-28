using Godot;
using FrogCreator.Client.Game.Graphics;

namespace FrogCreator.Client.Game;

public interface IFrogGame
{
    Camera2D GetCamera();
    GameBatch GetBatch();
}
