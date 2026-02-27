// Decompiled with JetBrains decompiler
// Type: TriggerSoundOnParticleCollision
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
