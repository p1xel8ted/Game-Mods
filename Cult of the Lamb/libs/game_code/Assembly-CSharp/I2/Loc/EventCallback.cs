// Decompiled with JetBrains decompiler
// Type: I2.Loc.EventCallback
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace I2.Loc;

[Serializable]
public class EventCallback
{
  public MonoBehaviour Target;
  public string MethodName = string.Empty;

  public void Execute(UnityEngine.Object Sender = null)
  {
    if (!this.HasCallback() || !Application.isPlaying)
      return;
    this.Target.gameObject.SendMessage(this.MethodName, (object) Sender, SendMessageOptions.DontRequireReceiver);
  }

  public bool HasCallback()
  {
    return (UnityEngine.Object) this.Target != (UnityEngine.Object) null && !string.IsNullOrEmpty(this.MethodName);
  }
}
