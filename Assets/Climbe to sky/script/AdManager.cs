using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static AdManager Instance;

    [Header("Ad Settings")]
    public bool testMode = true;

#if UNITY_ANDROID
    private string gameId = "5854079";
    private string interstitialAdUnitId = "Interstitial_Android";
    private string bannerAdUnitId = "Banner_Android";
    private string rewardedAdUnitId = "Rewarded_Android";
#elif UNITY_IOS
    private string gameId = "YOUR_IOS_GAME_ID";
    private string interstitialAdUnitId = "Interstitial_iOS";
    private string bannerAdUnitId = "Banner_iOS";
    private string rewardedAdUnitId = "Rewarded_iOS";
#endif

    private bool adsInitialized = false;
    private bool interstitialLoaded = false;
    private bool rewardedLoaded = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAds();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeAds()
    {
        Advertisement.Initialize(gameId, testMode, this);
        Debug.Log("Initializing Unity Ads...");
    }

    // Initialization callback
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads Initialized.");
        adsInitialized = true;
        LoadInterstitial();
        LoadRewarded();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Unity Ads Initialization Failed: {error} - {message}");
    }

    // Load Ads
    public void LoadInterstitial()
    {
        if (adsInitialized)
            Advertisement.Load(interstitialAdUnitId, this);
    }

    public void LoadRewarded()
    {
        if (adsInitialized)
            Advertisement.Load(rewardedAdUnitId, this);
    }

    // Show Ads
    public void ShowInterstitial()
    {
        if (adsInitialized && interstitialLoaded)
        {
            Advertisement.Show(interstitialAdUnitId, this);
        }
        else
        {
            Debug.Log("Interstitial ad not ready.");
        }
    }

    public void ShowRewarded()
    {
        if (adsInitialized && rewardedLoaded)
        {
            Advertisement.Show(rewardedAdUnitId, this);
        }
        else
        {
            Debug.Log("Rewarded ad not ready.");
        }
    }

    public void ShowBanner()
    {
        if (adsInitialized)
        {
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
            Advertisement.Banner.Show(bannerAdUnitId);
        }
        else
        {
            Debug.Log("Banner ad not ready.");
        }
    }

    public void HideBanner()
    {
        if (adsInitialized)
            Advertisement.Banner.Hide();
    }

    // Load Callbacks
    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (placementId == interstitialAdUnitId)
        {
            interstitialLoaded = true;
            Debug.Log("Interstitial ad loaded.");
        }
        else if (placementId == rewardedAdUnitId)
        {
            rewardedLoaded = true;
            Debug.Log("Rewarded ad loaded.");
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Failed to load Ad {placementId}: {error} - {message}");
    }

    // Show Callbacks
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log($"Ad {placementId} finished. Completion State: {showCompletionState}");

        if (placementId == interstitialAdUnitId)
        {
            interstitialLoaded = false;
            LoadInterstitial();
        }
        else if (placementId == rewardedAdUnitId)
        {
            rewardedLoaded = false;
            LoadRewarded();
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Ad Show Error: {placementId} - {error} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log($"Ad {placementId} started.");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log($"Ad {placementId} clicked.");
    }
}