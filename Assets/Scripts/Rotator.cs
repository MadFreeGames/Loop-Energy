using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Rotator : MonoBehaviour
{
    public float rotationAngle = 90f;
    [HideInInspector]
    public bool isPowerSource;
    [HideInInspector]
    public bool stopRotation;

    private void Start()
    {
        if (transform.GetChild(0).GetComponent<PowerSource>())
        {
            isPowerSource = true;
        }
    }
    private void OnMouseDown()
    {
        if (!stopRotation)
        {
            RotateConduit();
            GameManager.Instance.TapPopVibrate();
        }
    }
    private void RotateConduit()
    {
        if (isPowerSource)
        {
            transform.DOShakeRotation(.1f, new Vector3(0,0,50)).SetEase(Ease.Linear);
            SoundManager.instance.PlaySFX("NonRotatable");

        }
        else
        {
            transform.DORotate(new Vector3(0, 0, transform.eulerAngles.z + 90), .1f, RotateMode.FastBeyond360).OnComplete(() =>
            {
                GameManager.Instance.CheckAllPowered();
            });
            SoundManager.instance.PlaySFX("rotate_1");

        }
    }
}
