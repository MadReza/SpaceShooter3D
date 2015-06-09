using UnityEngine;
using System.Collections;

public class Bonus : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody _rigidbody;

    [SerializeField] private float leftBorder;
    [SerializeField] private float rightBorder;
    [SerializeField] private float topBorder;
    [SerializeField] private float botBorder;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.velocity = Vector3.back*speed;
    }

    private void Update()
    {
        Clamp();
    }

    private void Clamp()
    {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, leftBorder, rightBorder),
            Mathf.Clamp(transform.position.y, botBorder, topBorder),
            transform.position.z
            );
    }
}
