using FrogCreator.Client.Application;

namespace FrogCreator.Client.Game.Graphics.Screens.Transitions;

public class FadeInTransition : ITransition
{
    private float _alpha;
    private float _duration;
    private float _elapsed;

    public FadeInTransition(float milliseconds)
    {
        _duration = milliseconds / 1000f;
    }

    public void Initialize()
    {
        _alpha = 1f;
        _elapsed = 0f;
    }

    public bool Act(float delta)
    {
        _elapsed += delta;
        _alpha = 1 - (_elapsed / _duration);

        if (_alpha >= 0f)
            GameClient.GetInstance().GetBatch().SetAlpha(_alpha);

        return _elapsed < _duration;
    }
}
