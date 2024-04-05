using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Required for pointer events

public class Translate : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool isPointerOver = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
    }

    void Update()
    {
        if (isPointerOver && Input.GetButton("js2")) 
        {
            transform.Translate(Vector3.left * 0.1f); //Time.deltaTime
        }
    }
}
