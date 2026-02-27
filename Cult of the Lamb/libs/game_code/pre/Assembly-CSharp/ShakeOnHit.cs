// Decompiled with JetBrains decompiler
// Type: ShakeOnHit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using UnityEngine;

#nullable disable
public class ShakeOnHit : BaseMonoBehaviour
{
  public Health health;
  private Quaternion StartRotation;

  private void Start()
  {
    this.StartRotation = this.transform.rotation;
    if (!(bool) (Object) this.health)
      return;
    this.health.OnHit += new Health.HitAction(this.OnHit);
  }

  private void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes type,
    bool hit)
  {
    this.transform.DOKill();
    this.transform.rotation = this.StartRotation;
    this.transform.DOPunchRotation(new Vector3(0.0f, 0.0f, 10f), 1f);
  }
}
