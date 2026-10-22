using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    public Transform target;                 // drag the cube here

    [Header("Mode")]
    public bool firstPerson = false;

    [Header("Look")]
    public float mouseSensitivity = 0.1f;
    public float minPitch = -40f;
    public float maxPitch = 75f;

    [Header("Third person")]
    public float distance = 6f;
    public Vector3 thirdPersonPivotOffset = new Vector3(0f, 1.5f, 0f);

    [Header("First person")]
    public Vector3 firstPersonOffset = new Vector3(0f, 0.6f, 0f);

    private float yaw;
    private float pitch;

    // Flattened camera facing, used by the movement script
    public Quaternion YawRotation => Quaternion.Euler(0f, yaw, 0f);

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   // hide + lock cursor
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void Update()
    {
        var kb = Keyboard.current;
        if (kb != null && kb.vKey.wasPressedThisFrame)
            firstPerson = !firstPerson;

        if (kb != null && kb.escapeKey.wasPressedThisFrame)
            Cursor.lockState = CursorLockMode.None;   // free the cursor to click in editor

        var mouse = Mouse.current;
        if (mouse != null)
        {
            Vector2 d = mouse.delta.ReadValue() * mouseSensitivity;
            yaw += d.x;
            pitch -= d.y;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        if (firstPerson)
        {
            transform.position = target.position + firstPersonOffset;
            transform.rotation = rotation;
        }
        else
        {
            Vector3 pivot = target.position + thirdPersonPivotOffset;
            transform.position = pivot - rotation * Vector3.forward * distance;
            transform.rotation = rotation;
        }
    }
}