using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class EndMenuController : MonoBehaviour, IUnityAdsShowListener, IUnityAdsLoadListener
{
    [SerializeField] private string androidAdUnitId = "Interstitial_Android";
    [SerializeField] private string iosAdUnitId = "Interstitial_iOS";
    private string adUnitId; // Platform-specific Ad Unit ID
    private bool isAdLoaded = false;

    void Start()
    {
        // Determine platform-specific Ad Unit ID
        adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer) ? iosAdUnitId : androidAdUnitId;

        // Initialize Unity Ads (optional if already done elsewhere)
        if (!Advertisement.isInitialized)
        {
            Advertisement.Initialize("yourGameID", true); // Replace "yourGameID" with your actual Game ID
        }

        // Load the ad
        Advertisement.Load(adUnitId, this);
    }

    public void StartGame()
    {
        if (isAdLoaded)
        {
            Advertisement.Show(adUnitId, this); // Show the ad
        }
        else
        {
            Debug.LogWarning("Ad not ready. Proceeding to StartGame.");
            LoadLevel("StartGame");
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void LoadLevel(string levelName)
    {
        StartCoroutine(LoadLevelAsync(levelName));
    }

    private IEnumerator LoadLevelAsync(string levelName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName);
        if (asyncLoad == null)
        {
            Debug.LogError("Failed to load level " + levelName);
            yield return null;
        }

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    // IUnityAdsLoadListener implementation
    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (placementId == adUnitId)
        {
            isAdLoaded = true;
            Debug.Log("Ad loaded successfully.");
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        if (placementId == adUnitId)
        {
            isAdLoaded = false;
            Debug.LogError($"Ad failed to load: {error.ToString()} - {message}");
        }
    }

    // IUnityAdsShowListener implementation
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == adUnitId && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            Debug.Log("Ad finished. Proceeding to load StartGame scene.");
            LoadLevel("StartGame");
        }
        else
        {
            Debug.LogWarning("Ad not completed. Proceeding to load StartGame scene.");
            LoadLevel("StartGame");
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        if (placementId == adUnitId)
        {
            Debug.LogError($"Ad failed to show: {error.ToString()} - {message}");
            LoadLevel("StartGame");
        }
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log($"Ad started: {placementId}");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log($"Ad clicked: {placementId}");
    }
}