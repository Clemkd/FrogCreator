using FrogCreator.Client.Game.Graphics.Screens.Transitions;

namespace FrogCreator.Client.Game.Graphics.Screens;

public class TransitionScreen : AbstractScreen
{
    private AbstractScreen? _current;
    private ITransition _transition;

    public TransitionScreen(IFrogGame game, ITransition transition1, AbstractScreen nextScreen, ITransition transition2)
        : base(game)
    {
        _current = ScreenManager.GetCurrentScreen();

        _transition = new SequenceTransition(
            transition1,
            new InlineTransition(
                () => { nextScreen.Load(); nextScreen.Update(0); _current = nextScreen; },
                (delta) => false),
            transition2,
            new InlineTransition(
                () => { ScreenManager.SetScreen(nextScreen); },
                (delta) => false));

        _transition.Initialize();
    }

    public override void Unload()
    {
        // Nothing
    }

    public override void Update(float delta)
    {
        _current?.Update(delta);
        _transition.Act(delta);
    }

    public override void Draw(GameBatch batch)
    {
        _current?.Draw(batch);
    }

    public override void Resize(int width, int height)
    {
    }

    public override void Load()
    {
    }
}

/// <summary>
/// Helper class to replace anonymous Transition implementations from Java
/// </summary>
internal class InlineTransition : ITransition
{
    private readonly Action _initAction;
    private readonly Func<float, bool> _actFunc;

    public InlineTransition(Action initAction, Func<float, bool> actFunc)
    {
        _initAction = initAction;
        _actFunc = actFunc;
    }

    public void Initialize() => _initAction();
    public bool Act(float delta) => _actFunc(delta);
}
