using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class DynamoDB_interface_test : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void WriteData(
        string tableName,
        string testID,
        string var1,
        string var2);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            WriteData(
                "empty_template",
                1.ToString(),
                "var1 value",
                "var2 value");
        }
    }
}
