// Decompiled with JetBrains decompiler
// Type: Microsoft.Mixer.InteractiveLabelControl
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace Microsoft.Mixer;

[Serializable]
public class InteractiveLabelControl : InteractiveControl
{
  [CompilerGenerated]
  public string \u003CText\u003Ek__BackingField;

  public string Text
  {
    get => this.\u003CText\u003Ek__BackingField;
    set => this.\u003CText\u003Ek__BackingField = value;
  }

  public void SetText(string text)
  {
    InteractivityManager singletonInstance = InteractivityManager.SingletonInstance;
    singletonInstance._QueuePropertyUpdate(this._sceneID, this.ControlID, singletonInstance._InteractiveControlPropertyToString(InteractiveControlProperty.Text), text);
  }

  public InteractiveLabelControl(string controlID, string text, string sceneID)
    : base(controlID, "label", InteractiveEventType.Unknown, false, "", "", sceneID, new Dictionary<string, object>())
  {
    this.Text = text;
  }
}
