using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    [Header("Game Objects")]
    public Transform startPoint;
    public Transform endPoint;
    public GameObject playerPrefab;
    [Tooltip("Assign your invisible death plane here")]
    public GameObject deathPlane;

    [Header("UI Elements")]
    public GameObject winUI;
    public GameObject loseUI;
    public Button retryButton;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI heightText;

    [Header("Settings")]
    public float loseDistanceThreshold = 50f;
    public float winDelay = 5f;
    public float maxTimeLimit = 60f;

    private GameObject playerInstance;
    private float timer;
    private bool gameRunning = true;
    private float startY;

    void Start()
    {
        // Check if a Player already exists
        playerInstance = GameObject.FindGameObjectWithTag("Player");

        if (playerInstance == null)
        {
            // If not, spawn player at start point
            playerInstance = Instantiate(playerPrefab, startPoint.position, startPoint.rotation);
        }

        // Hook up retry button
        retryButton.onClick.AddListener(RestartGame);

        // Initialize UI
        winUI.SetActive(false);
        loseUI.SetActive(false);

        // Start timer & starting height
        timer = 0f;
        startY = startPoint.position.y;

        // Add trigger script to DeathPlane if assigned
        if (deathPlane != null)
        {
            if (!deathPlane.TryGetComponent(out DeathPlaneTrigger trigger))
            {
                trigger = deathPlane.AddComponent<DeathPlaneTrigger>();
            }
            trigger.manager = this;
        }

        // Show Banner Ad at game start
        if (AdManager.Instance != null)
        {
            AdManager.Instance.ShowBanner();
        }
    }

    void Update()
    {
        if (!gameRunning || playerInstance == null) return;

        // Update timer
        timer += Time.deltaTime;
        timerText.text = "Time: " + timer.ToString("F2") + "s";

        // Update height
        float currentY = playerInstance.transform.position.y;
        float climbedHeight = currentY - startY;
        heightText.text = "Height: " + Mathf.Max(0, climbedHeight).ToString("F1") + "m";

        // Check win condition
        float distanceToEnd = Vector3.Distance(playerInstance.transform.position, endPoint.position);
        if (distanceToEnd < 3f)
        {
            gameRunning = false;
            Invoke("ShowWinScreen", winDelay);
        }

        // Check time-based lose condition
        if (timer >= maxTimeLimit)
        {
            gameRunning = false;
            ShowLoseScreen();
        }

        // Optional: Check if player too far from end point
        float distanceToStart = Vector3.Distance(playerInstance.transform.position, endPoint.position);
        if (distanceToStart > loseDistanceThreshold)
        {
            gameRunning = false;
            ShowLoseScreen();
        }
    }

    public void ShowWinScreen()
    {
        winUI.SetActive(true);

        if (AdManager.Instance != null)
        {
            AdManager.Instance.HideBanner();
        }
    }

    public void ShowLoseScreen()
    {
        loseUI.SetActive(true);
        gameRunning = false;

        if (AdManager.Instance != null)
        {
            AdManager.Instance.ShowInterstitial();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

// DeathPlane trigger helper script
public class DeathPlaneTrigger : MonoBehaviour
{
    [HideInInspector]
    public GameUIManager manager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.ShowLoseScreen();
        }
    }
}