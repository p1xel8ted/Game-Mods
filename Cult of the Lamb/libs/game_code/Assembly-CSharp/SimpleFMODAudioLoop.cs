// Decompiled with JetBrains decompiler
// Type: SimpleFMODAudioLoop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
