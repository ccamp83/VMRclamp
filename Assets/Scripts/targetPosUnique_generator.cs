using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetPosUnique_generator : MonoBehaviour
{
    public static int[] targetPosUnique;
    
    void Start()
    {
        experiment_specs.buildExperimentMatrix();
        targetPosUnique = experiment_specs.targetPosUnique;
    }
}
