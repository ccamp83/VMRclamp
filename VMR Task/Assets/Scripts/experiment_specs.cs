using UnityEngine;

public class experiment_specs : MonoBehaviour {

    public static float CameraSize = 13.4f;
    public static float CameraHeight = CameraSize * 2f;
    public static float CameraWidth = CameraSize * Screen.width/Screen.height * 2f;

    public static float pointer_size = .75f;
    public static float target_size = .75f;

    public static Color pointer_color_baseline = Color.white;
    public static Color pointer_color_adaptation = Color.white;

    public static Color target_color = new Color(110/255.0F, 110 / 255.0F, 110 / 255.0F);
    // http://mkweb.bcgsc.ca/colorblind/
    public static Color speed_too_fast = new Color(238 / 255.0F, 136 / 255.0F, 102 / 255.0F);
    public static Color speed_too_slow = new Color(119/255.0F, 170/255.0F, 221/255.0F);
    public static Color speed_ok = new Color(68 / 255.0F, 187 / 255.0F, 153 / 255.0F);

    static System.Random _random = new System.Random();
    public static void Shuffle<T>(T[] array)
    {
        int n = array.Length;
        for (int i = 0; i < (n - 1); i++)
        {
            // Use Next on random instance with an argument.
            // ... The argument is an exclusive bound.
            //     So we will not go past the end of the array.
            int r = i + _random.Next(n - i);
            T t = array[r];
            array[r] = array[i];
            array[i] = t;
        }
    }

    public static int maxTrials = 0;
    public static int[] targetPosUnique;
    public static int[] targetPos;
    public static string[] phase;
    public static string[] adaptType;
    public static int[] rotation;
    public static int bkgdImg = 0;

    public static void buildExperimentMatrix()
    {
        //---- parameters ----//
        // int bkgdImg = 0;
        int[] targetPosTemp = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };
        string[] phaseTemp = { "Baseline", "Adaptation", "NoFeedback" };
        string[] adaptTypeTemp = { "V", "R", "N" };
        int[] rotationTemp = { 0, 45, 0 };

        int cyclesBaseline = 1;
        int cyclesAdaptation = 1;
        int cyclesNoFeedback = 2;
        //int cyclesWashout = 1;

        //maxTrials = targetPosTemp.Length * (cyclesBaseline + cyclesAdaptation + cyclesNoFeedback + cyclesWashout);
        maxTrials = targetPosTemp.Length * (cyclesBaseline + cyclesAdaptation + cyclesNoFeedback);
        var targetPosALL = new int[maxTrials];
        var phaseALL = new string[maxTrials];
        var adaptTypeALL = new string[maxTrials];
        var rotationALL = new int[maxTrials];

        // ----- targetPosition ----- ///
        // Baseline
        int trialsBaseline = targetPosTemp.Length * cyclesBaseline;
        var targetPosBaseline = new int[trialsBaseline];
        var phaseBaseline = new string[trialsBaseline];
        var adaptTypeBaseline = new string[trialsBaseline];
        var rotationBaseline = new int[trialsBaseline];
        int indexBaseline = 0;
        for (int cycles = 0; cycles < cyclesBaseline; cycles++)
        {
            Shuffle(targetPosTemp);
            targetPosTemp.CopyTo(targetPosBaseline, targetPosTemp.Length * cycles);

            var phaseCycle = new string[targetPosTemp.Length];
            var adaptTypeCycle = new string[targetPosTemp.Length];
            var rotationCycle = new int[targetPosTemp.Length];
            for (int t = 0; t < targetPosTemp.Length; t++)
            {
                phaseCycle[t] = phaseTemp[indexBaseline];
                adaptTypeCycle[t] = adaptTypeTemp[indexBaseline];
                rotationCycle[t] = rotationTemp[indexBaseline];
            }
            phaseCycle.CopyTo(phaseBaseline, targetPosTemp.Length * cycles);
            adaptTypeCycle.CopyTo(adaptTypeBaseline, targetPosTemp.Length * cycles);
            rotationCycle.CopyTo(rotationBaseline, targetPosTemp.Length * cycles);
        }
        targetPosBaseline.CopyTo(targetPosALL, trialsBaseline * indexBaseline);
        phaseBaseline.CopyTo(phaseALL, trialsBaseline * indexBaseline);
        adaptTypeBaseline.CopyTo(adaptTypeALL, trialsBaseline * indexBaseline);
        rotationBaseline.CopyTo(rotationALL, trialsBaseline * indexBaseline);

        // Adaptation
        int trialsAdaptation = targetPosTemp.Length * cyclesAdaptation;
        var targetPosAdaptation = new int[trialsAdaptation];
        var phaseAdaptation = new string[trialsAdaptation];
        var adaptTypeAdaptation = new string[trialsAdaptation];
        var rotationAdaptation = new int[trialsAdaptation];
        int indexAdaptation = 1;
        for (int cycles = 0; cycles < cyclesAdaptation; cycles++)
        {
            Shuffle(targetPosTemp);
            targetPosTemp.CopyTo(targetPosAdaptation, targetPosTemp.Length * cycles);

            var phaseCycle = new string[targetPosTemp.Length];
            var adaptTypeCycle = new string[targetPosTemp.Length];
            var rotationCycle = new int[targetPosTemp.Length];
            for (int t = 0; t < targetPosTemp.Length; t++)
            {
                phaseCycle[t] = phaseTemp[indexAdaptation];
                adaptTypeCycle[t] = adaptTypeTemp[indexAdaptation];
                rotationCycle[t] = rotationTemp[indexAdaptation];
            }
            phaseCycle.CopyTo(phaseAdaptation, targetPosTemp.Length * cycles);
            adaptTypeCycle.CopyTo(adaptTypeAdaptation, targetPosTemp.Length * cycles);
            rotationCycle.CopyTo(rotationAdaptation, targetPosTemp.Length * cycles);
        }
        targetPosAdaptation.CopyTo(targetPosALL, targetPosBaseline.Length);
        phaseAdaptation.CopyTo(phaseALL, phaseBaseline.Length);
        adaptTypeAdaptation.CopyTo(adaptTypeALL, adaptTypeBaseline.Length);
        rotationAdaptation.CopyTo(rotationALL, rotationBaseline.Length);

        // NoFeedback
        int trialsNoFeedback = targetPosTemp.Length * cyclesNoFeedback;
        var targetPosNoFeedback = new int[trialsNoFeedback];
        var phaseNoFeedback = new string[trialsNoFeedback];
        var adaptTypeNoFeedback = new string[trialsNoFeedback];
        var rotationNoFeedback = new int[trialsNoFeedback];
        int indexNoFeedback = 2;
        for (int cycles = 0; cycles < cyclesNoFeedback; cycles++)
        {
            Shuffle(targetPosTemp);
            targetPosTemp.CopyTo(targetPosNoFeedback, targetPosTemp.Length * cycles);

            var phaseCycle = new string[targetPosTemp.Length];
            var adaptTypeCycle = new string[targetPosTemp.Length];
            var rotationCycle = new int[targetPosTemp.Length];
            for (int t = 0; t < targetPosTemp.Length; t++)
            {
                phaseCycle[t] = phaseTemp[indexNoFeedback];
                adaptTypeCycle[t] = adaptTypeTemp[indexNoFeedback];
                rotationCycle[t] = rotationTemp[indexNoFeedback];
            }
            phaseCycle.CopyTo(phaseNoFeedback, targetPosTemp.Length * cycles);
            adaptTypeCycle.CopyTo(adaptTypeNoFeedback, targetPosTemp.Length * cycles);
            rotationCycle.CopyTo(rotationNoFeedback, targetPosTemp.Length * cycles);
        }
        targetPosNoFeedback.CopyTo(targetPosALL, targetPosBaseline.Length + targetPosAdaptation.Length);
        phaseNoFeedback.CopyTo(phaseALL, phaseBaseline.Length + phaseAdaptation.Length);
        adaptTypeNoFeedback.CopyTo(adaptTypeALL, adaptTypeBaseline.Length + adaptTypeAdaptation.Length);
        rotationNoFeedback.CopyTo(rotationALL, rotationBaseline.Length + rotationAdaptation.Length);

        //// Washout
        //int trialsWashout = targetPosTemp.Length * cyclesWashout;
        //var targetPosWashout = new int[trialsWashout];
        //var phaseWashout = new string[trialsWashout];
        //var adaptTypeWashout = new string[trialsWashout];
        //var rotationWashout = new int[trialsWashout];
        //int indexWashout = 3;
        //for (int cycles = 0; cycles < cyclesWashout; cycles++)
        //{
        //    Shuffle(targetPosTemp);
        //    targetPosTemp.CopyTo(targetPosWashout, targetPosTemp.Length * cycles);

        //    var phaseCycle = new string[targetPosTemp.Length];
        //    var adaptTypeCycle = new string[targetPosTemp.Length];
        //    var rotationCycle = new int[targetPosTemp.Length];
        //    for (int t = 0; t < targetPosTemp.Length; t++)
        //    {
        //        phaseCycle[t] = phaseTemp[indexWashout];
        //        adaptTypeCycle[t] = adaptTypeTemp[indexWashout];
        //        rotationCycle[t] = rotationTemp[indexWashout];
        //    }
        //    phaseCycle.CopyTo(phaseWashout, targetPosTemp.Length * cycles);
        //    adaptTypeCycle.CopyTo(adaptTypeWashout, targetPosTemp.Length * cycles);
        //    rotationCycle.CopyTo(rotationWashout, targetPosTemp.Length * cycles);
        //}
        //targetPosWashout.CopyTo(targetPosALL, trialsWashout * indexWashout);
        //phaseWashout.CopyTo(phaseALL, trialsWashout * indexWashout);
        //adaptTypeWashout.CopyTo(adaptTypeALL, trialsWashout * indexWashout);
        //rotationWashout.CopyTo(rotationALL, trialsWashout * indexWashout);

        targetPosUnique = targetPosTemp;
        targetPos = targetPosALL;
        phase = phaseALL;
        adaptType = adaptTypeALL;
        rotation = rotationALL;

        //foreach (var item in rotation)
        //{
        //    Debug.Log(item.ToString());
        //}
    }

    public static void setGraphics()
    {
        // Improve refresh rate
        QualitySettings.vSyncCount = 0;

        // Set minimum FPS
        Application.targetFrameRate = 300;
    }

    private void Awake()
    {
        buildExperimentMatrix();
    }
}
