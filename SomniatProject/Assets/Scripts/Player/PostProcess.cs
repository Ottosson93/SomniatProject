using UnityEngine;

public class PostProcess : MonoBehaviour
{
    public Shader shader;
    private Material material;
    public Player player;

    public float initialRadius = 0.5f; // Initial radius when lucidity is at max
    public float minRadius = 0.01f;    // Minimum radius when lucidity is at its lowest

    private void Start()
    {
        material = new Material(shader);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // Update shader properties based on lucidity
        UpdateShaderProperties();

        Graphics.Blit(source, destination, material);
    }

    private void UpdateShaderProperties()
    {
        // Calculate the radius based on lucidity
        float radius = Mathf.Lerp(minRadius, initialRadius, player.lucidity / player.maxLucidity);

        // Pass the radius to the shader
        material.SetFloat("_Radius", radius);
    }

    
}
