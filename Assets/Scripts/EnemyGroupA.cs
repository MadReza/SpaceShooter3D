using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyGroupA : MonoBehaviour
{
    public const float leftEdge = -0.5f;
    public const float rightEdge = 0.40f;
    public const float topEdge = 2.75f;
    public const float bottomEdge = -0.49f;

    [SerializeField] private float spawnZ = 150;
    [SerializeField] private GameObject bonusPrefab;

    private int minionsNotKilled = 5;
    private Vector3 lastMinionDeathPosition;
    private GameController gameController;

    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();        
    }

    void Update()
    {
        if (transform.childCount <= 0)
        {
            if (minionsNotKilled == 0)
            {
                gameController.AddScore(500);
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
