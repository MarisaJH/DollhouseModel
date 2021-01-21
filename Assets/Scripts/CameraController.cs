using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using CielaSpike;

public class CameraController : MonoBehaviour
{
    //public Camera firstPersonCamera;
    //private Camera overheadCamera;
	public List<Camera> cameras;
	public int port;
    private UdpClient client;
	private byte[] data = new byte[4];
	private bool update_cam = false;

    void Start()
    {	/*
        overheadCamera = Camera.main;
        overheadCamera.enabled = true;
        firstPersonCamera.enabled = false;
		*/
		EnableCamera(0);
		//StartCoroutine(ReceiveData());
		this.StartCoroutineAsync(ReceiveData());
    }

    void Update()
    {
		if (update_cam) {
			if (BitConverter.IsLittleEndian) {
				Array.Reverse(data);
			}
			
			int i = BitConverter.ToInt32(data, 0);
		    EnableCamera(i);
			update_cam = false;
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			EnableCamera(1);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2)) {
			EnableCamera(2);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3)) {
			EnableCamera(3);
		}
		else if (Input.GetKeyDown(KeyCode.O)) {
			EnableCamera(0);
		}
		else if (Input.GetKeyDown(KeyCode.R)) {
			EnableCamera(4);
		}
    }
	
	private void EnableCamera(int i) {
		foreach (Camera cam in cameras) {
			cam.enabled = false;
		}
		cameras[i].enabled = true;
		Debug.Log("enabled " + i.ToString());
	}
	
	IEnumerator ReceiveData()
    {
        Debug.Log("in ReceiveData()");
        client = new UdpClient(port);
        while (true)
        {
 
            try
            {
                // get data in bytes
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                data = client.Receive(ref anyIP);
				Debug.Log("received data");
				Debug.Log(BitConverter.ToString(data));
				update_cam = true;
				//int i = BitConverter.ToInt32(data, 0);
				//EnableCamera(i);
               
            }
            catch (Exception err)
            {
                Debug.Log(err.ToString());
            }
            yield return null;
        }
    }
}
