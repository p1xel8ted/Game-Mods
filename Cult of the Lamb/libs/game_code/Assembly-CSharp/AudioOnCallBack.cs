// Decompiled with JetBrains decompiler
// Type: AudioOnCallBack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
