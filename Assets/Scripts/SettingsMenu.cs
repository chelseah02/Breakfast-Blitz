using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsMenuPanel; 
    public Button[] menuButtons; 
    private int currentButtonIndex = 0; 

    private int currentRaycastLengthIndex = 0; 
    public float[] raycastLengths = { 1f, 10f, 50f }; 

    private int currentSpeedIndex = 0; 
    public float[] speedValues = { 20f, 10f, 5f };

    private float inputDelay = 0.2f; 
    private float nextInputTime = 0f; 
    private RC raycastController;
    private CharacterMovement characterMovement;

    void Start()
    {
        // Assign the raycastController reference
        raycastController = FindObjectOfType<RC>();
        characterMovement = FindObjectOfType<CharacterMovement>();


    }

    void Update()
    {
        if (Input.GetButtonDown("js3") || Input.GetButtonDown("js7")) // Button OK , PC-js3 , Andriod- js0 or js7 || Input.GetButtonDown("js7")
        {
            ToggleSettingsMenu();
        }

        if (settingsMenuPanel.activeSelf && Time.time >= nextInputTime)
        {
            if (Input.GetAxisRaw("Vertical") > 0) // Up arrow pressed
            {
                NavigateMenu(-1); 
            }
            else if (Input.GetAxisRaw("Vertical") < 0) // Down arrow pressed
            {
                NavigateMenu(1); 
            }

            if (currentButtonIndex == 1) 
            {
                if (Input.GetButtonDown("js10")) //  Button A, PC-js8 , Andriod- js10
                {   
                    ChangeRaycastLength(-1);
                }
                else if (Input.GetButtonDown("js3")) //  Button Y, PC-js0 , Andriod- js3
                {
                    ChangeRaycastLength(1);
                }
            }

            if (currentButtonIndex == 2)
            {
                if (Input.GetButtonDown("js10")) //  Button A, PC-js8 , Andriod- js10
                {
                    ChangeSpeed(-1);
                }
                else if (Input.GetButtonDown("js3"))  //  Button Y, PC-js0 , Andriod- js3
                {
                    ChangeSpeed(1);
                }
            }

            if (Input.GetButtonDown("js2"))  // Button X , PC-js1 , Andriod- js2
            {
                PerformButtonAction();
            }
        }
    }

    private void ToggleSettingsMenu()
    {
        bool isActive = !settingsMenuPanel.activeSelf;
        settingsMenuPanel.SetActive(!settingsMenuPanel.activeSelf);

        if (isActive)
        {
            currentButtonIndex = 0; // Reset to the first button
            HighlightButton(currentButtonIndex);

            // Disablemovement
           characterMovement.SetMovementEnabled(false);
        }
        else
        {
            ResumeGame();
        }
    }

    private void ResumeGame()
    {
        settingsMenuPanel.SetActive(false); // Deactivate the settings menu panel

        // Re-enable movement
         characterMovement.SetMovementEnabled(true);
    }

    private void NavigateMenu(int direction)
    {
        currentButtonIndex = (currentButtonIndex + direction + menuButtons.Length) % menuButtons.Length;
        HighlightButton(currentButtonIndex);
        nextInputTime = Time.time + inputDelay; // Set the time for the next allowed input
    }

    private void HighlightButton(int index)
    {
        for (int i = 0; i < menuButtons.Length; i++)
        {
            ColorBlock cb = menuButtons[i].colors;
            cb.normalColor = (i == index) ? Color.yellow : Color.white; // Highlight the selected button
            menuButtons[i].colors = cb;
        }
    }

    // Raycast 
    private void ChangeRaycastLength(int direction) // Change length 
    {
        currentRaycastLengthIndex = (currentRaycastLengthIndex + direction + raycastLengths.Length) % raycastLengths.Length;
        UpdateRaycastLengthButtonText();
    }

    private void UpdateRaycastLengthButtonText() // Update length button text
    {
        menuButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Length: " + raycastLengths[currentRaycastLengthIndex] + "m";
    }

    //Speed
    private void ChangeSpeed(int direction)
    {
        currentSpeedIndex = (currentSpeedIndex + direction + speedValues.Length) % speedValues.Length;
        UpdateSpeedButtonText();
    }

    private void UpdateSpeedButtonText()
    {
        string speedText;

        if (currentSpeedIndex == 0)
        {
            speedText = "High";
        }
        else if (currentSpeedIndex == 1)
        {
            speedText = "Medium";
        }
        else 
        {
            speedText = "Low";
        }

        menuButtons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Speed: " + speedText;
    }


    private void PerformButtonAction()
    {
        switch (currentButtonIndex)
        {
            case 0: // Resume
                ResumeGame();
                break;
            case 1: // Raycast Length
                
                float length = raycastLengths[currentRaycastLengthIndex];
                raycastController.SetRaycastLength(length);
                break;
            case 2: // Speed
                float speed = speedValues[currentSpeedIndex];
                characterMovement.SetMovementSpeed(speed); 
                break;
            case 3: // Quit
                Application.Quit();
                break;
        }
    }

}