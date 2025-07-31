using System.ComponentModel;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;
public class MyEventModel : INotifyPropertyChanged
{
    // Use the DontCreateProperty on the private var to keep it isolated
    [DontCreateProperty]
    private string eventMessage = "Click Me";

    // It is considered best practice to use the public param as the property
    // so that you can perform other functions such as validation
    [CreateProperty]
    public string EventMessage
    {
        get => eventMessage;
        set
        {
            if (eventMessage != value)
            {
                eventMessage = value;
                OnPropertyChanged(nameof(EventMessage));
            }
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
public class MyEventUI : MonoBehaviour
{
    private MyEventModel eventModel;
    private Button eventButton;
    void Start()
    {
        eventModel = new MyEventModel();

        var root = GetComponent<UIDocument>().rootVisualElement;

        eventButton = new Button(() => OnButtonClicked());
        eventButton.dataSource = eventModel;
        eventButton.SetBinding("text", new DataBinding()
        {
            dataSourcePath = new PropertyPath("EventMessage")
        });

        // Bind the button to the data model
        eventButton.RegisterCallback<ClickEvent>(evt =>
        {
            eventModel.EventMessage = $"Button Clicked! {Random.Range(0, 1000)}";
            Debug.Log(eventModel.EventMessage);
        });

        // Add the button to the UI
        root.Add(eventButton);
    }
    private void OnButtonClicked()
    {
        // Handle button click event
        Debug.Log("Button was clicked!");
    }
}