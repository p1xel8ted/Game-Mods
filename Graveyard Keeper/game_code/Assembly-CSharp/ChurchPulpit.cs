// Decompiled with JetBrains decompiler
// Type: ChurchPulpit
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ChurchPulpit : MonoBehaviour
{
  public SpriteRenderer buff_spr;
  public Animator animator;

  public void DoBuffSuccessAnimation()
  {
    MainGame.me.player.components.character.player.CreatePrayBuffFlyingObject((Vector2) this.buff_spr.transform.position);
    PrayLogics.DropPrayItems();
  }

  public void OnMiddleAnimation() => GUIElements.me.pray_craft.OnMiddlePrayBuffAnimation();

  public void OnPrayAnimationDone()
  {
    MainGame.me.player.components.character.SetAnimationState(CharAnimState.Idle);
    GUIElements.me.pray_craft.OnFinishedPrayBuffAnimation();
  }
}
