// Decompiled with JetBrains decompiler
// Type: GoatShadowController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;

#nullable disable
public class GoatShadowController : MonoBehaviour
{
  public SkeletonAnimation GoatSpine;
  public SkeletonAnimation LambSpine;
  public PlayerFarming Player;

  public void OnEnable()
  {
    Interaction_CorruptedMonolith.OnCorruptedComplete += new System.Action(this.OnCorruptedComplete);
  }

  public void OnDisable()
  {
    Interaction_CorruptedMonolith.OnCorruptedComplete -= new System.Action(this.OnCorruptedComplete);
  }

  public void OnCorruptedComplete()
  {
  }

  public void SetPlayer(PlayerFarming Player) => this.Player = Player;

  public void Update()
  {
    if ((UnityEngine.Object) this.Player != (UnityEngine.Object) null)
    {
      this.GoatSpine.transform.position = this.Player.transform.position;
      if (this.GoatSpine.AnimationName != this.Player.Spine.AnimationName)
        this.GoatSpine.AnimationState.SetAnimation(0, this.Player.Spine.AnimationName, this.Player.Spine.AnimationState.GetCurrent(0).Loop);
      this.GoatSpine.skeleton.ScaleX = -this.Player.Spine.skeleton.ScaleX;
    }
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null))
      return;
    if (CoopManager.CoopActive && PlayerFarming.players.Count > 1)
    {
      if (!this.LambSpine.gameObject.activeSelf)
        this.LambSpine.gameObject.SetActive(true);
      PlayerFarming player = PlayerFarming.players[1];
      this.LambSpine.transform.position = player.transform.position;
      if (this.LambSpine.AnimationName != player.Spine.AnimationName)
        this.LambSpine.AnimationState.SetAnimation(0, player.Spine.AnimationName, player.Spine.AnimationState.GetCurrent(0).Loop);
      this.LambSpine.skeleton.ScaleX = -player.Spine.skeleton.ScaleX;
    }
    else
    {
      if (!this.LambSpine.gameObject.activeSelf)
        return;
      this.LambSpine.gameObject.SetActive(false);
    }
  }
}
