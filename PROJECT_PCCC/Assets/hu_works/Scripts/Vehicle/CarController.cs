using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;
    public Vector3 wheelsOffSet;
    // Settings
    [SerializeField] private float motorForce, breakForce, maxSteerAngle;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0); // Hạ thấp trọng tâm để tăng độ ổn định
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();

        // Áp dụng thanh cân bằng
        ApplyStabilizerBar(frontLeftWheelCollider, frontRightWheelCollider);
        ApplyStabilizerBar(rearLeftWheelCollider, rearRightWheelCollider);
    }

    private void GetInput()
    {
        // Steering Input
        horizontalInput = Input.GetAxis("Horizontal");

        // Acceleration Input
        verticalInput = Input.GetAxis("Vertical");

        // Breaking Input
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;

        float speed = GetComponent<Rigidbody>().linearVelocity.magnitude; // Lấy tốc độ hiện tại
        float adjustedBrakeForce = Mathf.Lerp(breakForce, breakForce / 2, speed / 50f); // Giảm lực phanh ở tốc độ cao
        currentbreakForce = isBreaking ? adjustedBrakeForce : 0f;

        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        float speed = GetComponent<Rigidbody>().linearVelocity.magnitude; // Lấy tốc độ hiện tại
        float adjustedSteerAngle = Mathf.Lerp(maxSteerAngle, maxSteerAngle / 2, speed / 50f); // Giảm góc lái khi tốc độ tăng
        currentSteerAngle = adjustedSteerAngle * horizontalInput;

        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);

        // Áp dụng offset vào vị trí và góc quay của bánh xe
        wheelTransform.position = pos;
        wheelTransform.rotation = rot * Quaternion.Euler(wheelsOffSet); 
    }

    private void ApplyStabilizerBar(WheelCollider leftWheel, WheelCollider rightWheel)
    {
        WheelHit hit;
        float travelLeft = 1.0f;
        float travelRight = 1.0f;

        if (leftWheel.GetGroundHit(out hit))
        {
            travelLeft = (-leftWheel.transform.InverseTransformPoint(hit.point).y - leftWheel.radius) / leftWheel.suspensionDistance;
        }
        if (rightWheel.GetGroundHit(out hit))
        {
            travelRight = (-rightWheel.transform.InverseTransformPoint(hit.point).y - rightWheel.radius) / rightWheel.suspensionDistance;
        }

        float antiRollForce = (travelLeft - travelRight) * 5000f; // Điều chỉnh giá trị lực cân bằng

        if (leftWheel.isGrounded)
        {
            GetComponent<Rigidbody>().AddForceAtPosition(leftWheel.transform.up * -antiRollForce, leftWheel.transform.position);
        }
        if (rightWheel.isGrounded)
        {
            GetComponent<Rigidbody>().AddForceAtPosition(rightWheel.transform.up * antiRollForce, rightWheel.transform.position);
        }
    }
}