using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IC_Project_KBM_Controller : MonoBehaviour {

    public static IC_Project_KBM_Controller instance { get; private set; }

    [Header ("Inputs")]
    public Vector2 moveInput;
    //public Vector2 look;
    public bool jump = false;
    public bool sprint = false;
    public bool crouch = false;

    [Space(10)]
    public bool analogMovement;
    public bool isNetworkPlayer = false;

    private void Awake() {

        if (instance == null) {
            instance = this;
        } else if (instance != this) {

            Debug.Log("Instance already exists, destroying object");
            Destroy(transform.gameObject);
        }

    }

    // Input Updates

    /// <summary>
    /// Update our horizontal movement input
    /// </summary>
    /// <param name="newMoveDirection"></param>
    public void MoveInput(Vector2 newMoveDirection) {
        moveInput = newMoveDirection;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newJumpState"></param>
    public void JumpInput(bool newJumpState) {
        jump = newJumpState;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newSprintState"></param>
    public void SprintInput(bool newSprintState) {
        sprint = newSprintState;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newCrouchState"></param>
    public void CrouchInput(bool newCrouchState) {
        crouch = newCrouchState;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public void OnHorizontalMovementInput(InputAction.CallbackContext context) {

        // Runs when our input is pressed (started, performed and cancelled, the whole lot)
        if (context.started || context.performed || context.canceled) {

            //Debug.Log($"Move Input: {context.ReadValue<Vector2>()}");
            MoveInput(context.ReadValue<Vector2>());
        }


    }

    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public void OnJump(InputAction.CallbackContext context) {

        if (context.started || context.performed || context.canceled) {

            //Debug.Log($"Jump Input: {context.ReadValue<float>() == 1f}");
            JumpInput(context.ReadValue<float>() == 1f);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public void OnSprint(InputAction.CallbackContext context) {

        if (context.started || context.performed || context.canceled) {

            //Debug.Log($"Sprint Input: {context.ReadValue<float>()}");
            SprintInput(context.ReadValue<float>() == 1f);

        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public void OnCrouch(InputAction.CallbackContext context) {

        if (context.started || context.performed | context.canceled) {

            //Debug.Log($"Crouch Input: {context.ReadValue<float>()}");
            CrouchInput(context.ReadValue<float>() == 1f);

        }

    }

}
