using UnityEditor.Rendering;
using UnityEngine;

public class LucidityPostProcess : MonoBehaviour
{

    private Transform lucidCamera;
    private Transform lucidCapsule;
    private Player player;
    public float minScale; // Initial radius when lucidity is at its lowest
    public float maxScale;    // Minimum radius when lucidity is at its max
    private float minCapsuleScale = 0.005F;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        lucidCamera = GameObject.FindGameObjectWithTag("Mask").GetComponent<Transform>();
        lucidCapsule = GameObject.FindGameObjectWithTag("LucidCapsule").GetComponent<Transform>();
    }

    public void UpdateLucidityMask(float lucidity)
    {
      //  Debug.Log($"KVOTEN: {lucidity / player.maxLucidity}");
        float radius = Mathf.Lerp(minScale, maxScale, lucidity / player.maxLucidity);
        float capsuleRadius = Mathf.Lerp(minCapsuleScale, maxScale, lucidity / player.maxLucidity);

        //       Debug.Log($"KVOTEN: {lucidity / player.maxLucidity} RADIUS: {radius}");
        lucidCamera.localScale = new Vector3(radius*1.10F, radius*0.65F, radius);  
        lucidCapsule.localScale = new Vector3(capsuleRadius * 1.10F, capsuleRadius * 0.65F, capsuleRadius);
    }

    
}
