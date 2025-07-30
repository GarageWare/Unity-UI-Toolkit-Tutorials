using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.ComponentModel;
using Unity.Properties;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class MyDropdownModel : INotifyPropertyChanged
{

    private string _selectedOption;
    public string SelectedOption
    {
        get => _selectedOption;
        set
        {
            if (_selectedOption != value)
            {
                _selectedOption = value;
                OnPropertyChanged(nameof(SelectedOption));
            }
        }
    }

    // Use the DontCreateProperty on the private var to keep it isolated
    [DontCreateProperty]
    private List<string> _options;
    // It is considered best practice to use the public param as the property
    // so that you can perform other functions such as validation
    [CreateProperty]public List<string> Options
    {
        get => _options;
        set
        {
            if (_options != value)
            {
                _options = value;
                OnPropertyChanged(nameof(Options));
            }
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

// Create a monobehavior class to manage the UI
public class DropdownBindingExample : MonoBehaviour
{
    private MyDropdownModel _dropdownModel;
    private DropdownField _dropdownField;
    private InputAction _spaceAction;

    void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _dropdownModel = new MyDropdownModel
        {
            Options = new List<string>
            {
                "Option 1",
                "Option 2",
                "Option 3"
            },
            SelectedOption = "Option 1"
        };

        _dropdownField = new DropdownField();
        _dropdownField.dataSource = _dropdownModel;
        _dropdownField.SetBinding("choices", new DataBinding()
        {
            dataSourcePath = new PropertyPath("Options")
        });
        // Update the data model when the selection changes
        _dropdownField.RegisterValueChangedCallback(evt =>
        {
            _dropdownModel.SelectedOption = evt.newValue;
        });

        // Add the DropdownField to the UI
        root.Add(_dropdownField);
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
        _dropdownModel.Options = new List<string>
        {
            $"Item 1 with rand {Random.Range(1, 1000)}",
            $"Item 2 with rand {Random.Range(1, 1000)}",
            $"Item 3 with rand {Random.Range(1, 1000)}"
        };
    }
}