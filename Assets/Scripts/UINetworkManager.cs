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
    private GameObject GuiCanvas;

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
        NetworkCanvas.SetActive(false);
        GuiCanvas.SetActive(true);
    }
}
