using System.Text;
using FrogCreator.Api.Math;
using FrogCreator.Api.Utils;

namespace FrogCreator.Api.Environment.Map;

public class GameMap
{
    private List<List<GameMapChunk?>> _chunks;
    private int _tileWidth, _tileHeight;
    private int _chunkTilesCountRow, _chunkTilesCountColumn;
    private int _currentMapWidth;

    /// <summary>
    /// Constructeur de map dynamique
    /// </summary>
    /// <param name="tileWidth">Largeur des tuiles</param>
    /// <param name="tileHeight">Hauteur des tuiles</param>
    /// <param name="chunkTilesCountRow">Nombre de tuiles par ligne de chunk</param>
    /// <param name="chunkTilesCountColumn">Nombre de tuiles par colonne de chunk</param>
    public GameMap(int tileWidth, int tileHeight, int chunkTilesCountRow, int chunkTilesCountColumn)
    {
        _chunks = new List<List<GameMapChunk?>>();
        _tileWidth = tileWidth;
        _tileHeight = tileHeight;
        _chunkTilesCountRow = chunkTilesCountRow;
        _chunkTilesCountColumn = chunkTilesCountColumn;
        _currentMapWidth = 0;
    }

    /// <summary>
    /// Ajoute une nouvelle ligne dans la map.
    /// Ligne ajoutée en bas de la map (axe y)
    /// </summary>
    private void AddRow()
    {
        _chunks.Add(new List<GameMapChunk?>());
    }

    /// <summary>
    /// Ajoute des colonnes dans la carte jusqu'à l'index spécifié
    /// </summary>
    /// <param name="maxIndex">L'index de la dernière colonne à droite (axe x)</param>
    private void AddColumns(int maxIndex)
    {
        for (int i = 0; i < _chunks.Count; i++)
        {
            while (_chunks[i].Count <= maxIndex)
            {
                _chunks[i].Add(null);
            }
        }
        _currentMapWidth = maxIndex + 1;
    }

    /// <summary>
    /// Obtient le chunk à la position spécifiée
    /// </summary>
    /// <param name="x">La valeur horizontale (axe x) de la position</param>
    /// <param name="y">La valeur verticale (axe y) de la position</param>
    /// <returns>Obtient le chunk si existant, retourne null dans le cas contraire</returns>
    public GameMapChunk? GetChunk(int x, int y)
    {
        if (_chunks.Count > y && _chunks[y].Count > x)
            return _chunks[y][x];
        return null;
    }

    /// <summary>
    /// Met à jour un chunk à une position donnée
    /// </summary>
    /// <param name="chunk">Le nouveau chunk (peut être null)</param>
    /// <param name="x">La valeur horizontale (axe x) de la position</param>
    /// <param name="y">La valeur verticale (axe y) de la position</param>
    /// <exception cref="FrogException">Exception jetée si les indexs sont incorrects, ou que la map n'a pas été redimensionnée correctement</exception>
    public void SetChunk(GameMapChunk? chunk, int x, int y)
    {
        if (x < 0 || y < 0)
            throw new FrogException("Les indexs ne peuvent être négatifs");

        while (_chunks.Count <= y)
            AddRow();
        while (_chunks[y].Count <= x)
            AddColumns(x); // Mets à jour la map entière

        if (_chunks.Count >= y && _chunks[y].Count >= x)
            _chunks[y][x] = chunk;
        else
            throw new FrogException("Error lors de la mise à jour d'un chunk, il se peut que l'augmentation des arrays ait échouée");
    }

    /// <summary>
    /// Obtient la position du chunk selon les coordonnées absolues données (en nombre de tuiles)
    /// </summary>
    /// <param name="absoluteX">La valeur horizontale (axe x) de la position absolue</param>
    /// <param name="absoluteY">La valeur verticale (axe y) de la position absolue</param>
    /// <returns>La position du chunk encapsulant la position absolue</returns>
    public Vector2<int> GetChunkCoordinatesFromAbsLocation(int absoluteX, int absoluteY)
    {
        return new Vector2<int>(absoluteX / _chunkTilesCountRow, absoluteY / _chunkTilesCountColumn);
    }

    /// <summary>
    /// Met à jour la tuile à la position absolue spécifiée
    /// </summary>
    /// <param name="layerIndex">La couche de la tuile</param>
    /// <param name="absoluteX">La valeur horizontale de la position</param>
    /// <param name="absoluteY">La valeur verticale de la position</param>
    /// <param name="value">La nouvelle valeur de la tuile</param>
    /// <exception cref="FrogException">Exception jetée lorsqu'une erreur survient lors de la mise à jour d'un chunk</exception>
    public void SetTile(int layerIndex, int absoluteX, int absoluteY, int value)
    {
        Vector2<int> chunkCoords = GetChunkCoordinatesFromAbsLocation(absoluteX, absoluteY);
        GameMapChunk? chunk = GetChunk(chunkCoords.GetX(), chunkCoords.GetY());
        if (chunk == null)
        {
            chunk = new GameMapChunk(this);
            if (!chunk.HasLayer(layerIndex))
                chunk.AddLayer(layerIndex);
            SetChunk(chunk, chunkCoords.GetX(), chunkCoords.GetY());
        }

        int relativeX = absoluteX % _chunkTilesCountRow;
        int relativeY = absoluteY % _chunkTilesCountColumn;
        chunk.SetTile(layerIndex, relativeX, relativeY, value);
    }

    /// <summary>
    /// Découpe la map automatiquement en fonction des chunks qui l'a compose.
    /// La map est coupée pour être la plus petite possible (largeur / hauteur).
    /// La découpe est faite en partant par le bord bas-droit vers le bord haut-gauche de la map.
    /// </summary>
    public void AutoCrop()
    {
        AutoCropRows();
        AutoCropColumns();
    }

    /// <summary>
    /// Découpe automatiquement les lignes de la carte en partant du bas de la map
    /// </summary>
    private void AutoCropRows()
    {
        bool isRowNull = true;

        while (_chunks.Count > 0 && isRowNull)
        {
            for (int i = 0; i < _chunks.Count; i++)
            {
                if (GetChunk(i, _chunks.Count - 1) != null)
                {
                    isRowNull = false;
                    break;
                }
            }

            if (isRowNull)
            {
                _chunks.RemoveAt(_chunks.Count - 1);
            }
        }
    }

    /// <summary>
    /// Découpe automatiquement les colonnes de la carte en partant de la droite de la map
    /// </summary>
    public void AutoCropColumns()
    {
        bool isColumnNull = true;

        while (_currentMapWidth > 0 && isColumnNull)
        {
            for (int i = 0; i < _chunks.Count; i++)
            {
                if (GetChunk(_currentMapWidth - 1, i) != null)
                {
                    isColumnNull = false;
                    break;
                }
            }

            if (isColumnNull)
            {
                for (int i = 0; i < _chunks.Count; i++)
                {
                    _chunks[i].RemoveAt(_currentMapWidth - 1);
                }
                _currentMapWidth--;
            }
        }
    }

    /// <summary>
    /// Obtient la largeur des tuiles de la map
    /// </summary>
    internal int GetTileWidth() { return _tileWidth; }

    /// <summary>
    /// Obtient la hauteur des tuiles de la map
    /// </summary>
    internal int GetTileHeight() { return _tileHeight; }

    /// <summary>
    /// Obtient la largeur des chunks de la map, en nombre de tuiles
    /// </summary>
    internal int GetChunkTilesCountRow() { return _chunkTilesCountRow; }

    /// <summary>
    /// Obtient la hauteur des chunks de la map, en nombre de tuiles
    /// </summary>
    internal int GetChunkTilesCountColumn() { return _chunkTilesCountColumn; }

    /// <summary>
    /// Obtient la largeur de la map en nombre de chunks
    /// </summary>
    public int GetWidth() { return _currentMapWidth; }

    /// <summary>
    /// Obtient la hauteur de la map en nombre de chunks
    /// </summary>
    public int GetHeight() { return _chunks.Count; }

    /// <summary>
    /// Obtient la représentation textuelle de la map
    /// </summary>
    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        for (int y = 0; y < _chunks.Count; y++)
        {
            for (int x = 0; x < _chunks[y].Count; x++)
            {
                if (GetChunk(x, y) != null)
                    builder.Append("[C]");
                else
                    builder.Append("[ ]");
            }
            builder.Append('\n');
        }
        return builder.ToString();
    }
}
