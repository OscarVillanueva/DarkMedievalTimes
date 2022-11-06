using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager sharedInstance;

    private GameStates currentState;
    private int score;
    public int currentLevel = 1;
    public bool isCurrentLevelClear;

    // Settear y obtener el valor de current state
    public GameStates CurrentState
    {
        get => currentState;
        set
        {
            switch (value)
            {
                case GameStates.inGame:
                    Time.timeScale = 1.0f;
                    break;

                case GameStates.gameOver:
                    Time.timeScale = 0.0f;
                    break;

                case GameStates.pause:
                    Time.timeScale = 0.0f;
                    break;
            }

            currentState = value;
        }
    }

    // setter y obtener el valor de Score
    public int Score {
        get => score;
        set
        {
            score = value;
        }
    }

    // Compartir la instancia de shared instance
    private void Awake()
    {
        if (!sharedInstance) sharedInstance = this;
    }

    public void LevelCompleted()
    {
        currentLevel = currentLevel + 1;
        isCurrentLevelClear = true;

        // TODO: Abrir las puertas 
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && CurrentState != GameStates.gameOver)
        {
            if (CurrentState == GameStates.pause) CurrentState = GameStates.inGame;
            else CurrentState = GameStates.pause;
        }

    }
}
