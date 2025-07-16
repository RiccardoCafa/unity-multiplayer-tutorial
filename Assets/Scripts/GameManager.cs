using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField]
    private GameObject _playerPrefab;

    public NetworkVariable<int> BananaCounter { get; private set; } = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
            return;
        }
    }

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

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // Check if the UIBananaCounter exists
        FindObjectOfType<UIBananaCounter>().SetBananaCounterTo(BananaCounter.Value);
    }

    public void AddBanana()
    {
        if (IsServer)
        {
            BananaCounter.Value++;
        }
    }
}
