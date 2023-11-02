using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    bool isOpen = false;
    [SerializeField] GameObject[] itemsToPickUp;
    [SerializeField] float[] dropPercentagePerItem;
    [SerializeField] float dropArea;

    public void Open()
    {
        isOpen = true;

    }

    bool PlaceItem(GameObject gameObject)
    {
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, dropArea);

        float goArea = gameObject.GetComponent<Collider>().bounds.size.x * gameObject.GetComponent<Collider>().bounds.size.y;
        
        


        return false;
    }


    GameObject GetItemToDrop()
    {
        float nr = Random.Range(0f, 1f);
        for(int i = 0; i<dropPercentagePerItem.Length; ++i)
        {
            float k = 1.0f-dropPercentagePerItem[i];
            if(nr<=k&&nr>0.000f)
                return itemsToPickUp[i];
        }
        return null;
    }




    // Start is called before the first frame update
    void Start()
    {
        if(dropPercentagePerItem.Length != itemsToPickUp.Length)
        {
            float[] temp = new float[itemsToPickUp.Length];
            for(int i = 0; i<itemsToPickUp.Length; ++i)
            {
                if (i < dropPercentagePerItem.Length)
                {
                    temp[i] = dropPercentagePerItem[i];
                }
                else
                    temp[i] = 0f;
            }
            dropPercentagePerItem = temp;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
