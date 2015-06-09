using UnityEngine;
using System.Collections;

public class Shake : MonoBehaviour
{
    public float ShakeTime = 0.0f;
    [SerializeField] private float decreaseRate = 5.0f;
    [SerializeField] private float magnitude = 0.7f;

    private Vector3 originalPosition;

    void OnEnable()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        if (ShakeTime > 0)
        {
            transform.position = originalPosition + Random.insideUnitSphere*magnitude;
            ShakeTime -= Time.deltaTime*decreaseRate;
        }
        else
        {
            transform.position = originalPosition;
        }
    }
}
