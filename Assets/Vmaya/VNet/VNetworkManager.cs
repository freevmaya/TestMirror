using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace Vmaya.VNet
{
    public class VNetworkManager : NetworkManager
    {
        public UnityEvent<NetworkConnectionToClient> OnConnect;
        public UnityEvent<NetworkConnectionToClient> OnDisconnect;
        public UnityEvent OnStartAsHost;
        public UnityEvent OnConnectClient;
        public UnityEvent OnDisconnectClient;

        public override void OnStartHost()
        {
            base.OnStartHost();
            OnStartAsHost.Invoke();
        }

        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            base.OnServerConnect(conn);
            Debug.Log($"Клиент {conn.connectionId} подключился к серверу");
            OnConnect.Invoke(conn);
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);
            Debug.Log($"Клиент {conn.connectionId} отключился от сервера");
            OnDisconnect.Invoke(conn);
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            Debug.Log("Клиент успешно подключился к серверу");
            OnConnectClient.Invoke();
        }

        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();
            Debug.Log("Клиент отключился от сервера");
            OnDisconnectClient.Invoke();
        }
    }
}