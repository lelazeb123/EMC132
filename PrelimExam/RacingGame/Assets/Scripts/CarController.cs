using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    public WheelCollider[] wheels = new WheelCollider[4];
    public GameObject[] WheelMesh = new GameObject[4];
    public int motorTorque = 200;
    public float steeringMax = 30;
    public float radius = 10;
    public float brake = 5000;
    public float DownForceValue = 100;
    
    private Vector3 centerOfMass;
    private Rigidbody rb;
    private Inputs inputManager;

    void Start()
    {
        getObjects();
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass;
    }

    private void FixedUpdate()
    {
        Drive();
        Animate();
        Steering();
        Downforce();
    }

    // Car controls, driving
    private void Drive()
    {
        
        for (int i = 0; i < wheels.Length; i++){
             wheels[i].motorTorque = inputManager.vertical * motorTorque;
        }

        if (inputManager.handbrake){
            wheels[3].brakeTorque = wheels[2].brakeTorque = brake;
        } else {
            wheels[3].brakeTorque = wheels[2].brakeTorque = 0;
        }
         
    }

    // Ackerman steering formula, for steering during high speeds.
    private void Steering()
    {
        if (inputManager.horizontal > 0) {
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 3))) * inputManager.horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 3))) * inputManager.horizontal;
        } else if (inputManager.horizontal < 0) {
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 3))) * inputManager.horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 3))) * inputManager.horizontal;
        } else {
            wheels[0].steerAngle = 0;
            wheels[1].steerAngle = 0;
        }
    }

    // Animate the wheels
    private void Animate()
    {
        Vector3 wheelPosition = Vector3.zero;
        Quaternion wheelRotation = Quaternion.identity;

        for (int i = 0; i < 4; i++)
        {
            wheels [i].GetWorldPose (out wheelPosition, out wheelRotation);
            WheelMesh [i].transform.position = wheelPosition;
            WheelMesh [i].transform.rotation = wheelRotation;
        }
    }

    // Get inputs
    private void getObjects()
    {
        inputManager = GetComponent<Inputs>();
    }

    // Creates downforce, increase ground grip 
    private void Downforce()
    {
        rb.AddForce(-transform.up * DownForceValue * rb.velocity.magnitude);
    }

}
