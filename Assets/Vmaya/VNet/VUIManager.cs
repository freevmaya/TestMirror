using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vmaya.VNet;

namespace Vmaya.NetMsg
{
    public class VUIManager : MonoBehaviour, Vmaya.VNet.ILogger
    {
        [Header("UI References")]
        [SerializeField] private Button _hostButton;
        [SerializeField] private Button _clientButton;
        [SerializeField] private Button _disconnectButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _sendMessageButton;
        [SerializeField] private TMP_Text _statusText;
        [SerializeField] private TMP_Text _messagesText;

        [Header("Components")]
        [SerializeField] private NetworkManager _networkManager;
        private string _logMessages = "";

        private void Start()
        {
            SetupButtons();
            UpdateUI();
            AddLog("Application started. Click Host or Client to begin.");
        }

        private void SetupButtons()
        {
            if (_hostButton != null)
                _hostButton.onClick.AddListener(StartHost);

            if (_clientButton != null)
                _clientButton.onClick.AddListener(StartClient);

            if (_disconnectButton != null)
                _disconnectButton.onClick.AddListener(Disconnect);

            if (_closeButton != null)
                _closeButton.onClick.AddListener(CloseApplication);
        }

        private void StartHost()
        {
            if (_networkManager != null)
            {
                _networkManager.StartHost();
                AddLog("Started as HOST");
                UpdateUI();
            }
        }

        private void StartClient()
        {
            if (_networkManager != null)
            {
                _networkManager.StartClient();
                AddLog("Started as CLIENT");
                UpdateUI();
            }
        }

        private void Disconnect()
        {
            if (_networkManager != null)
            {
                _networkManager.StopHost();
                _networkManager.StopClient();
                _networkManager.StopServer();


                VSubscriptionManager smanager = _networkManager.GetComponent<VSubscriptionManager>();
                if (smanager) smanager.ClearAll();
                AddLog("Disconnected");
                UpdateUI();
            }
        }

        public void AddLog(string message)
        {
            string timestamp = System.DateTime.Now.ToString("HH:mm:ss");
            string logEntry = $"[{timestamp}] {message}\n";
            _logMessages += logEntry;

            if (_messagesText != null)
                _messagesText.text = _logMessages;

            Debug.Log(message);
        }

        private void UpdateUI()
        {
            bool isConnected = NetworkServer.active || NetworkClient.active;

            if (_hostButton != null)
                _hostButton.interactable = !isConnected;

            if (_clientButton != null)
                _clientButton.interactable = !isConnected;

            if (_disconnectButton != null)
                _disconnectButton.interactable = isConnected;

            if (_sendMessageButton != null)
                _sendMessageButton.interactable = NetworkServer.active;

            if (_statusText != null)
            {
                string status = "Status: ";
                if (NetworkServer.active && NetworkClient.active)
                    status += "HOST";
                else if (NetworkServer.active)
                    status += "SERVER";
                else if (NetworkClient.active)
                    status += "CLIENT";
                else
                    status += "DISCONNECTED";

                _statusText.text = status;
            }
        }

        private void CloseApplication()
        {
            AddLog("Closing...");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
        }
    }
}