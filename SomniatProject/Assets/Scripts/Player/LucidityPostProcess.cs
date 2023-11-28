using UnityEditor.Rendering;
using UnityEngine;

public class LucidityPostProcess : MonoBehaviour
{

    private Transform lucidCamera;
    private Player player;
    public float minScale = 0.01f; // Initial radius when lucidity is at its lowest
    public float maxScale = 3.0f;    // Minimum radius when lucidity is at its max

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        lucidCamera = GameObject.FindGameObjectWithTag("Mask").GetComponent<Transform>();
    }

    public void UpdateLucidityMask(float lucidity)
    {
        float radius = Mathf.Lerp(minScale, maxScale, lucidity / player.maxLucidity);
       
        lucidCamera.localScale = new Vector3(radius, radius*0.5F, radius);
    }

    
}
