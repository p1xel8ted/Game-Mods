// Decompiled with JetBrains decompiler
// Type: NextBiome
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
