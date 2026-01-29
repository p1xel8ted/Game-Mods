// Decompiled with JetBrains decompiler
// Type: FacePlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FacePlayer : BaseMonoBehaviour
{
  public StateMachine state;
  public GameObject Player;

  public void Start() => this.state = this.GetComponent<StateMachine>();

  public void Update()
  {
    if ((Object) (this.Player = GameObject.FindWithTag("Player")) == (Object) null)
      return;
    this.state.facingAngle = Utils.GetAngle(this.transform.position, this.Player.transform.position);
  }
}
