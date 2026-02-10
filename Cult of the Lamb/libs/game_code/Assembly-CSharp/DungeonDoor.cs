// Decompiled with JetBrains decompiler
// Type: DungeonDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public bool Activated;
  public StateMachine state;

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (this.Activated || !collision.gameObject.CompareTag("Player"))
      return;
    this.state = collision.gameObject.GetComponent<StateMachine>();
    this.state.CURRENT_STATE = StateMachine.State.InActive;
    this.Activated = true;
  }

  public void CancelCallback()
  {
    this.Activated = false;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.state.transform.position = this.transform.position + Vector3.down * 3f;
  }
}
