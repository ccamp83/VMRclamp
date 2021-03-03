using UnityEngine;

public class target : MonoBehaviour {

    public static bool isVisible = true;

    // private variables
    static public SpriteRenderer targetSpriteR;

    void Start()
    {
        targetSpriteR = GetComponent<SpriteRenderer>();
        targetSpriteR.color = experiment_specs.target_color;
    }

    void Update()
    {
        // toggle visibility on/off
        targetSpriteR.enabled = isVisible;
    }
}
