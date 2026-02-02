// Decompiled with JetBrains decompiler
// Type: LightningRingMortar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class LightningRingMortar : MortarBomb
{
  public float LightningEnemyDamage = 4f;
  public float LightningStrikeRadius = 2f;
  public float LightningExpansionSpeed = 5f;
  public float MaxRadius = 6.5f;
  public const float LIGHTNING_DAMAGE_PLAYER = 1f;

  public override IEnumerator MoveRock(Vector3 startPos)
  {
    LightningRingMortar lightningRingMortar = this;
    lightningRingMortar.BombVisual.SetActive(true);
    lightningRingMortar.BombShadow.SetActive(true);
    lightningRingMortar.BombVisual.transform.position = startPos;
    Vector2 targetPos = (Vector2) lightningRingMortar.transform.position;
    float t = 0.0f;
    while ((double) t < (double) lightningRingMortar.moveDuration)
    {
      if (!PlayerRelic.TimeFrozen)
      {
        t += Time.deltaTime * lightningRingMortar.SpineTimeScale;
        lightningRingMortar.BombVisual.transform.position = Vector3.Lerp(startPos, (Vector3) targetPos, t / lightningRingMortar.moveDuration);
        lightningRingMortar.BombShadow.transform.position = Vector3.Lerp(startPos, (Vector3) targetPos, t / lightningRingMortar.moveDuration);
        lightningRingMortar.BombShadow.transform.localScale = Vector3.one * lightningRingMortar.circleCollider2D.radius * (float) (1.5 - (double) Mathf.Clamp01(lightningRingMortar.arcCurve.Evaluate(t / lightningRingMortar.moveDuration)) * 0.5);
        lightningRingMortar.BombVisual.transform.position += new Vector3(0.0f, 0.0f, -lightningRingMortar.arcCurve.Evaluate(t / lightningRingMortar.moveDuration) * lightningRingMortar.arcHeight);
      }
      yield return (object) null;
    }
    Explosion.CreateExplosion(lightningRingMortar.transform.position, lightningRingMortar.bombTeam, lightningRingMortar.owner, 1f, 1f, lightningRingMortar.LightningEnemyDamage);
    LightningRingExplosion.CreateExplosion(lightningRingMortar.transform.position, lightningRingMortar.bombTeam, lightningRingMortar.owner, lightningRingMortar.LightningExpansionSpeed, 1f, lightningRingMortar.LightningEnemyDamage, maxRadiusTarget: lightningRingMortar.MaxRadius);
    MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
    if ((Object) lightningRingMortar.SmokeParticles != (Object) null)
    {
      lightningRingMortar.SmokeParticles.transform.parent = lightningRingMortar.transform.parent;
      lightningRingMortar.SmokeParticles.Stop();
    }
    if (lightningRingMortar.destroyOnFinish)
      Object.Destroy((Object) lightningRingMortar.gameObject);
    else if ((Object) lightningRingMortar != (Object) null)
      lightningRingMortar.gameObject.Recycle();
  }
}
