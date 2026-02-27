// Decompiled with JetBrains decompiler
// Type: Interaction_ButtonPrompt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_ButtonPrompt : Interaction
{
  private Indicator IndicatorInstance;
  private GameObject Player;
  public string Label_;
  public UnityEvent callbackRing;

  private void Start()
  {
  }

  public override void GetLabel()
  {
  }

  public override void OnInteract(StateMachine state) => this.callbackRing.Invoke();
}
