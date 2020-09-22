// the start area triggers the loading of the next trial, as soon as a mouse-controlled collider enters it
using UnityEngine;
using System.Runtime.InteropServices;
// https://docs.unity3d.com/Manual/webgl-interactingwithbrowserscripting.html

public class startArea_trial : MonoBehaviour {

    // public variables (accessible by other classes)
	public bool isCursorAtStart = false; // can flip between true and false during the scene
	public bool cursorArrivedAtStart = false; // this only turns true once during a scene then stays so until the end
	public float timeWhenArrivedAtStart = 0; // ########### what do I need this for?
    bool waitLaunchNewTrial = false;
    float waitElapsedTime = 0;
    float waitTime = .2f;
    bool cursorReadyToStart = false;

    // private variables
	private trialManager game_manager; // because I need to access NextTrial method

    /**** WEBGL VARIABLES ****/
    static string dataTableName = "VMRtemplate"; // must be static to be accepted by WriteData

    [DllImport("__Internal")]
    private static extern void WriteData(string tableName,
        string subjID,
        string subjName,
        string x,
        string y,
        string movementTime,
        string movementSpeed,
        string phase,
        string targetPos,
        string adaptType,
        string rotation,
        string trialN);

    void Start (){
        // listens to other classes
		game_manager = FindObjectOfType<trialManager>();
	}

	void OnTriggerEnter2D (Collider2D trigger)
	{
		isCursorAtStart = true;
        // if the cursor has entered the start area and has already moved out of the end area, then it's time to load the next trial
        if (endArea_trial.isPointerOut)
        {
            cursorArrivedAtStart = true;
            timeWhenArrivedAtStart = Time.time;
            endArea_trial.isPointerOut = false;
#if UNITY_WEBGL && !UNITY_EDITOR
            sendResults();
#endif

            waitLaunchNewTrial = true;
        }
    }

    private void waitThenLaunchNewTrial(bool doIt)
    {
        if (doIt)
        {
            pointer.moveWithMouse = true;
            pointer.isVisible = true;
            pointer.adaptationType = "V";
            Vector3 mousePos = Input.mousePosition;
            mousePos = new Vector3(mousePos.x / Screen.width - .5f, mousePos.y / Screen.height - .5f);
            mousePos = new Vector3(mousePos.x * experiment_specs.CameraWidth, mousePos.y * experiment_specs.CameraHeight, -1f);
            float distanceToCenter = Mathf.Sqrt(Mathf.Pow(mousePos.x, 2) + Mathf.Pow(mousePos.y, 2));
            float cursorDistanceToCenterMax = .15f;
            bool cursorAtCenter = distanceToCenter <= cursorDistanceToCenterMax;

            if(!cursorReadyToStart & cursorAtCenter)
            {
                cursorReadyToStart = true;
            }

            if(cursorReadyToStart)
            {
                waitElapsedTime += Time.deltaTime;
                if (waitElapsedTime > waitTime)
                {
                    game_manager.NextTrial();
                    waitLaunchNewTrial = false;
                }
            }
        }
    }

    private static void sendResults()
    {
        if (trialManager.experiment)
        {
            float endpointX = (Mathf.Round(trialManager.endpointX * 100f) / 100.0f);
            float endpointY = (Mathf.Round(trialManager.endpointY * 100f) / 100.0f);
            float movTime = (Mathf.Round(endArea_trial.movementTime * 100f) / 100.0f);

            string phase = experiment_specs.phase[trialManager.trialNumber];
            int targetPos = experiment_specs.targetPos[trialManager.trialNumber];
            string adaptType = experiment_specs.adaptType[trialManager.trialNumber];
            int rotation = experiment_specs.rotation[trialManager.trialNumber];

            WriteData(dataTableName, trialManager.subjName + "_" + trialManager.trialNumber.ToString(), trialManager.subjName,
                endpointX.ToString(), endpointY.ToString(),
                movTime.ToString(), endArea_trial.movementSpeed.ToString(),
                phase, targetPos.ToString(), adaptType, rotation.ToString(), trialManager.trialNumber.ToString());
        }
    }

    void OnTriggerExit2D (Collider2D trigger){
		isCursorAtStart = false;
	}

    private void Update()
    {
        waitThenLaunchNewTrial(waitLaunchNewTrial);
    }
}
