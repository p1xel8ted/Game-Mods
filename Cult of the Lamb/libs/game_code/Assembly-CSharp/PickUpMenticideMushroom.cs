// Decompiled with JetBrains decompiler
// Type: PickUpMenticideMushroom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
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
