using FrogCreator.Api.System.Objects;
using FrogCreator.Api.Utils;

namespace FrogCreator.Api.Environment.Map;

public class GameMapLayer : IResource
{
    private int?[,] _tiles;
    private int _width, _height;

    public GameMapLayer(int width, int height)
    {
        _tiles = new int?[height, width];
        _width = width;
        _height = height;
    }

    public void Load()
    {
        /*for(int x = 0; x < _width; x++)
        {
            for(int y = 0; y < _height; y++)
            {
                // Load Tile (x, y)
            }
        }*/
    }

    public void Unload()
    {
    }

    /// <summary>
    /// Obtient la valeur de la tuile à la position donnée
    /// </summary>
    /// <param name="relativeX">La valeur horizontale relative au chunk de la position</param>
    /// <param name="relativeY">La valeur verticale relative au chunk de la position</param>
    /// <returns>La valeur de la tuile à la position donnée, null si inexistant</returns>
    public int? GetTile(int relativeX, int relativeY)
    {
        if (relativeX >= 0 && relativeX < _width && relativeY >= 0 && relativeY < _height)
            return _tiles[relativeY, relativeX];
        return null;
    }

    /// <summary>
    /// Met à jour la valeur de la tuile à la position donnée
    /// </summary>
    /// <param name="relativeX">La valeur horizontale relative au chunk de la position donnée</param>
    /// <param name="relativeY">La valeur verticale relative au chunk de la position donnée</param>
    /// <param name="value">La nouvelle valeur de la tuile</param>
    /// <exception cref="FrogException">Exception jetée lorsque la position donnée est située en dehors des limites définies</exception>
    public void SetTile(int relativeX, int relativeY, int value)
    {
        if (relativeX >= 0 && relativeX < _width && relativeY >= 0 && relativeY < _height)
            _tiles[relativeY, relativeX] = value;
        else
            throw new FrogException("Tentative de modification d'une tuile en dehors des limites définies");
    }
}
