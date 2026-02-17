// Decompiled with JetBrains decompiler
// Type: SimpleFMODAudioLoop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using UnityEngine;

#nullable disable
public class SimpleFMODAudioLoop : MonoBehaviour
{
  public string audioLoopEvent = "";
  public bool isLocational;
  public STOP_MODE stopMode;
  public EventInstance audioLoop;

  public void OnEnable()
  {
    if (this.isLocational)
      this.audioLoop = AudioManager.Instance.CreateLoop(this.audioLoopEvent, this.gameObject, true);
    else
      this.audioLoop = AudioManager.Instance.CreateLoop(this.audioLoopEvent, true);
  }

  public void OnDisable() => AudioManager.Instance.StopLoop(this.audioLoop, this.stopMode);
}
