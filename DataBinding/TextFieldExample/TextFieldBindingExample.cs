using UnityEngine;
using UnityEngine.UIElements;
using System.ComponentModel;
using Unity.Properties;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

/// <summary>
/// Data model that powers the UI.
/// This can be anywhere and is included in this file for simplicity
/// In a production environment you would want the data model to be in a separate file.
/// </summary>
public class TextFieldDataModel : INotifyPropertyChanged
{
    // Use the DontCreateProperty on the private var to keep it isolated
    [DontCreateProperty]
    private string myText;

    // It is considered best practice to use the public param as the property
    // so that you can perform other functions such as validation
    [CreateProperty]
    public string MyText
    {
        get => myText;
        set
        {
            if (myText != value)
            {
                myText = value;
                OnPropertyChanged(nameof(MyText));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    // This is a helper function to notify the UI that a property has changed
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}


/// <summary>
/// MonoBehaviour that allows attachment to a game object.
/// This file should be attached to a GameObject with a UIDocument component and a UXML document assigned
/// the UXML can be blank for this demonstration
/// </summary>
public class TextFieldBindingExample : MonoBehaviour
{
    private TextFieldDataModel _dataModel;
    private TextField _textField;
    private InputAction _spaceAction;
    void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        _dataModel = new TextFieldDataModel();
        _textField = new TextField();

        // Setting the data source of an element will automatically apply to its children
        // UNLESS specifically changed at a later time.
        root.dataSource = _dataModel;

        // Bind the text field to the data model using SetBinding
        // Set Binding needs the items destination container, here it is the VALUE for the Textfield element
        // and a binding that contains the datasourcepath within the datasource container
        // Here it is using a new PropertyPath with the name of the datasource and the field within.
        _textField.SetBinding("value", new DataBinding
        {
            dataSourcePath = new PropertyPath(nameof(_dataModel.MyText))

        });
        // Thats it, any data that changes to the value of datamodel.MyText will be populated into the UI element
        // Further options can be set that allow for two way data exchange and can be set when setting the datasource
        // like so: bindingMode = BindingMode.TwoWay
        // See: https://docs.unity3d.com/Manual/UIE-runtime-binding-mode-update.html

        // Create an action to listen for a space bar press
        _spaceAction = new InputAction(binding: "<Keyboard>/space");
    }
    private void OnEnable()
    {
        _spaceAction.Enable();
        _spaceAction.performed += ChangeData;
    }

    private void OnDisable()
    {
        _spaceAction.Disable();
        _spaceAction.performed -= ChangeData;
    }


    private void ChangeData(InputAction.CallbackContext callbackContext)
    {
        // Change the data in the DataModel and see it update the UI
        _dataModel.MyText = $"Hello, World! {Random.Range(0, 1000)} ";
    }
}