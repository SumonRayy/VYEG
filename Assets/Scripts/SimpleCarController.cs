using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCarController : MonoBehaviour
{
    public void GetInput()
    {
        Mem_horizontalInput = Input.GetAxis("Horizontal");
        Mem_verticalInput = Input.GetAxis("Vertical");
    }

    private void Steer()
    {
        Mem_steeringAngle = maxSteerAngle*Mem_horizontalInput;
        W_frontLeft.steerAngle = Mem_steeringAngle;
        W_frontRight.steerAngle = Mem_steeringAngle;
    }

    private void Accelerate()
    {
        W_frontRight.motorTorque = Mem_verticalInput * motorForce; 
        W_frontLeft.motorTorque = Mem_verticalInput * motorForce;
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(W_frontRight,   T_frontRight);
        UpdateWheelPose(W_frontLeft,   T_frontLeft);
        UpdateWheelPose(W_backRight,   T_backRight);
        UpdateWheelPose(W_backLeft,   T_backLeft);
    }

    private void UpdateWheelPose(WheelCollider  _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;

        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.rotation = _quat;
    }

    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPoses();
    }

    private float Mem_horizontalInput;
    private float Mem_verticalInput;
    private float Mem_steeringAngle;
    
    public WheelCollider W_frontRight , W_frontLeft;
    public WheelCollider W_backRight , W_backLeft;
    public Transform T_frontRight , T_frontLeft;
    public Transform T_backRight , T_backLeft;

    public float maxSteerAngle =  30;
    public float motorForce = 50;

}
