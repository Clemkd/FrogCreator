namespace FrogCreator.Client.Game.Graphics.Screens.Transitions;

public sealed class SequenceTransition : ITransition
{
    private List<ITransition> _transitionList;
    private int _currentTransitionIndex;

    public SequenceTransition(params ITransition[] transitions)
    {
        _transitionList = new List<ITransition>();
        _currentTransitionIndex = 0;

        foreach (var transition in transitions)
            _transitionList.Add(transition);
    }

    public void Initialize()
    {
        _currentTransitionIndex = 0;
        _transitionList[_currentTransitionIndex].Initialize();
    }

    public bool Act(float delta)
    {
        if (_currentTransitionIndex >= _transitionList.Count)
            return false;

        if (!_transitionList[_currentTransitionIndex].Act(delta))
        {
            _currentTransitionIndex++;
            if (_currentTransitionIndex < _transitionList.Count)
                _transitionList[_currentTransitionIndex].Initialize();
        }

        return _currentTransitionIndex < _transitionList.Count;
    }
}
