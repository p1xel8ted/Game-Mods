// Decompiled with JetBrains decompiler
// Type: PickUpMenticideMushroom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
