using Godot;
using FrogCreator.Client.Game;
using FrogCreator.Client.Game.Graphics;
using FrogCreator.Client.Game.Graphics.Screens;
using FrogCreator.Client.Game.Graphics.Screens.Effects;
using FrogCreator.Client.Game.Graphics.Screens.Transitions;

namespace FrogCreator.Client.Application;

public partial class GameClient : Node2D, IFrogGame
{
    public static readonly Api.Math.Vector2<int> VIEWPORT = new Api.Math.Vector2<int>(1280, 720);
    private static GameClient? _instance;

    private GameBatch _batch = null!;
    private Camera2D _camera = null!;

    public static GameClient GetInstance()
    {
        return _instance!;
    }

    public override void _Ready()
    {
        _instance = this;
        _batch = new GameBatch();
        _camera = new Camera2D();
        AddChild(_camera);
        _camera.MakeCurrent();

        SplashScreen sp = new SplashScreen(this);
        sp.StartEffect(new ShakeEffect(10, 2000));
        ScreenManager.SetScreen(sp, new FadeOutTransition(3000), new FadeInTransition(3000));
    }

    public override void _Process(double delta)
    {
        float dt = (float)delta;
        ScreenManager.GetCurrentScreen()?.Update(dt);
    }

    public Camera2D GetCamera()
    {
        return _camera;
    }

    public GameBatch GetBatch()
    {
        return _batch;
    }
}
