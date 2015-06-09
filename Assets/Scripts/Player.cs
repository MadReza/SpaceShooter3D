using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] private float leftBorder;
    [SerializeField] private float rightBorder;
    [SerializeField] private float topBorder;
    [SerializeField] private float botBorder;
    [SerializeField] private float zLocation;

    [SerializeField] private float speed = 5.0f;

    public int PlayerUpgrade = 1; //Also Life. Death at 0
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private List<GameObject> spawnLocations; //0 center, 1 left, 2 right.
    [SerializeField] private float fireRate = 0.25f;
    private float nextFire = 0;
    private AudioSource fireAudioSource;

    private Rigidbody _rigidbody;
    private Shake cameraShake;
    private Renderer _renderer;

    [SerializeField] private GameObject explosion;
    [SerializeField] private AudioClip explosionAudioClip;
    private bool invincible = false;
    private float invincibleStart;
    [SerializeField] private float invincibleLimite = 3.0f;

    // Use this for initialization
    private void Start()
    {
        GetObjectComponents();
    }

    private void GetObjectComponents()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        fireAudioSource = GetComponent<AudioSource>();

        cameraShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Shake>();
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
        Clamp();
        Shoot();

        if (invincible)
        {
            ChangeLayer("Invincible");
            InitiateInvicibility();
        }
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, vertical, 0.0f);
        _rigidbody.velocity = direction * speed;
    }

    private void Clamp()
    {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, leftBorder, rightBorder),
            Mathf.Clamp(transform.position.y, botBorder, topBorder),
            zLocation
            );
    }

    private void Shoot()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            fireAudioSource.Play();
            switch (PlayerUpgrade)
            {
                case 1: //Spawn Middle
                    MidStraightShot();
                    break;
                case 2: //Spawn Sides
                    TwoMidStriaghtShot();
                    break;
                case 3: //Spawn All 3, Angle the 2 sides
                    MidStraightShot();
                    TwoSideAngleShot();
                    break;
            }
        }
    }

    private void MidStraightShot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, spawnLocations[0].transform.position, spawnLocations[0].transform.rotation) as GameObject;
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.SetDirection(Vector3.forward);
    }

    private void TwoMidStriaghtShot()
    {
        GameObject leftBulletObj = Instantiate(bulletPrefab, spawnLocations[1].transform.position, spawnLocations[1].transform.rotation) as GameObject;
        GameObject rightBulletObj = Instantiate(bulletPrefab, spawnLocations[2].transform.position, spawnLocations[2].transform.rotation) as GameObject;
        Bullet leftBullet = leftBulletObj.GetComponent<Bullet>();
        Bullet rightBullet = rightBulletObj.GetComponent<Bullet>();
        leftBullet.SetDirection(Vector3.forward);
        rightBullet.SetDirection(Vector3.forward);
    }

    private void TwoSideAngleShot()
    {
        GameObject leftBulletObject = Instantiate(bulletPrefab, spawnLocations[1].transform.position, spawnLocations[1].transform.rotation) as GameObject;
        GameObject rightBulletObject = Instantiate(bulletPrefab, spawnLocations[2].transform.position, spawnLocations[2].transform.rotation) as GameObject;
        Bullet leftBullet = leftBulletObject.GetComponent<Bullet>();
        Bullet rightBullet = rightBulletObject.GetComponent<Bullet>();
        leftBullet.SetDirection(Vector3.forward - new Vector3(0.25f, 0));
        rightBullet.SetDirection(Vector3.forward + new Vector3(0.25f, 0));
    }

    void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;
        if (tag == "Ground")
        {
            cameraShake.ShakeTime = 5;
            _rigidbody.AddExplosionForce(150, transform.position + Vector3.down*2,15.0f, 15.0f, ForceMode.Impulse);
            Damaged();
            return;
        }
        if (tag == "Bonus")
        {
            Bonus();
        }
        else if (tag == "EnemyBullet" || tag == "Enemy")
        {
            Damaged();
        }
        Destroy(other.gameObject);
    }

    private void Damaged()
    {
        PlayerUpgrade--;
        Instantiate(explosion, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(explosionAudioClip, transform.position);
       // SpawnPlayer();
        //CheckStatus();
        invincible = true;
        invincibleStart = Time.time;
    }

    private void Bonus()
    {
        PlayerUpgrade++;
        if (PlayerUpgrade > 3)
        {
            PlayerUpgrade = 3;
            //TODO: add bonus points....meh
        }
    }

    private void InitiateInvicibility()
    {
        float timeRemaining = invincibleLimite - (Time.time - invincibleStart);
        Color colorValues = _renderer.material.color;


        if (timeRemaining <= 0)
        {
            colorValues.a = 255;
            _renderer.material.color = colorValues;
            invincible = false;
            ChangeLayer("Player");
            return;
        }

        var lerp = Mathf.PingPong(Time.time, invincibleLimite) / invincibleLimite;
        colorValues.a = Mathf.Lerp(0.0f, 1.0f, lerp);
        _renderer.material.color = colorValues;

    }

    private void ChangeLayer(string name)
    {
        if (gameObject.layer != LayerMask.NameToLayer(name))
        {
            gameObject.layer = LayerMask.NameToLayer(name);
        }
    }
}
