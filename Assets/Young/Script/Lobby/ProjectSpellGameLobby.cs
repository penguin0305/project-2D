using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using Multiplay;
using Unity.Networking.Transport.Relay;
using Random = UnityEngine.Random;

public class ProjectSpellGameLobby : MonoBehaviour
{
    public static ProjectSpellGameLobby Singleton { get; private set; }

    public event EventHandler<OnLobbyListChangedEventArgs> OnLobbyListChanged;

    private float _heartbeatTimer;
    private const float MaxHeartbeatTimer = 15f;

    public class OnLobbyListChangedEventArgs : EventArgs
    {
        public List<Lobby> Lobbies;
    }

    private Lobby _joinedLobby;
    private const float MaxListLobbyTimer = 3f;
    private const int MaxPlayerInLobby = 4;
    private const string RelayJoinCode = "RelayJoinCode";
    private float _listLobbiesTimer = MaxListLobbyTimer;

    private void Awake()
    {
        if (Singleton && Singleton != this)
        {
            Destroy(gameObject);
            return;
        }

        Singleton = this;
        DontDestroyOnLoad(this);

        _ = InitializeUnityAuthentication();
    }

    private void Update()
    {
        HandlePeriodicListLobbies();
        HandleLobbyHeartbeat();
    }

    private void HandlePeriodicListLobbies()
    {
        if (_joinedLobby != null) return;
        if (!AuthenticationService.Instance.IsSignedIn) return;

        _listLobbiesTimer -= Time.deltaTime;
        if (_listLobbiesTimer <= 0f)
        {
            _listLobbiesTimer = MaxListLobbyTimer;
            _ = ListLobbies();
        }
    }

    private async void HandleLobbyHeartbeat()
{
    if (_joinedLobby == null || _joinedLobby.HostId != AuthenticationService.Instance.PlayerId) return;

    _heartbeatTimer -= Time.deltaTime;
    if (_heartbeatTimer <= 0f)
    {
        _heartbeatTimer = MaxHeartbeatTimer;
        try
        {
            await LobbyService.Instance.SendHeartbeatPingAsync(_joinedLobby.Id);
            // Debug.Log("Lobby Heartbeat Sent");
        }
        catch (LobbyServiceException e)
        {
            Debug.LogException(e);
        }
    }
}    


    private async Task<Allocation> AllocateRelay()
    {
        try
        {
            return await RelayService.Instance.CreateAllocationAsync(MaxPlayerInLobby - 1);
        }
        catch (RelayServiceException e)
        {
            Debug.LogException(e);
            return null;
        }
    }

    private async Task<string> GetRelayJoinCode(Allocation allocation)
    {
        try
        {
            var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log($"Relay Join Code: {joinCode}");
            return joinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.LogException(e);
            return null;
        }
    }

    private async Task<JoinAllocation> JoinRelay(string joinCode)
    {
        try
        {
            return await RelayService.Instance.JoinAllocationAsync(joinCode);
        }
        catch (RelayServiceException e)
        {
            Debug.LogException(e);
            return null;
        }
    }

    private async Task InitializeUnityAuthentication()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            var options = new InitializationOptions();
            options.SetProfile(Random.Range(1000000, 9999999).ToString());

            await UnityServices.InitializeAsync(options);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    public async void CreateLobby(string lobbyName)
    {
        try
        {
            _joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, MaxPlayerInLobby);
            
            _heartbeatTimer = MaxHeartbeatTimer;

            var allocation = await AllocateRelay();
            string relayJoinCode = await GetRelayJoinCode(allocation);

            await LobbyService.Instance.UpdateLobbyAsync(_joinedLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    { RelayJoinCode, new DataObject(DataObject.VisibilityOptions.Member, relayJoinCode) }
                }
            });

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));

            // 🚨 주의: ProjectSpellGameMultiplayer.cs 도 에러 없이 복사되어 있어야 합니다!
            ProjectSpellGameMultiplayer.Singleton.StartHost();
        }
        catch (LobbyServiceException e)
        {
            Debug.LogException(e);
        }
    }

    public async void QuickJoinLobby()
    {
        try
        {
            _joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            var joinAllocation = await JoinRelay(_joinedLobby.Data[RelayJoinCode].Value);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));
            ProjectSpellGameMultiplayer.Singleton.StartClient();
        }
        catch (LobbyServiceException e)
        {
            if (e.Reason == LobbyExceptionReason.NoOpenLobbies)
            {
                Debug.Log("No Room");
            }

            Debug.LogException(e);
        }
    }

    public async void JoinLobby(string lobbyId)
    {
        try
        {
            _joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
            var joinAllocation = await JoinRelay(_joinedLobby.Data[RelayJoinCode].Value);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));
            ProjectSpellGameMultiplayer.Singleton.StartClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.LogException(e);
        }
    }

    private async Task ListLobbies()
    {
        var options = new QueryLobbiesOptions
        {
            Count = 10,
            Filters = new List<QueryFilter>
            {
                new(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT),
            }
        };

        try
        {
            var response = await LobbyService.Instance.QueryLobbiesAsync(options);
            OnLobbyListChanged?.Invoke(this, new OnLobbyListChangedEventArgs { Lobbies = response.Results });
        }
        catch (LobbyServiceException e)
        {
            Debug.LogException(e);
        }
    }

    public async Task DisableLobbyPublicVisible()
    {
        if (_joinedLobby == null) return;
        try
        {
            _joinedLobby = await LobbyService.Instance.UpdateLobbyAsync(_joinedLobby.Id, new UpdateLobbyOptions { IsPrivate = true });
        }
        catch (Exception e) { Debug.LogException(e); }
    }

    public async Task DeleteLobby()
    {
        if (_joinedLobby != null)
        {
            try
            {
                await LobbyService.Instance.DeleteLobbyAsync(_joinedLobby.Id);
                _joinedLobby = null;
            }
            catch (LobbyServiceException e) { Debug.LogException(e); }
        }
    }


    public async Task LeaveLobby()
    {
        if (_joinedLobby != null)
        {
            try
            {
                string playerId = AuthenticationService.Instance.PlayerId;
                await LobbyService.Instance.RemovePlayerAsync(_joinedLobby.Id, playerId);
                _joinedLobby = null;
            }
            catch (LobbyServiceException e) { Debug.LogException(e); }
        }
    }
    private async void OnApplicationQuit()
    {
        if (_joinedLobby != null && _joinedLobby.HostId == AuthenticationService.Instance.PlayerId)
        {
            try
            {
                await LobbyService.Instance.DeleteLobbyAsync(_joinedLobby.Id);
                Debug.Log("로비 삭제");
            }

            catch (Exception e)
            {
                Debug.LogError($"로비 삭제 실패:");
            }
        }
    }
}