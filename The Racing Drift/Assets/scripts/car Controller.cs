using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class carController : MonoBehaviour
{
    //data
    public CarStats carStats;
    private float smoothTime = 0.01f;
    public Rigidbody rb;

    //Inputs
    private float turningInput;
    private float gassInput;
    private float gearInput;
    private bool input1 = false;
    private bool HandBrake = false;
    private bool nos = false;

    //wheel data
    public List<wheelinfo> wheels = new List<wheelinfo>();
    public float boost = 3;
    private float brakeTorque = 200;
    private float steeringR = 8;
    public float WB = 3.2f;
    public float TW = 2.4f;

    //engien
    public float totalPower;
    public float KPH;
    public float wheelsRPM;
    public AnimationCurve EnginenTorque;
    public float engineRPM;
    public float[] gears = new float[6];
    public int gearNum = 0;

    //drift
    private float tempo;
    private float handBrakeFriction = 0.05f;
    public float driftMultiplier = 5;
    private WheelFrictionCurve forwardFriction, sidewaysFriction;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        setStearingR();
        setBrakeTorque();
        setBoost();
        setDrift();
    }

    //car stats that affects variables
    private void setStearingR()
    {
        for (int i = 0; i <= 10; i++)
        {
            if (i != 0)
            {
                steeringR -= 0.4f;
            }
        }
    }
    private void setBrakeTorque()
    {
        for(int i = 0;i <= carStats.Braking; i++)
        {
            if (i != 0)
            {
                brakeTorque += 4.5f;
            }
        }
    }
    private void setBoost()
    {
        boost += carStats.Boost;
    }
    private void setDrift()
    {
        for (int i = 0; i <= carStats.Drift; i++)
        {
            if (i != 0)
            {
                driftMultiplier -= 0.4f;
            }
        }
    }

    //Inputs
    public void gass(InputAction.CallbackContext ctxGass)
    {
        bool NotPressed = ctxGass.ReadValue<float>() == default;
        if (NotPressed == false)
        {
            input1 = true;
            gassInput = ctxGass.ReadValue<float>();

        }
        else
        {
            input1 = false;
            gassInput = 0;
        }

    }
    public void turn(InputAction.CallbackContext ctxTurn)
    {
        bool NotPressed = ctxTurn.ReadValue<float>() == default;
        if (NotPressed == false)
        {
            turningInput = ctxTurn.ReadValue<float>();
        }
        else
        {
            turningInput = 0;
        }
    }
    public void handbrake(InputAction.CallbackContext ctxHandbrake)
    {
        bool NotPressed = ctxHandbrake.ReadValue<float>() == default;
        if (NotPressed == false)
        {
            HandBrake = true;
        }
        else
        {
            HandBrake = false;
        }
    }
    public void NOS(InputAction.CallbackContext ctxNOS)
    {
        bool NotPressed = ctxNOS.ReadValue<float>() == default;
        if (NotPressed == false)
        {
            nos = true;
        }
        else
        {
            nos = false;
        }
    }
    public void gear(InputAction.CallbackContext ctxGear) 
    {
        if ((ctxGear.ReadValue<float>() == default) == false) 
        {
            gearInput = ctxGear.ReadValue<float>();
        }
        else
        {
            gearInput = 0;
        }
    }

    private void FixedUpdate()
    {
        
        SteerCar();
        BrakeCar();
        CalculateEnginePower();
        gear();
        checkWheelSpin();
        adjusTraction();
    }

    //controls
    private void MoveCar()
    {
        foreach (wheelinfo wheel in wheels)
        {
            if (wheel.motor)
            {
                if (input1 == true)
                {
                    wheel.leftWheel.motorTorque = (totalPower / 2);
                    wheel.rightWheel.motorTorque = (totalPower / 2);
                }
                else
                {
                    wheel.leftWheel.motorTorque = 0;
                    wheel.rightWheel.motorTorque = 0;
                }
                if (HandBrake == true)
                {
                    wheel.leftWheel.brakeTorque = brakeTorque;
                    wheel.rightWheel.brakeTorque = brakeTorque;
                }
                else
                {
                    wheel.leftWheel.brakeTorque = 0;
                    wheel.rightWheel.brakeTorque = 0;
                }
                //Nitrous Oxide System 
                if (nos == true)
                {
                    wheel.leftWheel.motorTorque = (totalPower / 2) * boost;
                    wheel.rightWheel.motorTorque = (totalPower / 2) * boost;
                }
            }
        }
        KPH = rb.velocity.magnitude * 3.6f;
    }
    private void SteerCar()
    {
        foreach (wheelinfo wheel in wheels)
        {
            if (wheel.steering)
            {
                if (turningInput > 0)
                {
                    wheel.leftWheel.steerAngle = Mathf.Rad2Deg * Mathf.Atan(WB / (steeringR + (TW / 2))) * turningInput;
                    wheel.rightWheel.steerAngle = Mathf.Rad2Deg * Mathf.Atan(WB / (steeringR - (TW / 2))) * turningInput;
                }
                else if (turningInput < 0)
                {
                    wheel.leftWheel.steerAngle = Mathf.Rad2Deg * Mathf.Atan(WB / (steeringR - (TW / 2))) * turningInput;
                    wheel.rightWheel.steerAngle = Mathf.Rad2Deg * Mathf.Atan(WB / (steeringR + (TW / 2))) * turningInput;
                }
                else
                {
                    wheel.leftWheel.steerAngle = turningInput;
                    wheel.rightWheel.steerAngle = turningInput;
                }
            }
        }
    }
    private void BrakeCar()
    {
        foreach (wheelinfo wheel in wheels)
        {
            if (wheel.backwheels)
            {
                if (HandBrake == true)
                {
                    wheel.leftWheel.brakeTorque = brakeTorque;
                    wheel.rightWheel.brakeTorque = brakeTorque;
                }
                else
                {
                    wheel.leftWheel.brakeTorque = 0;
                    wheel.rightWheel.brakeTorque = 0;
                }
            }
        }
    }

    //engien
    private void CalculateEnginePower()
    {
        wheelRPM();
        totalPower = (EnginenTorque.Evaluate(engineRPM) + (carStats.Power * 2)) * (gears[gearNum]) * gassInput;
        float velocety = 0;
        engineRPM = Mathf.SmoothDamp(engineRPM, 1000 + (Mathf.Abs(wheelsRPM) * (gears[gearNum])), ref velocety, smoothTime);
        MoveCar();
    }
    private void wheelRPM()
    {
        float sum = 0;
        int R = 0;
        foreach (wheelinfo wheel in wheels)
        {
            if (wheel.motor)
            {
                sum += wheel.leftWheel.rpm;
                R++;
                sum += wheel.rightWheel.rpm;
                R++;
            }
        }
        wheelsRPM = (R != 0) ? sum / R : 0;
    }
    private void gear()
    {
        if (gearInput == 1)
        {
            if (gearNum < 4)
            {
                gearNum++;
            }
        }
        else if (gearInput == -1)
        {
            if (gearNum > 0)
            {
                gearNum--;
            }
        }
        gearInput = 0;
    }

    //Drift
    private void checkWheelSpin()
    {
        foreach (wheelinfo wheel in wheels)
        {
            WheelHit wheelHit = new WheelHit();
            if (wheel.backwheels) 
            {
                wheel.rightWheel.GetGroundHit(out wheelHit);
                wheel.leftWheel.GetGroundHit(out wheelHit);
            }
            if (wheelHit.sidewaysSlip < 0)
            {
                
                tempo = (1 + -gassInput) * Mathf.Abs(wheelHit.sidewaysSlip * driftMultiplier);
            }
            if (wheelHit.sidewaysSlip > 0)
            {
                tempo = (1 + gassInput) * Mathf.Abs(wheelHit.sidewaysSlip * driftMultiplier);
            }
            if (wheelHit.sidewaysSlip > 1.5f || wheelHit.sidewaysSlip < -1.5f)
            {
                float velocity = 0;
                handBrakeFriction = Mathf.SmoothDamp(handBrakeFriction, tempo * 3, ref velocity, 0.1f * Time.deltaTime);
            }
            else
            {
                handBrakeFriction = tempo;
            }
        }
    }
    
    private void adjusTraction()
    {
        if (HandBrake == false)
        {
            foreach (wheelinfo wheel in wheels)
            {
                if (wheel.backwheels)
                {
                    forwardFriction = wheel.leftWheel.forwardFriction;
                    sidewaysFriction = wheel.leftWheel.sidewaysFriction;
                }
            }
            forwardFriction.extremumValue = forwardFriction.asymptoteValue = ((KPH * driftMultiplier) / 300) + 1;
            sidewaysFriction.extremumValue = sidewaysFriction.asymptoteValue = ((KPH * driftMultiplier) / 300) + 1;
            foreach (wheelinfo wheel in wheels)
            {
                wheel.leftWheel.forwardFriction = forwardFriction;
                wheel.rightWheel.forwardFriction = forwardFriction;
                wheel.leftWheel.sidewaysFriction = sidewaysFriction;
                wheel.rightWheel.sidewaysFriction = sidewaysFriction;
            }
        }
        else if (HandBrake == true)
        {
            foreach (wheelinfo wheel in wheels)
            {
                if (wheel.backwheels)
                {
                    forwardFriction = wheel.leftWheel.forwardFriction;
                    sidewaysFriction = wheel.leftWheel.sidewaysFriction;
                }
            }
            
            float velocity = 0;

            sidewaysFriction.extremumValue = sidewaysFriction.asymptoteValue = Mathf.SmoothDamp(sidewaysFriction.asymptoteValue, handBrakeFriction, ref velocity, 0.05f * Time.deltaTime);
            forwardFriction.extremumValue = forwardFriction.asymptoteValue = Mathf.SmoothDamp(forwardFriction.asymptoteValue, handBrakeFriction, ref velocity, 0.05f * Time.deltaTime);

            foreach (wheelinfo wheel in wheels)
            {
                if (wheel.backwheels)
                {
                    wheel.leftWheel.forwardFriction = forwardFriction;
                    wheel.leftWheel.sidewaysFriction = sidewaysFriction;
                    wheel.rightWheel.forwardFriction = forwardFriction;
                    wheel.rightWheel.sidewaysFriction = sidewaysFriction;
                }
            }
            forwardFriction.extremumValue = forwardFriction.asymptoteValue = 1.1f;
            sidewaysFriction.extremumValue = sidewaysFriction.asymptoteValue = 1.1f;
            foreach (wheelinfo wheel in wheels)
            {
                if (wheel.forontWheels)
                {
                    wheel.leftWheel.forwardFriction = forwardFriction;
                    wheel.leftWheel.sidewaysFriction = sidewaysFriction;
                    wheel.rightWheel.forwardFriction = forwardFriction;
                    wheel.rightWheel.sidewaysFriction = sidewaysFriction;
                }
            }
        }
    }
}

[System.Serializable]
public class wheelinfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
    public bool backwheels;
    public bool forontWheels;
}


