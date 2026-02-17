// Decompiled with JetBrains decompiler
// Type: ShakeOnHit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
