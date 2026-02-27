// Decompiled with JetBrains decompiler
// Type: NextBiome
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using UnityEngine;

#nullable disable
public class NextBiome : BaseMonoBehaviour
{
  public string SceneName = "";
  [TermsPopup("")]
  public string PlaceName = "";

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (!collision.gameObject.CompareTag("Player"))
      return;
    collision.gameObject.GetComponent<StateMachine>().CURRENT_STATE = StateMachine.State.InActive;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, this.SceneName, 1f, this.PlaceName, (System.Action) null);
  }
}
