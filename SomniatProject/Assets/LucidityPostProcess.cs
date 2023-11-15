using UnityEngine;

public class LucidityPostProcess : MonoBehaviour
{

    public Transform circle;
    public Player player;
    public float initialRadius = 0.5f; // Initial radius when lucidity is at max
    public float minRadius = 0.01f;    // Minimum radius when lucidity is at its lowest
    private float width;
    private float height;
    private void Start()
    {
        height = 1;
        width = 1;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        circle = GameObject.FindGameObjectWithTag("Mask").GetComponent<Transform>();
    }
    private void UpdateLucidityMask()
    {
        // Calculate the radius based on lucidity
        //float radius = Mathf.Lerp(minRadius, initialRadius, circle.lucidity / circle.maxLucidity);

//        Debug.wri
    }

    
}
