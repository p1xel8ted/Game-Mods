// Decompiled with JetBrains decompiler
// Type: Interaction_ButtonPrompt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_ButtonPrompt : Interaction
{
  public Indicator IndicatorInstance;
  public GameObject Player;
  public string Label_;
  public UnityEvent callbackRing;

  public void Start()
  {
  }

  public override void GetLabel()
  {
  }

  public override void OnInteract(StateMachine state)
  {
    this.state = state;
    this.callbackRing.Invoke();
  }
}
