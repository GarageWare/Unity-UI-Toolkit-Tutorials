using System;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Properties;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Represents a data model that can be used to power the UI by binding its properties
/// to visual elements. Provides functionality to notify subscribers of property value changes.
/// </summary>
public class MyInventoryDataModel : INotifyPropertyChanged
{
    // This is the data model that will be used to power the UI
    private readonly List<ItemData> _masterItemList;

    // Each visual element in the tutorial has its own #region defined
#region TextField Example
    // It is considered best practice to use the public param as the property with a getter and setter
    // so that you can perform other functions such as validation and raising the PropertyChanged event

    /// <summary>
    /// Represents the public property for accessing or modifying the text content encapsulated by the data model.
    /// Changes to this property trigger notifications to inform subscribers of updates, enabling UI bindings
    /// or other reactionary mechanisms to reflect the altered state of the data.
    /// </summary>
    [CreateProperty] // Use the CreateProperty attribute to mark this field as bindable
    public string FilterText
    {
        // Get just returns the value
        get
        {
            return _filterText;
        }
        // Set saves the new value and raises an event to alert subscribers of the change
        set
        {
            if (_filterText != value)
            {
                _filterText = value;
                // Filter the results based on the new text
                FilterByFilterText(value);
                OnPropertyChanged(nameof(FilterText));
            }
        }
    }
    /// <summary>
    /// Stores the raw text content for the data model. This private field holds the text value
    /// and acts as the backing field for the public <see cref="FilterText"/> property. The value
    /// changes can be observed via the property change notification mechanism to support UI updates
    /// or other reactive programming patterns.
    /// </summary>
    [DontCreateProperty] // Use the DontCreateProperty attribute on the private var to keep it isolated
    private string _filterText;
#endregion

#region DropdownField Example

    /// <summary>
    /// Represents the currently selected option in a collection of choices.
    /// This property supports data binding and notifies subscribers of value changes, enabling dynamic updates
    /// to the associated UI elements or other dependent components.
    /// </summary>
    [CreateProperty]
    public string SelectedItemType
    {
        get => _selectedItemType;
        set
        {
            if (_selectedItemType != value)
            {
                _selectedItemType = value;
                FilterByItemType();
                OnPropertyChanged(nameof(SelectedItemType));
            }
        }
    }
    /// <summary>
    /// Stores the underlying value for the selected option in the data model.
    /// This private backing field is accessed and modified via the public property 'SelectedItemType',
    /// which handles validation, notification of changes, and other associated logic.
    /// </summary>
    [DontCreateProperty]
    private string _selectedItemType;

    /// <summary>
    /// Represents the public property for managing a collection of selectable options within the data model.
    /// Changes to this property notify subscribers, facilitating dynamic UI updates or other mechanisms
    /// relying on the modification and reflection of available options.
    /// </summary>
    [CreateProperty]
    public List<string> ItemTypes
    {
        get => _itemTypes;
        set
        {
            if (_itemTypes != value)
            {
                _itemTypes = value;
                OnPropertyChanged(nameof(ItemTypes));
            }
        }
    }
    /// <summary>
    /// Defines the private backing field for storing a list of options managed within the data model.
    /// Interactions with this field are typically mediated through its corresponding public property
    /// to ensure proper encapsulation, event triggering, or validation logic.
    /// </summary>
    [DontCreateProperty]
    private List<string> _itemTypes = new List<string>();
#endregion

#region Listview Example
    /// <summary>
    /// Represents a collection of items within the data model, allowing for retrieval or modification
    /// of the list. Changes to this property notify subscribers of updates, enabling dynamic UI bindings
    /// or other mechanisms to respond to the modified collection data.
    /// </summary>
    [CreateProperty]
    public List<ItemData> Items
    {
        get => _items;
        set
        {
            if (_items != value)
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }
    }
    /// <summary>
    /// Represents a collection of strings that can be accessed or modified.
    /// Changes to this collection notify subscribers, facilitating updates to UIs or other dependent systems.
    /// </summary>
    [DontCreateProperty]
    private List<ItemData> _items = new List<ItemData>();
#endregion

#region Event Example

    /// <summary>
    /// Represents an unbindable property that, when set to true, triggers the invocation of the form reset process.
    /// It is used to signal the need for resetting the state of the form and does not provide
    /// a backing store for persistent data. Setting this property performs the reset operation directly
    /// and offers a simple way to capture a button event in the UI.
    /// </summary>
    public bool RaiseResetFormEvent
    {
        set
        {
            if (value)
            {
                ResetForm();
            }
        }
    }

    /// <summary>
    /// Represents the message associated with an event, which can be dynamically updated
    /// and notifies subscribers about its changes. Typically used to display or log event-related
    /// information, supporting reactive UI updates or behavior when modified.
    /// </summary>
    [CreateProperty]
    public string EventMessage
    {
        get => _eventMessage;
        set
        {
            if (_eventMessage != value)
            {
                _eventMessage = value;
                OnPropertyChanged(nameof(EventMessage));
            }
        }
    }
    /// <summary>
    /// Represents the event message associated with the data model. This property is typically utilized
    /// to provide feedback or display status messages resulting from user interaction or internal logic.
    /// Changes to this property trigger notification events to update any observers or bound UI elements.
    /// </summary>
    [DontCreateProperty]
    private string _eventMessage = "Reset";
#endregion

#region CustomDataBinding Example

    /// <summary>
    /// Represents the property for accessing or modifying the currently selected item
    /// in a collection. Changing this property's value triggers the notification mechanism
    /// to inform subscribers of the update, enabling responsive actions such as updating
    /// the user interface or performing related operations.
    /// </summary>
    [CreateProperty]
    public ItemData SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (_selectedItem != value)
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
    }
    /// <summary>
    /// Represents the currently selected item from a collection of available items within the data model.
    /// Assigning a new value to this property triggers a notification to inform subscribers of the change,
    /// enabling UI updates or other actions dependent on the selected item's state.
    /// </summary>
    [DontCreateProperty]
    private ItemData _selectedItem;

#endregion

    /// <summary>
    /// Represents a data model that implements the INotifyPropertyChanged interface to provide
    /// notifications when properties of the model change. This class is designed to handle and manage
    /// data, including initialization and loading of values, which can then be observed by subscribers.
    /// </summary>
    public MyInventoryDataModel()
    {
        // Initialize the data model with some default values
        // In an actual project, this is where you would load your data from whatever source you are using
        // For this tutorial we will load some static data from a JSON array
        _masterItemList = new List<ItemData>();
        LoadSimulatedData();
    }

    /// <summary>
    /// Filters the `Items` list based on the currently selected item type.
    /// If the selected item type is `AllItems`, all items are included.
    /// Otherwise, only items matching the selected type are retained in the list.
    /// </summary>
    private void FilterByItemType()
    {
        // Filter the results based on the selected option
        if (SelectedItemType == nameof(ItemType.AllItems))
        {
            Items = new List<ItemData>(_masterItemList);
        }
        else
        {
            Items = new List<ItemData>(_masterItemList);
            Items.RemoveAll(item => item.type != (ItemType)Enum.Parse(typeof(ItemType), SelectedItemType));
        }
    }

    /// <summary>
    /// Filters the items in the collection based on the provided filter text. The method updates the
    /// `Items` property to contain only those items that match the specified criteria in their
    /// name, description, or value.
    /// </summary>
    /// <param name="filterText">The text used to filter items. Items are retained if their
    /// name, description, or value contains this text, ignoring case.</param>
    private void FilterByFilterText(string filterText)
    {
        // If the filter text is empty, reset the list to the full list
        if (string.IsNullOrEmpty(filterText))
        {
            Items = new List<ItemData>(_masterItemList);
            return;
        }

        // Filter the list by the filter text, look in name OR description OR value converted to int
        Items = new List<ItemData>(_masterItemList);
        // First filter to the type
        FilterByItemType();
        // Then filter by the text
        Items.RemoveAll(item => item.name.ToLower().IndexOf(filterText.ToLower(), StringComparison.Ordinal) == -1 &&
                                item.description.ToLower().IndexOf(filterText.ToLower(), StringComparison.Ordinal) == -1 &&
                                item.value.ToString().ToLower().IndexOf(filterText.ToLower(), StringComparison.Ordinal) == -1);
    }

    // This is the event raised when a property changes
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Notifies subscribers that a property value has changed. This method is commonly used
    /// within property setters to trigger UI updates or other responses in code that observes
    /// property change events.
    /// </summary>
    /// <param name="propertyName">The name of the property whose value has changed. Typically passed as the name of the calling property.</param>
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Resets the form by clearing the filter text, setting the selected item type to "AllItems",
    /// deselecting any selected item, and updating the event message to indicate the form has been reset.
    /// This method is typically invoked when a user action or event requires the UI form to be cleared
    /// and restored to its default state.
    /// </summary>
    private void ResetForm()
    {
        FilterText = "";
        SelectedItemType = nameof(ItemType.AllItems);
        SelectedItem = null;
        EventMessage = "Form Reset";
    }

    /// <summary>
    /// Populates the data model with simulated data for testing or demonstration purposes.
    /// This method initializes various properties with predefined values to simulate
    /// the structure and content of the data typically handled by the model.
    /// </summary>
    private void LoadSimulatedData()
    {
        // turn the item type enum into a list to use in the model
        // so it can be displayed easily in a dropdown
        foreach (var item in Enum.GetValues(typeof(ItemType)))
        {
            ItemTypes.Add(item.ToString());
        }
        SelectedItemType = null;
        SelectedItem = null;

        // Normally this would be a loop of your data output during a load event
        // of some type, but for this tutorial we are just creating 9 static items
        // add 3 foods
        var item1 = new ItemData
        {
            name = "Apple",
            type = ItemType.Food,
            description = "A delicious apple",
            value = 10,
            icon = Resources.Load<Sprite>("DataBinding/Apple")
        };
        _masterItemList.Add(item1);

        var item2 = new ItemData()
        {
            name = "Banana",
            type = ItemType.Food,
            description = "A delicious banana",
            value = 15,
            icon = Resources.Load<Sprite>("DataBinding/Banana")
        };
        _masterItemList.Add(item2);

        var item3 = new ItemData()
        {
            name = "Pear",
            type = ItemType.Food,
            description = "A delicious pear",
            value = 20,
            icon = Resources.Load<Sprite>("DataBinding/Pear")
        };
        _masterItemList.Add(item3);

        // add 3 weapons
        var item4 = new ItemData()
        {
            name = "Sword",
            type = ItemType.Weapon,
            description = "A sharp sword",
            value = 10,
            icon = Resources.Load<Sprite>("DataBinding/Sword")
        };
        _masterItemList.Add(item4);

        var item5 = new ItemData()
        {
            name = "Axe",
            type = ItemType.Weapon,
            description = "A trusty axe",
            value = 15,
            icon = Resources.Load<Sprite>("DataBinding/Axe")
        };
        _masterItemList.Add(item5);

        var item6 = new ItemData()
        {
            name = "Long Bow",
            type = ItemType.Weapon,
            description = "A hunters long bow",
            value = 20,
            icon = Resources.Load<Sprite>("DataBinding/LongBow")
        };
        _masterItemList.Add(item6);

        // add 3 armor
        var item7 = new ItemData()
        {
            name = "Leather Armor",
            type = ItemType.Armor,
            description = "Sturdy Leather armor",
            value = 10,
            icon = Resources.Load<Sprite>("DataBinding/LeatherArmor")
        };
        _masterItemList.Add(item7);

        var item8 = new ItemData()
        {
            name = "Chain Mail",
            type = ItemType.Armor,
            description = "Reliable Chain mail armor",
            value = 15,
            icon = Resources.Load<Sprite>("DataBinding/ChainMail")
        };
        _masterItemList.Add(item8);

        var item9 = new ItemData()
        {
            name = "Plate Mail",
            type = ItemType.Armor,
            description = "Heavy Full Plate mail armor",
            value = 20,
            icon = Resources.Load<Sprite>("DataBinding/PlateMail")
        };
        _masterItemList.Add(item9);

        SelectedItemType = nameof(ItemType.AllItems);
    }
}

/// <summary>
/// Defines the types of items that can be categorized and filtered within the application.
/// Used to classify and manage items based on their specific category.
/// </summary>
public enum ItemType
{
    AllItems,
    Food,
    Weapon,
    Armor
}

/// <summary>
/// Represents an item with associated metadata such as name, type, description, value, and icon.
/// </summary>
[Serializable]
public class ItemData
{
    public string name;
    public ItemType type;
    public string description;
    public int value;
    public Sprite icon;
}