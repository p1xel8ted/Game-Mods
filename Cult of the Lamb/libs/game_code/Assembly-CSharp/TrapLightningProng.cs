// Decompiled with JetBrains decompiler
// Type: TrapLightningProng
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using UnityEngine;

#nullable disable
public class TrapLightningProng : MonoBehaviour
{
  [SerializeField]
  public SpriteRenderer activeSpriteRenderer;
  [SerializeField]
  public SpriteRenderer redCircleSpriteRenderer;
  public GameObject LightningRoot;
  [SerializeField]
  public ParticleSystem chargeSparks;
  [SerializeField]
  public ParticleSystem chargeSphere;
  [SerializeField]
  public ParticleSystem impactParticle;
  [SerializeField]
  public ParticleSystem impactSparks;
  public string ChargeSFX = "event:/dlc/dungeon05/trap/lightning_rod/charge";
  public EventInstance chargeInstanceSFX;

  public void StartChargingEffect()
  {
    this.chargeSparks.Play();
    this.chargeSphere.Play();
    this.SetActive(true);
    if (string.IsNullOrEmpty(this.ChargeSFX))
      return;
    this.chargeInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(this.ChargeSFX, this.transform);
  }

  public void StopChargingEffect()
  {
    this.chargeSparks.Stop();
    this.chargeSphere.Stop();
    this.SetActive(false);
    AudioManager.Instance.StopOneShotInstanceEarly(this.chargeInstanceSFX, STOP_MODE.ALLOWFADEOUT);
  }

  public void PlayImpactEffect()
  {
    this.impactParticle.Play();
    this.impactSparks.Play();
    MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
  }

  public void SetActive(bool state) => this.activeSpriteRenderer.gameObject.SetActive(state);

  public void SetRedCircleActive(bool state)
  {
    this.redCircleSpriteRenderer.gameObject.SetActive(state);
  }
}
