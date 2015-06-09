using UnityEngine;
using System.Collections;

public class EnemyB : MonoBehaviour
{

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject spawnLocation;
    [SerializeField] private float fireRate = 0.5f;
    private float nextFire = 0;
    private AudioSource fireAudioSource;

    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private AudioClip explosionAudioClip;

    [SerializeField] private GameObject smokePrefab;

    private Rigidbody _rigidbody;
    private Animator _animator;
    private EnemyGroupB parentClass;
    private bool dead;
    private GameController gameController;

    // Use this for initialization
    private void Start()
    {
        GetRequiredComponents();
        StartCoroutine(Shoot());
    }

    public void SetParent(GameObject parent)
    {
        transform.parent = parent.transform;
        parentClass = parent.GetComponent<EnemyGroupB>();
    }

    private void GetRequiredComponents()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        fireAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private IEnumerator Shoot()
    {
        while (true)
        {
            if (transform.position.z <= 100) //Z coordiantes
            {
                nextFire = Time.time + fireRate;
                fireAudioSource.Play();

                GameObject BulletObject =
                    Instantiate(bulletPrefab, spawnLocation.transform.position, spawnLocation.transform.rotation) as
                        GameObject;
                BulletObject.tag = "EnemyBullet";
                Bullet bullet = BulletObject.GetComponent<Bullet>();
                bullet.SetDirection(Vector3.back);
            }

            float shootingWaitTime = nextFire - Time.time;
            if (shootingWaitTime <= 0) //While Out Of bounds.
            {
                shootingWaitTime = 1;
            }

            yield return new WaitForSeconds(shootingWaitTime);
        }
    }

    private void OnTriggerEnter(Collider other)
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
        _animator.enabled = false;
        _rigidbody.useGravity = true;
        //_rigidbody.velocity += Vector3.down * 1.0f;
        GameObject smoke = Instantiate(smokePrefab, transform.position, Quaternion.identity) as GameObject;
        smoke.transform.parent = gameObject.transform;
        parentClass.MinionDied(transform.position);
    }

    private void Dead()
    {
        gameController.AddScore(200);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(explosionAudioClip, transform.position);
        //Tell Parent
        Destroy(gameObject);
    }
}
