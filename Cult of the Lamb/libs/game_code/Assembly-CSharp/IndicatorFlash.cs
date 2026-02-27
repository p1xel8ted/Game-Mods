// Decompiled with JetBrains decompiler
// Type: IndicatorFlash
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class IndicatorFlash : MonoBehaviour
{
  public SpriteRenderer indicatorIcon;
  public Color indicatorColor = Color.white;
  public float indicatorFlashTimer;
  public System.Action cleanupCallback;
  public float progress;
  public float timeBeforeCleanup;

  public void Awake() => this.indicatorIcon = this.GetComponent<SpriteRenderer>();

  public void OnEnable() => this.progress = 0.0f;

  public void Update()
  {
    if ((double) Time.timeScale == 0.0)
      return;
    if ((double) (this.indicatorFlashTimer += Time.deltaTime) >= 0.11999999731779099 && BiomeConstants.Instance.IsFlashLightsActive)
    {
      this.indicatorColor = this.indicatorColor == Color.white ? Color.red : Color.white;
      this.indicatorIcon.material.SetColor("_Color", this.indicatorColor);
      this.indicatorFlashTimer = 0.0f;
    }
    if (this.cleanupCallback == null)
      return;
    this.progress += Time.deltaTime;
    if ((double) this.progress < (double) this.timeBeforeCleanup)
      return;
    System.Action cleanupCallback = this.cleanupCallback;
    if (cleanupCallback == null)
      return;
    cleanupCallback();
  }

  public void SetCleanupCallback(System.Action callback, float timeBeforeCleanup)
  {
    this.cleanupCallback = callback;
    this.timeBeforeCleanup = timeBeforeCleanup;
  }

  public void SetIndicatorSprite(Sprite sprite) => this.indicatorIcon.sprite = sprite;
}
