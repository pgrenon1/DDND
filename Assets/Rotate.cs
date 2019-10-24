using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed = 5f;

    private void Start()
    {
        transform.DOLocalRotate(new Vector3(0, 180, 0), speed / 2).SetSpeedBased(true)
            .OnComplete(() => transform.DOLocalRotate(new Vector3(0, 360, 0), speed / 2).SetSpeedBased(true));
    }
}
