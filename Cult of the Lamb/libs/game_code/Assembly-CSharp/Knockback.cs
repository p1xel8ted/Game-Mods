// Decompiled with JetBrains decompiler
// Type: Knockback
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Knockback : MonoBehaviour
{
  [Header("Knockback Settings")]
  [SerializeField]
  [Tooltip("The base force applied for knockback.")]
  public float baseForce = 1000f;
  [SerializeField]
  [Tooltip("Modifier for the knockback force.")]
  public float knockbackForceModifier = 1f;
  [SerializeField]
  [Tooltip("Duration of the knockback effect in seconds.")]
  public float knockbackDuration = 0.5f;
  public Rigidbody2D rb;
  public Health healthComponent;
  [CompilerGenerated]
  public bool \u003CIsKnockbackActive\u003Ek__BackingField;

  public bool IsKnockbackActive
  {
    get => this.\u003CIsKnockbackActive\u003Ek__BackingField;
    set => this.\u003CIsKnockbackActive\u003Ek__BackingField = value;
  }

  public void Awake()
  {
    this.rb = this.GetComponent<Rigidbody2D>();
    if ((UnityEngine.Object) this.rb == (UnityEngine.Object) null)
      Debug.LogError((object) "KnockbackBehavior requires a Rigidbody2D component on the same GameObject.");
    this.healthComponent = this.GetComponent<Health>();
    if (!((UnityEngine.Object) this.healthComponent != (UnityEngine.Object) null))
      return;
    this.healthComponent.OnHit += new Health.HitAction(this.OnHit);
  }

  public void OnDestroy()
  {
    if (!((UnityEngine.Object) this.healthComponent != (UnityEngine.Object) null))
      return;
    this.healthComponent.OnHit -= new Health.HitAction(this.OnHit);
  }

  public void OnHit(
    GameObject attacker,
    Vector3 attackLocation,
    Health.AttackTypes attackType,
    bool fromBehind)
  {
    if (attackType == Health.AttackTypes.NoKnockBack)
      return;
    this.ApplyKnockback(attacker);
  }

  public void ApplyKnockback(GameObject attacker)
  {
    if (this.IsKnockbackActive)
      return;
    this.StartCoroutine((IEnumerator) this.ApplyKnockbackRoutine(attacker));
  }

  public IEnumerator ApplyKnockbackRoutine(GameObject attacker)
  {
    Knockback knockback = this;
    knockback.IsKnockbackActive = true;
    float f = Utils.GetAngle(attacker.transform.position, knockback.transform.position) * ((float) Math.PI / 180f);
    Vector2 force = new Vector2(Mathf.Cos(f), Mathf.Sin(f)) * knockback.baseForce * knockback.knockbackForceModifier;
    knockback.rb.AddForce(force);
    float elapsed = 0.0f;
    while ((double) elapsed < (double) knockback.knockbackDuration)
    {
      elapsed += Time.deltaTime;
      yield return (object) null;
    }
    knockback.rb.velocity = Vector2.zero;
    knockback.IsKnockbackActive = false;
  }
}
