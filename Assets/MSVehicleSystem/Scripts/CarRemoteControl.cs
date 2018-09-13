using System;
using UnityEngine;


[RequireComponent(typeof(MSVehicleControllerFree))]
public class CarRemoteControl : MonoBehaviour
{
	private MSVehicleControllerFree m_Car; // the car controller we want to use

	public float SteeringAngle { get; set; }
	public float Acceleration { get; set; }

	private void Awake()
	{
		// get the car controller
		m_Car = GetComponent<MSVehicleControllerFree>();
	}

	private void FixedUpdate()
	{
		m_Car.Move(SteeringAngle, Acceleration, Acceleration, 0f);
	}
}
