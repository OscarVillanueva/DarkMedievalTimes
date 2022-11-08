using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Image spinner;
    [SerializeField] private Canvas mainMenuCanvas;
    [SerializeField] private TMP_Text diffText;

    private int currentDifficulty = 1;
    private bool isLoading = true;

    private void Start()
    {
        mainMenuCanvas.enabled = false;
        StartCoroutine(ShowMainMenuAfter());

        currentDifficulty = PlayerPrefs.GetInt("diff", 1);
        SetTextButton();
    }

    private void Update()
    {
        if (isLoading) spinner.transform.Rotate(new Vector3(0, 30f, 0) * Time.deltaTime * 5);
    }

    public void Play()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    public void ChangeDifficulty()
    {
        currentDifficulty = currentDifficulty + 1;

        if (currentDifficulty > 3) currentDifficulty = 1;

        SetTextButton();
        PlayerPrefs.SetInt("diff", currentDifficulty);
    }

    private void SetTextButton()
    {
        string text = "Dificultad actual: ";

        switch (currentDifficulty)
        {
            case 1:
                text = text + "Fácil";
                break;

            case 2:
                text = text + "Intermedio";
                break;

            case 3:
                text = text + "Difícil";
                break;
        }

        diffText.text = text;
    }

    IEnumerator ShowMainMenuAfter()
    {
        yield return new WaitForSeconds(5);
        isLoading = false;
        mainMenuCanvas.enabled = true;
        
    }
}
