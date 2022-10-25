using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IC_Project_FPS_Look : MonoBehaviour {

    //public GameObject hand;
    public GameObject playerBase;
    public GameObject mainCamera;

    public Vector2 look;


    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    public float RotationSpeed = 10f;

    public float targetPitch = 0f;
    public float TopClamp = 89f;
    public float BottomClamp = -89f;
    public float threshold = 0.01f;

    private float rotationVelocity;


    public void Update() {
        //GetControllerLookInput();
    }

    public void LateUpdate() {
        CameraRotation();
    }

    public void CameraRotation() {

        if (look.sqrMagnitude >= threshold) {

            float deltaTimeMultiplier = Time.deltaTime;
            targetPitch += -look.y * RotationSpeed * deltaTimeMultiplier;
            targetPitch = ClampAngle(targetPitch, BottomClamp, TopClamp);
            rotationVelocity = look.x * RotationSpeed * deltaTimeMultiplier;


            // now use our values
            // Our Vertical Rotation
            mainCamera.transform.localRotation = Quaternion.Euler(targetPitch, 0.0f, 0.0f);
            //hand.transform.localRotation = Quaternion.Euler(targetPitch, 0.0f, 0.0f);

            // Horizontal Camera rotation applied to our base transform
            playerBase.transform.Rotate(Vector3.up * rotationVelocity);

        }

    }

    public void LookInputX(float newX) {
        look.x = newX;
    }

    public void LookInputY(float newY) {
        look.y = newY;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public void OnLookX(InputAction.CallbackContext context) {

        if (cursorInputForLook) {
            //Debug.Log($"Look Input X: {context.ReadValue<float>()}");
            LookInputX(context.ReadValue<float>());
        }

    }

    public void OnLookY(InputAction.CallbackContext context) {

        if (cursorInputForLook) {
            //Debug.Log($"Look Input Y: {context.ReadValue<float>()}");
            LookInputY(context.ReadValue<float>());

        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lfAngle"></param>
    /// <param name="lfMin"></param>
    /// <param name="lfMax"></param>
    /// <returns></returns>
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax) {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    /// <summary>
    /// Locks our cursor to the screen
    /// </summary>
    /// <param name="hasFocus"></param>
    private void OnApplicationFocus(bool hasFocus) {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState) {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }


    private void OnDisable() {
        SetCursorState(!cursorLocked);
    }

}
