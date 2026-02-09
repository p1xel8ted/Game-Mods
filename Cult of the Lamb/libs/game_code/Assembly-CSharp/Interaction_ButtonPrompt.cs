// Decompiled with JetBrains decompiler
// Type: Interaction_ButtonPrompt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
