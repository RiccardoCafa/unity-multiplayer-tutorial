using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : NetworkBehaviour
{
    [SerializeField]
    private string sceneName;

    [SerializeField]
    private Scene currentScene;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            NetworkManager.SceneManager.OnSceneEvent += SceneManager_OnSceneEvent;
        }
    }

    public void GoToGame()
    {
        sceneName = "Game";
        NetworkManager.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    private void SceneManager_OnSceneEvent(SceneEvent sceneEvent)
    {
        var clientOrServer = sceneEvent.ClientId == NetworkManager.ServerClientId ? "server" : "client";

        switch (sceneEvent.SceneEventType)
        {
            case SceneEventType.LoadComplete:
                {
                    if (sceneEvent.ClientId == NetworkManager.ServerClientId)
                    {
                        currentScene = sceneEvent.Scene;
                    }
                    Debug.Log($"Loaded the {sceneEvent.SceneName} scene on " +
                        $"{clientOrServer}-({sceneEvent.ClientId}).");
                    break;
                }
            case SceneEventType.UnloadComplete:
                {
                    Debug.Log($"Unloaded the {sceneEvent.SceneName} scene on " +
                        $"{clientOrServer}-({sceneEvent.ClientId}).");
                    break;
                }
            case SceneEventType.LoadEventCompleted:
            case SceneEventType.UnloadEventCompleted:
                {
                    var loadUnload = sceneEvent.SceneEventType == SceneEventType.LoadEventCompleted ? "Load" : "Unload";
                    Debug.Log($"{loadUnload} event completed for the following client " +
                        $"identifiers:({sceneEvent.ClientsThatCompleted})");
                    if (sceneEvent.ClientsThatTimedOut.Count > 0)
                    {
                        Debug.LogWarning($"{loadUnload} event timed out for the following client " +
                            $"identifiers:({sceneEvent.ClientsThatTimedOut})");
                    }
                    break;
                }
        }
    }

    public void UnloadScene()
    {
        if (!IsServer || !IsSpawned || !currentScene.IsValid() || !currentScene.isLoaded)
        {
            return;
        }

        // Unload the scene
        var status = NetworkManager.SceneManager.UnloadScene(currentScene);

        if (status != SceneEventProgressStatus.Started)
        {
            Debug.LogWarning($"Failed to unload {sceneName} with" +
                $" a {nameof(SceneEventProgressStatus)}: {status}");
        }
    }
}
