using Godot;

namespace FrogCreator.Client.Game.Graphics.Screens.Effects;

/// <summary>
/// Initialise l'effet
/// </summary>
public interface IScreenEffect
{
    /// <summary>
    /// Initialise l'effet
    /// </summary>
    /// <param name="camera">La caméra de jeu</param>
    void Initialize(Camera2D camera);

    /// <summary>
    /// Met à jour l'effet
    /// </summary>
    /// <param name="delta">Le temps écoulé depuis la dernière mise à jour</param>
    void Update(float delta);

    /// <summary>
    /// Obtient l'état de l'effet
    /// </summary>
    /// <returns>True si l'effet est terminé, False dans le cas contraire</returns>
    bool IsFinished();
}
