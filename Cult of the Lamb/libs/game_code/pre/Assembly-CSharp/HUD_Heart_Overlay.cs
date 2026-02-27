// Decompiled with JetBrains decompiler
// Type: HUD_Heart_Overlay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class HUD_Heart_Overlay : BaseMonoBehaviour
{
  public SkeletonGraphic healthOverlay;
  public float playerHealth;
  private bool overlayAdded;
  private bool disabling;

  private void Start() => this.healthOverlay.enabled = false;

  private void Update()
  {
    this.playerHealth = DataManager.Instance.PLAYER_HEALTH + DataManager.Instance.PLAYER_BLUE_HEARTS;
    if ((double) this.playerHealth <= 2.0)
    {
      if (this.healthOverlay.enabled)
        return;
      this.disabling = false;
      this.healthOverlay.enabled = true;
      this.overlayAdded = true;
      this.healthOverlay.AnimationState.SetAnimation(0, "fastIn", false);
      this.healthOverlay.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    }
    else
    {
      if (!this.healthOverlay.enabled || this.disabling)
        return;
      this.StopAllCoroutines();
      this.StartCoroutine((IEnumerator) this.disable());
    }
  }

  private IEnumerator disable()
  {
    this.disabling = true;
    float duration = this.healthOverlay.Skeleton.Data.FindAnimation("fastOut").Duration;
    this.healthOverlay.AnimationState.SetAnimation(0, "fastOut", false);
    yield return (object) new WaitForSeconds(duration);
    this.healthOverlay.enabled = false;
  }
}
