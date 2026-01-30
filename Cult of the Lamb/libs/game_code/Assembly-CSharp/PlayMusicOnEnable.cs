// Decompiled with JetBrains decompiler
// Type: PlayMusicOnEnable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
