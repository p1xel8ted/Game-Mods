// Decompiled with JetBrains decompiler
// Type: UIInputOnGUI
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (UIInput))]
public class UIInputOnGUI : MonoBehaviour
{
  [NonSerialized]
  public UIInput mInput;

  public void Awake() => this.mInput = this.GetComponent<UIInput>();

  public void OnGUI()
  {
    if (Event.current.rawType != UnityEngine.EventType.KeyDown)
      return;
    this.mInput.ProcessEvent(Event.current);
  }
}
