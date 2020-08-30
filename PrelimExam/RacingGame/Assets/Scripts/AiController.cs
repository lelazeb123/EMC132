using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour
{

    public float maxSteerAngle = 45f;
    public Transform path;
    [Header("WheelColliders")]
    public WheelCollider FrontLeft;
    public WheelCollider FrontRight;
    public WheelCollider RearLeft;
    public WheelCollider RearRight;
    [Header("Engine")]
    public float maxMotorTorque = 50f;
    public float currentSpeed;
    public float maxSpeed = 50;
    public float DownForceValue = 100;
    public float brakingTorque = 9000f;
    public bool isBraking = false;


    private List<Transform> nodes;
    private int currentNode = 0;
    private Rigidbody rb;
    private Vector3 centerOfMass;
    


    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass;

        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != path.transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        AiSteer();
        AiDrive();
        PointDistance();
        AiDownforce();
        AiBraking();
    }
    
    private void AiSteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        FrontLeft.steerAngle = newSteer;
        FrontRight.steerAngle = newSteer;

    }

    private void AiDrive()
    {
        currentSpeed = 2 * Mathf.PI * FrontLeft.radius * FrontLeft.rpm * 60 / 1000;

        if (currentSpeed < maxSpeed && !isBraking)
        {
            FrontLeft.motorTorque = maxMotorTorque;
            FrontRight.motorTorque = maxMotorTorque;
        } else 
        {
            FrontLeft.motorTorque = 0;
            FrontRight.motorTorque = 0;
        }
        
    }

    // the Ai will follow the path created
    private void PointDistance() 
    {
        if(Vector3.Distance(transform.position, nodes[currentNode].position) < 5f)
        {
            if(currentNode == nodes.Count - 1)
            {
                currentNode = 0;
            } else {
                currentNode++;
            }
        }
    }

    // Creates downforce, increase ground grip on corners
    private void AiDownforce()
    {
        rb.AddForce(-transform.up * DownForceValue * rb.velocity.magnitude);
    }

    
    private void AiBraking()
    {
        if (isBraking){
            RearLeft.brakeTorque = brakingTorque;
            RearRight.brakeTorque = brakingTorque;
        } else {
            RearLeft.brakeTorque = 0;
            RearRight.brakeTorque = 0;
        }
    }

        
}  

