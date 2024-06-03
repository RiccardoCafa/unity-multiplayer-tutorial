using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UINetworkManager : MonoBehaviour
{
    [SerializeField]
    private Button JoinBtn;
    [SerializeField]
    private Button HostBtn;
    [SerializeField]
    private Button ServerBtn;

    [SerializeField]
    private GameObject NetwrokCanvas;


    private void Awake()
    {
        JoinBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            CloseCanvas();
        });

        HostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            CloseCanvas();
        });

        ServerBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
            CloseCanvas();
        });
    }

    private void CloseCanvas()
    {
        NetwrokCanvas.SetActive(false);
    }
}
