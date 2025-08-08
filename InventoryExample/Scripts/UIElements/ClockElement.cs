using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// This example is taken from the Unity 6 documentation examples and is here for an example of using a UXML object in Runtime Code
/// </summary>
[UxmlElement]
public partial class ClockElement : VisualElement
{
    [UxmlAttribute]
    public string TimeFormat = "HH:mm:ss";
    public ClockElement()
    {
        Label label = new Label();
        label.SetBinding("text",  new CurrentTimeBinding(){TimeFormat = TimeFormat});
        Add(label);

        // The rest is just for looks and can be applied in your style sheet
        style.alignContent = Align.Center;
        style.justifyContent = Justify.Center;
        label.style.fontSize = 10;
        label.style.color = Color.white;
        label.style.alignContent = Align.Center;
        label.style.justifyContent = Justify.Center;
        label.style.unityTextAlign = TextAnchor.MiddleCenter;
        label.style.unityFontStyleAndWeight = FontStyle.Bold;
        label.style.backgroundColor = new StyleColor(Color.black);
    }
}
