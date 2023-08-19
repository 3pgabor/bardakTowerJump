using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class torchScript : MonoBehaviour
{
    Animator animator;

    public Light lightObj;
    public UnityEngine.Rendering.Universal.Light2D m_Light2D;
    public float maxValue;
    public float minValue;
    public float changedMaxValue;
    public float changedMinValue;
    public float intens;

    public float time;

    public bool isCurrent;
    public bool lightDecrease;
    public bool isTriggerLight;
    public bool isTriggered;
    public bool isLast;
    public bool isFirst;

    public GameObject triggerObject;

    public GameObject managerScriptObject;
    private manager managerScript;

    // Start is called before the first frame update
    void Start()
    {
        managerScriptObject = GameObject.Find("Manager");
        managerScript = managerScriptObject.GetComponent<manager>();
        m_Light2D.gameObject.SetActive(false);
        isTriggered = (PlayerPrefs.GetInt(gameObject.name) != 0);

        string name = gameObject.name + "current";
        isCurrent = (PlayerPrefs.GetInt(name) != 0);
        animator = GetComponent<Animator>();
        if (!isTriggerLight)
        {
            m_Light2D.intensity = intens;
            StartCoroutine(changeRadius());
        }

        if (isFirst)
        {
            m_Light2D.gameObject.SetActive(true);
            animator.SetTrigger("isLighted");
        } else
        {
            if (isTriggered)
            {
                m_Light2D.gameObject.SetActive(true);
                animator.SetTrigger("isLighted");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lightDecrease && maxValue > 0)
        {
            maxValue -= 0.0005f;
            minValue -= 0.0005f;
            if (m_Light2D.intensity > 0) m_Light2D.intensity -= 0.0005f;
        }

    }

    public void changeToHardcore(bool hardcore)
    {
        if (!hardcore) return;


        m_Light2D.gameObject.SetActive(false);
        string name1 = gameObject.name + "Hardcore";
        isTriggered = (PlayerPrefs.GetInt(name1) != 0);

        string name = gameObject.name + "currentHardcore";
        isCurrent = (PlayerPrefs.GetInt(name) != 0);
        animator = GetComponent<Animator>();
        
        if (!isTriggerLight)
        {
            m_Light2D.intensity = intens;
            StartCoroutine(changeRadius());
        }

        if (isFirst)
        {
            m_Light2D.gameObject.SetActive(true);
            animator.SetTrigger("isLighted");
            isCurrent = true;
        }
        else
        {
            if (isTriggered)
            {
                m_Light2D.gameObject.SetActive(true);
                animator.SetTrigger("isLighted");
            }
        }
    }

    IEnumerator changeRadius()
    {

        yield return new WaitForSeconds(time);
        m_Light2D.pointLightOuterRadius = Random.Range(minValue, maxValue);
        StartCoroutine(changeRadius());

    }

    IEnumerator lightOff()
    {
        yield return new WaitForSeconds(1);
        lightDecrease = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!isTriggered)
        {
            animator.SetTrigger("isLighted");
            isTriggered = true;
            m_Light2D.gameObject.SetActive(true);
            bool isTrig = true;
            if(managerScript.isHardcore)
            {
                string name = gameObject.name + "Hardcore";
                PlayerPrefs.SetInt(name, isTrig ? 1 : 0);
            } else
            {
                PlayerPrefs.SetInt(gameObject.name, isTrig ? 1 : 0);
            }
            managerScript.showTorchLot();
        }
        playerMovement.rewindUsedCount = 1;
        managerScript.refreshRewindCoinText();
    }
    public void updateToFalseTorch()
    {
        if (managerScript.isHardcore)
        {

            bool isTrig = false;
            string name = gameObject.name + "Hardcore";
            PlayerPrefs.SetInt(name, isTrig ? 1 : 0);
        }
        else
        {
            bool isTrig = false;
            PlayerPrefs.SetInt(gameObject.name, isTrig ? 1 : 0);
        }
    }

    public void updateToCurrentTorch(bool current)
    {
        if(managerScript.isHardcore)
        {

            bool isTrig = current;
            string name = gameObject.name + "currentHardcore";
            PlayerPrefs.SetInt(name, isTrig ? 1 : 0);
            isCurrent = current;
        } else
        {
            bool isTrig = current;
            string name = gameObject.name + "current";
            PlayerPrefs.SetInt(name, isTrig ? 1 : 0);
            isCurrent = current;
        }
        
    }
}
