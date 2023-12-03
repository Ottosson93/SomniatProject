using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor.Networking.PlayerConnection;
#endif
using UnityEngine;

public class PostProcess : MonoBehaviour
{
    private Transform lucidCamera;
    public Shader shader;
    private Material material;
    public Player player;
    public float initialRadius = 3f; // Initial radius when lucidity is at max
    public float minRadius = 0.5f;    // Minimum radius when lucidity is at its lowest

    private void Start()
    {
        material = new Material(shader);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        lucidCamera = GameObject.FindGameObjectWithTag("Mask").GetComponent<Transform>();
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // Update shader properties based on lucidity

        float radius = Mathf.Lerp(minRadius, initialRadius, lucidCamera.localScale.x / 3);

        // Pass the radius to the shader
        material.SetFloat("_Radius", radius);

        Graphics.Blit(source, destination, material);
    }

    
}
