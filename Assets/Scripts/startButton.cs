using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class startButton : MonoBehaviour {

    public static bool dataRead = false;

    // Update is called once per frame
    void Update () {
        if (dataRead)
            gameObject.GetComponent<Image>().color = Color.red;
	}
}
