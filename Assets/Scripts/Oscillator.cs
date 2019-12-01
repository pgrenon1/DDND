using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : UIBaseBehaviour
{
    public Vector3 scale;
    public float speed = 3f;

    void FixedUpdate()
    {
        RectTransform.Translate(new Vector3(Oscillate() * scale.x, Oscillate() * scale.y, Oscillate() * scale.z));
    }

    private float Oscillate()
    {
        return Mathf.Cos(Time.time * speed / Mathf.PI);
    }
}
