using UnityEditor.Rendering;
using UnityEngine;

public class LucidityPostProcess : MonoBehaviour
{

    private Transform lucidCamera;
    private Player player;
    public float minScale; // Initial radius when lucidity is at its lowest
    public float maxScale;    // Minimum radius when lucidity is at its max

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        lucidCamera = GameObject.FindGameObjectWithTag("Mask").GetComponent<Transform>();
    }

    public void UpdateLucidityMask(float lucidity)
    {
      //  Debug.Log($"KVOTEN: {lucidity / player.maxLucidity}");
        float radius = Mathf.Lerp(minScale, maxScale, lucidity / player.maxLucidity);
 //       Debug.Log($"KVOTEN: {lucidity / player.maxLucidity} RADIUS: {radius}");
        lucidCamera.localScale = new Vector3(radius*1.10F, radius*0.65F, radius);   
    }

    
}
