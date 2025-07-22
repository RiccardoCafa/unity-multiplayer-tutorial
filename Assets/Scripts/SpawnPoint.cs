using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public static SpawnPoint Instance;

    [SerializeField]
    private Transform[] Points;

    private int count = 0;

    private void Awake()
    {
        Instance = this;
    }

    public Transform GetPoint()
    {
        if (Points.Length == 0) return null;

        count++;

        if (count >= Points.Length)
            count = 0;
        
        return Points[count];
    }

    private void OnDrawGizmos()
    {
        foreach(var point in Points)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(point.position, 0.5f);
        }
    }
}
