using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fxRemove : MonoBehaviour
{
    public float timeLeft = 1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            Destroy(this.gameObject);
        }
    }
}