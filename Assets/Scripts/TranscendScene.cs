using System;
using UnityEngine;
using System.Collections;

public class TranscendScene : MonoBehaviour
{
    private int score = 0;

    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeScene(String name)
    {
        Application.LoadLevel(name);
    }
}