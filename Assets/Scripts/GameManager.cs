using System.Collections;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    [SerializeField]
    private GameObject _playerPrefab;

    public NetworkVariable<int> BananaCounter { get; private set; } = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public static GameManager Instance { get; private set; }

    [SerializeField]
    private Scene CurrentMap { get; set; }

    private string[] Maps;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Maps = new string[2]
            {
                "Map_01",
                "Map_02"
            };
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Log(string message)
    {
        print($"[{OwnerClientId}][{(IsServer ? "Server" : "Client")}][Game Manager] {message}");
    }

    private IEnumerator DelayedPlayerSpawn(ulong clientId)
    {
        for (int i = 0; i < 10; i++) yield return null;

        Log($"Spawning player Client-({clientId})");

        var instance = Instantiate(_playerPrefab);
        var spawn = SpawnPoint.Instance.GetPoint();

        Log($"Spawn point {spawn.position}");

        instance.transform.SetPositionAndRotation(spawn.position, spawn.rotation);

        Log("Spawning as Player Object");

        var netObj = instance.GetComponent<NetworkObject>();
        netObj.SpawnAsPlayerObject(clientId);

        EnableAllCharactersRpc();
    }

    [Rpc(SendTo.Everyone)]
    private void EnableAllCharactersRpc()
    {
        var characters = FindObjectsOfType<CharacterController>();

        foreach (var character in characters)
        {
            character.enabled = true;
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        Log("On Network Spawn");

        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnSceneEvent += SceneManager_OnSceneEvent;
        }

        Log("Looking for UI Banana Counter");
        var bananCounter = FindObjectOfType<UIBananaCounter>();
        if (bananCounter != null)
        {
            Log("Initializing Banana Counter...");
            bananCounter.SetBananaCounterTo(BananaCounter.Value);
        }
    }

    private void LoadComplete(SceneEvent sceneEvent)
    {
        if (sceneEvent.ClientId == NetworkManager.ServerClientId)
        {
            CurrentMap = sceneEvent.Scene;
        }

        if (Maps.Contains(sceneEvent.SceneName) && !NetworkManager.ConnectedClients[sceneEvent.ClientId].PlayerObject)
        {
            Log($"Player {sceneEvent.ClientId} is not spawned");
            StartCoroutine(DelayedPlayerSpawn(sceneEvent.ClientId));
        }

        if (sceneEvent.SceneName == "Map_02")
        {
            Log("This is Map_02");

            foreach (var player in NetworkManager.ConnectedClients)
            {
                var point = SpawnPoint.Instance.GetPoint();

                Log($"Setting new spawn point for players at {point}");

                player.Value.PlayerObject.transform.SetPositionAndRotation(point.position, point.rotation);
            }
        }
    }

    private void LoadEventCompleted(SceneEvent sceneEvent)
    {
        if (sceneEvent.SceneName == "Game")
        {
            Log("Is a Game scene, loading first map Map_01");

            NetworkManager.SceneManager.LoadScene("Map_01", LoadSceneMode.Additive);
        }
    }

    private void UnloadComplete(SceneEvent sceneEvent)
    {
        if (sceneEvent.SceneName == "Map_01")
        {
            Log("Unloaded Map_01, loading Map_02...");

            var status = NetworkManager.SceneManager.LoadScene("Map_02", LoadSceneMode.Additive);

            if (status != SceneEventProgressStatus.Started)
            {
                Log($"Map_02 failed to start loading. Status: {status}");
            }
            else
            {
                Log("Map_02 load started successfully.");
            }
        }

        if (sceneEvent.ClientId == NetworkManager.ServerClientId)
        {
            Log("Setting current scene object reference");
            CurrentMap = default;
        }
    }

    private void SceneManager_OnSceneEvent(SceneEvent sceneEvent)
    {
        switch (sceneEvent.SceneEventType)
        {
            case SceneEventType.LoadComplete:
                LoadComplete(sceneEvent);
                break;
            case SceneEventType.LoadEventCompleted:
                LoadEventCompleted(sceneEvent);
                break;
            case SceneEventType.UnloadEventCompleted:
                UnloadComplete(sceneEvent);
                break;
        }
    }

    public void AddBanana()
    {
        if (IsServer)
        {
            BananaCounter.Value++;

            if (BananaCounter.Value == 5 && CurrentMap.IsValid())
            {
                NetworkManager.SceneManager.UnloadScene(CurrentMap);
            }
        }
    }

    private new void OnDestroy()
    {
        if (IsServer)
            NetworkManager.SceneManager.OnSceneEvent -= SceneManager_OnSceneEvent;
    }
}
