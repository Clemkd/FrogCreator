namespace FrogCreator.Api.Plugin;

/// <summary>
/// Obtient le nom du plugin
/// </summary>
public interface IPlugin
{
    /// <summary>
    /// Obtient le nom du plugin
    /// </summary>
    string GetName();

    /// <summary>
    /// Obtient le nom de l'auteur du plugin
    /// </summary>
    string GetAuthor();

    /// <summary>
    /// Obtient la version du plugin
    /// </summary>
    string GetVersion();

    /// <summary>
    /// Etape de chargement du plugin
    /// </summary>
    void Load();

    /// <summary>
    /// Etape d'execution du plugin.
    /// Lancé après le chargement.
    /// </summary>
    void Execute();

    /// <summary>
    /// Etape de libération des ressources utilisées par le plugin.
    /// Appelé lors de la fin de vie du plugin, après l'avoir préalablement chargé et exécuté.
    /// </summary>
    void Unload();
}
