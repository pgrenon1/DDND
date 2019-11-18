using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScalingType
{
    Gain,
    MinRef
}

[System.Serializable]
public class ScalableFloat
{
    public ScalingType scalingType = ScalingType.Gain;
    public float minimumValue;
    [ShowIf("scalingType", ScalingType.MinRef)]
    public float referenceValue;
    [ShowIf("scalingType", ScalingType.Gain)]
    public float gain;

    public float GetValue(float scalingValue)
    {
        if (scalingType == ScalingType.Gain)
            Debug.LogWarning("Scaling a Gain by MinRef!");

        return Mathf.LerpUnclamped(minimumValue, referenceValue, scalingValue);
    }

    public float GetValue(int level)
    {
        if (scalingType == ScalingType.MinRef)
            Debug.LogWarning("Scaling a MinRef by Gain!");
        return minimumValue + gain * level;
    }
}
