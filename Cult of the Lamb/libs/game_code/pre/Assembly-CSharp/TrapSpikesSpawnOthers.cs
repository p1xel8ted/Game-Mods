// Decompiled with JetBrains decompiler
// Type: TrapSpikesSpawnOthers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TrapSpikesSpawnOthers : BaseMonoBehaviour
{
  [SerializeField]
  private SpriteRenderer spriteRenderer;
  [SerializeField]
  private SkeletonAnimation spine;
  private BoxCollider2D boxCollider2D;
  private List<Collider2D> colliders;
  private ContactFilter2D contactFilter2D;
  private Vector2 ScaleX = Vector2.zero;
  private Vector2 ScaleY = Vector2.zero;
  private Health EnemyHealth;

  public SkeletonAnimation Spine => this.spine;

  public Color OverrideColor { get; set; } = Color.white;

  private void Start()
  {
    this.boxCollider2D = this.GetComponent<BoxCollider2D>();
    this.contactFilter2D = new ContactFilter2D();
    this.contactFilter2D.NoFilter();
    this.StartCoroutine((IEnumerator) this.DoAttack());
    this.StartCoroutine((IEnumerator) this.DoScale());
  }

  private IEnumerator DoScale()
  {
    TrapSpikesSpawnOthers spikesSpawnOthers = this;
    float Timer = 0.0f;
    while ((double) (Timer += Time.deltaTime) < 5.0)
    {
      spikesSpawnOthers.ScaleX.y += (float) ((1.0 - (double) spikesSpawnOthers.ScaleX.x) * 0.30000001192092896);
      spikesSpawnOthers.ScaleX.x += (spikesSpawnOthers.ScaleX.y *= 0.7f);
      spikesSpawnOthers.ScaleY.y += (float) ((1.0 - (double) spikesSpawnOthers.ScaleY.x) * 0.30000001192092896);
      spikesSpawnOthers.ScaleY.x += (spikesSpawnOthers.ScaleY.y *= 0.7f);
      spikesSpawnOthers.transform.localScale = new Vector3(spikesSpawnOthers.ScaleX.x, spikesSpawnOthers.ScaleY.x, 1f);
      yield return (object) null;
    }
  }

  private IEnumerator DoAttack()
  {
    TrapSpikesSpawnOthers spikesSpawnOthers = this;
    AudioManager.Instance.PlayOneShot("event:/boss/deathcat/chain_spawner", spikesSpawnOthers.transform.position);
    spikesSpawnOthers.spriteRenderer.color = spikesSpawnOthers.OverrideColor != Color.white ? spikesSpawnOthers.OverrideColor : Color.white;
    yield return (object) new WaitForSeconds(0.25f);
    spikesSpawnOthers.spriteRenderer.color = spikesSpawnOthers.OverrideColor != Color.white ? spikesSpawnOthers.OverrideColor : Color.yellow;
    yield return (object) new WaitForSeconds(0.25f);
    spikesSpawnOthers.spine.gameObject.SetActive(true);
    spikesSpawnOthers.spriteRenderer.color = spikesSpawnOthers.OverrideColor != Color.white ? spikesSpawnOthers.OverrideColor : Color.red;
    CameraManager.shakeCamera(0.3f, (float) Random.Range(0, 360));
    spikesSpawnOthers.colliders = new List<Collider2D>();
    spikesSpawnOthers.boxCollider2D.OverlapCollider(spikesSpawnOthers.contactFilter2D, (List<Collider2D>) spikesSpawnOthers.colliders);
    foreach (Collider2D collider in spikesSpawnOthers.colliders)
    {
      spikesSpawnOthers.EnemyHealth = collider.GetComponent<Health>();
      if ((Object) spikesSpawnOthers.EnemyHealth != (Object) null && spikesSpawnOthers.EnemyHealth.team != Health.Team.Team2)
        spikesSpawnOthers.EnemyHealth.DealDamage(spikesSpawnOthers.EnemyHealth.team == Health.Team.PlayerTeam ? 1f : 10f, spikesSpawnOthers.gameObject, spikesSpawnOthers.transform.position);
    }
    yield return (object) new WaitForSeconds(0.75f);
    spikesSpawnOthers.spriteRenderer.color = spikesSpawnOthers.OverrideColor != Color.white ? spikesSpawnOthers.OverrideColor : Color.white;
    yield return (object) new WaitForSeconds(0.25f);
    Object.Destroy((Object) spikesSpawnOthers.gameObject);
  }
}
