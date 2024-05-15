using UnityEngine;

public class PlayerGravity : MonoBehaviour
{
    public float gravity = 9.81f; // Adjust this to set the strength of gravity
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        ApplyGravity();
    }

    void ApplyGravity()
    {
        if (!controller.isGrounded)
        {
            Vector3 gravityVector = Vector3.down * gravity * Time.deltaTime;
            controller.Move(gravityVector);
        }
    }
}
