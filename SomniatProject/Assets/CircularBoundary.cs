using UnityEngine;
using UnityEngine.UI;

public class CircularBoundary : MonoBehaviour
{
    public RectTransform boundaryImage;
    public Transform player;  // The player's Transform
    public float maxLucidity = 100f;
    public float lucidityRadiusMultiplier = 0.1f;

    private float currentLucidity;

    private void Start()
    {
        currentLucidity = maxLucidity;
    }

    private void Update()
    {
        // Assuming lucidity decreases with time or certain events
        currentLucidity -= Time.deltaTime;
        currentLucidity = Mathf.Clamp(currentLucidity, 0f, maxLucidity);

        UpdateCircularBoundary();
    }

    private void UpdateCircularBoundary()
    {
        // Calculate the radius based on lucidity
        float lucidityRadius = currentLucidity * lucidityRadiusMultiplier;

        // Update position to match the player
        boundaryImage.position = Camera.main.WorldToScreenPoint(player.position);

        // Update size based on lucidity radius
        boundaryImage.sizeDelta = new Vector2(lucidityRadius * 2, lucidityRadius * 2);
    }
}
