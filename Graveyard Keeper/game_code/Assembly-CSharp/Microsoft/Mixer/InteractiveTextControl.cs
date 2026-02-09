// Decompiled with JetBrains decompiler
// Type: Microsoft.Mixer.InteractiveTextControl
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Microsoft.Mixer;

[Serializable]
public class InteractiveTextControl(
  string controlID,
  InteractiveEventType type,
  bool disabled,
  string helpText,
  string eTag,
  string sceneID,
  Dictionary<string, object> metaproperties) : InteractiveControl(controlID, "textbox", type, disabled, helpText, eTag, sceneID, metaproperties)
{
  public IList<InteractiveTextResult> TextResults
  {
    get => InteractivityManager.SingletonInstance._GetText(this.ControlID);
  }
}
