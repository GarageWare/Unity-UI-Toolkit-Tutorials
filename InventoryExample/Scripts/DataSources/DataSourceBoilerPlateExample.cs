using System;
using System.Runtime.CompilerServices;
using Unity.Properties;
using UnityEngine.UIElements;

/// <summary>
/// The DataSourceBoilerPlateExample class represents a bindable data source that can notify observers
/// when its properties change or when its view state is updated. It implements
/// <see cref="IDataSourceViewHashProvider"/> and <see cref="INotifyBindablePropertyChanged"/>
/// to support view state versioning and property change notifications.<br/><br/>
/// Code compiled from Unity Documentation Examples at:<br/>
/// see https://docs.unity3d.com/Manual/UIE-runtime-binding-define-data-source.html
/// </summary>
public class DataSourceBoilerPlateExample : IDataSourceViewHashProvider, INotifyBindablePropertyChanged
{
    /// <summary>
    /// Represents an integer property that triggers change notifications when its value is modified.
    /// This property is monitored for state updates, ensuring dependent systems can respond
    /// appropriately to changes by invoking the <see cref="Notify"/> method whenever the value changes.
    /// </summary>
    [CreateProperty]
    public int Value
    {
        get => _value;
        set
        {
            if (_value == value)
                return;
            _value = value;
            Notify();       // Always call Notify() when a change happens
        }
    }
    private int _value;

    /// <summary>
    /// Represents an integer property that triggers change notifications when its value is modified.
    /// This property is monitored for state updates, ensuring dependent systems can respond
    /// appropriately to changes by invoking the <see cref="Notify"/> method whenever the value changes.
    /// </summary>
    [CreateProperty]
    public int OtherValue
    {
        get => _otherValue;
        set
        {
            if (_otherValue == value)
                return;
            _otherValue = value;
            Notify();       // Always call Notify() when a change happens
        }
    }
    private int _otherValue;

    // Boilerplate code below, just needs fields for data as shown above
#region boilerplate
    /// <summary>
    /// Represents a version number that tracks changes to the view state of a data source.
    /// This version is incremented whenever a change occurs by calling the <see cref="Publish"/> method,
    /// ensuring that any dependent systems can detect and react to updates in the data source.
    /// </summary>
    private long m_ViewVersion;
    public event EventHandler<BindablePropertyChangedEventArgs> propertyChanged;

    /// <summary>
    /// Increments the internal versioning identifier used to track changes in the data source view state.
    /// This method ensures the data source's view version is updated, enabling systems relying
    /// on this version to identify and respond to updates or changes in the data source.
    /// </summary>
    public void Publish()
    {
        ++m_ViewVersion;
    }

    /// <summary>
    /// Retrieves the current versioning identifier for the data source's view state.
    /// This identifier is used to track changes, allowing dependent systems to detect and respond
    /// to updates in the data source.
    /// </summary>
    /// <returns>
    /// A long integer representing the current view state version of the data source.
    /// </returns>
    public long GetViewHashCode()
    {
        return m_ViewVersion;
    }

    /// <summary>
    /// Notifies listeners about a change to a bindable property.
    /// This method is responsible for triggering the <see cref="propertyChanged"/> event
    /// when a property change occurs, passing the name of the property that changed.
    /// </summary>
    /// <param name="property">
    /// The name of the property that has changed. Automatically resolved using
    /// the calling member's name if not explicitly provided.
    /// </param>
    void Notify([CallerMemberName] string property = "")
    {
        propertyChanged?.Invoke(this, new BindablePropertyChangedEventArgs(property));
    }
#endregion
}