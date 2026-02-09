// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.AudioTransformTracker
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace DarkTonic.MasterAudio;

public class AudioTransformTracker : MonoBehaviour
{
  public int _frames;
  public Transform _trans;

  public Transform Trans
  {
    get
    {
      if ((Object) this._trans == (Object) null)
        this._trans = this.transform;
      return this._trans;
    }
  }

  public void Update() => ++this._frames;
}
