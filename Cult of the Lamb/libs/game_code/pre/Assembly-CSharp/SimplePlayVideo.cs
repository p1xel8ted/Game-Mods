// Decompiled with JetBrains decompiler
// Type: SimplePlayVideo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMTools;
using UnityEngine;

#nullable disable
public class SimplePlayVideo : BaseMonoBehaviour
{
  public string VideoFile = "Intro";

  public void Play()
  {
    Time.timeScale = 0.0f;
    UnityEngine.Object.FindObjectOfType<IntroManager>().DisableBoth();
    MMVideoPlayer.Play(this.VideoFile, new System.Action(this.Continue), MMVideoPlayer.Options.DISABLE, MMVideoPlayer.Options.DISABLE);
  }

  public void Continue()
  {
    Time.timeScale = 1f;
    UnityEngine.Object.FindObjectOfType<IntroManager>().ToggleGameScene();
  }
}
