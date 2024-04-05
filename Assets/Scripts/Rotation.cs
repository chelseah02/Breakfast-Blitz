using UnityEngine;
using UnityEngine.EventSystems; // Required for pointer events

public class Rotation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
            transform.Rotate(Vector3.up * Time.deltaTime * 90); // Rotate 90 degrees per second
        }
    }

}




