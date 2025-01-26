using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public Transform playerCamera;
    public FixedJoystick moveJoystick;
    public FloatingJoystick cameraJoystick;
    public float cameraSensitivity = 1.5f;
    public float speed = 6f;
    public float gravity = -10f;
    public Transform groundCheck;
    public LayerMask interactableLayer;
    public float raycastDistance = 2f;
    public Button dropButton;

    public GameObject defaultCursor;
    public GameObject interactCursor;

    private CharacterController controller;
    private float cameraCap = 0f;
    private Vector3 velocity;
    private bool isGrounded;

    private GameObject currentInteractable;
    private static bool objectInHand = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        dropButton.onClick.AddListener(DropObject);

        defaultCursor.SetActive(true);
        interactCursor.SetActive(false);
    }

    void Update()
    {
        UpdateCamera();
        UpdateMovement();
        CheckForInteractable();
    }

    void UpdateCamera()
    {
        float cameraInputX = cameraJoystick.Horizontal;
        float cameraInputY = cameraJoystick.Vertical;

        cameraCap -= cameraInputY * cameraSensitivity;
        cameraCap = Mathf.Clamp(cameraCap, -90f, 90f);

        playerCamera.localEulerAngles = Vector3.right * cameraCap;
        transform.Rotate(Vector3.up * cameraInputX * cameraSensitivity);
    }

    void UpdateMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f);

        float horizontal = moveJoystick.Horizontal;
        float vertical = moveJoystick.Vertical;

        Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;
        moveDirection.Normalize();

        velocity.x = moveDirection.x * speed;
        velocity.z = moveDirection.z * speed;

        if (isGrounded)
        {
            velocity.y = -2f;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    void CheckForInteractable()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.red);

        if (Physics.Raycast(ray, out hit, raycastDistance, interactableLayer))
        {
            GameObject interactableObject = hit.collider.gameObject;

            if (currentInteractable != interactableObject)
            {
                if (currentInteractable != null)
                {
                    interactCursor.SetActive(false);
                }

                currentInteractable = interactableObject;

                interactCursor.SetActive(true);
            }

            if (Input.GetMouseButtonDown(0) && !objectInHand)
            {
                currentInteractable.GetComponent<TakeOBJ>()?.Interact();
                objectInHand = true;
            }
        }
        else
        {
            if (currentInteractable != null)
            {
                currentInteractable = null;
                interactCursor.SetActive(false);
            }
            defaultCursor.SetActive(true);
        }
    }

    void DropObject()
    {
        if (currentInteractable != null)
        {
            currentInteractable.GetComponent<TakeOBJ>()?.DropItem();
            objectInHand = false;
        }
    }
}
