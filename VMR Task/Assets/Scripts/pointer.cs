﻿using UnityEngine;
using System.Collections.Generic;

public class pointer : MonoBehaviour {

    public static string adaptationType = "C"; // C = clamp | V = veridical | "N" = No feedback
    public static float rotation;
    public static bool isVisible = false;
    public static bool moveWithMouse = true;
    public static float pointerTime = 0;
    public static bool hasStartedMoving = false;
    float mouseSpeed = 1;
    float mouseDist;
    bool pointerFeedback = false;
    public static Vector3 mousePos;
    public static List<float> mousePosX = new List<float>();
    public static List<float> mousePosY = new List<float>();

    // private variables
    public static SpriteRenderer pointerSpriteR;

    void Start()
    {
        pointerSpriteR = GetComponent<SpriteRenderer>();

		/*
        if(rotation == 0)
            pointerSpriteR.color = experiment_specs.pointer_color_baseline;
        else
            pointerSpriteR.color = experiment_specs.pointer_color_adaptation;
		*/
		pointerSpriteR.color = experiment_specs.pointer_color_adaptation;

        pointerTime = 0;
        isVisible = true;
        hasStartedMoving = false;

        // reset mouse positions
        mousePosX = new List<float>();
        mousePosY = new List<float>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, .5f);
    }

    void mouseVisibility(float _mouseDist)
    {
        if (!hasStartedMoving)
        {
            if (_mouseDist > 1f)
            {
                hasStartedMoving = true;
            }
        }
    }

    void Update () {

        // toggle visibility on/off
        pointerSpriteR.enabled = isVisible;

        bool takePointerTime = hasStartedMoving && !endArea_trial.isPointerOut && isVisible;

        if (takePointerTime)
        {
            pointerTime += Time.deltaTime;
        }
        
        // move with the mouse
        if (moveWithMouse)
        {
            mousePos = Input.mousePosition;
            mousePos = new Vector3(mousePos.x / Screen.width - .5f, mousePos.y / Screen.height - .5f);
            mousePos = new Vector3(mousePos.x * experiment_specs.CameraWidth, mousePos.y * experiment_specs.CameraHeight, -1f);
            mousePos = new Vector3(mousePos.x * mouseSpeed, mousePos.y * mouseSpeed);

            // calculate the distance between the mouse and the start
            mouseDist = Vector3.Distance(mousePos, Vector3.zero);

            if (mouseDist > targetSpawner.targetDistance && trialManager.isTrialStarted)
            {
                pointerSpriteR.color = new Color(pointerSpriteR.color.r, pointerSpriteR.color.g, pointerSpriteR.color.b, 0);
            }

            mousePosX.Add(mousePos.x);
            mousePosY.Add(mousePos.y);

            //Debug.Log(endArea_trial.isPointerOut);
            // cursor disappears as soon as it passes the target
            if (endArea_trial.isPointerOut)
            {
                // angle relative to center
                float mouseAngle = functions.SignedAngleBetween(Vector3.up, new Vector3(mousePos.x, mousePos.y, 0), -Vector3.forward);
                // reposition mousePos smack-bang at the target distance
                float mouseX = targetSpawner.targetDistance * Mathf.Sin(Mathf.Deg2Rad * mouseAngle);
                float mouseY = targetSpawner.targetDistance * Mathf.Cos(Mathf.Deg2Rad * mouseAngle);
                mousePos = new Vector3(mouseX, mouseY);
                mouseDist = targetSpawner.targetDistance;
                pointerSpriteR.color = new Color(pointerSpriteR.color.r, pointerSpriteR.color.g, pointerSpriteR.color.b, 1f);
            }

            mouseVisibility(mouseDist);

            switch (adaptationType)
            {
                case "V":
                    {
                        //if (mouseDist > 1f)
                        {
                            // move with the mouse
                            this.transform.position = mousePos;
                        }
                        break;
                    }
                case "R":
                    {
                        this.transform.position = mousePos;
                        transform.RotateAround(Vector3.zero, -Vector3.forward, rotation);
                        break;
                    }
                case "C":
                    {
                        //if (mouseDist > 1f)
                        {
                            // move the pointer upwards by mouseDist
                            this.transform.position = Vector3.up * mouseDist;
                            // rotate the pointer in the direction of the target + a radial offset (rotation)
                            transform.RotateAround(Vector3.zero, -Vector3.forward, targetSpawner.targetAngle + rotation);
                        }
                        break;
                    }
                case "N":
                    {
                        //if (mouseDist > 1f)
                        {
                            // move with the mouse
                            this.transform.position = new Vector3(mousePos.x, mousePos.y, -10f);
                        }
                        break;
                    }
            }
		}
    }
}
