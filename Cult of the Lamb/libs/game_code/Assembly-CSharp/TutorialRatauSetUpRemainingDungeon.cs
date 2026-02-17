// Decompiled with JetBrains decompiler
// Type: TutorialRatauSetUpRemainingDungeon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;

#nullable disable
public class TutorialRatauSetUpRemainingDungeon : BaseMonoBehaviour
{
  public void Play()
  {
    BiomeGenerator.Instance.DoFirstArrivalRoutine = true;
    BiomeGenerator.Instance.ShowDisplayName = false;
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
      PlayerFarming.players[index].health.GodMode = Health.CheatMode.Demigod;
    DataManager.Instance.InTutorial = false;
  }

  public void OnEnable()
  {
    AudioManager.Instance.PlayOneShot("event:/Stings/tarot_room", this.transform.position);
  }
}
