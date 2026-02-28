using System.Reflection;
using FrogCreator.Api.Plugin;

namespace FrogCreator.Server.Plugin;

// TODO: Javadoc et ajout d'exceptions
public class PluginLoader
{
    private static readonly string DLL_EXTENSION = ".dll";

    private string _pluginFolder;

    /// <summary>
    /// Constructeur du chargeur de plugins
    /// </summary>
    /// <param name="pluginFolder">Chemin vers le dossier de plugins à charger</param>
    public PluginLoader(string pluginFolder)
    {
        _pluginFolder = pluginFolder;
    }

    /// <summary>
    /// Obtient la liste des fichiers ".dll" du dossier de plugins
    /// </summary>
    /// <returns>La liste des fichiers ".dll" du dossier de plugins</returns>
    private string[] GetDllFiles()
    {
        if (!Directory.Exists(_pluginFolder))
            return Array.Empty<string>();

        return Directory.GetFiles(_pluginFolder, "*" + DLL_EXTENSION);
    }

    /// <summary>
    /// Obtient la liste des plugins valides contenus dans le dossier de plugins
    /// </summary>
    /// <returns>La liste des plugins valides</returns>
    public List<IPlugin> GetPlugins()
    {
        List<IPlugin> plugins = new List<IPlugin>();

        foreach (string dllFile in GetDllFiles())
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(dllFile);

                foreach (Type type in assembly.GetTypes())
                {
                    if (type.GetCustomAttribute<FrogPluginAttribute>() != null &&
                        typeof(IPlugin).IsAssignableFrom(type))
                    {
                        try
                        {
                            IPlugin? plugin = (IPlugin?)Activator.CreateInstance(type);
                            if (plugin != null)
                                plugins.Add(plugin);
                        }
                        catch (InvalidCastException)
                        {
                            // Cas de cast d'une classe avec l'attribut FrogPlugin mais qui n'implémente pas IPlugin
                            Console.Error.WriteLine($"Une classe annotée ne correspond pas au type de plugin attendu. ({type.FullName})");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }

        return plugins;
    }
}
