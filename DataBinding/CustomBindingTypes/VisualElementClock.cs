using UnityEngine;
using UnityEngine.UIElements;

public class VisualElementClock : MonoBehaviour
{
    VisualElement root;
    private void Awake()
    {
        // Bare minimum for working clock using data binding
        root = GetComponent<UIDocument>().rootVisualElement;
        Label label = new Label();
        label.SetBinding("text",  new CurrentTimeBinding());
        root.Add(label);

        // The rest is just for looks and can be applied in your style sheet
        root.style.alignContent = Align.Center;
        root.style.justifyContent = Justify.Center;
        label.style.fontSize = 100;
        label.style.color = Color.white;
        label.style.alignContent = Align.Center;
        label.style.justifyContent = Justify.Center;
        label.style.unityTextAlign = TextAnchor.MiddleCenter;
        label.style.unityFontStyleAndWeight = FontStyle.Bold;
        label.style.backgroundColor = new StyleColor(Color.black);
    }
}
