using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ArenaObjectLauncher : NetworkBehaviour
{
    [SerializeField]
    private GameObject _bananaPrefab;

    [SerializeField]
    private LayerMask _groundLayer;

    private bool _sendSpawn;

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _sendSpawn = true;
        }
    }

    private void FixedUpdate()
    {
        if (_sendSpawn)
        {
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, 500f, _groundLayer))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red);
                SpawnObjectRpc(hit.point, Quaternion.identity);
            }

            _sendSpawn = false;
        }
    }

    [Rpc(SendTo.Server)]
    private void SpawnObjectRpc(Vector3 position, Quaternion rotation)
    {
        var instance = Instantiate(_bananaPrefab, position, rotation);

        instance.GetComponent<NetworkObject>().Spawn();
    }
}
