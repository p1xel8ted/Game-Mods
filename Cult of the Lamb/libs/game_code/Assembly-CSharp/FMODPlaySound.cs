// Decompiled with JetBrains decompiler
// Type: FMODPlaySound
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class FMODPlaySound : MonoBehaviour
{
  [SerializeField]
  public string soundPath;
  [SerializeField]
  public float delay;
  [Space]
  [SerializeField]
  public bool playOnEnable = true;

  public void OnEnable()
  {
    if (!this.playOnEnable)
      return;
    this.PlaySound();
  }

  public void PlaySound() => this.StartCoroutine((IEnumerator) this.PlaySoundIE());

  public IEnumerator PlaySoundIE()
  {
    if ((double) this.delay != 0.0)
      yield return (object) new WaitForSeconds(this.delay);
    AudioManager.Instance.PlayOneShot(this.soundPath);
  }
}
