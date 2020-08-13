using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointer_test : MonoBehaviour {

    private void Start()
    {
        Debug.Log(Screen.width);
        Debug.Log(Screen.height);
        Debug.Log(trialManager.started);
        Debug.Log(endArea_trial.isPointerOut);
    }
    // Update is called once per frame
    void Update () {
        Vector3 mousePos = Input.mousePosition;
        mousePos = new Vector3(mousePos.x/Screen.width - .5f, mousePos.y/Screen.height - .5f);
        mousePos = new Vector3(mousePos.x * experiment_specs.CameraWidth, mousePos.y * experiment_specs.CameraHeight, 1f);

        this.transform.position = mousePos;
    }
}
