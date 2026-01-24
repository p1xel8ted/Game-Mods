// Decompiled with JetBrains decompiler
// Type: HUD_ProblemUnlock
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using TMPro;
using UnityEngine;

#nullable disable
public class HUD_ProblemUnlock : BaseMonoBehaviour
{
  public SkeletonGraphic Avatar;
  public TextMeshProUGUI TitleText;
  public TextMeshProUGUI SubtitleText;
  public HUD_ProblemUnlockItem[] Items;

  public void Init(
    UnlockManager.UnlockType type,
    UnlockManager.UnlockNotificationData[] unlockTypes)
  {
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.InActive;
    this.Avatar.AnimationState.SetAnimation(0, UnlockManager.GetUnlockAnimationName(type), true);
    this.TitleText.text = UnlockManager.GetUnlockTitle(type);
    this.SubtitleText.text = UnlockManager.GetUnlockSubtitle(type);
    int index;
    for (index = 0; index < unlockTypes.Length; ++index)
    {
      HUD_ProblemUnlockItem problemUnlockItem = this.Items[index];
      problemUnlockItem.gameObject.SetActive(true);
      problemUnlockItem.Init(unlockTypes[index]);
    }
    for (; index < this.Items.Length; ++index)
      this.Items[index].gameObject.SetActive(false);
  }

  public void Update()
  {
    if (!InputManager.UI.GetAcceptButtonDown())
      return;
    PlayerFarming.SetStateForAllPlayers();
    Object.Destroy((Object) this.gameObject);
  }
}
