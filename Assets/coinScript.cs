using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinScript : MonoBehaviour
{
    public Animator animator;
    public GameObject fx_explosion;

    public float timeLeft = 1f;
    public int randomNumber = 0;

    private bool taken;

    public GameObject managerScriptObject;
    private manager managerScript;


    // Start is called before the first frame update
    void Start()
    {
        managerScriptObject = GameObject.Find("Manager");

        managerScript = managerScriptObject.GetComponent<manager>();

        randomNumber = Random.Range(0, 2);
        animator = GetComponent<Animator>();
        //managerScript = GameObject.Find("Player").GetComponent("playerMovement");
        //managerScript = gameObject.AddComponent<manager>() as manager;
        if(randomNumber == 1)
        {
        } else
        {
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(taken)
        {
            timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            Destroy(this.gameObject);
        }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!taken)
        {
        //Check to see if the tag on the collider is equal to Enemy
        if (collision.gameObject.tag == "Player")
        {
                soundManager.PlaySound("coin");
                animator.SetTrigger("isTake");
            Instantiate(fx_explosion, transform.position, Quaternion.identity);
            taken = true;
            managerScript.addCoin(1);
            }
        }
    }

}
