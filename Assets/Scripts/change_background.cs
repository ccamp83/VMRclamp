using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class change_background : MonoBehaviour {

    public Sprite[] spriteR;
    static int scrollSprites;
    public static int spriteIndex = 0;
    public static int bkgdNumber;

    // Use this for initialization
    void Start () {
        // load the image pointed by spriteIndex
        this.GetComponent<SpriteRenderer>().sprite = spriteR[spriteIndex];
        // rotate to target's position
        transform.rotation = Quaternion.Euler(0, 0, -targetSpawner.targetAngle);
        bkgdNumber = spriteR.Length;
        scrollSprites = bkgdNumber;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            scrollSprites++;
            spriteIndex = scrollSprites % spriteR.Length;
            this.GetComponent<SpriteRenderer>().sprite = spriteR[spriteIndex];
        }
	}
}
