using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public GameObject gameOverPanel;

    public TextMeshProUGUI score;
    public TextMeshProUGUI highScore;

    public TextMeshProUGUI gamePlayScore;

    void Awake()
    {
        Instance = this;
    }

    public void LoadScores(int score, int highScore)
    {
        this.score.text = $"{score} Meteres";
        this.highScore.text = $"{highScore} Meteres";
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(1);
        SoundManager.Instance.PlaySFX("Button", 2f);

    }

    public void UpdateScore(int value)
    {
        gamePlayScore.text = value.ToString();
    }

    public void Home()
    {
        SceneManager.LoadScene(0);
        SoundManager.Instance.PlaySFX("Button", 2f);

    }

    public void pause()
    {
        Time.timeScale = 0.0f;
        SoundManager.Instance.PlaySFX("Button", 2f);

    }

    public void unpause()
    {
        Time.timeScale = 1.0f;
        SoundManager.Instance.PlaySFX("Button", 2f);

    }

    public void exit()
    {
        Application.Quit();
        SoundManager.Instance.PlaySFX("Button", 2f);
    }

    public void ControlButton()
    {
        SoundManager.Instance.PlaySFX("Button", 2f);

    }
}