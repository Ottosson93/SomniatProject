using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FixedScale : MonoBehaviour
{
    // Reference to the child object
    public Transform childObject;

    void Start()
    {
        // Reset the rotation of the child object without affecting the parent
        ResetChildRotationWithoutAffectingParent();
    }

    void ResetChildRotationWithoutAffectingParent()
    {
        // Store the parent's rotation
        Quaternion parentRotation = transform.rotation;

        // Reset the child's local rotation
        childObject.localRotation = Quaternion.identity;

        // Apply the parent's rotation back to the child in world space
        childObject.rotation = parentRotation;
    }
}
