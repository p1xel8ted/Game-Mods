// Decompiled with JetBrains decompiler
// Type: UIFMODLoopSound
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using Lamb.UI;
using UnityEngine;

#nullable disable
public class UIFMODLoopSound : BaseMonoBehaviour
{
  [EventRef]
  [SerializeField]
  public string soundEventPath = string.Empty;
  public EventInstance _loopEventInstance;

  public void OnEnable()
  {
    this._loopEventInstance = UIManager.CreateLoop(this.soundEventPath);
    int num = (int) this._loopEventInstance.start();
    Debug.Log((object) ("Started loop sound: " + this.soundEventPath));
  }

  public void OnDisable()
  {
    int num1 = (int) this._loopEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    int num2 = (int) this._loopEventInstance.release();
    Debug.Log((object) ("Stopped loop sound: " + this.soundEventPath));
  }

  public void OnDestroy()
  {
    int num1 = (int) this._loopEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    int num2 = (int) this._loopEventInstance.release();
    Debug.Log((object) ("Destroyed loop sound: " + this.soundEventPath));
  }
}
