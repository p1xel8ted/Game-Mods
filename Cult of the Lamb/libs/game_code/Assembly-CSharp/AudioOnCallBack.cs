// Decompiled with JetBrains decompiler
// Type: AudioOnCallBack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class AudioOnCallBack : BaseMonoBehaviour
{
  public void PlayAmbient() => AmbientMusicController.PlayAmbient(0.0f);

  public void PlayCombat()
  {
    AmbientMusicController.PlayCombat();
    AudioManager.Instance.SetMusicCombatState();
  }

  public void StopCombat()
  {
    AmbientMusicController.StopCombat();
    AudioManager.Instance.SetMusicCombatState(false);
  }
}
