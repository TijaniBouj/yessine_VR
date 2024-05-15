using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizedMovement : MonoBehaviour
{
    // Range for random speed
    public float minSpeed = 1.0f;
    public float maxSpeed = 20.0f;

    // Range for random rotation speed
    public float minRotationSpeed = 70.0f;
    public float maxRotationSpeed = 150.0f;

        // Range for random velocity
    public float minVelocity = 30.0f;
    public float maxVelocity = 100.0f;

    private float speed;
    private float rotationSpeed;
    private float velocity;

    void Start()
    {
        // Randomize initial speed and rotation speed
        speed = Random.Range(minSpeed, maxSpeed);
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        velocity = Random.Range(minVelocity, maxVelocity);
    }

    void Update()
    {
        // Move the object forward along the x-axis
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // Rotate the object around its up axis (y-axis) and right axis (x-axis)
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        transform.Rotate(Vector3.right * velocity * Time.deltaTime);
    }
}

