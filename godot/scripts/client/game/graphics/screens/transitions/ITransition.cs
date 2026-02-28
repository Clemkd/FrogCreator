namespace FrogCreator.Client.Game.Graphics.Screens.Transitions;

public interface ITransition
{
    void Initialize();
    bool Act(float delta);
}
