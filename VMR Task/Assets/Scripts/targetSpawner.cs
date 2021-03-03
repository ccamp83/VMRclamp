using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetSpawner : MonoBehaviour {

    public GameObject targetPrefab;
    public GameObject positionPrefab;

    public int targetsNumber = 9;
    public float targetsDistance = 6.89f;

    // parameters available publicly
    public static int targetPositionIndex = 0;
    public static int targetCount = 0;
    public static float targetAngle;
    public static Vector3 targetLocation;
    public static float targetDistance;

    AudioClip targetAppears;

    void Start () {
        targetAppears = Resources.Load<AudioClip>("Sounds/targetAppears");

        if (!trialManager.experiment && (trialManager.trainingTrialN == 13 || trialManager.trainingTrialN == 14 || trialManager.trainingTrialN == 15))
        {
            targetsNumber = 1;
        }
        targetCount = targetsNumber;
        targetDistance = targetsDistance;
        createPositions();
        SpawnTarget();
	}

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, targetsDistance);
    }

    void SpawnTarget()
    {
        // instantiate a target at a position (given externally)
        GameObject target = Instantiate(targetPrefab, this.gameObject.transform.GetChild(targetPositionIndex).position, Quaternion.identity) as GameObject;
        // make the target a child of that position
        target.transform.parent = this.gameObject.transform.GetChild(targetPositionIndex);
        // calculate the angle of the target relative to the center, starting from top vertical rotating in clockwise direction
        targetAngle = functions.SignedAngleBetween(Vector3.up, new Vector3 (target.transform.position.x, target.transform.position.y, 0), -Vector3.forward);
        // initialize targetLocation for external use
        targetLocation = target.transform.position;
        // play clip as go signal
        // AudioSource.PlayClipAtPoint(targetAppears, this.transform.position, 1f);
    }

    void createPositions()
    {
        // angle between positions (deg)
        float betweenTargetAngle = 360f / targetsNumber;
        
        for(int tPos = 0; tPos < targetsNumber; tPos++)
        {
            // calculate target angle (deg)
            float targetAngle = betweenTargetAngle / 2f + betweenTargetAngle * tPos;
            // calculate target x,y positions
            float targetX = targetsDistance * Mathf.Sin(Mathf.Deg2Rad * targetAngle);
            float targetY = -targetsDistance * Mathf.Cos(Mathf.Deg2Rad * targetAngle);
            // instantiate position
            GameObject targetPosition = Instantiate(positionPrefab, new Vector3(targetX, targetY, 1f), Quaternion.identity) as GameObject;
            // make the position a child of this object
            targetPosition.transform.parent = this.gameObject.transform;
        }
    }
}
