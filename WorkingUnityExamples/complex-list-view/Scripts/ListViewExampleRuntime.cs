using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkitExamples
{
    public class ListViewExampleRuntime : MonoBehaviour
    {

        UIDocument rootDocument;
        VisualElement root;
        private void OnEnable()
        {
            TryGetComponent<UIDocument>(out rootDocument);
            root = rootDocument.rootVisualElement;

            root.Add(new ListViewExample());
        }
    }
}
