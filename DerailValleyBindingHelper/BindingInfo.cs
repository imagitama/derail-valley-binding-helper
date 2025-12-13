using System;
using UnityEngine;

namespace DerailValleyBindingHelper;

[Serializable]
public class BindingInfo
{
    public BindingInfo()
    {
    }
    public BindingInfo(int actionId)
    {
        ActionId = actionId;
    }
    public BindingInfo(string label, int actionId, KeyCode keyCode)
    {
        Label = label;
        ControllerType = Rewired.ControllerType.Keyboard;
        ControllerId = 0;
        ControllerName = "Keyboard";
        ButtonId = BindingHelper.GetButtonId(keyCode);
        ButtonName = keyCode.ToString();
        ActionId = actionId;
    }
    public string Label;
    public Rewired.ControllerType? ControllerType;
    public string? ControllerName; // not required but helpful
    public int? ControllerId;
    public string? ButtonName; // not required but helpful
    public int? ButtonId;
    public int? ActionId; // optional for non-standard bindings
    public bool Removable = false;
    public bool Clearable = false;
    public bool DisableDefault = false;

    public override bool Equals(object obj)
    {
        if (obj is BindingInfo other)
            return
                ControllerType == other.ControllerType &&
                ControllerName == other.ControllerName &&
                ControllerId == other.ControllerId &&
                ButtonName == other.ButtonName &&
                ButtonId == other.ButtonId &&
                ActionId == other.ActionId;
        return false;
    }

    public BindingInfo Clone()
    {
        return new BindingInfo()
        {
            Label = Label,
            ControllerType = ControllerType,
            ControllerName = ControllerName,
            ControllerId = ControllerId,
            ButtonName = ButtonName,
            ButtonId = ButtonId,
            ActionId = ActionId,
            Removable = Removable,
            Clearable = Clearable,
            DisableDefault = DisableDefault
        };
    }

    public override string ToString()
    {
        return $"Binding(cType={ControllerType},cName={ControllerName},cId={ControllerId},bName={ButtonName},bId={ButtonId},aId={ActionId})";
    }

    public string GetLabel()
    {
        if (ControllerType == null || ButtonId == null)
            return "(none)";
        return $"{ControllerName}.{ButtonName}";
    }
}