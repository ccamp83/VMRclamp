// the end area detects the position of the cursor relative to itself

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class endArea_trial : MonoBehaviour {

    AudioClip pointerOut;
	AudioClip badTrial;

    public static bool isPointerOut = false;
    public static float movementTime = 0;
    public static int movementSpeed = 0;
    public static int consecutive_wrong_trials = 0;

    // private
    float timePassed = 0;
    CircleCollider2D colliderOut;
    Text speed_alert;
    bool feedbackSent = false;

    private void Start()
    {
        movementTime = 0;
        movementSpeed = 0;

        colliderOut = GetComponent<CircleCollider2D>();
        colliderOut.radius = Vector3.Distance(targetSpawner.targetLocation, Vector3.zero) - .5f;

        pointerOut = Resources.Load<AudioClip>("Sounds/pointerOut");
        badTrial = Resources.Load<AudioClip>("Sounds/badTrial");

        feedbackSent = false;

        Instructions();
    }

    private static void Instructions()
    {
        if (!trialManager.experiment)
        {
            switch (trialManager.trainingTrialN)
            {
                case 4:
                    {
                        Text intro = GameObject.Find("Intro").GetComponent<Text>();
                        intro.color = Color.white;
                        intro.text = "We ask that you reach always with a certain speed.\nDuring the experiment, audio and visual prompts may pop up to help you maintain the correct speed.";
                        break;
                    }
                case 13:
                    {
                        Text intro = GameObject.Find("IntroLeft").GetComponent<Text>();
                        intro.color = Color.white;
                        intro.alignment = TextAnchor.UpperLeft;
                        intro.text = "<b>Move towards the LEFT with the mouse\nuntil the cursor hits the target.</b>";
                        break;
                    }
                case 14:
                    {
                        Text intro = GameObject.Find("IntroRight").GetComponent<Text>();
                        intro.color = Color.white;
                        intro.alignment = TextAnchor.UpperRight;
                        intro.text = "<b>Move towards the RIGHT with the mouse\nuntil the cursor hits the target.</b>";
                        break;
                    }
                case 15:
                    {
                        Text intro = GameObject.Find("IntroBottom").GetComponent<Text>();
                        intro.color = Color.white;
                        intro.alignment = TextAnchor.UpperCenter;
                        intro.text = "<b>Move towards the BOTTOM with the mouse until the cursor hits the target.</b>";
                        break;
                    }
                case 16:
                    {
                        Text intro = GameObject.Find("Intro").GetComponent<Text>();
                        intro.color = Color.white;
                        intro.text = "\nLet's practice a few times.";
                        break;
                    }
                case 20:
                    {
                        Text intro = GameObject.Find("Intro").GetComponent<Text>();
                        intro.color = Color.white;
                        intro.text =
                            "The cursor may or may not hit the target. Like before, because you don’t have control over its direction, there is nothing you can do to make the cursor hit the target." + 
                            "\n\nWe want you to ignore it and to continue to aim directly to the target.\n\nThe goal of this experiment is to see how accurately you can aim to the target despite seeing the cursor hit or miss it." + 
                            "\n\nLeft click on the white cursor to continue.";
                        break;
                    }
                case 24:
                    {
                        Text intro = GameObject.Find("Intro").GetComponent<Text>();
                        intro.color = Color.white;
                        intro.text = "\n\nFinally, towards the end of the experiment, the cursor will disappear altogether. As for the rest of the task, just keep aiming straight to the target as best as you can." +
                            "\n\nLeft click on the white cursor to continue.";
                        break;
                    }
            }
        
            
        }
    }

    AudioSource PlayClipAt(AudioClip clip, Vector3 pos, float volume = 1f, float pitch = 1f)
    {
        GameObject tempGO = new GameObject("TempAudio"); // create the temp object
        tempGO.transform.position = pos; // set its position
        AudioSource aSource = tempGO.AddComponent<AudioSource>(); // add an audio source
        aSource.clip = clip; // define the clip
        aSource.volume = volume;
        aSource.pitch = pitch;

        aSource.Play(); // start the sound
        Destroy(tempGO, clip.length); // destroy object after clip duration
        return aSource; // return the AudioSource reference
    }

    void OnTriggerExit2D (Collider2D trigger)
	{
        if (trialManager.isTrialStarted)
        {
            if (trialManager.movePointerWithMouse)
            {
                speedFeedback();

                isPointerOut = true;

                // freeze the pointer
                trialManager.movePointerWithMouse = false;

                if (trialManager.experiment || Array.Exists(new int[] { 16, 17, 18, 19, 20, 21, 22, 23 }, element => element == trialManager.trainingTrialN))
                {
                    // record the (true) endpoint
                    Vector3 mousePos = Input.mousePosition;
                    mousePos = new Vector3(mousePos.x / Screen.width - .5f, mousePos.y / Screen.height - .5f);
                    mousePos = new Vector3(mousePos.x * experiment_specs.CameraWidth, mousePos.y * experiment_specs.CameraHeight, 1f);

                    // angle relative to center
                    float mouseAngle = functions.SignedAngleBetween(Vector3.up, new Vector3(mousePos.x, mousePos.y, 0), -Vector3.forward);
                    // reposition mousePos smack-bang at the target distance
                    float mouseX = targetSpawner.targetDistance * Mathf.Sin(Mathf.Deg2Rad * mouseAngle);
                    float mouseY = targetSpawner.targetDistance * Mathf.Cos(Mathf.Deg2Rad * mouseAngle);
                    mousePos = new Vector3(mouseX, mouseY);

                    check_consecutive_wrong_trials(mousePos);

                    trialManager.endpointX = mousePos.x;
                    trialManager.endpointY = mousePos.y;
                }
                else
                {
                    switch (trialManager.trainingTrialN)
                    {
                        case 0:
                            {
                                Text intro = GameObject.Find("Intro").GetComponent<Text>();
                                intro.color = Color.white;
                                intro.text = "Once the cursor has passed the target distance, you may return to the center of the screen.\nWatch the yellow slices shrink as you approach the start position with the mouse.";
                                break;
                            }
                        case 4:
                            {
                                Text intro = GameObject.Find("Intro").GetComponent<Text>();
                                intro.color = Color.white;
                                intro.text =
                                    "If the target turns:\n" +
                                    "<color=#44BB99><b>green</b></color> = you moved at the right speed\n" +
                                    "<color=#EE8866><b>orange</b></color> = you moved too fast\n" +
                                    "<color=#77AADD><b>purple</b></color> = you moved too slow";
                                break;
                            }
                        case 13:
                            {
                                Text intro = GameObject.Find("Intro").GetComponent<Text>();
                                intro.color = Color.yellow;
                                intro.text = "<b>Notice that the cursor went straight toward the target even if you moved all the way to the left.</b>";
                                Text introL = GameObject.Find("IntroLeft").GetComponent<Text>();
                                introL.text = "";
                                break;
                            }
                        case 14:
                            {
                                Text intro = GameObject.Find("Intro").GetComponent<Text>();
                                intro.text = "";
                                Text introR = GameObject.Find("IntroRight").GetComponent<Text>();
                                introR.text = "";
                                break;
                            }
                        case 15:
                            {
                                Text intro = GameObject.Find("IntroTop").GetComponent<Text>();
                                intro.color = Color.white;
                                intro.alignment = TextAnchor.UpperCenter;
                                intro.text = "Because you do not have control of the cursor's direction, we ask that you simply aim at the target at all times.";
                                Text introB = GameObject.Find("IntroBottom").GetComponent<Text>();
                                introB.text = "";
                                break;
                            }
                            //case 20:
                            //    {
                            //        Text intro = GameObject.Find("Intro").GetComponent<Text>();
                            //        intro.color = Color.yellow;
                            //        intro.text = "Notice that the cursor moved to the right regardless of your hand direction.";
                            //        break;
                            //    }
                            //case 21:
                            //    {
                            //        Text intro = GameObject.Find("Intro").GetComponent<Text>();
                            //        intro.color = Color.yellow;
                            //        intro.text = "";
                            //        break;
                            //    }
                            //case 22:
                            //    {
                            //        Text intro = GameObject.Find("Intro").GetComponent<Text>();
                            //        intro.color = Color.yellow;
                            //        intro.text = "Notice how the cursor keeps moving to the right no matter what direction you move with the mouse.";
                            //        break;
                            //    }
                    }

                }
            }
        }
    }

    private void check_consecutive_wrong_trials(Vector3 _mousePos)
    {
        float mouseAngle = functions.SignedAngleBetween(Vector3.up, new Vector3(_mousePos.x, _mousePos.y, 0), -Vector3.forward);
        float mouseError = Mathf.Abs(mouseAngle - targetSpawner.targetAngle);
        
        if(mouseError > 90f)
        {
            consecutive_wrong_trials++;
        } else
        {
            consecutive_wrong_trials = 0;
        }

        int consecutive_wrong_trials_limit = 0;
        if(trialManager.experiment)
        {
            consecutive_wrong_trials_limit = 3;
        } else
        {
            consecutive_wrong_trials_limit = 1;
        }

        if(consecutive_wrong_trials >= consecutive_wrong_trials_limit)
        {
            Text intro = GameObject.Find("consecutive_wrong_trials").GetComponent<Text>();
            intro.color = Color.yellow;
            if (trialManager.experiment)
            {
                intro.text = "Remember to aim at the target.";
            } else
            {
                if(!Array.Exists(new int[] { 13, 14, 15 }, element => element == trialManager.trainingTrialN))
                    intro.text = "Please aim at the target.";
            }
            PlayClipAt(badTrial, this.transform.position, .5f);
        }
    }

    private void speedFeedback()
    {
        if (trialManager.experiment || (trialManager.trainingTrialN >= 4 && !Array.Exists(new int[] { 13, 14, 15 }, element => element == trialManager.trainingTrialN)))
        {
            // play sound depending on how fast the subject moved
            bool fast = pointer.pointerTime < trialManager.trialMinTime;
            bool slow = pointer.pointerTime > trialManager.trialMaxTime;

            if (fast)
            {
                speed_alert = GameObject.Find("alert").GetComponent<Text>();
                speed_alert.color = experiment_specs.speed_too_fast;
                speed_alert.text = "too fast";
                PlayClipAt(badTrial, this.transform.position, .5f, 2f);
                target.targetSpriteR.color = experiment_specs.speed_too_fast;
                movementSpeed = 1;
            }
            else if (slow)
            {
                speed_alert = GameObject.Find("alert").GetComponent<Text>();
                speed_alert.color = experiment_specs.speed_too_slow;
                speed_alert.text = "too slow";
                PlayClipAt(badTrial, this.transform.position, .5f);
                target.targetSpriteR.color = experiment_specs.speed_too_slow;
                movementSpeed = -1;
            }
            else
            {
                PlayClipAt(pointerOut, this.transform.position, .5f);
                target.targetSpriteR.color = experiment_specs.speed_ok;
                movementSpeed = 0;
            }
        }
        else
        {
            PlayClipAt(pointerOut, this.transform.position, .5f);
        }

    }

    private void Update()
    {
        if(!isPointerOut)
        {
            movementTime += Time.deltaTime;
        }

        if (isPointerOut)
        {
            timePassed += Time.deltaTime;

            // after feedbackTime seconds
            float feedbackTime = .3f;

            if (timePassed > feedbackTime)
            {
                // make pointer and target disappear
                trialManager.isPointerVisible = false; // pointer disappears
                // target disappears
                target target = GameObject.FindObjectOfType<target>();
                target.isVisible = false;
                pointer.isVisible = false;
                //trialManager.movePointerWithMouse = true;
                trialManager.isTrialStarted = false;
            }
        }
    }
}
