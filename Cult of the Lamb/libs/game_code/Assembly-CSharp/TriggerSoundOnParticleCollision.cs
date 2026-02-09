// Decompiled with JetBrains decompiler
// Type: TriggerSoundOnParticleCollision
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TriggerSoundOnParticleCollision : MonoBehaviour
{
  public string sfxEvent = "";

  public void OnParticleCollision(GameObject other)
  {
    if (string.IsNullOrEmpty(this.sfxEvent))
      return;
    AudioManager.Instance.PlayOneShot(this.sfxEvent, this.transform.position);
  }
}
