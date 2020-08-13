using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class returnSlice_trial : MonoBehaviour {

	private Vector3 mousePos;
	Transform[] quadrants = new Transform[4];
    bool activateSlices = false;
    bool showSlices = false;
    float timeDelay = .3f;
    float timer = 0;

	// Use this for initialization
	void Start ()
	{
		// get children
		for (int c = 0; c < 4; c++) {
			quadrants[c] = this.transform.GetChild(c);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
        // TODO improve return slices
		mousePos = new Vector3 (Input.mousePosition.x / Screen.width * 32, Input.mousePosition.y / Screen.height * 24, 0) - new Vector3 (16f, 12f, 0);
        float mouseDist = Vector3.Distance(mousePos, Vector3.zero);

        for (int c = 0; c < 4; c++) {
			quadrants [c].transform.position = new Vector3 (0,0,11);
		}

        activateSlices = !trialManager.isPointerVisible && endArea_trial.isPointerOut;
        if (activateSlices)
        {
            timer += Time.deltaTime;
            if (timer > timeDelay)
                showSlices = true;
        }

        if (showSlices & activateSlices)
        {
            int quadrantIndex = 0;
            if (Mathf.Sign(mousePos.x) == 1 && Mathf.Sign(mousePos.y) == 1)
            {
                quadrantIndex = 0;
            }
            else if (Mathf.Sign(mousePos.x) == 1 && Mathf.Sign(mousePos.y) == -1)
            {
                quadrantIndex = 1;
            }
            else if (Mathf.Sign(mousePos.x) == -1 && Mathf.Sign(mousePos.y) == -1)
            {
                quadrantIndex = 2;
            }
            else if (Mathf.Sign(mousePos.x) == -1 && Mathf.Sign(mousePos.y) == 1)
            {
                quadrantIndex = 3;
            }
            quadrants[quadrantIndex].transform.position = new Vector3(0, 0, 0);
            if (mouseDist <= targetSpawner.targetDistance + 5f)
            {
                quadrants[quadrantIndex].transform.localScale = new Vector3(mouseDist, mouseDist, 0);
            } else
                quadrants[quadrantIndex].transform.localScale = new Vector3(targetSpawner.targetDistance + 5f, targetSpawner.targetDistance + 5f, 0);
        }
	}
}
