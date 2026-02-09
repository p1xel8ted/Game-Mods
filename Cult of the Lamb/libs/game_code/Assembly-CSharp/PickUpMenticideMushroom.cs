// Decompiled with JetBrains decompiler
// Type: PickUpMenticideMushroom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
