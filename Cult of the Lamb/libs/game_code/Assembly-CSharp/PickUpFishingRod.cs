// Decompiled with JetBrains decompiler
// Type: PickUpFishingRod
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class PickUpFishingRod : BaseMonoBehaviour
{
  public void start()
  {
    if (!CrownAbilities.CrownAbilityUnlocked(CrownAbilities.TYPE.Abilities_GrappleHook))
      return;
    this.gameObject.SetActive(false);
  }

  public void OnEnable()
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
