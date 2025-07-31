using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Unity.Properties;
using UnityEngine.UIElements;

public class DataSource : IDataSourceViewHashProvider, INotifyBindablePropertyChanged
{
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
    private long m_ViewVersion;
    public event EventHandler<BindablePropertyChangedEventArgs> propertyChanged;

    public void Publish()
    {
        ++m_ViewVersion;
    }
    public long GetViewHashCode()
    {
        return m_ViewVersion;
    }
    void Notify([CallerMemberName] string property = "")
    {
        propertyChanged?.Invoke(this, new BindablePropertyChangedEventArgs(property));
    }
#endregion
}