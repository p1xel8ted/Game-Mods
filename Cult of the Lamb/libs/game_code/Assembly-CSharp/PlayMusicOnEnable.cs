// Decompiled with JetBrains decompiler
// Type: PlayMusicOnEnable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMODUnity;
using WebSocketSharp;

#nullable disable
public class PlayMusicOnEnable : BaseMonoBehaviour
{
  [EventRef]
  public string musicPath;
  public string parameter;
  public int index;

  public void OnEnable()
  {
    if (!this.musicPath.IsNullOrEmpty())
      AudioManager.Instance.PlayMusic(this.musicPath);
    if (this.parameter.IsNullOrEmpty())
      return;
    AudioManager.Instance.SetMusicRoomID(this.index, this.parameter);
  }

  public void SetID()
  {
    if (this.parameter.IsNullOrEmpty())
      return;
    AudioManager.Instance.SetMusicRoomID(this.index, this.parameter);
  }
}
