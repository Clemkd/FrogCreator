using FrogCreator.Api.System.Objects;
using FrogCreator.Api.Utils;

namespace FrogCreator.Api.Environment.Map;

public class GameMapChunk : IResource
{
    private GameMap _parent;
    private int _tileWidth, _tileHeight;
    private int _chunkTilesCountRow, _chunkTilesCountColumn;
    private int _chunkWidth, _chunkHeight;
    private Dictionary<int, GameMapLayer> _layers;

    public GameMapChunk(GameMap parent)
    {
        _parent = parent;
        _tileWidth = _parent.GetTileWidth();
        _tileHeight = _parent.GetTileHeight();
        _chunkTilesCountRow = _parent.GetChunkTilesCountRow();
        _chunkTilesCountColumn = _parent.GetChunkTilesCountColumn();
        _chunkWidth = _tileWidth * _chunkTilesCountRow;
        _chunkHeight = _tileHeight * _chunkTilesCountColumn;
        _layers = new Dictionary<int, GameMapLayer>();
    }

    public void Load()
    {
        foreach (var key in _layers.Keys)
        {
            _layers[key].Load();
        }
    }

    public void Unload()
    {
        foreach (var key in _layers.Keys)
        {
            _layers[key].Unload();
        }
    }

    /// <summary>
    /// Obtient la largeur en pixels du chunk
    /// </summary>
    public int GetWidth() { return _chunkWidth; }

    /// <summary>
    /// Obtient la hauteur en pixels du chunk
    /// </summary>
    public int GetHeight() { return _chunkHeight; }

    /// <summary>
    /// Obtient l'état d'existence de la couche avec l'index spécifié
    /// </summary>
    /// <param name="index">L'index de la couche recherchée</param>
    /// <returns>Vrai si la couche existe, Faux dans le cas contraire</returns>
    public bool HasLayer(int index)
    {
        return _layers.ContainsKey(index);
    }

    /// <summary>
    /// Ajoute une nouvelle couche avec l'index spécifié
    /// </summary>
    /// <param name="index">L'index de la nouvelle couche à ajouter</param>
    public void AddLayer(int index)
    {
        _layers[index] = new GameMapLayer(_chunkTilesCountRow, _chunkTilesCountColumn);
    }

    /// <summary>
    /// Supprime la couche spécifiée
    /// </summary>
    /// <param name="index">L'index de la couche à supprimer</param>
    public void RemoveLayer(int index)
    {
        _layers.Remove(index);
    }

    /// <summary>
    /// Met à jour la tuile disponible à la position relative spécifiée, sur la couche donnée
    /// </summary>
    /// <param name="layerIndex">L'index de la couche de la tuile</param>
    /// <param name="relativeX">La valeur horizontale de la position</param>
    /// <param name="relativeY">La valeur verticale de la position</param>
    /// <param name="value">La nouvelle valeur de la tuile</param>
    /// <exception cref="FrogException">Exception jetée si la couche spécifiée n'existe pas</exception>
    public void SetTile(int layerIndex, int relativeX, int relativeY, int value)
    {
        if (!_layers.ContainsKey(layerIndex))
            throw new FrogException("Tentative de modification d'une tuile sur une couche inexistante");
        _layers[layerIndex].SetTile(relativeX, relativeY, value);
    }

    /// <summary>
    /// Obtient la tuile à la position relative spécifiée du chunk
    /// </summary>
    /// <param name="layerIndex">La couche du chunk</param>
    /// <param name="relativeX">La valeur horizontale de la position</param>
    /// <param name="relativeY">La valeur verticale de la position</param>
    /// <returns>La tuile à la position relative spécifiée</returns>
    /// <exception cref="FrogException">Exception jetée si la couche spécifiée n'existe pas</exception>
    public int? GetTile(int layerIndex, int relativeX, int relativeY)
    {
        if (!_layers.ContainsKey(layerIndex))
            throw new FrogException("Tentative d'obtention d'une tuile sur une couche inexistante");
        return _layers[layerIndex].GetTile(relativeX, relativeY);
    }
}
