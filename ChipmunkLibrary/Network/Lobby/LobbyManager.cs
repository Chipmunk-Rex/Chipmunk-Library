using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
enum EnumLobbyEventType
{
    OnJoinLobby, OnKickedFromLobby, OnLobbyChanged, OnLobbyDataChanged, OnDataChanged, OnLobbyDeleted,
    OnDataAdded,
    OnDataRemoved
}
[DisallowMultipleComponent]
public class LobbyManager : MonoSingleton<LobbyManager>
{
    [Header("LobbySetting")]
    [SerializeField] int minPlayer;
    [SerializeField] int maxPlayer;
    [Space(20)]
    [Header("LobbyEvent")]

    [SerializeField] EventBus<EnumLobbyEventType> lobbyEventBus = new EventBus<EnumLobbyEventType>();
    [SerializeField] public UnityEvent onGameStart;
    [SerializeField] public UnityEvent onJoinLobby;
    private Player player = new Player
    {
        Data = new Dictionary<string, PlayerDataObject>
        {
            {"Name", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, "I'm Player")}
        }
    };
    public Player Player
    {
        get => player;
    }
    public Lobby currentLobby { get; private set; }
    public LobbyEventCallbacks lobbyEventCallbacks { get; private set; } = new LobbyEventCallbacks();

    protected override void Awake()
    {
        base.Awake();
        onGameStart.AddListener(() => { SceneManager.LoadScene("InGameScene"); Debug.Log("ming");});
    }

    [ContextMenu("로비 생성")]
    public async void CreateLobby(string lobbyName = "lobby", int maxPlayer = 5, string lobbyDesc = null)
    {
        if (currentLobby != null)
        {
            Debug.LogError($"LobbyManager : 이미 로비에 참여중입니다.");
            return;
        }
        if (maxPlayer > this.maxPlayer)
        {
            Debug.LogError($"LobbyManager : 로비의 최대 플레이어 수가 너무 큽니다. \n 최대 플레이어 수 {this.maxPlayer} \n 입력된 플레이어 수 {maxPlayer}");
            return;
        }
        if (maxPlayer < this.minPlayer)
        {
            Debug.LogError($"LobbyManager : 로비의 최대 플레이어 수가 너무 적습니다. \n 최소 플레이어 수 {this.minPlayer} \n 입력된 플레이어 수 {maxPlayer}");
            return;
        }

        try
        {
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = false,
                Player = Player,
                Data = new Dictionary<string, DataObject>{
                        // { "JoinCode", new DataObject(DataObject.VisibilityOptions.Member, ) }
                        { "Desc", new DataObject(DataObject.VisibilityOptions.Public, lobbyDesc) },
                        { "GameMode", new DataObject(DataObject.VisibilityOptions.Public, EnumGameMode.Default.ToString()) },
                        { "RelayJoinCode", new DataObject(DataObject.VisibilityOptions.Member, null)}
                }
            };

            Lobby lobby = await Lobbies.Instance.CreateLobbyAsync(lobbyName, maxPlayer, createLobbyOptions);
            currentLobby = lobby;
            StartCoroutine(LobbyHeartBeat());
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    public IEnumerator LobbyHeartBeat()
    {
        WaitForSecondsRealtime timer = new WaitForSecondsRealtime(20f);
        while (currentLobby != null)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(currentLobby.Id);
            yield return timer;
        }
    }
    public async void StartLobby()
    {
        string joinCode = await RelayManager.Instance.Host(10);
        bool isHostStarted = NetworkManager.Singleton.StartHost();
        Debug.Log(isHostStarted);
        if (isHostStarted)
            onGameStart?.Invoke();


        if (joinCode == null)
            throw new Exception("JoinCode가 없음");

        UpdateLobbyOptions updateLobbyOptions = new UpdateLobbyOptions()
        {
            Data = new Dictionary<string, DataObject>{
                { "RelayJoinCode", new DataObject(DataObject.VisibilityOptions.Member, joinCode) }
            }
        };
        try
        {
            await Lobbies.Instance.UpdateLobbyAsync(currentLobby.Id, updateLobbyOptions);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log($"LobbyManager : {e}");
        }
    }
    [ContextMenu("로비 리스트")]
    public async Task<List<Lobby>> ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Count = 10,
                Filters = new List<QueryFilter>{
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT),
                }
            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);

            List<Lobby> lobbyList = new();
            int Count = 0;
            foreach (Lobby lobby in queryResponse.Results)
            {
                Count += 1;
                if (Count == 100)
                    return null;
                lobbyList.Add(lobby);
            }
            Debug.Log($"LobbyManager : 감지된 로비 개수{lobbyList.Count}");
            return lobbyList;
        }
        catch
        {

        }
        return null;
    }

    #region LobbyCallbacks
    ILobbyEvents lobbyEvents;
    public async Task<ILobbyEvents> SubscribeLobbyCallback()
    {
        LobbyEventCallbacks lobbyEventCallbacks = new LobbyEventCallbacks();
        lobbyEventCallbacks.KickedFromLobby += () => lobbyEventBus.Invoke(EnumLobbyEventType.OnKickedFromLobby);
        lobbyEventCallbacks.LobbyChanged += v => lobbyEventBus.Invoke(EnumLobbyEventType.OnLobbyChanged);
        lobbyEventCallbacks.LobbyDeleted += () => lobbyEventBus.Invoke(EnumLobbyEventType.OnLobbyDeleted);
        lobbyEventCallbacks.DataAdded += v => lobbyEventBus.Invoke(EnumLobbyEventType.OnDataAdded);
        lobbyEventCallbacks.DataChanged += v => lobbyEventBus.Invoke(EnumLobbyEventType.OnDataChanged);
        lobbyEventCallbacks.DataChanged += OnLobbyDataChanged;
        lobbyEventCallbacks.DataRemoved += v => lobbyEventBus.Invoke(EnumLobbyEventType.OnDataRemoved);

        ILobbyEvents lobbyEvents = await Lobbies.Instance.SubscribeToLobbyEventsAsync(currentLobby.Id, lobbyEventCallbacks);
        return lobbyEvents;
    }

    private async void OnLobbyDataChanged(Dictionary<string, ChangedOrRemovedLobbyValue<DataObject>> dictionary)
    {
        string dataName = "RelayJoinCode";
        if (dictionary.ContainsKey(dataName))
        {
            bool isClientConnected = await RelayManager.Instance.Client(dictionary[dataName].Value.Value);
            if (isClientConnected)
            {
                bool isClientStarted = NetworkManager.Singleton.StartClient();
                if (isClientStarted)
                    onGameStart?.Invoke();
            }

        }
    }
    public async Task UnsubscribeLobbyEventAsync()
    {
        await lobbyEvents?.UnsubscribeAsync();
    }
    #endregion

    #region JoinLobby Region
    private async void OnJoinLobby(Lobby currentLobby)
    {
        this.currentLobby = currentLobby;
        await SubscribeLobbyCallback();
        onJoinLobby?.Invoke();
        // lobbyEventCallbacks.LobbyChanged += Lobby
    }
    public async Task JoinLobbyById(string lobbyId)
    {
        JoinLobbyByIdOptions joinLobbyByCodeOptions = new JoinLobbyByIdOptions
        {
            Player = Player
        };
        try
        {
            Lobby lobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobbyId, joinLobbyByCodeOptions);
            OnJoinLobby(lobby);
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogError($"Failed to join lobby: {ex.Message}");
        }
    }
    public async void JoinLobbyByCode(string joinCode)
    {
        JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
        {
            Player = Player
        };
        try
        {
            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(joinCode, joinLobbyByCodeOptions);
            OnJoinLobby(lobby);
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogError($"Failed to join lobby: {ex.Message}");
        }
    }

    #endregion
    #region LeaveRobby Region
    public async void LeaveLobby()
    {
        Lobby currentLobby = await GetCurrentLobby();
        if (currentLobby == null)
        {
            Debug.LogError($"LobbyManager : 로비에 참여중이 아닙니다.");
            currentLobby = null;
            return;
        }
        LeaveLobby(currentLobby.Id);
    }
    public async void LeaveLobby(string lobbyId)
    {
        try
        {
            await UnsubscribeLobbyEventAsync();
            await LobbyService.Instance.RemovePlayerAsync(lobbyId, AuthenticationService.Instance.PlayerId);
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError($"LobbyManager : {e}");
        }
    }
    #endregion
    public async Task<Lobby> GetCurrentLobby()
    {
        if (currentLobby == null)
            Debug.LogError("LobbyManager : currentLobby Is Null");
        else
            currentLobby = await Lobbies.Instance.GetLobbyAsync(currentLobby.Id);
        return currentLobby;
    }
}
