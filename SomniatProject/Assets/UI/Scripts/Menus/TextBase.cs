using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBase : CustomUIComponent
{
    public TextSO textData;
    public Style style;

    public TextMeshProUGUI textMeshProUGUI;
    public override void Setup()
    {
        textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
    }

    public override void Configure()
    {
        textMeshProUGUI.color = textData.theme.GetTextColor(style);
        textMeshProUGUI.font = textData.font;
        textMeshProUGUI.fontSize = textData.size;
    }
}