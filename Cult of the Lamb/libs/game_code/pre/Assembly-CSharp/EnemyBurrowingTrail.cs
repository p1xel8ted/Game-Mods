// Decompiled with JetBrains decompiler
// Type: EnemyBurrowingTrail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class EnemyBurrowingTrail : BaseMonoBehaviour
{
  [SerializeField]
  private float damageColliderDelay = 0.3f;
  [SerializeField]
  private Collider2D damageCollider;

  private void OnEnable()
  {
    this.damageCollider.enabled = false;
    this.StartCoroutine((IEnumerator) this.EnableColliderIE());
  }

  private IEnumerator EnableColliderIE()
  {
    yield return (object) new WaitForSeconds(this.damageColliderDelay);
    this.damageCollider.enabled = true;
    yield return (object) new WaitForSeconds(0.5f);
    this.damageCollider.enabled = false;
  }
}
