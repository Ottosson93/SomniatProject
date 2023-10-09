using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpoweredRelic : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Emp empowered;
    PickUpScript pickUpScript;
    void Start()
    {
        pickUpScript = GetComponent<PickUpScript>();

    }

    private void Update()
    {
        if (empowered == null || pickUpScript ==null)
        {
            return;
        }

        if (pickUpScript.PickUp().Result)
        {
            empowered.Run();
        }


    }



}
