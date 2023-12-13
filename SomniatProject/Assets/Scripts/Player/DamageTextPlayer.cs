using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(menuName = "ScriptableObjects/DamgeTextPlayer", fileName = "DamgeTextPlayer")]
public class DamageTextPlayer : ScriptableObject
{
    public TextMeshPro textMeshPro;

    public async void SubtractHealth(int value, Transform transform)
    {

        await CreateDamageText(transform, SubtractHealthColor(value), value);

    }

    public async void AddHealth(int value, Transform transform)
    {
        await CreateDamageText(transform, AddHealthColor(value), value);
    }

    private async Task CreateDamageText(Transform transform, Color color, int value)
    {
        TextMeshPro damageText = Instantiate(textMeshPro, transform.position, textMeshPro.transform.rotation);

        damageText.transform.position = new Vector3(transform.position.x + Random.Range(-2, 2), transform.position.y + 1.5F + Random.Range(0, 1), transform.position.z + Random.Range(-2, 2));
        damageText.text = value.ToString();
        damageText.faceColor = new Color32((byte)color.r, (byte)color.g, (byte)color.b, 255);
        damageText.fontSize = FontSize(value);
        while (damageText.color.a > 0.01F)
        {
            damageText.color = new Color(color.r, color.g, color.b, damageText.color.a - 0.01F);
            await Task.Delay(10);
        }

        Destroy(damageText.gameObject);
    }

    private Color AddHealthColor(int value)
    {
        int modifier = 0;

        if (value < 10)
            modifier = 0;
        else if (value < 25)
            modifier = 10;
        else if (value < 50)
            modifier = 15;
        else if (value < 100)
            modifier = 20;

        return new Color(Mathf.Clamp(10 + modifier, 0, 255), Mathf.Clamp(192 + modifier, 0, 255), Mathf.Clamp(57 + modifier, 0, 255));

    }

    private Color SubtractHealthColor(int value)
    {
        int modifier = 0;

        if (value < 10)
            modifier = 0;
        else if (value < 25)
            modifier = 10;
        else if (value < 50)
            modifier = 15;
        else if (value < 100)
            modifier = 20;

        return new Color(Mathf.Clamp(192 - modifier, 0, 255), Mathf.Clamp(15 - modifier, 0, 255), Mathf.Clamp(10 - modifier, 0, 255));
    }

    private float FontSize(int value)
    {
        float modifier = 0;
        var baseSize = textMeshPro.fontSize;
        if (value < 10)
            modifier = 0;
        else if (value < 25)
            modifier = 20;
        else if (value < 50)
            modifier = 40;
        else if (value < 100)
            modifier = 60;

        return baseSize + modifier;
    }




}
