// Decompiled with JetBrains decompiler
// Type: PlayMusicOnEnable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
