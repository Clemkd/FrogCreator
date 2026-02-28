using NUnit.Framework;
using FrogCreator.Api.Net;

namespace FrogCreator.Tests;

[TestFixture]
public class PacketsTest
{
    [Test]
    public void PacketSerializeDeserialize()
    {
        Packet original = new Packet(PacketType.CONNECT, "testPayload");
        string json = original.ToJSON();

        Packet deserialized = Packet.GetPacket(json);

        Assert.That(deserialized.GetPacketType(), Is.EqualTo(PacketType.CONNECT));
        Assert.That(deserialized.GetSerializedObject(), Is.EqualTo("testPayload"));
    }

    [Test]
    public void PacketTypeValues()
    {
        Assert.That(PacketType.NONE, Is.EqualTo((PacketType)0));
        Assert.That(PacketType.PROTOCOL_VERSION, Is.EqualTo((PacketType)1));
        Assert.That(PacketType.CONNECT, Is.EqualTo((PacketType)2));
        Assert.That(PacketType.DISCONNECT, Is.EqualTo((PacketType)3));
    }

    [Test]
    public void PacketSubscriberNotifiesListener()
    {
        PacketSubscriber subscriber = new PacketSubscriber();
        Packet? received = null;

        var listener = new TestPacketListener(p => received = p);
        subscriber.Subscribe(PacketType.CONNECT, listener);

        Packet packet = new Packet(PacketType.CONNECT, "data");
        subscriber.PushPacket(packet);

        Assert.That(received, Is.Not.Null);
        Assert.That(received!.GetPacketType(), Is.EqualTo(PacketType.CONNECT));
    }

    [Test]
    public void PacketSubscriberDoesNotNotifyUnsubscribedType()
    {
        PacketSubscriber subscriber = new PacketSubscriber();
        Packet? received = null;

        var listener = new TestPacketListener(p => received = p);
        subscriber.Subscribe(PacketType.CONNECT, listener);

        Packet packet = new Packet(PacketType.DISCONNECT, "data");
        subscriber.PushPacket(packet);

        Assert.That(received, Is.Null);
    }

    [Test]
    public void PacketSubscriberUnsubscribeWorks()
    {
        PacketSubscriber subscriber = new PacketSubscriber();
        int callCount = 0;

        var listener = new TestPacketListener(_ => callCount++);
        subscriber.Subscribe(PacketType.SYNC, listener);

        subscriber.PushPacket(new Packet(PacketType.SYNC, "a"));
        Assert.That(callCount, Is.EqualTo(1));

        subscriber.Unsubscribe(PacketType.SYNC, listener);
        subscriber.PushPacket(new Packet(PacketType.SYNC, "b"));
        Assert.That(callCount, Is.EqualTo(1));
    }

    [Test]
    public void InvalidJsonThrowsFrogException()
    {
        Assert.Throws<Api.Utils.FrogException>(() => Packet.GetPacket("not valid json"));
    }

    private class TestPacketListener : IPacketListener
    {
        private readonly Action<Packet> _action;
        public TestPacketListener(Action<Packet> action) { _action = action; }
        public void OnPacketReceived(Packet packet) { _action(packet); }
    }
}
