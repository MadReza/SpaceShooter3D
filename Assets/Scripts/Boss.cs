using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Boss : MonoBehaviour
{
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject weakPointOne;
    [SerializeField] private GameObject weakPointTwo;
    [SerializeField] private List<GameObject> SpawnPoints;
    [SerializeField] private GameObject smokePrefab;

    private float nextFire = 0;
    [SerializeField] private float fireRate = 2.0f;
    [SerializeField] private GameObject bulletPrefab;
    private AudioSource fireAudioSource;

    private int shieldRemainder = 2;
    public bool TakeDamage = false;

    private int health = 30;
    public bool died = false;
    private Rigidbody _rigidbody;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private AudioClip explosionAudioClip;
    private GameController gameController;

    private Animator _animator;
    private Text bossHpText;

    private void Start()
    {
        GetRequiredComponents();
        StartCoroutine(Shoot());
    }

    private void GetRequiredComponents()
    {
        fireAudioSource = GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody>();
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        _animator = GetComponent<Animator>();
        bossHpText = gameController.GetBossHPText();
    }

    private IEnumerator Shoot()
    {
        while (true)
        {
            nextFire = Time.time + fireRate;
                fireAudioSource.Play();

            int random = Random.Range(1, 4);

            GameObject BulletObject =
                Instantiate(bulletPrefab, SpawnPoints[random].transform.position, Quaternion.identity) as GameObject;
            BulletObject.tag = "EnemyBullet";
            Bullet bullet = BulletObject.GetComponent<Bullet>();
            bullet.SetDirection(Vector3.back);

            float shootingWaitTime = nextFire - Time.time;
            if (shootingWaitTime <= 0)  //While Out Of bounds.
            {
                shootingWaitTime = 1;
            }

            yield return new WaitForSeconds(shootingWaitTime);
        }
    }

    public void WeakPointDown()
    {
        shieldRemainder--;
        if (shieldRemainder == 0)
        {
            shield.transform.position += new Vector3(0,0,1);
            TakeDamage = true;
            Debug.Log("TAKE DAMAGE");
            StartCoroutine(RegenerateShield());
        }
    }

    private IEnumerator RegenerateShield()
    {
        yield return new WaitForSeconds(5.0f);
        weakPointOne.GetComponent<BossWeakPoints>().health = 2;
        weakPointTwo.GetComponent<BossWeakPoints>().health = 2;
        shield.transform.position += new Vector3(0,0,-1);
        TakeDamage = false;
        shieldRemainder = 2;
        Debug.Log("Shield Up");
    }

    public void GotHit()
    {
        Debug.Log("Health: " + health);
        health--;
        bossHpText.text = health.ToString();
        if (health <= 0)
        {
            Dying();
        }
    }

    private void Dying()
    {
        died = true;
        _animator.enabled = false;
        _rigidbody.useGravity = true;
        //_rigidbody.velocity += Vector3.down * 1.0f;
        GameObject smoke = Instantiate(smokePrefab, transform.position, Quaternion.identity) as GameObject;
        smoke.transform.parent = gameObject.transform;
    }

    private void Dead()
    {
        gameController.BossKilled();
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(explosionAudioClip, transform.position);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground" && died)
        {
            Dead();
        }
    }
}
