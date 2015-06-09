using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public float speed = 25.0f;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void SetDirection(Vector3 direction)
    {
        _rigidbody.velocity = direction * speed;
    }

    private void Update()
    {
        OutOfBounds();   
    }

    private void OutOfBounds()
    {
        if (transform.position.z <= -6 || transform.position.z >= 92)
        {
            Destroy(gameObject);
        }
    }

}
