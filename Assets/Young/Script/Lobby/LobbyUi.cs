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
        }

        private void Start()
        {
            playerNameInputField.text = GetRandomName();

            ProjectSpellGameLobby.Singleton.OnLobbyListChanged += ProjectSpellGameLobby_OnOnLobbyListChanged;
            UpdateLobbyList(new List<Lobby>());

            lobbyTemplate.gameObject.SetActive(false);

            ProjectSpellGameMultiplayer.Singleton.OnPlayerInfosChanged += UpdateRoomUI;

            NetworkManager.Singleton.OnServerStarted += () =>
            {
                if (NetworkManager.Singleton.IsServer)
                {
                    ShowLobbyRoomPopup();
                }
            };

            if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost)
            {
                ShowLobbyRoomPopup();
            }

            NetworkManager.Singleton.OnClientConnectedCallback += (clientId) =>
            {
                if (clientId == NetworkManager.Singleton.LocalClientId)
                {
                    ShowLobbyRoomPopup();
                }
            };

            HideLobbyRoomPopup();
        }

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
            hostNameText.text = "Waiting";
            clientNameText.text = "Waiting";
            clientReadyText.text = "";
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
            lobbyRoomPanel.SetActive(false);
        }
        private void UpdateRoomUI(object sender, EventArgs e)
        {
            if (!lobbyRoomPanel.activeSelf) return;

            ClearRoomUI();

            startButton.gameObject.SetActive(NetworkManager.Singleton.IsServer);
            readyButton.gameObject.SetActive(NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer);

            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                var playerInfo = ProjectSpellGameMultiplayer.Singleton.GetPlayerInfoByClientId(clientId);
                if (string.IsNullOrEmpty(playerInfo.Name.ToString())) continue;

                if (clientId == NetworkManager.ServerClientId)
                {
                    hostNameText.text = playerInfo.Name.ToString();
                }
                else // ¼Õ´Ô
                {
                    clientNameText.text = playerInfo.Name.ToString();
                    clientReadyText.text = playerInfo.IsReady ? "READY" : "WAITING";
                }
            }
        }
        private void OnDestroy()
        {
            ProjectSpellGameLobby.Singleton.OnLobbyListChanged -= ProjectSpellGameLobby_OnOnLobbyListChanged;

            ProjectSpellGameMultiplayer.Singleton.OnPlayerInfosChanged -= UpdateRoomUI;
        }
    }
}