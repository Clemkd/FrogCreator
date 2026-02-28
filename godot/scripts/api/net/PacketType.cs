namespace FrogCreator.Api.Net;

public enum PacketType
{
    NONE,
    PROTOCOL_VERSION,
    CONNECT,
    DISCONNECT,
    SYNC,
    PROTOCOL_VERSION_RESULT,
    CONNECT_RESULT,
    UPDATE_MOVEMENT_RESULT
}
