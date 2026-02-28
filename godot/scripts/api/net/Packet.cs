using System.Text.Json;
using FrogCreator.Api.System.Objects;
using FrogCreator.Api.Utils;

namespace FrogCreator.Api.Net;

public class Packet : ISerializable
{
    protected static readonly string PACKET_TYPE_KEY = "PacketType";
    protected static readonly string PACKET_OBJECT_KEY = "PacketObject";

    private PacketType _packetType;
    private string _serialObject;

    private Packet()
    {
        _serialObject = string.Empty;
    }

    public Packet(PacketType packetType, string serialObject)
    {
        _packetType = packetType;
        _serialObject = serialObject;
    }

    public PacketType GetPacketType()
    {
        return _packetType;
    }

    public string GetSerializedObject()
    {
        return _serialObject;
    }

    public static Packet GetPacket(string jsonData)
    {
        Packet p = new Packet();
        try
        {
            using JsonDocument doc = JsonDocument.Parse(jsonData);
            JsonElement root = doc.RootElement;
            p._packetType = (PacketType)root.GetProperty(PACKET_TYPE_KEY).GetInt32();
            p._serialObject = root.GetProperty(PACKET_OBJECT_KEY).GetString() ?? string.Empty;
        }
        catch (Exception)
        {
            throw new FrogException("Impossible de convertir les données reçues");
        }

        return p;
    }

    public string ToJSON()
    {
        var obj = new Dictionary<string, object>
        {
            { PACKET_TYPE_KEY, (int)_packetType },
            { PACKET_OBJECT_KEY, _serialObject }
        };
        return JsonSerializer.Serialize(obj);
    }
}
