using System.Threading.Tasks;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public float rotationAngle = -90f;
    public float rotationSpeed = 5f;
    public Transform chestTop;
    private bool isOpen = false;
    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private Material eKeyPlaneMaterial;
    private bool displayKey = false;
    public ItemDropSystem itemDropSystem;

    private void Start()
    {
        if (this.gameObject.name == "ChestGreen")
            chestTop = transform.Find("chestTop_common");
        else if (this.gameObject.name == "ChestYellow")
            chestTop = transform.Find("chestTop_uncommon");
        else if (this.gameObject.name == "ChestRed")
            chestTop = transform.Find("chestTop_rare");

        if (chestTop == null)
            Debug.LogError("ChestTop not found! Make sure the child GameObject is named 'chestTop'.");

        initialRotation = chestTop.rotation;

        Transform eKeyPlane = transform.GetChild(0);
        if (eKeyPlane != null)
        {
            MeshRenderer renderer = eKeyPlane.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                eKeyPlaneMaterial = renderer.material;
            }
        }

        itemDropSystem = GetComponent<ItemDropSystem>();

    }

    void Update()
    {
        if (InRangeOfPlayer())
        {
            HandleChestInteraction();
        }

        chestTop.rotation = Quaternion.Lerp(chestTop.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private bool InRangeOfPlayer()
    {
        return (PlayerPosition() - transform.position).magnitude <= 3f;
    }

    private Vector3 PlayerPosition()
    {
        return GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    private void HandleChestInteraction()
    {
        if (!displayKey && CanIncreaseOpacity())
        {
            ChangeKeyOpacity(IncreaseOpacity());
        }

        if (Input.GetKeyDown(KeyCode.E) && !isOpen)
        {
            if (chestTop != null)
            {
                targetRotation = initialRotation * Quaternion.Euler(rotationAngle, 0, 0);
                if (itemDropSystem != null)
                {
                    Vector3 chestOffset = new Vector3(Random.Range(1f,4f), 0f, Random.Range(1f,4f));
                    itemDropSystem.HandleChestOpen(transform.position + chestOffset);
                }

                isOpen = true;
            }
        }
    }
    private void ChangeKeyOpacity(Color targetColor)
    {
        displayKey = true;
        eKeyPlaneMaterial.color = targetColor;
    }

    private bool CanIncreaseOpacity()
    {
        return eKeyPlaneMaterial.color.a <= 1;
    }

    private Color IncreaseOpacity()
    {
        return CanIncreaseOpacity() ? new Color(eKeyPlaneMaterial.color.r, eKeyPlaneMaterial.color.g, eKeyPlaneMaterial.color.b, (eKeyPlaneMaterial.color.a + 0.01F)) : eKeyPlaneMaterial.color;
    }
}
