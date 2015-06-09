using UnityEngine;
using System.Collections;

public class EnemyGroupB : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Vector3 spawnZone;
    [SerializeField] private GameObject bonusPrefab;

    private bool spawnFinished = false;
    private int minionsNotKilled = 0;
    private Vector3 lastMinionDeathPosition;
    private GameController gameController;

    // Use this for initialization
    private void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        float waveStartTime = Time.time;
        float waveLimit = 6.0f;
        while (Time.time - waveStartTime < waveLimit)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnZone, Quaternion.identity) as GameObject;
            enemy.GetComponent<EnemyB>().SetParent(gameObject);
            minionsNotKilled++;
            yield return new WaitForSeconds(1.0f);  //Wait Between Waves
        }
        spawnFinished = true;
    }

    private void Update()
    {
        if (!spawnFinished) return;
        
        if (transform.childCount <= 0)
        {
            if (minionsNotKilled == 0)
            {
                gameController.AddScore(1000);
                Instantiate(bonusPrefab, lastMinionDeathPosition, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    public void MinionDied(Vector3 deathPosition)
    {
        minionsNotKilled--;
        lastMinionDeathPosition = deathPosition;
    }
}