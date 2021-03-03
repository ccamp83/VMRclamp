using UnityEngine.SceneManagement;
using UnityEngine;

public class prepare : MonoBehaviour {

    float wait = 0;

    // Use this for initialization
    void Awake () {
        Cursor.visible = false;
        endArea_trial.isPointerOut = true;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 mousePos = Input.mousePosition;
        mousePos = new Vector3(mousePos.x / Screen.width - .5f, mousePos.y / Screen.height - .5f);
        mousePos = new Vector3(mousePos.x * experiment_specs.CameraWidth, mousePos.y * experiment_specs.CameraHeight, 1f);

        float mouseDist = Vector3.Distance(mousePos, Vector3.zero);

        if (Mathf.Abs(mouseDist) - 1f < .1f)
        {
            wait += Time.deltaTime;
            if (wait > .2f)
            {
                endArea_trial.isPointerOut = false;
                SceneManager.LoadScene("IntroTrial");
            }
        }
    }
}
