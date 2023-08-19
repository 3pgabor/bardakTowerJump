using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;

public class firebaseInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.Exception != null)
            {
                Debug.LogError($"firebase Task failed with {task.Exception}");
            }
            else if (task.Result != DependencyStatus.Available)
            {
                Debug.LogError($"Firebase dependencies not available with {task.Result}");
            }
            else
            {
                Debug.Log("firebase Everything's good!");
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
