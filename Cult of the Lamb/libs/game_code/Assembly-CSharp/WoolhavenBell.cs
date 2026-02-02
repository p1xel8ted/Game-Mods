// Decompiled with JetBrains decompiler
// Type: WoolhavenBell
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class WoolhavenBell : MonoBehaviour
{
  public Transform pivot;
  public Vector3 localSwingAxis = new Vector3(0.0f, 0.0f, 1f);
  public float swingMaxAngle = 80f;
  public float stiffness = 12f;
  public float damping = 1.1f;
  public float swingOnHitMin = 2f;
  public float swingOnHitMax = 4f;
  public string impactSFXBell = "event:/dlc/env/woolhaven/hanging_bell_knock";
  public float angle;
  public float rotationSpeed;
  public Quaternion initialLocalRotation;
  public Vector3 axisLocalNormalized;
  public bool hasContact;

  public void Awake()
  {
    if ((UnityEngine.Object) this.pivot == (UnityEngine.Object) null)
      this.pivot = this.transform;
    this.initialLocalRotation = this.pivot.localRotation;
    this.axisLocalNormalized = this.localSwingAxis.normalized;
  }

  public void Update()
  {
    float deltaTime = Time.deltaTime;
    this.rotationSpeed += (float) (-(double) this.stiffness * (double) this.angle - (double) this.damping * (double) this.rotationSpeed) * deltaTime;
    this.angle += this.rotationSpeed * deltaTime;
    float num = (float) Math.PI / 180f * Mathf.Abs(this.swingMaxAngle);
    if ((double) this.angle > (double) num)
    {
      this.angle = num;
      if ((double) this.rotationSpeed > 0.0)
        this.rotationSpeed = 0.0f;
    }
    else if ((double) this.angle < -(double) num)
    {
      this.angle = -num;
      if ((double) this.rotationSpeed < 0.0)
        this.rotationSpeed = 0.0f;
    }
    this.pivot.localRotation = this.initialLocalRotation * Quaternion.AngleAxis(this.angle * 57.29578f, this.axisLocalNormalized);
  }

  public void OnTriggerEnter2D(Collider2D other)
  {
    this.HandleContactEnter(other, (Vector3) other.ClosestPoint((Vector2) this.pivot.position));
  }

  public void OnTriggerExit2D(Collider2D other) => this.hasContact = false;

  public void OnCollisionEnter2D(Collision2D collision)
  {
    Vector3 contactPoint = collision.contacts.Length == 0 ? (Vector3) collision.GetContact(0).point : (Vector3) collision.contacts[0].point;
    this.HandleContactEnter(collision.collider, contactPoint);
  }

  public void OnCollisionExit2D(Collision2D collision) => this.hasContact = false;

  public void HandleContactEnter(Collider2D other, Vector3 contactPoint)
  {
    if (!other.CompareTag("Player"))
      return;
    if (!this.hasContact)
    {
      Vector3 lhs = this.pivot.InverseTransformPoint(contactPoint);
      Vector3 axisLocalNormalized = this.axisLocalNormalized;
      Vector3 normalized = Vector3.Cross(axisLocalNormalized, (double) Mathf.Abs(Vector3.Dot(axisLocalNormalized, Vector3.up)) >= 0.89999997615814209 ? Vector3.forward : Vector3.up).normalized;
      float num = Mathf.Sign(Vector3.Dot(lhs, normalized));
      this.rotationSpeed += UnityEngine.Random.Range(this.swingOnHitMin, this.swingOnHitMax) * num;
      if (!string.IsNullOrEmpty(this.impactSFXBell))
        AudioManager.Instance.PlayOneShot(this.impactSFXBell, this.transform.position);
    }
    this.hasContact = true;
  }
}
