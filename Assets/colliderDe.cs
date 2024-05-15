using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colliderDe : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        // Get the GameObject that was collided with
        GameObject collidedObject = collision.gameObject;

        // Print out the name of the collided object
        Debug.Log("Collided with: " + collidedObject.name);


    }
}
