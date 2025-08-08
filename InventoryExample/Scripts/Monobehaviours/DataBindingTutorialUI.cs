using System;
using Unity.Properties;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class DataBindingTutorialUI : MonoBehaviour
{
    // Stylesheet to use for the UI, in production this would most likely be added to your .tss theme file
    // that is assigned to the panel, but for this tutorial we have an option way to access one during development
    [SerializeField]
    private StyleSheet stylesheet;

    // Data model that will be bound to and used to populate the UI elements
    private MyInventoryDataModel _inventoryDataModel;

    // Used to get a reference to the root of the visual tree
    private VisualElement _root;

    // Used as a container that we can applu a datasource to for binding data
    private VisualElement _formContainer;

    // Form Elements
    private DropdownField _dropdownField;
    private TextField _textField;
    private ListView _listView;
    private Button _button;

    private void OnEnable()
    {
        // When connecting to UI elements, using the OnEnable method will help keep null ref errors in check

        // Add your DataModel so you can link the elements to the data
        _inventoryDataModel = new MyInventoryDataModel();




        // Find and set the root document for the panel so we can add elements to its visual tree
        _root = GetComponent<UIDocument>().rootVisualElement;




        // Create a container to hold the UI and isolate the datasource,
        // IF you have a single datasource for the entire Panel
        // then you could set the root datasource to your datamodel
        // But if you want this datasource isolated in the UI then an additional container is needed
        _formContainer = new VisualElement()
        {
            // When an element has a name you can use it with #Name in your stylesheets
            // This is optional, but it can be useful for debugging and is a good practice
            // since named elements in code only change if purposefully changed.
            // For this element it would be #FormLayout in the stylesheet
            name = "FormLayout",
        };

        // Set the datasource for the form to our datamodel
        _formContainer.dataSource = _inventoryDataModel;

        // Add the form container to the root element
        _root.Add(_formContainer);




        // Add the stylesheet to the form container element so it can be rendered (if it exists).
        // This is optional, but if you have a stylesheet you can use it to style the UI elements.
        // This could also be applied to the root element if you wanted the styles to apply to the entire panel
        if (stylesheet != null)
        {
            _formContainer.styleSheets.Add(stylesheet);
        }




        // Add a header with a clock using the ClockElement from the CustomBinding Examples
        VisualElement header = new ClockElement();
        header.name = "WindowHeader";
        _formContainer.Add(header);


        VisualElement dropDownContainer = new VisualElement
        {
            style =
            {
                flexDirection = FlexDirection.Row
            }
        };

        _formContainer.Add(dropDownContainer);

        _dropdownField = new DropdownField()
        {
            name = "ItemTypeDropdown",
            style =
            {
                flexGrow = 1,
            },
            label = "Select Item Type:",
        };

        // Making the Label look better is not the label as you would think,
        // its the labelElement style you need to change, we are doing here
        // because its easier than finding the USS selector in the child element
        _dropdownField.labelElement.style.unityTextAlign = TextAnchor.MiddleLeft;
        _dropdownField.labelElement.style.unityFontStyleAndWeight = FontStyle.Bold;
        _dropdownField.labelElement.style.marginRight = 0;
        _dropdownField.labelElement.style.paddingRight = 5;
        _dropdownField.labelElement.style.flexGrow = 0;

        // this is the one that make the dropdown label seam uselessly large
        _dropdownField.labelElement.style.minWidth = 100;

        // Bind the dropdown "choices" property (which expects a list of strings)
        // to the data models "ItemTypes" which IS a list of strings
        // and set the mode to read only (target being the dropdown)
        _dropdownField.SetBinding("choices", new DataBinding()
        {
            dataSourcePath = new PropertyPath("ItemTypes"),
            bindingMode = BindingMode.ToTarget // Read only setting
        });

        // Also, bind the selected value of the dropdown to a different property in the model.
        // This is the one that will be updated when the user selects an item type.
        // The model will then update the list based on the new filter value
        _dropdownField.SetBinding("value", new DataBinding()
        {
            dataSourcePath = new PropertyPath("SelectedItemType"),
            bindingMode = BindingMode.TwoWay // Read/Write setting
        });

        // Add the DropdownField to the UI
        dropDownContainer.Add(_dropdownField);


        // We will use a button to demonstrate capturing click events in a simple way
        Button formReset = new Button
        {
            text = "Reset",
            name = "ResetButton"
        };
        dropDownContainer.Add(formReset);
        // You could register a callback OR just use the one built into the button
        formReset.clicked += () =>
        {
            // We have a simple untracked 'property' that just calls a method when a value is set.
            // This is a super easy way to catch click events in a UI without having to register
            // a bunch of callbacks since the button has one built in
            _inventoryDataModel.RaiseResetFormEvent = true;
        };



        // Add a text field to filter the list items
        // This is a simple text field that will update the filter text property in the model
        _textField = new TextField
        {
            name = "FilterText",
        };

        _textField.SetBinding("value", new DataBinding()
        {
            dataSourcePath = new PropertyPath("FilterText"),
            bindingMode = BindingMode.ToSource  // Write Only setting
        });
        _formContainer.Add(_textField);




        // Add a list view to display the filtered items
        // This is a list view that will be updated by the data model
        _listView = new ListView
        {
            name = "ListView",
            // add some inline style to make it more visible
            showAlternatingRowBackgrounds = AlternatingRowBackground.All,
            // Use a hybrid approch to allow the use of a class to create the line items
            // this allows for advanced form views with multiple control elements at runtime
            makeItem = () =>
            {
                var lineItem = new MakeListViewLineItem();
                return lineItem;
            },
            fixedItemHeight = 71,
            showBorder = true,
            // Use an isolated method group to load data in the line items
            bindItem = Item
        };

        // Bind the list view data to the data models output
        _listView.SetBinding("itemsSource", new DataBinding() {dataSourcePath = new PropertyPath("Items")});

        _formContainer.Add(_listView);
        return;


        // This is only past the return because its an isolated method use to build list line items
        void Item(VisualElement e, int i) => BindItem(e as MakeListViewLineItem, i);
    }
    private void BindItem(MakeListViewLineItem makeListViewLineItem, int i)
    {
        var label = makeListViewLineItem.Q<Label>("nameLabel");
        var valueLabel = makeListViewLineItem.Q<Label>("valueLabel");
        var itemDescription = makeListViewLineItem.Q<Label>("itemDescription");
        var iconHolder = makeListViewLineItem.Q<VisualElement>("IconHolder");

        var item = _inventoryDataModel.Items[i];
        label.text = item.name;
        valueLabel.text = item.value.ToString();
        itemDescription.text = item.description;
        iconHolder.style.backgroundImage = new StyleBackground(item.icon);
    }

    public class MakeListViewLineItem: VisualElement
    {

        public MakeListViewLineItem()
        {
            VisualElement lineItem = new VisualElement()
            {
                name = "LineItem"
            };
            Add(lineItem);

            VisualElement iconHolder = new VisualElement()
            {
                name = "IconHolder",
            };
            lineItem.Add(iconHolder);

            VisualElement itemDetails = new VisualElement()
            {
                name = "ItemDetails"
            };

            lineItem.Add(itemDetails);

            VisualElement detailsHeader = new VisualElement()
            {
                name = "DetailsHeader"
            };

            itemDetails.Add(detailsHeader);

            Label nameLabel = new Label()
            {
                name = "nameLabel"
            };

            detailsHeader.Add(nameLabel);

            Label valueLabel = new Label()
            {
                name = "valueLabel"
            };

            detailsHeader.Add(valueLabel);

            VisualElement descArea = new VisualElement()
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    justifyContent = Justify.FlexStart,
                    textOverflow = TextOverflow.Ellipsis,
                    overflow = Overflow.Hidden,
                    flexGrow = 1,
                }
            };
            itemDetails.Add(descArea);

            Label itemDescription = new Label()
            {
                name = "itemDescription"
            };

            descArea.Add(itemDescription);

        }

    }
}
