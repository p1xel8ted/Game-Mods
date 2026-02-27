// Decompiled with JetBrains decompiler
// Type: DungeonDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
public class DungeonDoor : BaseMonoBehaviour
{
  public string BiomeName0 = "Game Biome 0";
  [TermsPopup("")]
  public string PlaceName0 = "";
  public string BiomeName = "Game Biome 1";
  [TermsPopup("")]
  public string PlaceName = "";
  public GameObject WorldMap;
  private bool Activated;
  private StateMachine state;

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (this.Activated || !(collision.gameObject.tag == "Player"))
      return;
    this.state = collision.gameObject.GetComponent<StateMachine>();
    this.state.CURRENT_STATE = StateMachine.State.InActive;
    this.Activated = true;
  }

  private void CancelCallback()
  {
    this.Activated = false;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.state.transform.position = this.transform.position + Vector3.down * 3f;
  }
}
