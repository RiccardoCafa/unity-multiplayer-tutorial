using CartoonFX;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Banana : NetworkBehaviour
{
    public GameObject explosionEffectPrefab;

    private void Start()
    {
        if (IsServer)
        {
            StartCoroutine(ExplodeAfterTwoSeconds());
        }
    }

    private IEnumerator ExplodeAfterTwoSeconds()
    {
        yield return new WaitForSeconds(2);

        ExplodeRpc();
    }

    [Rpc(SendTo.Everyone)]
    private void ExplodeRpc()
    {
        var fx = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

        Destroy(fx, 2.0f);

        if (IsServer)
        {
            gameObject.GetComponent<NetworkObject>().Despawn();
        }
    }
}
