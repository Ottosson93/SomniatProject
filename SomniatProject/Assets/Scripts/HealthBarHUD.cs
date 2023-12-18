using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarHUD : MonoBehaviour
{
    private Enemy boss;

    [SerializeField]
    private RectTransform barRect;

    [SerializeField]
    private RectMask2D rectMask;

    private float maxRightMask;
    private float initialRightMask;

    public void Start()
    {
        maxRightMask = barRect.rect.width - rectMask.padding.x - rectMask.padding.z;
        initialRightMask = rectMask.padding.z;
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Enemy>();
    }

    public void Update()
    {
        var targetWidth = maxRightMask / boss.health;
        var newRightMask = maxRightMask + initialRightMask - targetWidth;
        var padding = rectMask.padding;
        padding.z = newRightMask;
    }
}
