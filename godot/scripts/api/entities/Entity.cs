using FrogCreator.Api.System.Components;
using FrogCreator.Api.System.Objects;
using FrogCreator.Api.Utils;

namespace FrogCreator.Api.Entities;

public abstract class Entity : IUpdatable
{
    /// <summary>
    /// Composants système de l'entité
    /// </summary>
    private Dictionary<string, AbstractComponent> _components;

    public Entity()
    {
        _components = new Dictionary<string, AbstractComponent>();
    }

    public virtual void Update(float delta)
    {
        foreach (var key in _components.Keys)
        {
            _components[key].Update(delta);
        }
    }

    /// <summary>
    /// Obtient un composant associé à l'entité
    /// </summary>
    /// <param name="componentKey">La clé du composant recherché</param>
    /// <returns>Le composant associé à l'entité</returns>
    /// <exception cref="FrogException">Exception jetée si le composant n'est pas associé à l'entité</exception>
    public AbstractComponent GetComponent(string componentKey)
    {
        if (_components.ContainsKey(componentKey))
            return _components[componentKey];
        throw new FrogException("L'entité ne dispose pas du composant recherché");
    }

    /// <summary>
    /// Associe un nouveau composant à l'entité
    /// </summary>
    /// <param name="component">Le composant à associer</param>
    /// <exception cref="FrogException">Exception jetée si un composant du même type est déjà associé à l'entité</exception>
    public void AddComponent(AbstractComponent component)
    {
        if (_components.ContainsKey(component.GetKey()))
            throw new FrogException("L'entité dispose déjà du composant");
        _components[component.GetKey()] = component;
    }

    /// <summary>
    /// Retourne vrai si l'entité dispose du composant avec la clé donnée, Faux dans le cas contraire
    /// </summary>
    /// <param name="componentKey">La clé du composant à tester</param>
    /// <returns>Vrai si le composant est associé à l'entité, Faux dans le cas contraire</returns>
    public bool ContainsComponent(string componentKey)
    {
        return _components.ContainsKey(componentKey);
    }
}
