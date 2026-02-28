using System.Globalization;
using FrogCreator.Api.Utils;

namespace FrogCreator.Api.Localization;

public class I18nProperties
{
    public static readonly string DEFAULT_RESOURCES_PATH = Path.Combine(Directory.GetCurrentDirectory(), "localization");
    public static readonly string DEFAULT_BUNDLE_NAME = "I18nResources";

    private List<string> _resourcesPaths;
    private Dictionary<string, string> _bundle;
    private CultureInfo _locale;

    public I18nProperties()
    {
        _resourcesPaths = new List<string>();
        _bundle = new Dictionary<string, string>();
        SetDefault(new CultureInfo("fr-FR"));
        AddResourcesPath(DEFAULT_RESOURCES_PATH);
        Reload();
    }

    /// <summary>
    /// Définit la langue du gestionnaire de ressources.
    /// Un rechargement de l'objet est nécessaire afin de prendre en compte la modification.
    /// </summary>
    /// <param name="locale">La langue que prendra en charge le gestionnaire de ressources</param>
    public void SetDefault(CultureInfo locale)
    {
        _locale = locale;
    }

    /// <summary>
    /// Ajoute un chemin vers un nouveau dossier de ressources.
    /// Un rechargement de l'objet est nécessaire afin de prendre en considération ce nouveau repertoire.
    /// </summary>
    /// <param name="folderPath">Le chemin vers le dossier de ressources à ajouter</param>
    /// <exception cref="FrogException">Exception jetée lorsque le chemin donné est incorrect</exception>
    public void AddResourcesPath(string folderPath)
    {
        if (!Directory.Exists(folderPath))
            throw new FrogException($"Le chemin spécifié n'existe pas : {folderPath}");
        _resourcesPaths.Add(folderPath);
    }

    /// <summary>
    /// Recharge le gestionnaire de ressources en prenant en compte les chemins de dossiers ressources et la langue
    /// </summary>
    public void Reload()
    {
        _bundle.Clear();

        string localeSuffix = _locale.Name.Replace("-", "_");
        string fileName = $"{DEFAULT_BUNDLE_NAME}_{localeSuffix}.properties";

        foreach (var path in _resourcesPaths)
        {
            string filePath = Path.Combine(path, fileName);
            if (File.Exists(filePath))
            {
                foreach (string line in File.ReadAllLines(filePath))
                {
                    string trimmed = line.Trim();
                    if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith('#'))
                        continue;

                    int eqIndex = trimmed.IndexOf('=');
                    if (eqIndex > 0)
                    {
                        string key = trimmed.Substring(0, eqIndex).Trim();
                        string value = trimmed.Substring(eqIndex + 1).Trim();
                        _bundle[key] = value;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Obtient la langue courante du gestionnaire de ressources
    /// </summary>
    /// <returns>La langue courante</returns>
    public CultureInfo GetCurrentLocale()
    {
        return _locale;
    }

    /// <summary>
    /// Obtient la valeur de la ressource définit par la clé donnée, pour la langue actuelle
    /// </summary>
    /// <param name="key">La clé de la ressource à obtenir</param>
    /// <returns>La valeur de la ressource recherchée, pour la langue actuelle</returns>
    /// <exception cref="FrogException">Exception jetée lorsque la clé est inexistante dans le gestionnaire de ressources</exception>
    public string GetString(string key)
    {
        if (!_bundle.ContainsKey(key))
            throw new FrogException($"The specified key \"{key}\" does not exists in the collection");

        return _bundle[key];
    }
}
