using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject Boss;
    [SerializeField] private GameObject BossPanel;
    [SerializeField] private Text bossHP;
    [SerializeField] private GameObject EnemyWaveAPrefab;
    [SerializeField] private GameObject EnemyWaveBPrefab;
    [SerializeField]float waveLimit = 45.0f;
    [SerializeField] private Vector3 spawnZone;
    [SerializeField] private List<Image> lives;

    [SerializeField] private Text scoreText;
    [SerializeField] private int score = 0;

    private Player player;
    private bool gameOver;
    private TranscendScene transcendScene;

    public Text GetBossHPText()
    {
        BossPanel.SetActive(true);
        return bossHP;
    }

    // Use this for initialization
    private void Start()
    {
        BossPanel.SetActive(false);
        GetTranscendedScene();
        UpdateScore();
        GetPlayerClass();
        SetLives();
        StartCoroutine(SpawnWaves());
    }

    private void GetTranscendedScene()
    {
        GameObject transcendedGameObject = GameObject.FindGameObjectWithTag("TranscendScene");
        transcendScene = transcendedGameObject.GetComponent<TranscendScene>();
    }

    private void GetPlayerClass()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void SetLives()
    {
        foreach (Image image in lives)
        {
            Color removeAlpha = image.color;
            removeAlpha.a = 0;
            image.color = removeAlpha;
        }

        for (int i = 0; i < player.PlayerUpgrade; i++)
        {
            Color addAlpha = lives[i].color;
            addAlpha.a = 255;
            lives[i].color = addAlpha;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        SetLives();

        if (player.PlayerUpgrade == 0 && !gameOver)
        {
            StartCoroutine(SwitchToGameOverScene());
        }
    }

    private IEnumerator SwitchToGameOverScene()
    {
        yield return new WaitForSeconds(0.5f); //wait for TSpaceShip To Explode
        transcendScene.Score = score;
        Application.LoadLevel("GameOver");
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(7.0f); //Wait for Take off from platform
        float waveStartTime = Time.time;
        while (Time.time - waveStartTime < waveLimit)
        {
            int randomEnemySelector = Random.Range(0, 100) % 2;
            if (randomEnemySelector == 0)
            {
                Vector3 spawnA = spawnZone + new Vector3(0, -1 + Random.Range(1, 4), 0);
                Instantiate(EnemyWaveAPrefab, spawnA, Quaternion.identity);
            }
            else
            {
                Instantiate(EnemyWaveBPrefab, new Vector3(0,0,0), Quaternion.identity);
            }
            yield return new WaitForSeconds(6.0f);  //Wait Between Waves
        }
        Instantiate(Boss);
    }

    public void AddScore(int points)
    {
        score += points;
        if (score < 0)
            score = 0;
        UpdateScore();
    }

    private void UpdateScore()
    {
        scoreText.text = score.ToString();
    }

    public void BossKilled()
    {
        AddScore(10000);
        StartCoroutine(SwitchToGameOverScene());
    }
}
