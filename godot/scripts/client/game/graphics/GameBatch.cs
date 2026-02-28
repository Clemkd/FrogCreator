using Godot;

namespace FrogCreator.Client.Game.Graphics;

public class GameBatch
{
    private static float _alpha = 1f;

    public void Draw(IDrawable drawable)
    {
        drawable.Draw(this);
    }

    public void SetAlpha(float a)
    {
        _alpha = a;
    }

    public float GetAlpha()
    {
        return _alpha;
    }

    public void Begin()
    {
        // Godot handles draw calls via _Draw() override; placeholder for batch begin
    }

    public void End()
    {
        // Placeholder for batch end
    }
}
