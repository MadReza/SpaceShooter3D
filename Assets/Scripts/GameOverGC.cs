using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverGC : MonoBehaviour
{
    [SerializeField] private Text scoreText;

    // Use this for initialization
    private void Start()
    {
        GetTranscendData();
    }

    private void GetTranscendData()
    {
        GameObject transcendedGameObject = GameObject.FindGameObjectWithTag("TranscendScene");
        TranscendScene transcendScene = transcendedGameObject.GetComponent<TranscendScene>();

        scoreText.text = transcendScene.Score.ToString();

    }

    public void LoadGameMenu()
    {
        Application.LoadLevel("MainMenu");
    }
}
