using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuGameController : MonoBehaviour
{
    [SerializeField] private GameObject transcendPrefab;
    [SerializeField] private List<Button> sceneButtons;

    private void Start()
    {
        GameObject transcendGameObject = GameObject.FindGameObjectWithTag("TranscendScene");

        if (transcendGameObject == null)
        {
            transcendGameObject = Instantiate(transcendPrefab);
        }

        sceneButtons[0].onClick.AddListener(() => {Application.LoadLevel("Game");});
    }
}