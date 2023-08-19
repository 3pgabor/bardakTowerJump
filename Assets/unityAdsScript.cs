using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class unityAdsScript : MonoBehaviour, IUnityAdsInitializationListener
{
    void Awake()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
        string unityAdID;

#if UNITY_ANDROID
        //app ID android
        unityAdID = "4698207";
        //ca-app-pub-6434100498411494~2568703845

#elif UNITY_IPHONE
        //app ID ios
        //ca-app-pub-6434100498411494~6316522951
        unityAdID = "4698206";

#else

#endif

        Advertisement.Initialize(unityAdID, true, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}
