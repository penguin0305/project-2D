using System;
using System.Collections.Generic;
using Multiplay;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Main
{
    public class LobbyUi : MonoBehaviour
    {
        [SerializeField] private CreateLobbyUi createLobbyUi;

        [SerializeField] private Button createLobbyButton;
        [SerializeField] private Button quickJoinButton;
        [SerializeField] private Transform lobbyContainer;
        [SerializeField] private Transform lobbyTemplate;

        [Header("Player Name")]
        [SerializeField] private Button randomNameButton;
        [SerializeField] private TMP_InputField playerNameInputField;

        [SerializeField] private GameObject lobbyRoomPanel;
        [SerializeField] private TMP_Text hostNameText;
        [SerializeField] private TMP_Text clientNameText;
        [SerializeField] private TMP_Text clientReadyText;
        [SerializeField] private Button readyButton;
        [SerializeField] private Button startButton;
        [SerializeField] private Button leaveButton;

        [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[4];
        [SerializeField] private TMP_Text[] playerReadyTexts = new TMP_Text[4];
        private void Awake()
        {
            createLobbyButton.onClick.AddListener(() => { createLobbyUi.Show(); });

            quickJoinButton.onClick.AddListener(() =>
            {
                ProjectSpellGameLobby.Singleton.QuickJoinLobby();
                Debug.Log("Quick Join button clicked");
            });

            playerNameInputField.onEndEdit.AddListener(name =>
            {
                if (string.IsNullOrEmpty(name))
                {
                    playerNameInputField.text = GetRandomName();
                }
            });
            playerNameInputField.onValueChanged.AddListener(name =>
            {
                if (!string.IsNullOrEmpty(name))
                {
                    ProjectSpellGameMultiplayer.Singleton.PlayerName = name;
                }
            });

            readyButton.onClick.AddListener(() =>
            {
                ProjectSpellGameMultiplayer.Singleton.ToggleReadyRpc();
            });

            startButton.onClick.AddListener(() =>
            {
                ProjectSpellGameMultiplayer.Singleton.StartMultiplayerGame();
            });

            leaveButton.onClick.AddListener(() =>
            {
                if (NetworkManager.Singleton.IsHost)
                {
                    ProjectSpellGameLobby.Singleton.DeleteLobby();
                }
                else
                {
                    ProjectSpellGameLobby.Singleton.LeaveLobby();
                }

                NetworkManager.Singleton.Shutdown();
                HideLobbyRoomPopup();
            });
        }

        private void Start()
        {
            playerNameInputField.text = GetRandomName();

            ProjectSpellGameLobby.Singleton.OnLobbyListChanged += ProjectSpellGameLobby_OnOnLobbyListChanged;
            UpdateLobbyList(new List<Lobby>());

            lobbyTemplate.gameObject.SetActive(false);

            ProjectSpellGameMultiplayer.Singleton.OnPlayerInfosChanged += UpdateRoomUI;

            NetworkManager.Singleton.OnServerStarted += HandleOnServerStarted;
            NetworkManager.Singleton.OnClientConnectedCallback += HandleOnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleOnClientDisconnected;
            
            if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost) ShowLobbyRoomPopup();
            HideLobbyRoomPopup();
        }

        private void HandleOnServerStarted() { if (NetworkManager.Singleton.IsServer) ShowLobbyRoomPopup(); }
        private void HandleOnClientConnected(ulong clientId) { if (clientId == NetworkManager.Singleton.LocalClientId) ShowLobbyRoomPopup(); }
        private void HandleOnClientDisconnected(ulong clientId) { if (clientId == NetworkManager.Singleton.LocalClientId || clientId == NetworkManager.ServerClientId) HideLobbyRoomPopup(); }

        private void ProjectSpellGameLobby_OnOnLobbyListChanged(object sender, ProjectSpellGameLobby.OnLobbyListChangedEventArgs e)
        {
            UpdateLobbyList(e.Lobbies);
        }

        private void UpdateLobbyList(List<Lobby> lobbies)
        {
            foreach (Transform child in lobbyContainer)
            {
                if (child == lobbyTemplate) continue;
                Destroy(child.gameObject);
            }

            foreach (var lobby in lobbies)
            {
                var lobbyTransform = Instantiate(lobbyTemplate, lobbyContainer);
                lobbyTransform.gameObject.SetActive(true);
                lobbyTransform.GetComponent<LobbyListItem>().SetLobby(lobby);
            }
        }

        private void ClearRoomUI()
        {
            for (int i = 0; i < playerNameTexts.Length; i++)
            {
                if (playerNameTexts[i] != null) playerNameTexts[i].text = "";
                if (playerReadyTexts[i] != null) playerReadyTexts[i].text = "";
            }
        }
        private string GetRandomName()
        {
            return "Player_" + UnityEngine.Random.Range(1000, 10000);
        }

        public void ShowLobbyRoomPopup()
        {
            lobbyRoomPanel.SetActive(true);
            ClearRoomUI();
            UpdateRoomUI(this, EventArgs.Empty);
        }

        public void HideLobbyRoomPopup()
        {
            if (lobbyRoomPanel != null)
            {
                lobbyRoomPanel.SetActive(false);
            }
        }
        private void UpdateRoomUI(object sender, EventArgs e)
        {
            if (!lobbyRoomPanel.activeSelf) return;

            ClearRoomUI();

            startButton.gameObject.SetActive(NetworkManager.Singleton.IsServer);
            readyButton.gameObject.SetActive(NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer);

            int slotIndex = 0;

            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                if (slotIndex >= 4) break;

                var playerInfo = ProjectSpellGameMultiplayer.Singleton.GetPlayerInfoByClientId(clientId);
                if (string.IsNullOrEmpty(playerInfo.Name.ToString())) continue;

                playerNameTexts[slotIndex].text = playerInfo.Name.ToString();

                if (clientId == NetworkManager.ServerClientId)
                {
                    playerReadyTexts[slotIndex].text = "HOST";
                }
                else
                {
                    playerReadyTexts[slotIndex].text = playerInfo.IsReady ? "READY" : "";
                }

                slotIndex++;
            }

        }
        
        private void OnDestroy()
        {
            if (ProjectSpellGameLobby.Singleton != null)
                ProjectSpellGameLobby.Singleton.OnLobbyListChanged -= ProjectSpellGameLobby_OnOnLobbyListChanged;

            if (ProjectSpellGameMultiplayer.Singleton != null)
                ProjectSpellGameMultiplayer.Singleton.OnPlayerInfosChanged -= UpdateRoomUI;

            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnServerStarted -= HandleOnServerStarted;
                NetworkManager.Singleton.OnClientConnectedCallback -= HandleOnClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback -= HandleOnClientDisconnected;
            }
        }
    }
}