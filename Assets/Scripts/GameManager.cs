using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager sharedInstance;

    [SerializeField] private int totalOfLevels;

    private BoxCollider2D currentExit;
    private GameStates currentState;
    private int score;
    private int currentLevel = 1;
    private bool isCurrentLevelClear;

    // Settear y obtener el valor de current state
    public GameStates CurrentState
    {
        get => currentState;
        set
        {
            switch (value)
            {
                case GameStates.inGame:
                    UIStatsManager.sharedInstance.ShowPauseCanvas(false);
                    Time.timeScale = 1.0f;
                    break;

                case GameStates.gameOver:
                    Time.timeScale = 0.0f;
                    UIStatsManager.sharedInstance
                        .ShowGameOverCanvas(true, isCurrentLevelClear ? "¡Ganaste!" : "¡Perdiste!");
                    break;

                case GameStates.pause:
                    Time.timeScale = 0.0f;
                    UIStatsManager.sharedInstance.ShowPauseCanvas(true);
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
            UIStatsManager.sharedInstance.UpdateScoreLabel(score);
        }
    }

    // Compartir la instancia de shared instance
    private void Awake()
    {
        if (!sharedInstance) sharedInstance = this;
    }

    private void Start()
    {
        Time.timeScale = 1.0f;
    }

    public void LevelCompleted()
    {
        currentLevel = currentLevel + 1;
        isCurrentLevelClear = true;

        currentExit.enabled = false;

        if (currentLevel > totalOfLevels) Invoke(nameof(WinningGameOver), 2.0f);
        else UIStatsManager.sharedInstance.LevelComplete();

    }

    public void WinningGameOver()
    {
        CurrentState = GameStates.gameOver;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && CurrentState != GameStates.gameOver)
        {
            if (CurrentState == GameStates.pause) CurrentState = GameStates.inGame;
            else CurrentState = GameStates.pause;
        }

    }

    public void StartLevel(BoxCollider2D exit, Transform[] spawners)
    {
        currentExit = exit;
        EnemyManager.sharedInstance.StartLevel(spawners);
        isCurrentLevelClear = false;
    }
}
