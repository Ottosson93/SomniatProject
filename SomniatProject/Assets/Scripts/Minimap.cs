using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Transform playerReference;

    private void LateUpdate()
    {
        Vector3 newPosition = playerReference.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }
}
