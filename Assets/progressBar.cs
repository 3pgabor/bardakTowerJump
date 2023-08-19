using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class progressBar : MonoBehaviour
{
    Vector3 startLocalScale;
    Vector3 localScale;

    // Start is called before the first frame update
    void Start()
    {
        startLocalScale = transform.localScale;
        localScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
       // print(playerMovement.validProgressDistance);
        localScale.x = startLocalScale.x * playerMovement.validProgressDistance;
        transform.localScale = localScale;
    }
}
