using Cinemachine;
using StarterAssets;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : NetworkBehaviour
{
    [SerializeField]
    private ThirdPersonController _tpsController;

    [SerializeField]
    private GameObject _playerCameraFollowPrefab;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            var instance = Instantiate(_playerCameraFollowPrefab);

            var virtualCam = instance.GetComponent<CinemachineVirtualCamera>();

            if (virtualCam != null )
            {
                virtualCam.Follow = _tpsController.CinemachineCameraTarget.transform;
            }
        } else
        {
            Destroy(_tpsController);
            Destroy(GetComponent<StarterAssetsInputs>());
            Destroy(GetComponent<PlayerInput>());
        }
    }
}
