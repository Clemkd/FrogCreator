using Godot;
using FrogCreator.Api.System.Objects;
using FrogCreator.Client.Application;
using FrogCreator.Client.Game.Graphics.Screens.Effects;

namespace FrogCreator.Client.Game.Graphics.Screens;

public abstract class AbstractScreen : IDrawable, IResource, IUpdatable, IResizable
{
    protected IFrogGame Game;
    protected bool IsLoaded;
    protected List<IScreenEffect> Effects;

    public AbstractScreen(IFrogGame game)
    {
        Game = game;
        Effects = new List<IScreenEffect>();
    }

    public virtual void Update(float delta)
    {
        UpdateEffects(delta);
    }

    public void StartEffect(IScreenEffect effect)
    {
        effect.Initialize(GameClient.GetInstance().GetCamera());
        Effects.Add(effect);
    }

    private void UpdateEffects(float delta)
    {
        for (int i = 0; i < Effects.Count; i++)
        {
            if (Effects[i].IsFinished())
            {
                Effects.RemoveAt(i);
                i--;
            }
            else
            {
                Effects[i].Update(delta);
            }
        }
    }

    public abstract void Draw(GameBatch batch);
    public abstract void Load();
    public abstract void Unload();
    public abstract void Resize(int width, int height);
}
