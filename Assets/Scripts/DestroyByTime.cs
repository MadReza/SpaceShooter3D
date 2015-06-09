using UnityEngine;
using System.Collections;

public class DestroyByTime : MonoBehaviour
{
    [SerializeField] private float lifeTime;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
