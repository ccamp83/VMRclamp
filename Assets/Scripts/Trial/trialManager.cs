// this is the blueprint of a rotation game manager script
// at start, it initializes the target position at random locations
// the random locations are hard coded as global variables at the top
// at runt`e (Update):
//      1. loads the exit scene if user pressess Esc
//      2. toggles mouse pointer visibility & motion of cursor by means of the same bool
// additionally, it contains a public method to reload the scene (load the next trial)

// specs are stored in experiment_specs.cs

/**** TRAINING MODE NAMESPACES ****/
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
/**** EXPERIMENT MODE NAMESPACES ****/
using UnityEngine.UI;
using System.Runtime.InteropServices;

/**** WEBGL VARIABLES ****/

public class trialManager : MonoBehaviour
{
    /**** TRAINING MODE VARIABLES ****/
    // public static variables -> survive after reloading a scene (global info)
    public static int trialNumber = -1;
    public static int trainingTrialN = 0;
    public static string subjName = "test";
    public static bool isMouseVisible = false;
    public static bool movePointerWithMouse = true;
    public static bool isPointerVisible = true;
    public static bool isTrialStarted = false;
    public static bool started = false;
    public static bool experiment = false;
    public static float endpointX = 0, endpointY = 0;
	public static float startTime = 0;
	public static float trialMinTime = .1f;
    public static float trialMaxTime = .35f;

    /**** EXPERIMENT MODE VARIABLES ****/
    // public static
    public static int maxTrialN = 0;

    [DllImport("__Internal")]
    private static extern void stopFullScreen();

    float elapsedTime = 0;
    Text please_move_alert;
    bool trainingTrial20started = false;
    bool trainingTrial24started = false;

    void Awake()
    {
        //experiment_specs.setGraphics();

        if (experiment)
        {
            if (trialNumber < 0)
                trialNumber = 0;
        }

        maxTrialN = experiment_specs.maxTrials;

        please_move_alert = GameObject.Find("alert").GetComponent<Text>();

        // initialize parameters
        if (trainingTrialN != 20 && trainingTrialN != 24)
        {
            initializeParameters();
        } else
            isMouseVisible = true;
    }

    void Update()
    {
        if (trainingTrialN == 20)
        {
            if (Input.GetMouseButtonDown(0) && !trainingTrial20started)
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos = new Vector3(mousePos.x / Screen.width - .5f, mousePos.y / Screen.height - .5f);
                mousePos = new Vector3(mousePos.x * experiment_specs.CameraWidth, mousePos.y * experiment_specs.CameraHeight, -1f);
                float mouseDist = Vector3.Distance(mousePos, Vector3.zero);

                if (mouseDist - 1 <= .6f)
                {
                    Text intro = GameObject.Find("Intro").GetComponent<Text>();
                    intro.text = "";
                    isMouseVisible = false;
                    initializeParameters();
                    trainingTrial20started = true;
                }
            }
        } else if (trainingTrialN == 24)
        {
            if (Input.GetMouseButtonDown(0) && !trainingTrial24started)
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos = new Vector3(mousePos.x / Screen.width - .5f, mousePos.y / Screen.height - .5f);
                mousePos = new Vector3(mousePos.x * experiment_specs.CameraWidth, mousePos.y * experiment_specs.CameraHeight, -1f);
                float mouseDist = Vector3.Distance(mousePos, Vector3.zero);

                if (mouseDist - 1 <= .6f)
                {
                    Text intro = GameObject.Find("Intro").GetComponent<Text>();
                    intro.text = "";
                    isMouseVisible = false;
                    initializeParameters();
                    trainingTrial24started = true;
                }
            }
        }

        elapsedTime += Time.deltaTime;

        if (experiment || !Array.Exists(new int[] { 4, 12, 20, 24 }, element => element == trainingTrialN))
        {
            if (elapsedTime > 10f && target.isVisible && !endArea_trial.isPointerOut)
            {
                please_move_alert.text = "Please move to the target";
            } else
            {
                if (please_move_alert.color == Color.yellow)
                {
                    please_move_alert.text = "";
                }
            }
        }

        if(!experiment)
        {
            switch(trainingTrialN)
            {
                case 13:
                    {
                        Vector3 mousePos = Input.mousePosition;
                        mousePos = new Vector3(mousePos.x / Screen.width - .5f, mousePos.y / Screen.height - .5f);
                        mousePos = new Vector3(mousePos.x * experiment_specs.CameraWidth, mousePos.y * experiment_specs.CameraHeight, -1f);

                        if ((mousePos.x > .5f || Mathf.Abs(mousePos.y) > 1f) && target.isVisible && !endArea_trial.isPointerOut)
                            please_move_alert.text = "Please move to the LEFT";
                        else
                            please_move_alert.text = "";
                        break;
                    }
                case 14:
                    {
                        Vector3 mousePos = Input.mousePosition;
                        mousePos = new Vector3(mousePos.x / Screen.width - .5f, mousePos.y / Screen.height - .5f);
                        mousePos = new Vector3(mousePos.x * experiment_specs.CameraWidth, mousePos.y * experiment_specs.CameraHeight, -1f);

                        if ((mousePos.x < .5f || Mathf.Abs(mousePos.y) > 1f) && target.isVisible && !endArea_trial.isPointerOut)
                            please_move_alert.text = "Please move to the RIGHT";
                        else
                            please_move_alert.text = "";
                        break;
                    }
                case 15:
                    {
                        Vector3 mousePos = Input.mousePosition;
                        mousePos = new Vector3(mousePos.x / Screen.width - .5f, mousePos.y / Screen.height - .5f);
                        mousePos = new Vector3(mousePos.x * experiment_specs.CameraWidth, mousePos.y * experiment_specs.CameraHeight, -1f);

                        if ((Mathf.Abs(mousePos.x) > 1 || mousePos.y > .5f) && target.isVisible && !endArea_trial.isPointerOut)
                            please_move_alert.text = "Please move to the BOTTOM";
                        else
                            please_move_alert.text = "";
                        break;
                    }
            }
        }

        // load exit scene
        //if (Input.GetKeyDown(KeyCode.Escape))
            //SceneManager.LoadScene("Exit");

        // control mouse visibility
        Cursor.visible = isMouseVisible;

        // control pointer mobility
        pointer.moveWithMouse = movePointerWithMouse;

        // control pointer visibility
        // pointer.isVisible = isPointerVisible;
    }

    // routine to go to the next trial
    public void NextTrial()
    {
        string nextScene = "empty";
        if (experiment)
        {
            trialNumber++;
            nextScene = "IntroTrial";
        } else
        {
            trainingTrialN++;

            switch(trainingTrialN)
            {
                // first three examples
                case 1:
                case 2:
                case 3:
                // explanation of timing feedback
                case 4:
                // 8 trials w/ veridical feedback to train on timing
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                    nextScene = "IntroTrial";
                    break;
                // explanation of clamp that hits target
                case 13: // left
                    if (!NextScene.instructions_given)
                    {
                        NextScene.instructions_given = true;
                        nextScene = "Intro3";
                    }
                    else
                        nextScene = "IntroTrial";
                    break;
                case 14: // right
                case 15: // bottom
                // four trials of practice
                case 16:
                case 17:
                case 18:
                case 19:
                // explanation of clamp that misses the target (four practice trials)
                case 20:
                case 21:
                case 22:
                case 23:
                // explanation of no feedback (1 trial)
                case 24:
                    nextScene = "IntroTrial";
                    break;
                // start scene
                case 25:
                    trainingTrialN = 0;
                    experiment = true;
                    nextScene = "Start";
                    break;

            }
        }

        SceneManager.LoadScene(nextScene);
    }

    void initializeParameters()
    {
        isTrialStarted = true;
        // hide and move the pointer
        // isPointerVisible = true;
        movePointerWithMouse = true;
		target.isVisible = true;

        // get startTime
        startTime = Time.time;

        if (!experiment)
        {
            experiment_specs.buildExperimentMatrix();
            change_background.spriteIndex = experiment_specs.bkgdImg;

            switch (trainingTrialN)
            {
                // first reach
                case 0:
                // first three examples
                case 1:
                case 2:
                case 3:
                    pointer.adaptationType = "V";
                    targetSpawner.targetPositionIndex = experiment_specs.targetPos[UnityEngine.Random.Range(0, experiment_specs.targetPos.Length)];
                    pointer.rotation = 0;
                    break;
                // explanation of timing feedback
                case 4:
                    pointer.adaptationType = "V";
                    targetSpawner.targetPositionIndex = experiment_specs.targetPos[UnityEngine.Random.Range(0, experiment_specs.targetPos.Length)];
                    pointer.rotation = 0;
                    break;
                // 8 trials w/ veridical feedback to train on timing
                case 5: 
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                    pointer.adaptationType = "V";
                    int pseudoRandomTrial = targetPosUnique_generator.targetPosUnique[trainingTrialN - 5];
                    targetSpawner.targetPositionIndex = pseudoRandomTrial;
                    pointer.rotation = 0;
                    break;
                // explanation of clamp that hits target
                case 13:
                case 14:
                case 15:
                    pointer.adaptationType = "C";
                    targetSpawner.targetPositionIndex = 0;
                    pointer.rotation = 0;
                    break;
                // four trials of practice
                case 16:
                case 17:
                case 18:
                case 19:
                    pointer.adaptationType = "C";
                    targetSpawner.targetPositionIndex = experiment_specs.targetPos[UnityEngine.Random.Range(0, experiment_specs.targetPos.Length)];
                    pointer.rotation = 0;
                    break;
                // explanation of clamp that misses the target (four practice trials)
                case 20:
                case 21:
                case 22:
                case 23:
                    pointer.adaptationType = "C";
                    targetSpawner.targetPositionIndex = experiment_specs.targetPos[UnityEngine.Random.Range(0, experiment_specs.targetPos.Length)];
                    pointer.rotation = 30;
                    break;
                // explanation of no feedback (1 trial)
                case 24:
                    pointer.adaptationType = "N";
                    targetSpawner.targetPositionIndex = experiment_specs.targetPos[UnityEngine.Random.Range(0, experiment_specs.targetPos.Length)];
                    pointer.rotation = 0;
                    break;
            }
        }
        else
        {
            // experiment loop
            if (trialNumber < maxTrialN)
            {
                targetSpawner.targetPositionIndex = experiment_specs.targetPos[trialNumber];
                pointer.adaptationType = experiment_specs.adaptType[trialNumber];
                pointer.rotation = experiment_specs.rotation[trialNumber];
                change_background.spriteIndex = experiment_specs.bkgdImg;
            }
            else
            {
#if UNITY_WEBGL && !UNITY_EDITOR
        stopFullScreen();
#endif
                isMouseVisible = true;
                Cursor.visible = isMouseVisible;
                SceneManager.LoadScene("Exit");
            }
        }
    }
}
