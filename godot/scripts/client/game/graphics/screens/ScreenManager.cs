using FrogCreator.Client.Application;
using FrogCreator.Client.Game.Graphics.Screens.Transitions;

namespace FrogCreator.Client.Game.Graphics.Screens;

public static class ScreenManager
{
    private static AbstractScreen? _currentScreen;

    public static AbstractScreen? GetCurrentScreen()
    {
        return _currentScreen;
    }

    public static void SetScreen(AbstractScreen screen)
    {
        AbstractScreen? oldScreen = _currentScreen;
        screen.Load();
        screen.Update(0);
        _currentScreen = screen;

        if (oldScreen != null && _currentScreen != screen)
            oldScreen.Unload();
    }

    public static void SetScreen(AbstractScreen screen, ITransition transition1, ITransition transition2)
    {
        if (GetCurrentScreen() == null)
            SetScreen(screen);

        TransitionScreen transitionScreen = new TransitionScreen(
            GameClient.GetInstance(), transition1, screen, transition2);
        transitionScreen.Load();
        _currentScreen = transitionScreen;
    }
}
