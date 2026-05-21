using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Vmaya.VNet
{
    [RequireComponent(typeof(NetworkManager))]
    public class VSubscriptionManager : VSubscriptionClient
    {
        private Dictionary<int, NetworkConnectionToClient> _connections = new();
        private Dictionary<int, List<string>> _clientSubscriptions = new();
        private int _nextClientId = 1;

        public void AfterStartAsHost()
        {
            NetworkServer.RegisterHandler<SubscriptionMessage>(OnSubscriptionReceived);
        }

        // При запросе клиента на подписку
        protected void OnSubscriptionReceived(NetworkConnectionToClient conn, SubscriptionMessage msg)
        {
            int index_cl = GetClientId(conn);
            if (index_cl > 0)
            {
                int mtIndex = _clientSubscriptions[index_cl].IndexOf(msg.MessageType);
                if (mtIndex > -1)
                {
                    if (!msg.Subscribe)
                        _clientSubscriptions[index_cl].RemoveAt(mtIndex);
                } else
                {
                    if (msg.Subscribe)
                    {
                        _clientSubscriptions[index_cl].Add(msg.MessageType);

                        // Если подписка на сообщения типа HelloMessage то сразу отправляем соответствующее сообщение клиенту
                        if (msg.MessageType == "HelloMessage")
                        {
                            SendToClient(index_cl, new HelloMessage()
                            {
                                Text = "Hello Client!"
                            });
                        }
                    }
                }
            }
        }

        private int GetClientId(NetworkConnectionToClient conn)
        {
            return _connections.FirstOrDefault(x => x.Value == conn).Key;
        }

        public void SendToClient<T>(int clientId, T message) where T : struct, NetworkMessage
        {
            if (_connections.TryGetValue(clientId, out var conn))
            {
                conn.Send(message);
            }
        }

        public void AddConnection(NetworkConnectionToClient conn)
        {
            if (GetClientId(conn) == 0)
            {
                int clientId = _nextClientId++;
                _connections[clientId] = conn;
                _clientSubscriptions[clientId] = new List<string>();

                Debug.Log($"[Server] New client {clientId} from Update");
            }
        }

        public void RemoveConnection(NetworkConnectionToClient conn)
        {
            int index = GetClientId(conn);
            if (index > 0)
            {
                _connections.Remove(index);
                _clientSubscriptions.Remove(index);
            }
        }
        public void ClearAll()
        {
            _connections.Clear();
            _clientSubscriptions.Clear();
            _nextClientId = 1;
            Debug.Log("[Server] State cleared");
        }


        // Использовать для отправки сообщений только подписаным клиентам
        public void SendToSubscribers<T>(T message) where T : struct, NetworkMessage
        {
            string type = typeof(T).FullName;

            foreach (var pair in _clientSubscriptions)
            {
                if (pair.Value.Contains(type) && _connections.TryGetValue(pair.Key, out var conn))
                {
                    conn.Send(message);
                }
            }
        }
    }
}