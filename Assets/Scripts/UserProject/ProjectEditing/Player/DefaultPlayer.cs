using System;
using System.Collections.Generic;
using SimpleInputNamespace;
using UnityEngine;

public class DefaultPlayer : MonoBehaviour
{
    [Header("Scene Management")]
    public bool isRuntime = false;
    public List<GameObject> playerRuntimeObjects;

    [Header("Player Settings")]
    public float speed = 5f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;
    public float lookSensitivity = 2f;

    [Header("Joystick Settings")]
    [SerializeField] private Joystick moveJoystick;
    [SerializeField] private Joystick lookJoystick;
    [SerializeField] private Transform cameraTransform;
    public CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;
    void Update()
    {
        if (isRuntime)
        {
            SetActiveObjects(playerRuntimeObjects, true);
            PlayerMovement();
            PlayerLook();
            PlayerGravity();

            isGrounded = characterController.isGrounded;
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
            velocity.y += gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }
        else
        {
            SetActiveObjects(playerRuntimeObjects, false);
            return;
        }
    }

    private void SetActiveObjects(List<GameObject> objects, bool isActive)
    {
        foreach (var obj in objects)
        {
            if (obj != null)
            {
                obj.SetActive(isActive);
            }
        }
    }

    public void PlayerMovement()
    {
        if (!moveJoystick || !lookJoystick || !cameraTransform)
        {
            Debug.LogWarning("Missing joystick or camera transform reference");
            return;
        }

        Vector2 moveInput = moveJoystick.Value;
        Vector2 lookInput = lookJoystick.Value;

        Vector3 moveDirection = cameraTransform.forward * moveInput.y + cameraTransform.right * moveInput.x;
        moveDirection.y = 0;
        moveDirection.Normalize();

        Vector3 movement = moveDirection * speed * Time.deltaTime;

        characterController.Move(movement);
    }
    public void PlayerLook()
    {
        if (!lookJoystick || !cameraTransform)
        {
            Debug.LogWarning("Missing joystick or camera transform reference");
            return;
        }

        Vector2 lookInput = lookJoystick.Value;

        float rotationX = lookInput.y * lookSensitivity * Time.deltaTime;
        float rotationY = lookInput.x * lookSensitivity * Time.deltaTime;

        Vector3 currentRotation = cameraTransform.rotation.eulerAngles;

        float newRotationX = currentRotation.x - rotationX;

        cameraTransform.rotation = Quaternion.Euler(
            newRotationX,
            currentRotation.y + rotationY,
            0f
        );
    }
    public void PlayerJump()
    {
        if (characterController.isGrounded)
        {
            velocity.y = jumpForce;
        }
        else
        {
            Debug.LogWarning("Player is not grounded, cannot jump.");
        }
    }

    public void PlayerGravity()
    {
        if (!characterController.isGrounded)
        {
            Vector3 gravityVector = new Vector3(0, gravity * Time.deltaTime, 0);
            characterController.Move(gravityVector);
        }
    }

}
