using UnityEngine.SceneManagement;
using UnityEngine;
using System.Runtime.InteropServices;

public class NextScene : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void goFullScreen();

    public static bool instructions_given = false;

    private void Awake()
    {
        Cursor.visible = true;
    }

    public void LoadNextScene()
    {
        switch(SceneManager.GetActiveScene().name)
        {
            case "Intro1":
#if UNITY_WEBGL && !UNITY_EDITOR
                // goFullScreen();
#endif
                SceneManager.LoadScene("IntroTrial");
                break;
            case "Intro3":
                SceneManager.LoadScene("IntroTrial");
                break;
        }
    }
}
