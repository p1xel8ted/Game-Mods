// Decompiled with JetBrains decompiler
// Type: I2.Loc.EventCallback
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
