// Decompiled with JetBrains decompiler
// Type: ForestScuttlerSlamBarricade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class ForestScuttlerSlamBarricade : BaseMonoBehaviour
{
  public ColliderEvents ColliderEvents;
  public SpriteRenderer SpriteRenderer;
  public float RaiseDuration = 0.1f;
  private Health EnemyHealth;

  private void OnEnable()
  {
    this.SpriteRenderer.gameObject.SetActive(false);
    this.ColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.ColliderEvents.SetActive(false);
  }

  private void OnDisable()
  {
    this.ColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.ColliderEvents.SetActive(false);
  }

  public void Play(float Delay)
  {
    this.gameObject.SetActive(true);
    this.StartCoroutine((IEnumerator) this.PlayRoutine(Delay));
  }

  private IEnumerator PlayRoutine(float Delay)
  {
    ForestScuttlerSlamBarricade scuttlerSlamBarricade = this;
    scuttlerSlamBarricade.transform.localScale = Vector3.zero;
    yield return (object) new WaitForSeconds(Delay);
    scuttlerSlamBarricade.SpriteRenderer.gameObject.SetActive(true);
    scuttlerSlamBarricade.ColliderEvents.SetActive(true);
    float Progress = 0.0f;
    float Duration = scuttlerSlamBarricade.RaiseDuration;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      scuttlerSlamBarricade.transform.localScale = Vector3.one * (Progress / Duration);
      yield return (object) null;
    }
    scuttlerSlamBarricade.transform.localScale = Vector3.one;
    yield return (object) new WaitForSeconds(0.2f);
    Progress = 0.0f;
    Duration = 0.3f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      scuttlerSlamBarricade.transform.localScale = Vector3.one * (float) (1.0 - (double) Progress / (double) Duration);
      yield return (object) null;
    }
    scuttlerSlamBarricade.ColliderEvents.SetActive(false);
    scuttlerSlamBarricade.gameObject.SetActive(false);
  }

  private void OnDamageTriggerEnter(Collider2D collider)
  {
    this.EnemyHealth = collider.GetComponent<Health>();
    if (!((Object) this.EnemyHealth != (Object) null) || this.EnemyHealth.team == Health.Team.Team2)
      return;
    this.EnemyHealth.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
  }
}
