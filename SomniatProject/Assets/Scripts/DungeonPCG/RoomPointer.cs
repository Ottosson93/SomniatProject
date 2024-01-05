using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPointer : MonoBehaviour
{
    public Image bossPointer;
    public Image upgradePointer;
    private Transform playerTransform;
    private float distanceFromCenter = 40f;

    void LateUpdate()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;


        if (playerTransform == null || bossPointer == null || upgradePointer == null)
        {
            Debug.LogError("Missing references. Make sure to assign all required fields in the Inspector.");
            return;
        }


        GameObject bossRoom = GameObject.FindWithTag("BossRoom");
        GameObject upgradeRoom = GameObject.FindWithTag("UpgradeRoom");


        if(bossRoom == null || upgradeRoom == null)
        {
            Debug.LogError("Boss Room or Upgrade Room not found!");
            return;
        }


        Vector3 bossDirection = (bossRoom.transform.position - playerTransform.position).normalized;
        Vector3 upgradeDirection = (upgradeRoom.transform.position - playerTransform.position).normalized;


        float bossAngle = Mathf.Atan2(bossDirection.x, bossDirection.z) * Mathf.Rad2Deg;
        float upgradeAngle = Mathf.Atan2(upgradeDirection.x,upgradeDirection.z) * Mathf.Rad2Deg;

        bossPointer.rectTransform.rotation = Quaternion.Euler(0f, 0f, -bossAngle);
        upgradePointer.rectTransform.rotation = Quaternion.Euler(0f, 0f, -upgradeAngle);
    }
}
