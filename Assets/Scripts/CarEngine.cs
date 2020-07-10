using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CarEngine : MonoBehaviour
{
    public Transform path;
    public float maxSteerAngle = 45f;
    public WheelCollider W_frontRight , W_frontLeft;
    public Transform T_frontRight , T_frontLeft;
    public float maxMotorTorque = 10f;
    public float currentSpeed;
    public float maxSpeed  = 10f;
    // public WheelCollider W_backRight , W_backLeft;
    private List<Transform> nodes;
    private int currentNode = 0;

    // Start is called before the first frame update
    void Start()
    {
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

    private void FixedUpdate()
    {
        ApplySteer();
        Drive();
        UpdateWheelPoses();
        CheckWayPointDistance();
    }

    private void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        //relativeVector = relativeVector / relativeVector.magnitude;
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        W_frontLeft.steerAngle = newSteer;
        W_frontRight.steerAngle = newSteer;

    }

    private void Drive()
    {
        currentSpeed = 2 * Mathf.PI * W_frontLeft.radius * W_frontRight.rpm * 60 / 1000;

        if(currentSpeed < maxSpeed){
        W_frontRight.motorTorque  = maxMotorTorque;
        W_frontLeft.motorTorque = maxMotorTorque;
        }else{
        W_frontRight.motorTorque  = 0;
        W_frontLeft.motorTorque = 0;            
        }

    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(W_frontRight,   T_frontRight);
        UpdateWheelPose(W_frontLeft,   T_frontLeft);
    }

    private void UpdateWheelPose(WheelCollider  _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;

        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.rotation = _quat;
    }

    private void CheckWayPointDistance()
    {
        if(Vector3.Distance(transform.position, nodes[currentNode].position) < 5f) {
            if (currentNode == nodes.Count - 1)
            {
                currentNode = 0;
            }else
            {
                currentNode++;
            }
        }
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
