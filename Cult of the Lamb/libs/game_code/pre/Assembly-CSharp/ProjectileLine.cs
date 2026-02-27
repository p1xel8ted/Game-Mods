// Decompiled with JetBrains decompiler
// Type: ProjectileLine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using UnityEngine;

#nullable disable
public class ProjectileLine : BaseMonoBehaviour
{
  [SerializeField]
  private float durationTillMaxLine;
  [SerializeField]
  private Projectile[] projectiles;
  private Projectile projectile;

  public void Init()
  {
    this.projectile = this.GetComponent<Projectile>();
    float num1 = 0.0f;
    float num2 = 360f / (float) this.projectiles.Length;
    float num3 = (float) this.projectiles.Length / 2f;
    Vector3 a = this.transform.position - this.transform.right * num3;
    Vector3 b = this.transform.position + this.transform.right * num3;
    for (int index = 0; index < this.projectiles.Length; ++index)
    {
      float t = (float) index / (float) this.projectiles.Length;
      Vector2 endValue = (Vector2) Vector3.Lerp(a, b, t);
      this.projectiles[index].transform.localPosition = Vector3.zero;
      this.projectiles[index].transform.DOLocalMove((Vector3) endValue, this.durationTillMaxLine);
      this.projectiles[index].health = this.projectile.health;
      this.projectiles[index].team = Health.Team.Team2;
      num1 += num2;
    }
  }

  public void InitDelayed(GameObject target, float shootDelay, float angle)
  {
    this.projectile = this.GetComponent<Projectile>();
    double num1 = 360.0 / (double) this.projectiles.Length;
    double num2 = (double) this.projectiles.Length / 2.0;
    foreach (Component projectile in this.projectiles)
      projectile.gameObject.SetActive(false);
    for (int index = 0; index < this.projectiles.Length; ++index)
    {
      double num3 = (double) index / (double) this.projectiles.Length;
      this.projectiles[index].health = this.projectile.health;
      this.projectiles[index].team = Health.Team.Team2;
    }
    this.StartCoroutine((IEnumerator) this.EnableProjectiles(target, shootDelay, angle));
  }

  private IEnumerator EnableProjectiles(GameObject target, float delay, float angle)
  {
    this.projectile.SpeedMultiplier = 0.0f;
    Projectile[] projectileArray = this.projectiles;
    for (int index = 0; index < projectileArray.Length; ++index)
    {
      projectileArray[index].gameObject.SetActive(true);
      yield return (object) new WaitForSeconds(0.02f);
    }
    projectileArray = (Projectile[]) null;
    yield return (object) new WaitForSeconds(0.25f + delay);
    this.projectile.Angle = angle;
    this.projectile.SpeedMultiplier = 1f;
  }
}
