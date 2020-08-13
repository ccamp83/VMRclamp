using UnityEngine.SceneManagement;
using UnityEngine;
using System.Runtime.InteropServices;

public class levelManager : MonoBehaviour {

    /**** WEBGL VARIABLES ****/
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void goFullScreen();
    //https://docs.unity3d.com/Manual/webgl-cursorfullscreen.html
#endif

    private void Awake()
    {
        Cursor.visible = true;
    }

    public void LoadExperiment()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        goFullScreen();
        #endif
        SceneManager.LoadScene("Prepare");
    }
}
