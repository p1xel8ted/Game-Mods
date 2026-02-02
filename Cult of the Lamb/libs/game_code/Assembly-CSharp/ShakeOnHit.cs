// Decompiled with JetBrains decompiler
// Type: ShakeOnHit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using UnityEngine;

#nullable disable
public class ShakeOnHit : BaseMonoBehaviour
{
  public Health health;
  public Quaternion StartRotation;
  public Bouncer bouncer;

  public void Start()
  {
    this.bouncer = this.transform.GetComponent<Bouncer>();
    this.StartRotation = this.transform.rotation;
    if (!(bool) (Object) this.health)
      return;
    this.health.OnHit += new Health.HitAction(this.OnHit);
  }

  public void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes type,
    bool hit)
  {
    this.Shake();
    if (!(bool) (Object) this.bouncer)
      return;
    UnitObject component = Attacker.GetComponent<UnitObject>();
    if (!(bool) (Object) component)
      return;
    this.bouncer.bounceUnit(component, component.transform.position - this.transform.position);
  }

  public void Shake()
  {
    this.transform.DOKill();
    this.transform.rotation = this.StartRotation;
    this.transform.DOPunchRotation(new Vector3(0.0f, 0.0f, 10f), 1f);
  }
}
