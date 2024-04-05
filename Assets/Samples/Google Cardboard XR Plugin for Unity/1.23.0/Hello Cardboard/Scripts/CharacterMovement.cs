using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    CharacterController charCntrl;
    [Tooltip("The speed at which the character will move.")]
    public float speed = 5f;
    [Tooltip("The camera representing where the character is looking.")]
    public GameObject cameraObj;
    [Tooltip("Should be checked if using the Bluetooth Controller to move. If using keyboard, leave this unchecked.")]
    public bool joyStickMode;
    public float heightOffset = 0.5f; //

    // Start is called before the first frame update
    void Start()
    {
        charCntrl = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Get horizontal and Vertical movements
        float horComp = Input.GetAxis("Horizontal");
        float vertComp = Input.GetAxis("Vertical");

        if (joyStickMode)
        {
            horComp = Input.GetAxis("Vertical");
            vertComp = Input.GetAxis("Horizontal") * -1;
        }

        Vector3 moveVect = Vector3.zero;

        //Get look Direction
        Vector3 cameraLook = cameraObj.transform.forward;
        cameraLook.y = 1f;
        cameraLook = cameraLook.normalized;

        Vector3 forwardVect = cameraLook;
        Vector3 rightVect = Vector3.Cross(forwardVect, Vector3.up).normalized * 1;

        moveVect += rightVect * horComp;
        moveVect += forwardVect * vertComp;

        moveVect *= speed;


        charCntrl.SimpleMove(moveVect);

        // Adjust the height of the character
        charCntrl.height = charCntrl.height + heightOffset; // 
        charCntrl.center = new Vector3(charCntrl.center.x, charCntrl.center.y + heightOffset / 2, charCntrl.center.z); // 


    }
    public void SetMovementSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetMovementEnabled(bool enabled)
    {

        charCntrl.enabled = enabled;

    }
}
