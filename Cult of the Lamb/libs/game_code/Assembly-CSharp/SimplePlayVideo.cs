// Decompiled with JetBrains decompiler
// Type: SimplePlayVideo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
