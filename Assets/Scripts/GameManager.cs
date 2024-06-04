using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField]
    private GameObject _playerPrefab;

    void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += Singleton_OnClientConnectedCallback;
    }

    private void Singleton_OnClientConnectedCallback(ulong obj)
    {
        if (IsServer)
        {
            var instance = Instantiate(_playerPrefab);

            var netObj = instance.GetComponent<NetworkObject>();

            netObj.SpawnAsPlayerObject(obj);
        }
    }
}
