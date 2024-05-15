using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Swing : MonoBehaviour
{
    public Transform startSwingHand;
    public float maxDistance = 35;
    public LayerMask SwingableLayer;
    private bool hasHit;
    public Transform predictionPoint;
    private Vector3 swingPoint;
    // Start is called before the first frame update

    public InputActionProperty swingAction;
    public Rigidbody playerRigidBody;
    private SpringJoint joint;

    public LineRenderer lr;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetSwingPoint();

        if(swingAction.action.WasPressedThisFrame())
        {
            StartSwing();
        }
        else if (swingAction.action.WasReleasedThisFrame())
        {
            StopSwing();
        }
        DrawRope();
    }


    public void StartSwing()
    {

        if (hasHit)
        {
            joint = playerRigidBody.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = swingPoint;

            float distance = Vector3.Distance(playerRigidBody.position, swingPoint);
            joint.maxDistance = distance;
            joint.spring = 4.5f;
            joint.damper = 7;
            joint.massScale = 4.5f;

        }

    }

    public void StopSwing()
    {
        Destroy(joint);

    }

    public void GetSwingPoint()
    {

        if (joint)
            {
            predictionPoint.gameObject.SetActive(false);
            return;
        }

        RaycastHit raycastHit;
        hasHit = Physics.Raycast(startSwingHand.position, startSwingHand.forward,out raycastHit ,maxDistance,SwingableLayer) ;

        if (hasHit)
        {
            swingPoint = raycastHit.point;
            predictionPoint.gameObject.SetActive(true);
            predictionPoint.position = swingPoint;

        }
        else
            predictionPoint.gameObject.SetActive(false);
    }

    public void DrawRope()
    {
        if(!joint)
        {
            lr.enabled = false;

        }
        else
        {


            lr.enabled = true;
            lr.positionCount = 2;
            lr.SetPosition(0, startSwingHand.position);
            lr.SetPosition(1, swingPoint);
        }

    }
}
