using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject gameOver;

    public static GameController gameController;

    private void Start()
    {
        gameController = this;
    }

    public void ShowGameOver()
    {
        gameOver.SetActive(true);
    }
}
