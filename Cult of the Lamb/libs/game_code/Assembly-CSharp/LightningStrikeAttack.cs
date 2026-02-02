// Decompiled with JetBrains decompiler
// Type: LightningStrikeAttack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class LightningStrikeAttack : MonoBehaviour, ISpellOwning
{
  public RelicData lightningData;
  public Health parentHealth;
  public Vector3 strikePosition;
  public System.Action cleanupCallback;
  public bool isFinished;
  public bool isTriggered;
  public bool showIndicator;
  public bool doRingLightning;
  public float timeBeforeExplosion = 0.5f;
  public float timeBeforeCleanup = 2f;
  public float progress;
  public float lightningExpansionSpeed = 5f;
  public float lightningDamageEnemy = 4f;
  public float lightningDamagePlayer = 1f;
  public float screenShakeMult = 1f;
  public IndicatorFlash indicator;
  public string customSFX = "";
  public Health Origin;
  public bool playHapticsOnExplosion;

  public void Awake()
  {
    this.lightningData = EquipmentManager.GetRelicData(RelicType.LightningStrike);
    this.indicator = this.GetComponentInChildren<IndicatorFlash>(true);
  }

  public void Update()
  {
    if (!this.isTriggered)
      return;
    this.progress += Time.deltaTime;
    if (!this.isFinished)
    {
      if ((double) this.progress < (double) this.timeBeforeExplosion)
        return;
      this.isFinished = true;
      Explosion.CreateExplosion(this.strikePosition, this.parentHealth.team, this.parentHealth, 1f, shakeMultiplier: this.screenShakeMult, playSFX: string.IsNullOrEmpty(this.customSFX));
      if (this.doRingLightning)
        LightningRingExplosion.CreateExplosion(this.strikePosition, this.parentHealth.team, this.parentHealth, this.lightningExpansionSpeed, this.lightningDamagePlayer, this.lightningDamageEnemy);
      if (this.showIndicator)
        this.indicator.gameObject.SetActive(false);
      if (this.playHapticsOnExplosion)
        MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
      if (string.IsNullOrEmpty(this.customSFX))
        return;
      AudioManager.Instance.PlayOneShot(this.customSFX);
    }
    else
    {
      if ((double) this.progress < (double) this.timeBeforeCleanup)
        return;
      System.Action cleanupCallback = this.cleanupCallback;
      if (cleanupCallback == null)
        return;
      cleanupCallback();
    }
  }

  public void TriggerLightningStrike(
    Health parentHealth,
    Vector3 strikePosition,
    System.Action cleanupCallback,
    bool showIndicator,
    bool doRingLightning = false,
    LightningStrikeAttack.IndicatorSize indicatorSize = LightningStrikeAttack.IndicatorSize.Normal,
    float screenShakeMult = 1f,
    float damagePlayer = 1f,
    float damageEnemy = 4f,
    string customSFX = "",
    bool playHapticsOnExplosion = true)
  {
    this.parentHealth = parentHealth;
    this.strikePosition = strikePosition;
    this.cleanupCallback = cleanupCallback;
    this.showIndicator = showIndicator;
    this.doRingLightning = doRingLightning;
    this.screenShakeMult = screenShakeMult;
    this.lightningDamagePlayer = damagePlayer;
    this.lightningDamageEnemy = damageEnemy;
    this.customSFX = customSFX;
    this.isTriggered = true;
    this.isFinished = false;
    this.progress = 0.0f;
    this.transform.position = strikePosition;
    this.lightningData.VFXData.ImpactVFXObject.SpawnVFX(this.transform, true);
    this.playHapticsOnExplosion = playHapticsOnExplosion;
    this.SetIndicatorSize(indicatorSize);
    this.indicator.gameObject.SetActive(showIndicator);
  }

  public void SetIndicatorSize(LightningStrikeAttack.IndicatorSize size)
  {
    if (size != LightningStrikeAttack.IndicatorSize.Normal)
    {
      if (size != LightningStrikeAttack.IndicatorSize.Large)
        return;
      this.indicator.transform.localScale = Vector3.one;
    }
    else
      this.indicator.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
  }

  public void OnDisable() => this.SetIndicatorSize(LightningStrikeAttack.IndicatorSize.Normal);

  public GameObject GetOwner()
  {
    return !((UnityEngine.Object) this.Origin != (UnityEngine.Object) null) ? (GameObject) null : this.Origin.gameObject;
  }

  public void SetOwner(GameObject owner) => this.Origin = owner.GetComponent<Health>();

  public enum IndicatorSize
  {
    Normal,
    Large,
  }
}
