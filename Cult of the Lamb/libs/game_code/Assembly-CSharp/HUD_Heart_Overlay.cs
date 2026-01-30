// Decompiled with JetBrains decompiler
// Type: HUD_Heart_Overlay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class HUD_Heart_Overlay : BaseMonoBehaviour
{
  public SkeletonGraphic healthOverlay;
  public float playerHealth;
  public bool overlayAdded;
  public bool disabling;

  public void Start() => this.healthOverlay.enabled = false;

  public void Update()
  {
    this.playerHealth = 100f;
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      PlayerFarming player = PlayerFarming.players[index];
      float num = player.health.HP + player.health.BlueHearts;
      if ((double) num < (double) this.playerHealth)
        this.playerHealth = num;
    }
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

  public IEnumerator disable()
  {
    this.disabling = true;
    float duration = this.healthOverlay.Skeleton.Data.FindAnimation("fastOut").Duration;
    this.healthOverlay.AnimationState.SetAnimation(0, "fastOut", false);
    yield return (object) new WaitForSeconds(duration);
    this.healthOverlay.enabled = false;
  }
}
