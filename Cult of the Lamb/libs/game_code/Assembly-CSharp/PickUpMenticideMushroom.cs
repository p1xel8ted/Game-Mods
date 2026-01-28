// Decompiled with JetBrains decompiler
// Type: PickUpMenticideMushroom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class PickUpMenticideMushroom : PickUp
{
  public override void PickMeUp()
  {
    base.PickMeUp();
    if (DataManager.Instance.CollectedMenticide)
      return;
    DataManager.Instance.CollectedMenticide = true;
    UIAbilityUnlock.Play(UIAbilityUnlock.Ability.MenticideMushroom);
  }
}
