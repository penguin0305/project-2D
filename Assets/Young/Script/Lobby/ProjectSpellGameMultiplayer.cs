using System;
using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.Collections;

namespace Multiplay
{
    public struct NetworkPlayerInfo : INetworkSerializable, IEquatable<NetworkPlayerInfo>
    {
        public ulong ClientId;
        public FixedString64Bytes Name;
        public bool IsReady;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref ClientId);
            serializer.SerializeValue(ref Name);
            serializer.SerializeValue(ref IsReady);

        }

        public bool Equals(NetworkPlayerInfo other)
        {
            return ClientId == other.ClientId && Name == other.Name && IsReady == other.IsReady;
        }
    }
    public class ProjectSpellGameMultiplayer : NetworkBehaviour
    {
        public static ProjectSpellGameMultiplayer Singleton { get; private set; }
        public static bool _playMultiplayer = true;
        public event EventHandler OnPlayerInfosChanged;

        private NetworkManager NetworkManager => NetworkManager.Singleton;
        private NetworkList<NetworkPlayerInfo> _playerInfos = new NetworkList<NetworkPlayerInfo>();

        public string gameSceneName;
        public string PlayerName { get; set; } = "Player";

        private void Awake()
        {
            if (Singleton && Singleton != this)
            {
                Destroy(gameObject);
                return;
            }

            Singleton = this;

            DontDestroyOnLoad(this);

            _playerInfos.OnListChanged += PlayerDataNetworkList_OnOnListChanged;
        }

        private void PlayerDataNetworkList_OnOnListChanged(NetworkListEvent<NetworkPlayerInfo> changeEvent)
        {
            OnPlayerInfosChanged?.Invoke(this, EventArgs.Empty);
        }

        #region Host

        public void StartHost()
        {
            NetworkManager.OnClientConnectedCallback += NetworkManager_Server_OnClientConnectedCallback;
            NetworkManager.StartHost();
        }

        private void NetworkManager_Server_OnClientConnectedCallback(ulong clientId)
        {
            _playerInfos.Add(new NetworkPlayerInfo()
            {
                ClientId = clientId,
            });
            SetPlayerNameRpc(PlayerName);
        }

        #endregion

        #region Client

        public void StartClient()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_Client_OnClientConnectedCallback;
            NetworkManager.Singleton.StartClient();
        }

        private void NetworkManager_Client_OnClientConnectedCallback(ulong obj)
        {
            SetPlayerNameRpc(PlayerName);
        }

        #endregion

        [Rpc(SendTo.Server)]
        private void SetPlayerNameRpc(string newName, RpcParams rpcParams = default)
        {
            int playerInfoIndex = GetIndexFromClientId(rpcParams.Receive.SenderClientId);

            var playerInfo = _playerInfos[playerInfoIndex];
            playerInfo.Name = newName;
            _playerInfos[playerInfoIndex] = playerInfo;
        }
        public NetworkPlayerInfo GetPlayerInfo()
        {
            return GetPlayerInfoByClientId(NetworkManager.Singleton.LocalClientId);
        }

        public NetworkPlayerInfo GetPlayerInfoByClientId(ulong clientId)
        {
            foreach (var playerInfo in _playerInfos)
            {
                if (playerInfo.ClientId == clientId)
                {
                    return playerInfo;
                }
            }

            return default;
        }
        private int GetIndexFromClientId(ulong clientId)
        {
            for (int i = 0; i < _playerInfos.Count; i++)
            {
                if (_playerInfos[i].ClientId == clientId)
                {
                    return i;
                }
            }

            return -1;
        }
        public bool IsPlayerIndexConnected(int playerIndex)
        {
            return playerIndex < _playerInfos.Count;
        }

        [Rpc(SendTo.Server)]
        public void ToggleReadyRpc(RpcParams rpcParams = default)
        {
            int index = GetIndexFromClientId(rpcParams.Receive.SenderClientId);
            if (index == -1) return;

            var playerInfo = _playerInfos[index];
            playerInfo.IsReady = !playerInfo.IsReady;
            _playerInfos[index] = playerInfo;
        }

        public void StartMultiplayerGame()
        {
            if (!IsServer) return;

            bool isClientReady = true;

            foreach (var player in _playerInfos)
            {
                if (player.ClientId != NetworkManager.LocalClientId && !player.IsReady)
                {
                    isClientReady = false;
                    break;
                }
            }

            if (isClientReady)
            {
                NetworkManager.Singleton.SceneManager.LoadScene(gameSceneName, LoadSceneMode.Single);
            }
            else
            {
                Debug.Log("아직 레디하지 않은 플레이어가 있습니다!");
            }

        }
    }
}