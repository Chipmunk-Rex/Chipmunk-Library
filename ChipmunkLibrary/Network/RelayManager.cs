using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum RelayType
{
    Host, Client, None
}

public class RelayManager : MonoSingleton<RelayManager>
{
    [field: SerializeField] public string joinCode { get; private set; }
    [SerializeField] string sceneName = "MultiGame";
    public RelayType relayType = RelayType.None;
    private async void Start()
    {
        InitializationOptions initializationOptions = new InitializationOptions();
        initializationOptions.SetProfile($"UUID_{UnityEngine.Random.Range(100000000, 999999999)}");
        await UnityServices.InitializeAsync(initializationOptions);

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        Debug.Log(AuthenticationService.Instance.Profile);

    }
    public async void StartHost(int maxTryConnect)
    {
        if (await Host(maxTryConnect) != null)
        {
            SceneManager.activeSceneChanged += StartNetwork;
            SceneManager.LoadScene("MultiGame");
        }
    }
    public async void StartClient(string joinCode)
    {
        if (await Client(joinCode))
        {
            SceneManager.activeSceneChanged += StartNetwork;
            SceneManager.LoadScene("MultiGame");
        }
    }

    private void StartNetwork(Scene scene, Scene scene2)
    {
        if (scene2.name != "MultiGame") return;

        if (relayType == RelayType.Host)
            NetworkManager.Singleton.StartHost();
        if (relayType == RelayType.Client)
            NetworkManager.Singleton.StartClient();

        SceneManager.activeSceneChanged -= StartNetwork;
    }
    public async Task<string> Host(int maxTryConnect)
    {
        if (relayType != RelayType.None) return null;
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxTryConnect);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));

        joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        if (joinCode == null) return null;
        relayType = RelayType.Host;
        return joinCode;
    }
    public async Task<bool> Client(string joinCode)
    {
        if (relayType != RelayType.None) return false;
        try
        {
            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));
            relayType = RelayType.Client;
            return true;
        }
        catch
        {
            relayType = RelayType.None;
            return false;
        }
    }
}
