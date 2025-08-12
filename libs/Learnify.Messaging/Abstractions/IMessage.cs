namespace Learnify.Messaging.Abstractions;

public interface IMessage
{
    public string MessageType { get; }
    public object Payload { get; }
}