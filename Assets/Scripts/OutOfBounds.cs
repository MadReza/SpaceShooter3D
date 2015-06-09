using UnityEngine;
using System.Collections;

public class OutOfBounds : MonoBehaviour
{
    [SerializeField] private float lowerZLimit = -6.0f;
    [SerializeField] private float higherZLimit = 160.0f;

    private void Update()
    {
        if (transform.position.z <= lowerZLimit || transform.position.z >= 160)
        {
            Destroy(gameObject);
        }
    }
}
