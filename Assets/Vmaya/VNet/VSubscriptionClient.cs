using Mirror;
using System;
using UnityEngine;
using Vmaya.NetMsg;
using Zenject;

namespace Vmaya.VNet
{
    public class VSubscriptionClient : MonoBehaviour
    {
        [Inject] private VUIManager _uiManager;

        public void AfterConnect()
        {
            Subscribe(true, (HelloMessage msg) => {
                string logMsg = "Получено сообщение от сервера: " + msg.Text;
                if (_uiManager)
                    _uiManager.AddLog(logMsg);
                else Debug.Log(logMsg);
            });
        }

        // Подписывает клиента на определенный тип сообщений
        public void Subscribe<T>(bool stateValue, Action<T> handler = null) where T : struct, NetworkMessage
        {
            if (stateValue && handler != null)
            {
                NetworkClient.RegisterHandler<T>(handler);
            }
            else if (!stateValue)
            {
                NetworkClient.UnregisterHandler<T>();
            }

            NetworkClient.Send(new SubscriptionMessage()
            {
                MessageType = typeof(T).Name,
                Subscribe = stateValue
            });
        }
    }
}
