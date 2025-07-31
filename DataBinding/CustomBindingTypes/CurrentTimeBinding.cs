using System;
using Unity.Properties;
using UnityEngine.UIElements;

[UxmlObject]
public partial class CurrentTimeBinding : CustomBinding
{
    [UxmlAttribute]
    public string timeFormat = "HH:mm:ss";

    /// <summary>
    /// By default, the binding system updates a CustomBinding instance on every frame.
    /// To define update triggers, use the following methods:<br/><br/>
    /// MarkDirty: Sets the binding object as dirty so that it gets updated during the next cycle.<br/>
    /// updateTrigger: Use this enum property to change how the binding is updated.<br/>
    /// BindingResult: Use this method to customize the update process.<br/><br/>
    /// The BindingResult return is a struct that tells you whether the update was successful.<br/>
    /// It contains a status and a message.
    /// </summary>
    /// <param name="context">BindingContext</param>
    /// <returns>BindingResult struct that tells you whether the update was successful</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public CurrentTimeBinding()
    {
        updateTrigger = BindingUpdateTrigger.EveryUpdate;
    }

    protected override BindingResult Update(in BindingContext context)
    {
        var timeNow = DateTime.Now.ToString(timeFormat);
        var element = context.targetElement;
        if (ConverterGroups.TrySetValueGlobal(ref element, context.bindingId, timeNow, out var errorCode))
            return new BindingResult(BindingStatus.Success);

        // Error handling
        var bindingTypename = TypeUtility.GetTypeDisplayName(typeof(CurrentTimeBinding));
        var bindingId = $"{TypeUtility.GetTypeDisplayName(element.GetType())}.{context.bindingId}";

        return errorCode switch
        {
            VisitReturnCode.InvalidPath => new BindingResult(BindingStatus.Failure, $"{bindingTypename}: Binding id `{bindingId}` is either invalid or contains a `null` value."),
            VisitReturnCode.InvalidCast => new BindingResult(BindingStatus.Failure, $"{bindingTypename}: Invalid conversion from `string` for binding id `{bindingId}`"),
            VisitReturnCode.AccessViolation => new BindingResult(BindingStatus.Failure, $"{bindingTypename}: Trying set value for binding id `{bindingId}`, but it is read-only."),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}