using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music Sources")]
    public AudioSource lobbyMusicSource;
    public AudioSource gameMusicSource;

    [Header("Sound Effects")]
    public AudioSource sfxSource;
    public AudioClip jumpClip;
    public AudioClip winClip;
    public AudioClip loseClip;

    [Header("Gameplay Points")]
    public GameObject deathPoint;
    public GameObject winPoint;

    [Header("Optional UI Button")]
    public Button retryButton;

    private bool hasWon = false;

    private void Awake()
    {
        // Singleton for current scene only (no DontDestroyOnLoad)
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (retryButton != null)
            retryButton.onClick.AddListener(RestartGame);
    }

    public void PlayJumpSound()
    {
        if (sfxSource && jumpClip)
            sfxSource.PlayOneShot(jumpClip);
    }

    public void CheckCollision(GameObject other)
    {
        if (other == deathPoint)
        {
            if (gameMusicSource && gameMusicSource.isPlaying)
                gameMusicSource.Stop();

            if (sfxSource && loseClip)
                sfxSource.PlayOneShot(loseClip);
        }
        else if (other == winPoint)
        {
            if (!hasWon)
            {
                hasWon = true;

                if (gameMusicSource && gameMusicSource.isPlaying)
                    gameMusicSource.Stop();

                if (sfxSource && winClip)
                    sfxSource.PlayOneShot(winClip);
            }
        }
    }

    public void RestartGame()
    {
        hasWon = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}