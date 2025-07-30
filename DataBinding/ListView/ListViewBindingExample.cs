using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.ComponentModel;
using Unity.Properties;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class ListExampleDataModel : INotifyPropertyChanged
{
    // Use the DontCreateProperty on the private var to keep it isolated
    [DontCreateProperty]
    private List<string> items;

    // It is considered best practice to use the public param as the property
    // so that you can perform other functions such as validation or raise events
    [CreateProperty]
    public List<string> Items
    {
        get => items;
        set
        {
            if (items != value)
            {
                items = value;
                OnPropertyChanged(nameof(Items));
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


public class ListViewBindingExample : MonoBehaviour
{
    private ListExampleDataModel _dataModel;
    private ListView _listView;
    private InputAction _spaceAction;
    void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _dataModel = new ListExampleDataModel
        {
            Items = new List<string>{ "Item 1", "Item 2", "Item 3" }
        };

        _listView = new ListView();
        // add some inline style to make it more visible
        _listView.showAlternatingRowBackgrounds = AlternatingRowBackground.All;
        _listView.style.backgroundColor = new StyleColor(Color.white);

        // Bind the ListView to the datasource
        _listView.dataSource = _dataModel;
        _listView.SetBinding("itemsSource", new DataBinding() {dataSourcePath = new PropertyPath("Items")});

        // Add the ListView to the UI
        root.Add(_listView);
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
        _dataModel.Items = new List<string>
        {
            $"Item 1 with rand {Random.Range(1, 1000)}",
            $"Item 2 with rand {Random.Range(1, 1000)}",
            $"Item 3 with rand {Random.Range(1, 1000)}"
        };
    }
}