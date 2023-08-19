using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingObject : MonoBehaviour
{

    private Vector2 startPosition;
    /// <summary>The objects updated position for the next frame.</summary>
    private Vector2 newPosition;

    public bool isReversed;
    public bool isMovingY;
    public bool isBat;
    public Animator animator;
    private bool isDirectionChanged;

    public Vector3 lastPos;

    /// <summary>The speed at which the object moves.</summary>
    [SerializeField] private float speed = 3;
    /// <summary>The maximum distance the object may move in either y direction.</summary>
    [SerializeField] private float maxDistance = 1;


    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
        startPosition = transform.position;
        newPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (isBat)
        {
            if (lastPos.x < transform.position.x && !isDirectionChanged)
            {
                animator.transform.Rotate(0, 180, 0);
                isDirectionChanged = true;
            }
            if (lastPos.x > transform.position.x && isDirectionChanged)
            {
                animator.transform.Rotate(0, 180, 0);
                isDirectionChanged = false;
            }
        }

        lastPos = transform.position;

        if (isBat)
        {

        }
        if (isMovingY)
        {
            if (isReversed)
            {
                newPosition.y = startPosition.y + (maxDistance * Mathf.Sin(Time.time * speed));
                transform.position = newPosition;
            }
            else
            {
                newPosition.y = startPosition.y + (maxDistance * Mathf.Sin(Time.time * -speed));
                transform.position = newPosition;
            }


        }
        else
        {

            if (isReversed)
            {
                if (isBat) animator.transform.Rotate(0, 180, 0);
                newPosition.x = startPosition.x + (maxDistance * Mathf.Sin(Time.time * speed));
                transform.position = newPosition;
            }
            else
            {
                newPosition.x = startPosition.x + (maxDistance * Mathf.Sin(Time.time * -speed));
                transform.position = newPosition;
            }
        }
    }

}
