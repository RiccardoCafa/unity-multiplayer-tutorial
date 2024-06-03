using Unity.Netcode;
using UnityEngine;

public class StartAsClientDebug : MonoBehaviour
{
#if !UNITY_EDITOR
    void Start()
    {
        NetworkManager.Singleton.StartClient();
    }
#endif
}
