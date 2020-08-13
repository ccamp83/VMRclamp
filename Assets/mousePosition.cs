using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mousePosition : MonoBehaviour
{
    public static Vector3 mousePos;
    public static float mouseSpeed = 1f;

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("mousePos");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        mousePos = new Vector3(mousePos.x / Screen.width - .5f, mousePos.y / Screen.height - .5f);
        mousePos = new Vector3(mousePos.x * experiment_specs.CameraWidth, mousePos.y * experiment_specs.CameraHeight, -1f);
        mousePos = new Vector3(mousePos.x * mouseSpeed, mousePos.y * mouseSpeed);
    }
}
