using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiWheelAnim : MonoBehaviour
{
    public WheelCollider targetWheel;
    
    private Vector3 wheelPosition = new Vector3();
    private Quaternion wheelRotation = new Quaternion();

    // Ai Wheels animation
    void Update()
    {
        targetWheel.GetWorldPose(out wheelPosition, out wheelRotation);
        transform.position = wheelPosition;
        transform.rotation = wheelRotation;
    }
}
