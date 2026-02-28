using Godot;

namespace FrogCreator.Client.Game.Graphics.Screens;

public class SplashScreen : AbstractScreen
{
    private Texture2D? _logoTexture;
    private Sprite2D? _logo;

    public SplashScreen(IFrogGame game) : base(game)
    {
    }

    public override void Draw(GameBatch batch)
    {
        // In Godot, drawing is handled by the scene tree
    }

    public override void Load()
    {
        _logoTexture = GD.Load<Texture2D>("res://assets/logo.png");
        if (_logoTexture != null)
        {
            _logo = new Sprite2D();
            _logo.Texture = _logoTexture;
        }
    }

    public override void Unload()
    {
    }

    public override void Update(float delta)
    {
        base.Update(delta);
    }

    public override void Resize(int width, int height)
    {
    }
}
