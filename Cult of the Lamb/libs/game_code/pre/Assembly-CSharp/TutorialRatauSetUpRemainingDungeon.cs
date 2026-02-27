// Decompiled with JetBrains decompiler
// Type: TutorialRatauSetUpRemainingDungeon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMBiomeGeneration;

#nullable disable
public class TutorialRatauSetUpRemainingDungeon : BaseMonoBehaviour
{
  public void Play()
  {
    BiomeGenerator.Instance.DoFirstArrivalRoutine = true;
    BiomeGenerator.Instance.ShowDisplayName = false;
    PlayerFarming.Instance.health.GodMode = Health.CheatMode.Demigod;
    DataManager.Instance.InTutorial = false;
  }

  private void OnEnable()
  {
    AudioManager.Instance.PlayOneShot("event:/Stings/tarot_room", this.transform.position);
  }
}
