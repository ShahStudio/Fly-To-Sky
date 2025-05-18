using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Button playButton;

    [Header("Scene Settings")]
    public string gameSceneName = "GameScene";

    private void Start()
    {
        // Make sure the button is assigned
        if (playButton != null)
        {
            playButton.onClick.AddListener(PlayGame);
        }
        else
        {
            Debug.LogWarning("Play Button is not assigned in the inspector.");
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed!");
    }
}
