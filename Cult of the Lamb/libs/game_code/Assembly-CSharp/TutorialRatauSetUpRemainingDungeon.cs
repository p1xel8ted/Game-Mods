// Decompiled with JetBrains decompiler
// Type: TutorialRatauSetUpRemainingDungeon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
