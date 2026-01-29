// Decompiled with JetBrains decompiler
// Type: SimpleParticleVFX
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[ExecuteAlways]
public class SimpleParticleVFX : BaseMonoBehaviour
{
  public bool setRotation = true;
  public float Rotation;
  public List<ParticleSystem> particlesToPlay = new List<ParticleSystem>();
  public bool useCustomEndTime;
  public float customEndTime;

  public void Play()
  {
    this.StopAllCoroutines();
    float num = 0.0f;
    this.gameObject.SetActive(true);
    if (this.useCustomEndTime)
      this.StartCoroutine((IEnumerator) this.customEndTimer());
    if (this.setRotation)
      this.Rotation = num + (float) Random.Range(-10, 10);
    foreach (ParticleSystem particleSystem in this.particlesToPlay)
      particleSystem.Play();
  }

  public void Play(Vector3 Position, float Angle = 0.0f)
  {
    this.StopAllCoroutines();
    this.transform.position = Position;
    this.gameObject.SetActive(true);
    if (this.useCustomEndTime)
      this.StartCoroutine((IEnumerator) this.customEndTimer());
    if (this.setRotation)
      this.Rotation = Angle + (float) Random.Range(-10, 10);
    foreach (ParticleSystem particleSystem in this.particlesToPlay)
      particleSystem.Play();
  }

  public IEnumerator customEndTimer()
  {
    SimpleParticleVFX simpleParticleVfx = this;
    yield return (object) new WaitForSeconds(simpleParticleVfx.customEndTime);
    if (Application.isPlaying)
      simpleParticleVfx.gameObject.Recycle();
    else
      simpleParticleVfx.gameObject.SetActive(false);
  }

  public void PlayAnimation() => this.Play(this.transform.position, (float) Random.Range(0, 360));

  public void Update()
  {
    if (!this.setRotation)
      return;
    this.transform.eulerAngles = new Vector3(0.0f, 0.0f, this.Rotation);
  }
}
