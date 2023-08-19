
using UnityEngine;
using System.Collections;

using UnityEngine;

[ExecuteInEditMode()]
public class LockUIItem : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector3 startPosition;

    protected void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = transform.position;
    }

    private void LateUpdate()
    {
        if (startPosition != null)
        {
            rectTransform.position = startPosition;
        }
    }
}