using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_settings : MonoBehaviour {

    public Camera room_409;

	// Use this for initialization
	void Start () {
        room_409.enabled = true;
        room_409.orthographicSize = experiment_specs.CameraSize;
    }
}
