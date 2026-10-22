using UnityEngine;
using UnityEngine.InputSystem;

public class CubeMovement : MonoBehaviour
{
    public float speed = 5f;
    public float turnSpeed = 15f;
    public CameraFollow cameraFollow;   // drag the Main Camera here

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        var kb = Keyboard.current;
        Vector2 input = Vector2.zero;
        if (kb != null)
        {
            if (kb.wKey.isPressed) input.y += 1;
            if (kb.sKey.isPressed) input.y -= 1;
            if (kb.dKey.isPressed) input.x += 1;
            if (kb.aKey.isPressed) input.x -= 1;
        }

        // Direction relative to where the camera is facing (flattened to the ground)
        Quaternion camYaw = cameraFollow != null ? cameraFollow.YawRotation : Quaternion.identity;
        Vector3 moveDir = camYaw * new Vector3(input.x, 0f, input.y);
        if (moveDir.sqrMagnitude > 1f) moveDir.Normalize();

        // Move (preserve gravity on Y)
        Vector3 velocity = moveDir * speed;
        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);

        // Rotate player to face movement direction — only when actually moving
        if (moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, turnSpeed * Time.fixedDeltaTime));
        }
    }
}