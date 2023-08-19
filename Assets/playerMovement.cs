using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.EventSystems;
//using UnityEngine.InputSystem;
//using Firebase.Analytics;
//using HmsPlugin;
//using HuaweiMobileServices.Ads;

public class playerMovement : MonoBehaviour
{
    private Touches touchController;

    public GameObject fx_Fall_Blue;
    public GameObject fx_Fall_Purple;
    public GameObject fx_Fall_Orange;
    public GameObject fx_Fall_Green;
    public GameObject fx_Wall_Hit;

    public GameObject jumpBar;

    public Vector3 jump;
    public Vector3 prePosition;
    public Vector3 lastPosition;

    public Camera mainCamera;

    public Vector2 boxSize;

    public GameObject startGate;
    public GameObject endGate;
    private closestTorch closestTorch;
    private closestTorch preclosestTorch;

    public float maxVelocity = 5;
    public float maxJumpHeight = 10;
    public float jumpForce = 1;
    public float jumpForceDirection = 1;
    public static float jumpLoadValue;
    public float progressDistance;
    public float startPos;
    public static float validProgressDistance;
    public static float preProgressDistance;
    public static float prePos;
    public static float score;
    public static int highScore;
    public static float level;
    public float score2;
    public static int life;
    public static int rewindUsedCount;

    public static float preLevelNumber;
    public static float nextLevelNumber;

    public bool isPro;
    public static bool isFreezed;
    public bool isGrounded;
    public bool isSlide;
    public bool isTop;
    public bool isJumping;
    public bool isPressed;
    public bool isPressedUp;
    private bool isJumpLoad = false;
    public bool isFlipped;
    public bool isLevelUp;
    public bool isJumpEnough;
    public bool isWall;
    public static bool isMenu;

    public PhysicsMaterial2D bouncingMat;
    public PhysicsMaterial2D notBouncingMat;
    public PhysicsMaterial2D slideMat;

    public RuntimeAnimatorController player01;
    public RuntimeAnimatorController player02;
    public RuntimeAnimatorController player03;
    public RuntimeAnimatorController player04;
    public RuntimeAnimatorController player05;

    public GameObject menuPlayer1;
    public GameObject menuPlayer2;
    public GameObject menuPlayer3;
    public GameObject menuPlayer4;
    public GameObject menuPlayer5;

    public static CapsuleCollider2D boxCollider2D;

    Rigidbody2D rb;
    Animator animator;
    float mousePosition;
    Touch touch;

    public Transform topCheck;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public LayerMask slideLayer;

    public GameObject managerScriptObject;
    private manager managerScript;

    public torchScript[] torches;
    public closestTorch[] torches2;

    void Awake()
    {
        touchController = new Touches();
        touchController.Enable();
    }

    void Start()
    {
        isPro = false;
        isMenu = true;
        isFreezed = true;
        managerScriptObject = GameObject.Find("Manager");

        managerScript = managerScriptObject.GetComponent<manager>();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider2D = transform.GetComponent<CapsuleCollider2D>();
        jumpBar = GameObject.FindGameObjectWithTag("Jumpbar");

        jumpBar.SetActive(false);
        validProgressDistance = -0.1f;
        startPos = this.transform.position.y;
        highScore = (PlayerPrefs.GetInt("HighScore", 0));
        life = managerScript.rewindCount;
        managerScript.refreshLife();
        rewindUsedCount = 1;
        managerScript.refreshRewindCoinText();
        checkTorches();

    }


    void Update()
    {

#if UNITY_STANDALONE || UNITY_EDITOR

           if (Input.GetMouseButtonUp(0) && isPressed)
          {
        ButtonUp();
        TapticLight();
           }

         if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
            {
        buttonPressed();
         TapticLight();
          }

#else


        if (Input.GetMouseButtonUp(0) && isPressed)
        {
            ButtonUp();
            TapticLight();
        }

        if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
        {
            buttonPressed();
            TapticLight();
        }
#endif


    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public void buttonPressed()
    {
        if (!isFreezed)
        {
            managerScript.hideAgainButton(true);
            ButtonPressed();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up, boxSize);
    }

    public void FixedUpdate()
    {

        JumpLoad();
        RaycastHit2D[] hits;

        hits = null;

        if (rb.velocity.y < maxVelocity)
        {
            // Korlátozzuk az esési sebességet a maximálisra
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, maxVelocity));
        }

        Vector2 positionToCheck = transform.position;
        hits = Physics2D.RaycastAll(positionToCheck, new Vector2(0, -1), 0.01f);

        //isGrounded = (Physics2D.OverlapCircle(groundCheck.position, 0.24f, groundLayer) && hits.Length > 0);
        isGrounded = (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, 1.3f, groundLayer) && hits.Length > 0);
        isSlide = (Physics2D.OverlapCircle(groundCheck.position, 0.24f, slideLayer) && hits.Length > 0);
        isTop = (Physics2D.OverlapCircle(topCheck.position, 0.2f, groundLayer) && hits.Length > 0);

        if (isGrounded)
        {
            rb.sharedMaterial = notBouncingMat;
            boxCollider2D.sharedMaterial = notBouncingMat;
        }
        else if(isTop)
        {
            rb.sharedMaterial = slideMat;
            boxCollider2D.sharedMaterial = slideMat;
        }
        else
        {
            rb.sharedMaterial = bouncingMat;
            boxCollider2D.sharedMaterial = bouncingMat;
        }

        if (isSlide)
        {
            rb.sharedMaterial = slideMat;
            boxCollider2D.sharedMaterial = slideMat;
        }

        if (isLevelUp)
        {
            if (1 > validProgressDistance)
            {
                validProgressDistance += 0.05f;
            }
        } else if (isGrounded)
        {
            if (progressDistance > validProgressDistance && isJumpEnough)
            {
                validProgressDistance += 0.05f;
            }
            if (progressDistance < validProgressDistance && isJumpEnough)
            {
                validProgressDistance -= 0.05f;
            }

            if (score2 > score && isJumpEnough)
            {
                score += 1;
            }
            if (score2 < score && isJumpEnough)
            {
                score -= 1;
            }

            //CalculateDistance();
        }
            CalculateDistance();
    }

    public void ButtonPressed()
    {
        if (isGrounded && !isPressed)
        {
            managerScript.addTap();
            isPressed = true;
            mousePosition = Input.mousePosition.x;
            if (mousePosition < Screen.width/2)
            {
                if(!isFlipped)
                {
                    transform.localRotation = Quaternion.Euler(0, -180, 0);
                }
                isFlipped = true;
            }
            else
            {
                if (isFlipped)
                {
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                isFlipped = false;
            }

            isJumpLoad = true;
            animator.SetTrigger("isStartJumping");
        }
        else
        {
            isJumpLoad = false;
        }
    }

    public void ButtonUp()
    {
            jumpBar.SetActive(false);
            if (isGrounded && isPressed && !isPressedUp)
            {
                isPressedUp = true;
                if (jumpLoadValue < 1)
                {
                    jumpLoadValue = 1f;
                }
                isJumpLoad = false;

                StartCoroutine(JumpAfterTime(0.05f));
                TapticLight();
                isGrounded = false;
            }
    }

    public void CalculateDistance()
    {
        progressDistance =  (transform.position.y - startGate.transform.position.y-2) / (endGate.transform.position.y - startGate.transform.position.y);
        score2 = this.transform.position.y - startPos;
        //print(progressDistance);
    }

    public void JumpLoad()
    {
        if (isJumpLoad && jumpLoadValue <= maxJumpHeight && isGrounded)
        {
            jumpLoadValue += 10 * Time.deltaTime;
            jumpBar.SetActive(true);
            //print(jumpLoadValue);
        }
    }

    bool IsPointerOverGameObject(int fingerId)
    {
        EventSystem eventSystem = EventSystem.current;
        return (eventSystem.IsPointerOverGameObject(fingerId)
            && eventSystem.currentSelectedGameObject != null);
    }

    IEnumerator JumpAfterTime(float time)
    {
        preProgressDistance = this.transform.position.y;
        prePosition = this.transform.position;
        isJumpEnough = false;
        managerScript.hideAgainButton(true);
        yield return new WaitForSeconds(time);

        if (isFlipped)
        {
            rb.velocity = Vector2.up * jumpLoadValue * jumpForce * 0.7f + Vector2.left * jumpLoadValue * jumpForceDirection * 0.7f;
        } else
        {
            rb.velocity = Vector2.up * jumpLoadValue * jumpForce * 0.7f + Vector2.right * jumpLoadValue * jumpForceDirection * 0.7f;
        }

        jumpLoadValue = 0;

        isJumping = true;

        animator.SetTrigger("isJumping");
        soundManager.PlaySound("jump");
        lastPosition = this.transform.position;
        //preProgressDistance = transform.position.y / endGate.transform.position.y;
        //print(preProgressDistance);
    }

    IEnumerator isWallAfterTime(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        isWall = false;
    }

    IEnumerator nextLevelAfterTime(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        isLevelUp = false;
        managerScript.refreshLife();
        isJumpEnough = true;
        managerScript.refreshLabel();
    }

    IEnumerator restartAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator nextMapAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        managerScript.win();
    }

    void Die()
    {
        isFreezed = true;
        animator.SetTrigger("isDie");
        rb.bodyType = RigidbodyType2D.Static;
        StartCoroutine(restartAfterTime(0.5f));
    }

    void Win()
    {
        StartCoroutine(updateTorches());
        isFreezed = true;
        StartCoroutine(nextMapAfterTime(1));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (lastPosition.y - this.transform.position.y > 5 && isGrounded)
        {
            managerScript.hideAgainButton(false);
            soundManager.PlaySound("fall");
        }

        if (isGrounded && isJumping )
        {
            if((Mathf.Abs(preProgressDistance - this.transform.position.y)) > 0.5)
            {
                isJumpEnough = true;
            }
            if(preProgressDistance - this.transform.position.y > 5)
            {
                //r?gi megold?s
            }

            if(score>highScore)
            {
                highScore = (int)score;
                PlayerPrefs.SetInt("HighScore", highScore);
            }

            FindClosestTorch();
            isPressed = false;
            isPressedUp = false;
            isJumping = false;
            animator.SetTrigger("isLanding");
            if (collision.gameObject.tag == "BlueGround")
            {
                Instantiate(fx_Fall_Blue, transform.position, Quaternion.identity);
            }
            if (collision.gameObject.tag == "PurpleGround")
            {
                Instantiate(fx_Fall_Purple, transform.position, Quaternion.identity);
            }
            if (collision.gameObject.tag == "OrangeGround")
            {
                Instantiate(fx_Fall_Orange, transform.position, Quaternion.identity);
            }
            if (collision.gameObject.tag == "GreenGround")
            {
                Instantiate(fx_Fall_Green, transform.position, Quaternion.identity);
            }
        } else
        {
            if(!isSlide)
            {
                if(!isWall)
                {
                Instantiate(fx_Wall_Hit, transform.position, Quaternion.identity);
                    if (!isMenu)
                    {
                        if (collision.gameObject.layer == 7)
                        {

                        } else
                        {
                            soundManager.PlaySound("hit");
                        }
                    }
                isWall = true;
                StartCoroutine(isWallAfterTime(0.05f));
                }
        }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wingate")
        {
            managerScript.showRestorePopup();
            life = managerScript.rewindCount;
        }
        if (collision.gameObject.tag == "LastTorch")
        {
            Win();
        }

        if (collision.gameObject.tag == "Lava")
        {
            Die();
        }
    }

    public void TakeBack()
    {
        if(prePosition.y < 2) return;
        if (life > 0)
        {
        isJumpLoad = false;
        this.transform.position = new Vector3(lastPosition.x, lastPosition.y + 0.5f, 0);
        managerScript.hideAgainButton(true);
        FindClosestTorch();
        isJumping = true;
        isPressedUp = true;
        mainCamera.transform.position = new Vector3(lastPosition.x, lastPosition.y + 2, -6);
        isGrounded = false;
        animator.SetTrigger("isLanding");
        life --;
        managerScript.refreshLife();
        managerScript.againTappad();

        } else
        {
            managerScript.showRewindPanel(true);
        }

}

    public void takeBackToTorch()
    {
        Vector3 pozi = new Vector3(startGate.transform.position.x, startGate.transform.position.y + 2, 0); ;
        foreach (torchScript currentTorch in torches)
        {
            if (currentTorch.isTriggered)
            {
                //transform.position = new Vector3(currentTorch.gameObject.transform.position.x, currentTorch.gameObject.transform.position.y+2, 0);
                //mainCamera.transform.position = new Vector3(currentTorch.gameObject.transform.position.x, currentTorch.gameObject.transform.position.y + 2, -6);
                pozi = new Vector3(currentTorch.transform.position.x, currentTorch.transform.position.y + 2, 0);
            }
        }

        isJumpLoad = false;
        this.transform.position = pozi;
        mainCamera.transform.position = new Vector3(startGate.transform.position.x, startGate.transform.position.y + 2, -6);
        managerScript.hideAgainButton(true);
        FindClosestTorch();
        isJumping = true;
        isPressedUp = true;
        isGrounded = false;
        animator.SetTrigger("isLanding");
        life = managerScript.rewindCount;
        managerScript.refreshLife();
        managerScript.continueTapped();

    }

    void FindClosestTorch()
    {   
        bool isRefreshLabel = false;
        float distanceToClosestTorch = Mathf.Infinity;
        int i = 0;
        //closestTorch[] allTorches = GameObject.FindObjectsOfType<closestTorch>().OrderBy(m => m.transform.GetSiblingIndex()).ToArray();
        closestTorch preclosestTorch2 = null;

        foreach (closestTorch currentTorch in torches2)
        {
            float distanceToTorch = (currentTorch.transform.position.y - this.transform.position.y);
            if (this.transform.position.y > currentTorch.transform.position.y-2f)
            {
                if(Mathf.Abs(this.transform.position.y - currentTorch.transform.position.y) < 3 && preclosestTorch != currentTorch)
                {
                    if(startGate.transform.position.y < currentTorch.gameObject.transform.position.y-2f)
                    {
                        isLevelUp = true;
                        StartCoroutine(nextLevelAfterTime(1));
                        isRefreshLabel = true;
                        //FirebaseAnalytics.LogEvent("next_level", "level", i);
                        if(i == 6)
                        {
                            managerScript.rateUsShow();
                        }
                    }
                }
                startGate = currentTorch.gameObject;
                preLevelNumber = i;
                nextLevelNumber = i+1;
                preclosestTorch2 = currentTorch;


                //preProgressDistance = startGate.transform.position.y / endGate.transform.position.y;
            }
            i++;
            if (distanceToTorch < distanceToClosestTorch && (this.transform.position.y < currentTorch.transform.position.y-2f))
            {
                distanceToClosestTorch = distanceToTorch;
                closestTorch = currentTorch;
                endGate = closestTorch.gameObject;
            }
        }
        preclosestTorch = preclosestTorch2;
        if(!isRefreshLabel)
        {
            managerScript.refreshLabel();
        }

        //endGate = closestEnemy.gameObject;
        //Debug.DrawLine(this.transform.position, closestEnemy.transform.position);
        updateCurrentTorches();
    }

    public void checkTorchesContinue()
    {
        int i = 1;
        //torchScript[] allTorches = GameObject.FindObjectsOfType<torchScript>().OrderBy(m => m.transform.GetSiblingIndex()).ToArray();

        foreach (torchScript currentTorch in torches)
        {
            if (currentTorch.isTriggered)
            {
                transform.position = new Vector3(currentTorch.gameObject.transform.position.x, currentTorch.gameObject.transform.position.y + 2, 0);
                mainCamera.transform.position = new Vector3(currentTorch.gameObject.transform.position.x, currentTorch.gameObject.transform.position.y + 2, -6);
                isJumping = true;
                level = i;
                managerScript.hideAgainButton(true);
                FindClosestTorch();
                //FirebaseAnalytics.LogEvent("continue", "level", level);
            }
            i++;
        }
        managerScript.refreshLabel();
    }

        public void checkTorches()
    {
        int l = 1;
            //torchScript[] allTorches = GameObject.FindObjectsOfType<torchScript>().OrderBy(m => m.transform.GetSiblingIndex()).ToArray();
            //print(l);
            foreach (torchScript currentTorch in torches)
            {
            currentTorch.changeToHardcore(managerScript.isHardcore);
                if (currentTorch.isCurrent)
                {
                    transform.position = new Vector3(currentTorch.gameObject.transform.position.x, currentTorch.gameObject.transform.position.y + 2, 0);
                    mainCamera.transform.position = new Vector3(currentTorch.gameObject.transform.position.x, currentTorch.gameObject.transform.position.y + 2, -6);
                    isJumping = true;
                    level = l;
                    managerScript.hideAgainButton(true);
                    FindClosestTorch();
                    //FirebaseAnalytics.LogEvent("continue", "level", level);
                }
                l++;
            }
            managerScript.refreshLabel();
    }

    public void updateT()
    {
        StartCoroutine(updateTorches());
    }

        IEnumerator updateTorches()
    {
        print("mivan torch 1 ");
        yield return new WaitForSeconds(0.1f);
        //torchScript[] allTorches = GameObject.FindObjectsOfType<torchScript>().OrderBy(m => m.transform.GetSiblingIndex()).ToArray();

        foreach (torchScript currentTorch in torches)
        {
            print("mivan torch darabok ");
            if (currentTorch.isTriggered)
            {
                currentTorch.updateToFalseTorch();
            }
            if (currentTorch.isCurrent)
            {
                currentTorch.updateToCurrentTorch(false);
            }
        }
    }

    public void updateCurrentTorches()
    {
        if(isFreezed)
        {
            return;
        }
        int i = 0;
        //torchScript[] allTorches = GameObject.FindObjectsOfType<torchScript>().OrderBy(m => m.transform.GetSiblingIndex()).ToArray();

        foreach (torchScript currentTorch in torches)
        {
            currentTorch.updateToCurrentTorch(false);
            if (preLevelNumber == i)
            {
                currentTorch.updateToCurrentTorch(true);
                print(preLevelNumber);
            }
            i++;
        }

    }

    public int checkTorchesToMenu()
    {

        int i = 0;
        //torchScript[] allTorches = GameObject.FindObjectsOfType<torchScript>().OrderBy(m => m.transform.GetSiblingIndex()).ToArray();

        foreach (torchScript currentTorch in torches)
        {
            print("mivannn");
            if (currentTorch.isTriggered)
            {
                i++;
                print("mivannn menu");
            }
        }
        if(i == 0)
        {
            i = 1;
        }
        return i;
    }

    public void changeCharacter(int index)
    {
        hideAllMenuPlayer();
        switch (index)
        {
            case 1:
                this.GetComponent<Animator>().runtimeAnimatorController = player01 as RuntimeAnimatorController;
                menuPlayer1.SetActive(true);
                break;
            case 2:
                this.GetComponent<Animator>().runtimeAnimatorController = player02 as RuntimeAnimatorController;
                menuPlayer2.SetActive(true);
                break;
            case 3:
                this.GetComponent<Animator>().runtimeAnimatorController = player03 as RuntimeAnimatorController;
                menuPlayer3.SetActive(true);
                break;
            case 4:
                this.GetComponent<Animator>().runtimeAnimatorController = player04 as RuntimeAnimatorController;
                menuPlayer4.SetActive(true);
                break;
            case 5:
                this.GetComponent<Animator>().runtimeAnimatorController = player05 as RuntimeAnimatorController;
                menuPlayer5.SetActive(true);
                break;
            default:
                this.GetComponent<Animator>().runtimeAnimatorController = player01 as RuntimeAnimatorController;
                menuPlayer1.SetActive(true);
                break;
        }
    }

    public void hideAllMenuPlayer()
    {
        menuPlayer1.SetActive(false);
        menuPlayer2.SetActive(false);
        menuPlayer3.SetActive(false);
        menuPlayer4.SetActive(false);
    }

    public void TapticLight()
    {

#if UNITY_IPHONE
        if(managerScript.vibrationMuted) return;
        Debug.Log("Triggered Light Haptic");
        TapticEngine.TriggerLight();
#endif
    }

    public void TapticMedium()
    {
#if UNITY_IPHONE
        if (managerScript.vibrationMuted) return;
        Debug.Log("Triggered Medium Haptic");
        TapticEngine.TriggerMedium();
#endif
    }
}



