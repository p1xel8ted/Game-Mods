// Decompiled with JetBrains decompiler
// Type: Microsoft.Mixer.InteractiveControl
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace Microsoft.Mixer;

[Serializable]
public class InteractiveControl
{
  [CompilerGenerated]
  public string \u003CControlID\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CDisabled\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CHelpText\u003Ek__BackingField;
  [CompilerGenerated]
  public IDictionary<string, object> \u003CMetaProperties\u003Ek__BackingField;
  public string _eTag;
  public string _sceneID;
  public string _kind;
  public InteractiveEventType _type;
  public int participantID;

  public string ControlID
  {
    get => this.\u003CControlID\u003Ek__BackingField;
    set => this.\u003CControlID\u003Ek__BackingField = value;
  }

  public bool Disabled
  {
    get => this.\u003CDisabled\u003Ek__BackingField;
    set => this.\u003CDisabled\u003Ek__BackingField = value;
  }

  public string HelpText
  {
    get => this.\u003CHelpText\u003Ek__BackingField;
    set => this.\u003CHelpText\u003Ek__BackingField = value;
  }

  public IDictionary<string, object> MetaProperties
  {
    get => this.\u003CMetaProperties\u003Ek__BackingField;
    set => this.\u003CMetaProperties\u003Ek__BackingField = value;
  }

  public void SetDisabled(bool disabled)
  {
    this.Disabled = disabled;
    InteractivityManager.SingletonInstance._SendSetButtonControlProperties(this.ControlID, nameof (disabled), disabled, 0.0f, string.Empty, 0U);
  }

  public void SetProperty(InteractiveControlProperty name, bool value)
  {
    this.SetPropertyImpl(InteractivityManager.SingletonInstance._InteractiveControlPropertyToString(name), value);
  }

  public void SetProperty(InteractiveControlProperty name, double value)
  {
    this.SetPropertyImpl(InteractivityManager.SingletonInstance._InteractiveControlPropertyToString(name), value);
  }

  public void SetProperty(InteractiveControlProperty name, string value)
  {
    this.SetPropertyImpl(InteractivityManager.SingletonInstance._InteractiveControlPropertyToString(name), value);
  }

  public void SetProperty(InteractiveControlProperty name, object value)
  {
    this.SetPropertyImpl(InteractivityManager.SingletonInstance._InteractiveControlPropertyToString(name), value);
  }

  public void SetProperty(string name, bool value) => this.SetPropertyImpl(name, value);

  public void SetProperty(string name, double value) => this.SetPropertyImpl(name, value);

  public void SetProperty(string name, string value) => this.SetPropertyImpl(name, value);

  public void SetProperty(string name, object value) => this.SetPropertyImpl(name, value);

  public void SetPropertyImpl(string name, bool value)
  {
    InteractivityManager.SingletonInstance._QueuePropertyUpdate(this._sceneID, this.ControlID, name, value);
  }

  public void SetPropertyImpl(string name, double value)
  {
    InteractivityManager.SingletonInstance._QueuePropertyUpdate(this._sceneID, this.ControlID, name, value);
  }

  public void SetPropertyImpl(string name, string value)
  {
    InteractivityManager.SingletonInstance._QueuePropertyUpdate(this._sceneID, this.ControlID, name, value);
  }

  public void SetPropertyImpl(string name, object value)
  {
    InteractivityManager.SingletonInstance._QueuePropertyUpdate(this._sceneID, this.ControlID, name, value);
  }

  public InteractiveControl(
    string controlID,
    string kind,
    InteractiveEventType type,
    bool disabled,
    string helpText,
    string eTag,
    string sceneID,
    Dictionary<string, object> metaProperties)
  {
    this.ControlID = controlID;
    this._kind = kind;
    this._type = type;
    this.Disabled = disabled;
    this.HelpText = helpText;
    this._eTag = eTag;
    this._sceneID = sceneID;
    this.participantID = -1;
    this.MetaProperties = (IDictionary<string, object>) metaProperties;
  }
}
