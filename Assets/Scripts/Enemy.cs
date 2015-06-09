using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject spawnLocation;
    [SerializeField] private float fireRate = 5.0f;
    private float nextFire = 0;
    private AudioSource fireAudioSource;

    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private AudioClip explosionAudioClip;

    [SerializeField] private GameObject smokePrefab;

    [SerializeField] private float speed = 20.0f;

    [SerializeField] private float leftEdge = -7.5f;
    [SerializeField] private float rightEdge = 7.40f;
    [SerializeField] private float topEdge = 2.75f;
    [SerializeField] private float bottomEdge = -0.49f;

    private Rigidbody _rigidbody;
    private EnemyGroupA parentClass;
    private bool dead;
    private GameController gameController;

    // Use this for initialization
    private void Start()
    {
        GetRequiredComponents();
        Move();
        StartCoroutine(Shoot());
    }


    private void GetRequiredComponents()
    {
        _rigidbody = GetComponent<Rigidbody>();
        fireAudioSource = GetComponent<AudioSource>();
        parentClass = transform.parent.GetComponent<EnemyGroupA>();
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    private void Update()
    {
    }


    private void Move()
    {
        _rigidbody.velocity = Vector3.back*speed;
    }


    private IEnumerator Shoot()
    {
        while (true)
        {
            if (transform.position.z <= 50)//Z coordiantes
            {
                nextFire = Time.time + fireRate;
                fireAudioSource.Play();

                GameObject BulletObject =
                    Instantiate(bulletPrefab, spawnLocation.transform.position, spawnLocation.transform.rotation) as GameObject;
                BulletObject.tag = "EnemyBullet";
                Bullet bullet = BulletObject.GetComponent<Bullet>();
                bullet.SetDirection(Vector3.back);
            }

            float shootingWaitTime = nextFire - Time.time;
            if (shootingWaitTime <= 0)  //While Out Of bounds.
            {
                shootingWaitTime = 1;
            }

            yield return new WaitForSeconds(shootingWaitTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet" && !dead)
        {
            Dying();

        }
        if (other.gameObject.tag == "Ground")
        {
            Dead();
        }
    }

    private void Dying()
    {
        dead = true;
        _rigidbody.useGravity = true;
        //_rigidbody.velocity += Vector3.down * 1.0f;
        GameObject smoke = Instantiate(smokePrefab, transform.position, Quaternion.identity) as GameObject;
        smoke.transform.parent = gameObject.transform;
        parentClass.MinionDied(transform.position);
    }

    private void Dead()
    {
        gameController.AddScore(100);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(explosionAudioClip, transform.position);
        Destroy(gameObject);
    }
}
