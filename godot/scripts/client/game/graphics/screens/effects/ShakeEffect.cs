using Godot;

namespace FrogCreator.Client.Game.Graphics.Screens.Effects;

public class ShakeEffect : IScreenEffect
{
    private float _elapsed;
    private float _duration;
    private float _intensity;
    private Random _random;
    private Camera2D? _camera;
    private Godot.Vector2 _origin;

    public ShakeEffect(float intensity, float duration)
    {
        _elapsed = 0;
        _duration = duration / 1000f;
        _intensity = intensity;
        _random = new Random();
    }

    public void Initialize(Camera2D camera)
    {
        _camera = camera;
        _origin = camera.Offset;
    }

    public void Update(float delta)
    {
        if (_camera == null) return;

        _camera.Offset = _origin;
        if (_elapsed < _duration)
        {
            float shakePower = _intensity * ((_duration - _elapsed) / _duration);
            float x = ((float)_random.NextDouble() - 0.5f) * 2 * shakePower;
            float y = ((float)_random.NextDouble() - 0.5f) * 2 * shakePower;
            _camera.Offset = new Godot.Vector2(_origin.X - x, _origin.Y - y);

            _elapsed += delta;
        }
    }

    public bool IsFinished()
    {
        return _elapsed >= _duration;
    }
}
