using UnityEngine;
using UnityEngine.EventSystems; // Needed for pointer events

public class Teleportation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool isPointerOver = false;
    public Transform playerTransform; // Assign this in the Inspector

    void Update()
    {
        if (isPointerOver && Input.GetButtonDown("js3")) 
        {
            // Teleport the player to the sphere's position
            playerTransform.position = transform.position;

            // Deactivate the sphere to make it disappear
            gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
    }
}