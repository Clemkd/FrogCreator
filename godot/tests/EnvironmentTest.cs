using NUnit.Framework;
using FrogCreator.Api.Environment.Map;
using FrogCreator.Api.Utils;

namespace FrogCreator.Tests;

[TestFixture]
public class EnvironmentTest
{
    [Test]
    public void CreateMapAndSetChunk()
    {
        GameMap map = new GameMap(32, 32, 16, 16);

        GameMapChunk chunk = new GameMapChunk(map);
        map.SetChunk(chunk, 0, 0);

        Assert.That(map.GetChunk(0, 0), Is.Not.Null);
        Assert.That(map.GetWidth(), Is.EqualTo(1));
        Assert.That(map.GetHeight(), Is.EqualTo(1));
    }

    [Test]
    public void SetChunkExpandsMap()
    {
        GameMap map = new GameMap(32, 32, 16, 16);

        GameMapChunk chunk = new GameMapChunk(map);
        map.SetChunk(chunk, 2, 3);

        Assert.That(map.GetWidth(), Is.EqualTo(3));
        Assert.That(map.GetHeight(), Is.EqualTo(4));
        Assert.That(map.GetChunk(2, 3), Is.Not.Null);
        Assert.That(map.GetChunk(0, 0), Is.Null);
    }

    [Test]
    public void NegativeIndexThrows()
    {
        GameMap map = new GameMap(32, 32, 16, 16);
        GameMapChunk chunk = new GameMapChunk(map);

        Assert.Throws<FrogException>(() => map.SetChunk(chunk, -1, 0));
        Assert.Throws<FrogException>(() => map.SetChunk(chunk, 0, -1));
    }

    [Test]
    public void AutoCropRemovesEmptyRowsAndColumns()
    {
        GameMap map = new GameMap(32, 32, 16, 16);

        GameMapChunk chunk = new GameMapChunk(map);
        map.SetChunk(chunk, 1, 1);

        // Set nulls around it by expanding
        map.SetChunk(null, 3, 3);

        Assert.That(map.GetWidth(), Is.EqualTo(4));
        Assert.That(map.GetHeight(), Is.EqualTo(4));

        map.AutoCrop();

        Assert.That(map.GetWidth(), Is.EqualTo(2));
        Assert.That(map.GetHeight(), Is.EqualTo(2));
    }

    [Test]
    public void SetTileCreatesChunkAndLayer()
    {
        GameMap map = new GameMap(32, 32, 16, 16);
        map.SetTile(0, 5, 5, 42);

        var chunkCoords = map.GetChunkCoordinatesFromAbsLocation(5, 5);
        GameMapChunk? chunk = map.GetChunk(chunkCoords.GetX(), chunkCoords.GetY());

        Assert.That(chunk, Is.Not.Null);
        Assert.That(chunk!.HasLayer(0), Is.True);
    }

    [Test]
    public void ChunkLayerTileOperations()
    {
        GameMap map = new GameMap(32, 32, 16, 16);
        GameMapChunk chunk = new GameMapChunk(map);
        chunk.AddLayer(0);

        chunk.SetTile(0, 3, 4, 99);
        int? tile = chunk.GetTile(0, 3, 4);

        Assert.That(tile, Is.EqualTo(99));
    }

    [Test]
    public void LayerOutOfBoundsThrows()
    {
        GameMapLayer layer = new GameMapLayer(10, 10);

        Assert.Throws<FrogException>(() => layer.SetTile(10, 0, 5));
        Assert.Throws<FrogException>(() => layer.SetTile(-1, 0, 5));
    }

    [Test]
    public void LayerGetTileOutOfBoundsReturnsNull()
    {
        GameMapLayer layer = new GameMapLayer(10, 10);
        Assert.That(layer.GetTile(10, 0), Is.Null);
        Assert.That(layer.GetTile(-1, 0), Is.Null);
    }
}
