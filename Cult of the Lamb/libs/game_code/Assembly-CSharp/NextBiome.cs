// Decompiled with JetBrains decompiler
// Type: NextBiome
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using UnityEngine;

#nullable disable
public class NextBiome : BaseMonoBehaviour
{
  public string SceneName = "";
  [TermsPopup("")]
  public string PlaceName = "";

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (!collision.gameObject.CompareTag("Player"))
      return;
    collision.gameObject.GetComponent<StateMachine>().CURRENT_STATE = StateMachine.State.InActive;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, this.SceneName, 1f, this.PlaceName, (System.Action) null);
  }
}
