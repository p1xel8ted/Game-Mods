// Decompiled with JetBrains decompiler
// Type: PickUpFishingRod
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class PickUpFishingRod : BaseMonoBehaviour
{
  private void start()
  {
    if (!CrownAbilities.CrownAbilityUnlocked(CrownAbilities.TYPE.Abilities_GrappleHook))
      return;
    this.gameObject.SetActive(false);
  }

  private void OnEnable()
  {
    if (!CrownAbilities.CrownAbilityUnlocked(CrownAbilities.TYPE.Abilities_GrappleHook))
      return;
    this.gameObject.SetActive(false);
  }

  public void PickMeUp()
  {
    if (CrownAbilities.CrownAbilityUnlocked(CrownAbilities.TYPE.Abilities_GrappleHook))
      return;
    CrownAbilities.UnlockAbility(CrownAbilities.TYPE.Abilities_GrappleHook);
    UIAbilityUnlock.Play(UIAbilityUnlock.Ability.FishingRod);
    this.gameObject.SetActive(false);
  }
}
