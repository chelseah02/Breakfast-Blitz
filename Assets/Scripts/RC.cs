using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RC : MonoBehaviour
{
    public float rayLength = 10f;
    public LineRenderer lineRenderer;
    private GameObject currentMenu = null;
    private GameObject lastHitObject = null;
    private GameObject copiedObject = null; 
    private Transform hitTransform = null;
    private Button targetedButton = null;
    private GameObject lastPlacedObject = null;
    private CharacterMovement characterMovement;

    private Quaternion savedRotation;
    private Vector3 savedPosition;
    private Vector3 savedScale;

    private bool isCutMode = false;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        characterMovement = FindObjectOfType<CharacterMovement>();
    }

    void Update()
    {
        RaycastHit hit;
        Vector3 forward = transform.forward;
        Vector3 origin = transform.position + new Vector3(0, -0.2f, 0);
        Ray ray = new Ray(origin, forward);

        lineRenderer.SetPosition(0, ray.origin);

        if (Physics.Raycast(ray, out hit, rayLength))
        {
            lineRenderer.SetPosition(1, hit.point);

            if (hit.collider.gameObject.CompareTag("Interactable"))
            {
                HandleHighlight(hit.collider.gameObject);
                if (Input.GetButtonDown("js2") && hitTransform != hit.collider.transform) // Button X , PC-js1 , Andriod- js2
                {
                    hitTransform = hit.transform; // Update the stored hit object
                    OpenMenu(hit.collider.gameObject); // Open the menu at the hit point
                }
            }

            if (Input.GetButtonDown("js10") && copiedObject != null) // Button A, PC-js8 , Andriod- js10
            {
                Vector3 position = hit.point ; // Adjust the height to place the object above the surface
                //+ Vector3.up * 1f

                if (isCutMode)
                {
                    if (lastPlacedObject != null)
                    {
                        lastPlacedObject.transform.position = position;
                        lastPlacedObject.transform.rotation = savedRotation;
                        lastPlacedObject.transform.localScale = savedScale;
                    }
                    else
                    {
                        lastPlacedObject = Instantiate(copiedObject, position, savedRotation);
                        lastPlacedObject.transform.localScale = savedScale;
                        lastPlacedObject.SetActive(true);
                    }
                }
                else
                {
                    GameObject clone = Instantiate(copiedObject, position, savedRotation);
                    clone.transform.localScale = savedScale;
                    clone.SetActive(true);
                }
            }

            // Handle teleportation
        if (Input.GetButtonDown("js3")) //  Button Y, PC-js0 , Andriod- js3
            {
                
                transform.position = hit.point + Vector3.up * 1.0f;
            }
        }
        else
        {
            lineRenderer.SetPosition(1, ray.origin + ray.direction * rayLength);
            HandleHighlight(null); // No object hit, remove highlights
        }

        CheckButtonTargeting();

        // Button actions
        if (Input.GetButtonDown("js5")) // Button B, PC-js10 , Andriod- js5
        {
            PerformButtonAction();
        }
    }

    public void SetRaycastLength(float length)
    {
        rayLength = length;
       
    }

    void OpenMenu(GameObject interactableObject)
    {
        CloseCurrentMenu(); // Close any existing menu

        Transform menuTransform = interactableObject.transform.Find("Menu"); 
        if (menuTransform != null)
        {
            currentMenu = menuTransform.gameObject;
            currentMenu.SetActive(true); // Show the menu
            DisablePlayerMovement();

            // Make the menu face the camera
            currentMenu.transform.LookAt(Camera.main.transform.position);
            currentMenu.transform.Rotate(0, 180, 0); // Adjust orientation

        }
        else
        {
            Debug.LogError("Menu not found on the interactable object");
        }
    }

    void CheckButtonTargeting()
    {
        // Perform a raycast from the camera forward
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayLength))
        {
            // Check if the hit object is a UI button
            Button button = hit.collider.GetComponent<Button>();
            if (button != null && button != targetedButton)
            {
                // Highlight the new targeted button
                ColorButton(button);
            }
        }
        else if (targetedButton != null)
        {
            // If no button is targeted, remove highlighting from the previously targeted button
            RemoveButtonColor(targetedButton);
            targetedButton = null;
        }
    }

    void ColorButton(Button button)
    {
        // Remove highlighting from the previously targeted button
        if (targetedButton != null)
        {
            RemoveButtonColor(targetedButton);
        }

        // Change the button's color or appearance to indicate it's targeted
        ColorBlock cb = button.colors;
        cb.normalColor = Color.red; // Example highlight color
        button.colors = cb;

        targetedButton = button;
    }

    void RemoveButtonColor(Button button)
    {
        // Reset the button's appearance
        ColorBlock cb = button.colors;
        cb.normalColor = Color.white; 
        button.colors = cb;
    }

    void PerformButtonAction()
    {
        if (targetedButton != null && hitTransform != null)
        {
            GameObject targetObject = hitTransform.gameObject; // The object to which the action will apply

            // Perform the action based on the button's name or another identifier
            switch (targetedButton.name)
            {
                case "CopyButton":
                    isCutMode = false; // Set the mode to copy
                    CopyAction(targetObject);
                    break;
                case "CutButton":
                    isCutMode = true; // Set the mode to cut
                    CutAction(targetObject);
                    break;
                case "ExitButton":
                    CloseCurrentMenu();
                    break;
            }
            RemoveButtonColor(targetedButton);
            targetedButton = null;
        }
    }

    void CopyAction(GameObject targetObject)
    {
        CloseCurrentMenu(); 
        if (targetObject != null)
        {
            // Duplicate the target object

            savedRotation = targetObject.transform.rotation;
            savedPosition = targetObject.transform.position;
            savedScale = targetObject.transform.localScale;
            copiedObject = targetObject;
                //Instantiate(targetObject, targetObject.transform.position + Vector3.right, targetObject.transform.rotation); 
            //copiedObject.name = targetObject.name + "_copy"; 

        }
    }

    void CutAction(GameObject targetObject)
    {
        CloseCurrentMenu(); 
        if (targetObject != null)
        {
            // "Cut" the object by deactivating it 
            savedRotation = targetObject.transform.rotation;
            savedPosition = targetObject.transform.position;
            savedScale = targetObject.transform.localScale;
            targetObject.SetActive(false);
            copiedObject = targetObject; // Store the cut object for potential pasting

            /*
            if (lastPlacedObject != null)
            {
                Destroy(lastPlacedObject);
            }
            */
        }
    }

    void CloseCurrentMenu()
    {
        if (currentMenu != null)
        {
            currentMenu.SetActive(false); 
            currentMenu = null;
            EnablePlayerMovement(); 
            hitTransform = null; // Reset the hit transform to avoid actions on a no longer relevant object
        }
    }

    void EnablePlayerMovement()
    {
        characterMovement.SetMovementEnabled(true);
    }

    void DisablePlayerMovement()
    {
        characterMovement.SetMovementEnabled(false);
    }

    void HandleHighlight(GameObject hitObject)
    {
        if (lastHitObject != hitObject)
        {
            if (lastHitObject != null)
            {
                Highlight highlight = lastHitObject.GetComponent<Highlight>();
                if (highlight != null)
                {
                    highlight.OnPointerExit();
                }
            }

            if (hitObject != null)
            {
                Highlight highlight = hitObject.GetComponent<Highlight>();
                if (highlight != null)
                {
                    highlight.OnPointerEnter();
                }
            }

            lastHitObject = hitObject;
        }
    }

}
