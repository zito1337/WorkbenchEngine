using SimpleInputNamespace;
using UnityEngine;

public class EditorCameraMovement : MonoBehaviour
{
    [Header("Joystick Settings")]
    [SerializeField] private Joystick moveJoystick;
    [SerializeField] private Joystick lookJoystick;
    [SerializeField] private Transform cameraTransform;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float verticalSpeed = 5f;

    private void Update()
    {
        if (!moveJoystick || !lookJoystick || !cameraTransform)
        {
            Debug.LogWarning("missing joystick or camera transform reference");
            return;
        }

        Vector2 moveInput = moveJoystick.Value;
        Vector2 lookInput = lookJoystick.Value;

        Vector3 moveDirection = Vector3.zero;
        if (moveJoystick.movementAxes == Joystick.MovementAxes.XandY)
        {
            moveDirection = cameraTransform.forward * moveInput.y + cameraTransform.right * moveInput.x;
        }
        else if (moveJoystick.movementAxes == Joystick.MovementAxes.X)
        {
            moveDirection = cameraTransform.right * moveInput.x;
        }
        else if (moveJoystick.movementAxes == Joystick.MovementAxes.Y)
        {
            moveDirection = cameraTransform.forward * moveInput.y;
        }

        moveDirection = moveDirection.normalized;

        Vector3 movement = moveDirection * moveSpeed * Time.deltaTime;

        float verticalInput = 0f;
        movement.y += verticalInput * verticalSpeed * Time.deltaTime;

        cameraTransform.position += movement;

        float rotationX = lookInput.y * rotationSpeed * Time.deltaTime;
        float rotationY = lookInput.x * rotationSpeed * Time.deltaTime;

        Vector3 currentRotation = cameraTransform.rotation.eulerAngles;

        float newRotationX = currentRotation.x - rotationX;

        cameraTransform.rotation = Quaternion.Euler(
            newRotationX,
            currentRotation.y + rotationY,
            0f
        );
    }
}