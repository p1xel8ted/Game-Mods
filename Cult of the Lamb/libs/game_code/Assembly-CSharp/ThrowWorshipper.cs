// Decompiled with JetBrains decompiler
// Type: ThrowWorshipper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class ThrowWorshipper : BaseMonoBehaviour
{
  public float vz;
  public float childZ;
  public Rigidbody2D rb;
  public float vx;
  public float vy;
  public float Speed;
  public float FacingAngle;
  public GameObject child;
  public float Timer;
  public float BloodTimer;
  public GameObject blood;
  public float StartVZ = -2f;
  public float StartSpeed = 5f;
  public SimpleSpineAnimator SpineAnimator;
  public Vector3 Scale = Vector3.one;
  public Vector2 ScaleSpeed;
  public float Rotation;
  [CompilerGenerated]
  public System.Action \u003COnComplete\u003Ek__BackingField;
  public float BounceSpeed = 0.8f;
  public float BounceZSpeed = -0.8f;

  public System.Action OnComplete
  {
    get => this.\u003COnComplete\u003Ek__BackingField;
    set => this.\u003COnComplete\u003Ek__BackingField = value;
  }

  public void OnEnable()
  {
    this.rb = this.GetComponent<Rigidbody2D>();
    this.vz = this.StartVZ;
    this.childZ = -0.5f;
    this.Speed = this.StartSpeed;
    this.SpineAnimator = this.GetComponentInChildren<SimpleSpineAnimator>();
  }

  public void OnDisable()
  {
  }

  public void Update()
  {
    this.ScaleSpeed.x += (float) ((1.0 - (double) this.Scale.x) * 0.30000001192092896);
    this.Scale.x += (this.ScaleSpeed.x *= 0.8f);
    this.ScaleSpeed.y += (float) ((1.0 - (double) this.Scale.y) * 0.30000001192092896);
    this.Scale.y += (this.ScaleSpeed.y *= 0.8f);
    this.BounceChild();
    this.vx = this.Speed * Mathf.Cos(this.FacingAngle * ((float) Math.PI / 180f));
    this.vy = this.Speed * Mathf.Sin(this.FacingAngle * ((float) Math.PI / 180f));
    if ((double) (this.BloodTimer += Time.deltaTime) <= 0.05000000074505806)
      return;
    this.BloodTimer = 0.0f;
    UnityEngine.Object.Instantiate<GameObject>(this.blood, this.child.transform.position, Quaternion.identity, this.transform.parent);
  }

  public void FixedUpdate()
  {
    if ((UnityEngine.Object) this.rb == (UnityEngine.Object) null)
      return;
    this.rb.MovePosition(this.rb.position + new Vector2(this.vx, this.vy) * Time.deltaTime);
  }

  public void BounceChild()
  {
    if ((double) this.childZ >= 0.0)
    {
      if ((double) this.vz > 0.10000000149011612)
      {
        this.Speed *= this.BounceSpeed;
        this.vz *= this.BounceZSpeed;
        this.Scale.x = 0.5f;
        this.Scale.y = 1.5f;
        this.SpineAnimator.Animate("thrown-bounce", 0, false);
        this.SpineAnimator.AddAnimate("thrown", 0, true, 0.0f);
      }
      else
      {
        this.vz = 0.0f;
        System.Action onComplete = this.OnComplete;
        if (onComplete != null)
          onComplete();
        this.OnComplete = (System.Action) null;
        this.enabled = false;
      }
      this.childZ = 0.0f;
    }
    else
      this.vz += 0.02f;
    this.childZ += this.vz;
    this.child.transform.localPosition = new Vector3(0.0f, 0.0f, this.childZ);
  }

  public void OnCollisionEnter2D(Collision2D collision)
  {
    this.FacingAngle += (float) (180 + UnityEngine.Random.Range(-20, 20));
    this.FacingAngle %= 360f;
    this.vx = this.Speed * Mathf.Cos(this.FacingAngle * ((float) Math.PI / 180f));
    this.vy = this.Speed * Mathf.Sin(this.FacingAngle * ((float) Math.PI / 180f));
  }
}
