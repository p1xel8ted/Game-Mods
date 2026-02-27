// Decompiled with JetBrains decompiler
// Type: PlayMusicOnEnable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using WebSocketSharp;

#nullable disable
public class PlayMusicOnEnable : BaseMonoBehaviour
{
  [EventRef]
  public string musicPath;
  public string parameter;
  public int index;

  private void OnEnable()
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
