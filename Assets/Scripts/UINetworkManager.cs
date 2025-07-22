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
    private GameObject NetworkCanvas;

    [SerializeField]
    private GameObject StartCanvas;

    [SerializeField]
    private Button StartBtn;

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

        StartBtn.onClick.AddListener(() =>
        {
            FindObjectOfType<MenuManager>().GoToGame();
        });
    }

    private void CloseCanvas()
    {
        NetworkCanvas.SetActive(false);
        StartCanvas.SetActive(true);
    }
}
