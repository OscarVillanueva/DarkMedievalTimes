using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIStatsManager : MonoBehaviour
{
    public static UIStatsManager sharedInstance;

    [SerializeField] private TMP_Text scoreLabel;
    [SerializeField] private TMP_Text lifesLabel;
    [SerializeField] private Image levelCompleteLabel;
    [SerializeField] private Canvas pauseCanvas;
    [SerializeField] private Canvas statsCanvas;
    [SerializeField] private Canvas gameOverCanvas;

    private void Awake()
    {
        if (!sharedInstance) sharedInstance = this;
    }

    private void Start()
    {
        scoreLabel.text = "0";
        lifesLabel.text = "3";
        pauseCanvas.enabled = false;
        gameOverCanvas.enabled = false;
    }

    public void UpdateScoreLabel(int score)
    {
        scoreLabel.text = score.ToString();
    }

    public void UpdateLifesLabel(int lifes)
    {
        lifesLabel.text = lifes.ToString();
    }

    public void LevelComplete()
    {
        StartCoroutine(ShowLevelCompleteLabel());
    }

    public void ShowPauseCanvas(bool show)
    {
        pauseCanvas.enabled = show;
        statsCanvas.enabled = !show;
    }

    public void ShowGameOverCanvas(bool show, string title)
    {
        gameOverCanvas.GetComponent<GameOverController>().SetMainText(title);
        gameOverCanvas.enabled = show;
        statsCanvas.enabled = !show;
    }

    IEnumerator ShowLevelCompleteLabel()
    {
        yield return new WaitForSeconds(1);

        levelCompleteLabel.enabled = true;
        levelCompleteLabel.GetComponentsInChildren<TMP_Text>()[0].enabled = true;

        yield return new WaitForSeconds(3);

        levelCompleteLabel.enabled = false;
        levelCompleteLabel.GetComponentsInChildren<TMP_Text>()[0].enabled = false;
    }
}
