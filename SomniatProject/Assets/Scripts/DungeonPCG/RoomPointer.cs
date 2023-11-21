using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPointer : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 bossPos, upgradePos;

    void Start()
    {
        Debug.Log("Pointing towards boss room");

        bossPos = GameObject.FindWithTag("BossRoom").transform.position;
        upgradePos = GameObject.FindWithTag("UpgradeRoom").transform.position;
        Debug.Log(bossPos);
        this.gameObject.transform.GetChild(0).transform.LookAt(bossPos);
        this.gameObject.transform.GetChild(1).transform.LookAt(upgradePos);
    }
}
