// Decompiled with JetBrains decompiler
// Type: UIKeyBinding
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Interaction/Key Binding")]
public class UIKeyBinding : MonoBehaviour
{
  public static List<UIKeyBinding> mList = new List<UIKeyBinding>();
  public KeyCode keyCode;
  public UIKeyBinding.Modifier modifier;
  public UIKeyBinding.Action action;
  [NonSerialized]
  public bool mIgnoreUp;
  [NonSerialized]
  public bool mIsInput;
  [NonSerialized]
  public bool mPress;

  public string captionText
  {
    get
    {
      string caption = NGUITools.KeyToCaption(this.keyCode);
      if (this.modifier == UIKeyBinding.Modifier.Alt)
        return "Alt+" + caption;
      if (this.modifier == UIKeyBinding.Modifier.Control)
        return "Control+" + caption;
      return this.modifier == UIKeyBinding.Modifier.Shift ? "Shift+" + caption : caption;
    }
  }

  public static bool IsBound(KeyCode key)
  {
    int index = 0;
    for (int count = UIKeyBinding.mList.Count; index < count; ++index)
    {
      UIKeyBinding m = UIKeyBinding.mList[index];
      if ((UnityEngine.Object) m != (UnityEngine.Object) null && m.keyCode == key)
        return true;
    }
    return false;
  }

  public virtual void OnEnable() => UIKeyBinding.mList.Add(this);

  public virtual void OnDisable() => UIKeyBinding.mList.Remove(this);

  public virtual void Start()
  {
    UIInput component = this.GetComponent<UIInput>();
    this.mIsInput = (UnityEngine.Object) component != (UnityEngine.Object) null;
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    EventDelegate.Add(component.onSubmit, new EventDelegate.Callback(this.OnSubmit));
  }

  public virtual void OnSubmit()
  {
    if (UICamera.currentKey != this.keyCode || !this.IsModifierActive())
      return;
    this.mIgnoreUp = true;
  }

  public virtual bool IsModifierActive() => UIKeyBinding.IsModifierActive(this.modifier);

  public static bool IsModifierActive(UIKeyBinding.Modifier modifier)
  {
    switch (modifier)
    {
      case UIKeyBinding.Modifier.Any:
        return true;
      case UIKeyBinding.Modifier.Shift:
        if (UICamera.GetKey(KeyCode.LeftShift) || UICamera.GetKey(KeyCode.RightShift))
          return true;
        break;
      case UIKeyBinding.Modifier.Control:
        if (UICamera.GetKey(KeyCode.LeftControl) || UICamera.GetKey(KeyCode.RightControl))
          return true;
        break;
      case UIKeyBinding.Modifier.Alt:
        if (UICamera.GetKey(KeyCode.LeftAlt) || UICamera.GetKey(KeyCode.RightAlt))
          return true;
        break;
      case UIKeyBinding.Modifier.None:
        return !UICamera.GetKey(KeyCode.LeftAlt) && !UICamera.GetKey(KeyCode.RightAlt) && !UICamera.GetKey(KeyCode.LeftControl) && !UICamera.GetKey(KeyCode.RightControl) && !UICamera.GetKey(KeyCode.LeftShift) && !UICamera.GetKey(KeyCode.RightShift);
    }
    return false;
  }

  public virtual void Update()
  {
    if (UICamera.inputHasFocus || this.keyCode == KeyCode.None || !this.IsModifierActive())
      return;
    bool flag1 = UICamera.GetKeyDown(this.keyCode);
    bool flag2 = UICamera.GetKeyUp(this.keyCode);
    if (flag1)
      this.mPress = true;
    if (this.action == UIKeyBinding.Action.PressAndClick || this.action == UIKeyBinding.Action.All)
    {
      if (flag1)
      {
        UICamera.currentTouchID = -1;
        UICamera.currentKey = this.keyCode;
        this.OnBindingPress(true);
      }
      if (this.mPress & flag2)
      {
        UICamera.currentTouchID = -1;
        UICamera.currentKey = this.keyCode;
        this.OnBindingPress(false);
        this.OnBindingClick();
      }
    }
    if ((this.action == UIKeyBinding.Action.Select || this.action == UIKeyBinding.Action.All) && flag2)
    {
      if (this.mIsInput)
      {
        if (!this.mIgnoreUp && !UICamera.inputHasFocus && this.mPress)
          UICamera.selectedObject = this.gameObject;
        this.mIgnoreUp = false;
      }
      else if (this.mPress)
        UICamera.hoveredObject = this.gameObject;
    }
    if (!flag2)
      return;
    this.mPress = false;
  }

  public virtual void OnBindingPress(bool pressed)
  {
    UICamera.Notify(this.gameObject, "OnPress", (object) pressed);
  }

  public virtual void OnBindingClick()
  {
    UICamera.Notify(this.gameObject, "OnClick", (object) null);
  }

  public override string ToString() => UIKeyBinding.GetString(this.keyCode, this.modifier);

  public static string GetString(KeyCode keyCode, UIKeyBinding.Modifier modifier)
  {
    return modifier == UIKeyBinding.Modifier.None ? keyCode.ToString() : $"{modifier.ToString()}+{keyCode.ToString()}";
  }

  public static bool GetKeyCode(string text, out KeyCode key, out UIKeyBinding.Modifier modifier)
  {
    key = KeyCode.None;
    modifier = UIKeyBinding.Modifier.None;
    if (string.IsNullOrEmpty(text))
      return false;
    if (text.Contains("+"))
    {
      string[] strArray = text.Split('+');
      try
      {
        modifier = (UIKeyBinding.Modifier) Enum.Parse(typeof (UIKeyBinding.Modifier), strArray[0]);
        key = (KeyCode) Enum.Parse(typeof (KeyCode), strArray[1]);
      }
      catch (Exception ex)
      {
        return false;
      }
    }
    else
    {
      modifier = UIKeyBinding.Modifier.None;
      try
      {
        key = (KeyCode) Enum.Parse(typeof (KeyCode), text);
      }
      catch (Exception ex)
      {
        return false;
      }
    }
    return true;
  }

  public static UIKeyBinding.Modifier GetActiveModifier()
  {
    UIKeyBinding.Modifier activeModifier = UIKeyBinding.Modifier.None;
    if (UICamera.GetKey(KeyCode.LeftAlt) || UICamera.GetKey(KeyCode.RightAlt))
      activeModifier = UIKeyBinding.Modifier.Alt;
    else if (UICamera.GetKey(KeyCode.LeftShift) || UICamera.GetKey(KeyCode.RightShift))
      activeModifier = UIKeyBinding.Modifier.Shift;
    else if (UICamera.GetKey(KeyCode.LeftControl) || UICamera.GetKey(KeyCode.RightControl))
      activeModifier = UIKeyBinding.Modifier.Control;
    return activeModifier;
  }

  public enum Action
  {
    PressAndClick,
    Select,
    All,
  }

  public enum Modifier
  {
    Any,
    Shift,
    Control,
    Alt,
    None,
  }
}
