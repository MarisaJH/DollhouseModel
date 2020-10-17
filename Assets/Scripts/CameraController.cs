using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //public Camera firstPersonCamera;
    //private Camera overheadCamera;
	public List<Camera> cameras;

    private void Start()
    {	/*
        overheadCamera = Camera.main;
        overheadCamera.enabled = true;
        firstPersonCamera.enabled = false;
		*/
		EnableCamera(0);
    }

    private void Update()
    {/*
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (overheadCamera.enabled)
            {
                ShowFirstPersonView();
            }
            else
            {
                ShowOverheadView();
            }
        }*/
		
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
	/*
    // Call this function to disable FPS camera,
    // and enable overhead camera.
    public void ShowOverheadView()
    {
        firstPersonCamera.enabled = false;
        overheadCamera.enabled = true;
    }

    // Call this function to enable FPS camera,
    // and disable overhead camera.
    public void ShowFirstPersonView()
    {
        firstPersonCamera.enabled = true;
        overheadCamera.enabled = false;
    }*/
}
