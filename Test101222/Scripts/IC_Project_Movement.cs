using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IC_Project_Movement : MonoBehaviour {

    // Process whatever inputs ore movements this thing has
    public GameObject baseTransform;
    public CharacterController charController;

    // Inputs
    public Vector2 moveInput;
    // public Vector2 look;
    public bool jump = false;
    public bool sprint = false;
    public bool crouch = false;

    public bool analogMovement;

    public float MoveSpeed = 6.0f;
    public float SprintSpeed = 12.0f;
    //public float RotationSpeed = 10.0f;
    public float SpeedChangeRate = 10f;

    public float JumpHeight = 1.2f;
    public float GravityPersonal = -15f;
    public float JumpTimeout = 0.1f;
    public float FallTimeout = 0.15f;

    private float speed = 6.0f;
    //private float rotationVelocity;
    private float verticalVelocity;
    private float terminalVelocity = 53f;

    // timeout deltaTime;
    private float jumpTimeoutDelta;
    private float fallTimeoutDelta;

    private bool Grounded = false;
    public float GroundedOffset = 0.15f;
    public float GroundedRadius = 0.5f;
    public LayerMask GroundLayers;

    public bool isPlayerControlled = true;

    // Start is called before the first frame update
    void Start() {
        jumpTimeoutDelta = JumpTimeout;
        fallTimeoutDelta = FallTimeout;
        charController = baseTransform.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() {
        if (isPlayerControlled) {
            GetControllerMovementInput();
        }

        JumpAndGravity();
        GroundedCheck();
        Move();
    }

    public void GetControllerMovementInput() {
        moveInput = IC_Project_KBM_Controller.instance.moveInput;
        jump = IC_Project_KBM_Controller.instance.jump;
        sprint = IC_Project_KBM_Controller.instance.sprint;
        crouch = IC_Project_KBM_Controller.instance.crouch;
    }

    private void GroundedCheck() {
        Vector3 spherePosition = new Vector3(baseTransform.transform.position.x,
            baseTransform.transform.position.y - GroundedOffset,
            baseTransform.transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

    }

    private void Move() {
        float targetSpeed = sprint ? SprintSpeed : MoveSpeed;

        if (moveInput == Vector2.zero) targetSpeed = 0.0f;

        float currentHorizontalSpeed = new Vector3(charController.velocity.x, 0.0f, charController.velocity.z).magnitude;
        float speedOffset = 0.1f;
        float inputMagnitude = analogMovement ? moveInput.magnitude : 1f;

        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset) {

            speed = Mathf.Lerp(currentHorizontalSpeed,
                targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

            speed = Mathf.Round(speed * 1000f) / 1000f;

        } else {
            speed = targetSpeed;
        }

        Vector3 inputDirection = new Vector3(moveInput.x, 0.0f, moveInput.y).normalized;

        if (moveInput != Vector2.zero) {
            inputDirection = baseTransform.transform.right * moveInput.x +
                baseTransform.transform.forward * moveInput.y;
        }

        charController.Move(inputDirection.normalized * (speed * Time.deltaTime) +
            new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);

    }

    private void JumpAndGravity() {

        if (Grounded) {
            fallTimeoutDelta = FallTimeout;

            if (verticalVelocity < 0.0f) {
                verticalVelocity = -2f;
            }

            if (jump && jumpTimeoutDelta < 0.0f) {

                verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * GravityPersonal);

            }

            // Jump timeout
            if (jumpTimeoutDelta >= 0.0f) {
                jumpTimeoutDelta -= Time.deltaTime;
            }

        } else {
            // reset the Jump timeout timer
            jumpTimeoutDelta = JumpTimeout;

            // Fall timeout
            if (fallTimeoutDelta >= 0.0f) {
                fallTimeoutDelta -= Time.deltaTime;
            }

            jump = false;

        }

        if (verticalVelocity < terminalVelocity) {
            verticalVelocity += GravityPersonal * Time.deltaTime;
        }

    }

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

    private void OnDrawGizmosSelected() {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (Grounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
    }


}
