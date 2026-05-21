using Mirror;

namespace Vmaya.VNet
{

    public interface ILogger
    {
        public void AddLog(string message);
    }

    // Сообщение для подписки/отписки на типы сообщений
    public struct SubscriptionMessage : NetworkMessage
    {
        public string MessageType;      // Полное имя типа сообщения
        public bool Subscribe;          // true - подписаться, false - отписаться
    }

    // Сообщение Hello
    public struct HelloMessage : NetworkMessage
    {
        public string Text;
    }
}