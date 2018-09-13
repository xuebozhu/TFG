﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using SocketIO;
using System;
using System.Security.AccessControl;

public class CommandServer : MonoBehaviour
{
	public CarRemoteControl CarRemoteControl;
	public Camera FrontFacingCamera;
	private SocketIOComponent _socket;
	private MSVehicleControllerFree _carController;

	// Use this for initialization
	void Start()
	{
		_socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();
		_socket.On("open", OnOpen);
		_socket.On("steer", OnSteer);
		_socket.On("manual", onManual);
		_carController = CarRemoteControl.GetComponent<MSVehicleControllerFree>();
	}

	// Update is called once per frame
	void Update()
	{
	}

	void OnOpen(SocketIOEvent obj)
	{
		Debug.Log("Connection Open");
		EmitTelemetry(obj);
	}

	// 
	void onManual(SocketIOEvent obj)
	{
		EmitTelemetry (obj);
	}

	void OnSteer(SocketIOEvent obj)
	{
		JSONObject jsonObject = obj.data;
		//    print(float.Parse(jsonObject.GetField("steering_angle").str));
		CarRemoteControl.SteeringAngle = float.Parse(jsonObject.GetField("steering_angle").str);
		CarRemoteControl.Acceleration = float.Parse(jsonObject.GetField("throttle").str);
		EmitTelemetry(obj);
	}

	void EmitTelemetry(SocketIOEvent obj)
	{
		UnityMainThreadDispatcher.Instance().Enqueue(() =>
		{
			//print("Attempting to Send...");
			// send only if it's not being manually driven


			// Collect Data from the Car
			Dictionary<string, string> data = new Dictionary<string, string>();
			data["steering_angle"] = _carController.angleRefVolant.ToString("N4");
			data["throttle"] = _carController.verticalInput.ToString("N4");
			data["speed"] = _carController.KMh.ToString("N4");
			data["image"] = Convert.ToBase64String(CaptureFrame(FrontFacingCamera));
			_socket.Emit("telemetry", new JSONObject(data));
			
		});

		//    UnityMainThreadDispatcher.Instance().Enqueue(() =>
		//    {
		//      	
		//      
		//
		//		// send only if it's not being manually driven
		//		if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.S))) {
		//			_socket.Emit("telemetry", new JSONObject());
		//		}
		//		else {
		//			// Collect Data from the Car
		//			Dictionary<string, string> data = new Dictionary<string, string>();
		//			data["steering_angle"] = _carController.CurrentSteerAngle.ToString("N4");
		//			data["throttle"] = _carController.AccelInput.ToString("N4");
		//			data["speed"] = _carController.CurrentSpeed.ToString("N4");
		//			data["image"] = Convert.ToBase64String(CameraHelper.CaptureFrame(FrontFacingCamera));
		//			_socket.Emit("telemetry", new JSONObject(data));
		//		}
		//      
		////      
		//    });
	}
  public byte[] CaptureFrame(Camera camera)
  {
	RenderTexture targetTexture = camera.targetTexture;
	RenderTexture.active = targetTexture;
	Texture2D texture2D = new Texture2D (320, 160, TextureFormat.RGB24, false);
	texture2D.ReadPixels (new Rect (0, 0, 320, 160), 0, 0);
	texture2D.Apply ();
	byte[] image = texture2D.EncodeToJPG ();	
	DestroyImmediate(texture2D); // Required to prevent leaking the texture
	return image;
  }
	
}