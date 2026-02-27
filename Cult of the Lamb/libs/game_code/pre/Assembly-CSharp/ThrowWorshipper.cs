// Decompiled with JetBrains decompiler
// Type: ThrowWorshipper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class ThrowWorshipper : BaseMonoBehaviour
{
  private float vz;
  private float childZ;
  private Rigidbody2D rb;
  private float vx;
  private float vy;
  private float Speed;
  public float FacingAngle;
  public GameObject child;
  private float Timer;
  private float BloodTimer;
  public GameObject blood;
  public float StartVZ = -2f;
  public float StartSpeed = 5f;
  private SimpleSpineAnimator SpineAnimator;
  private Vector3 Scale = Vector3.one;
  private Vector2 ScaleSpeed;
  private float Rotation;
  public float BounceSpeed = 0.8f;
  public float BounceZSpeed = -0.8f;

  public System.Action OnComplete { get; set; }

  private void OnEnable()
  {
    this.rb = this.GetComponent<Rigidbody2D>();
    this.vz = this.StartVZ;
    this.childZ = -0.5f;
    this.Speed = this.StartSpeed;
    this.SpineAnimator = this.GetComponentInChildren<SimpleSpineAnimator>();
  }

  private void OnDisable()
  {
  }

  private void Update()
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

  private void FixedUpdate()
  {
    if ((UnityEngine.Object) this.rb == (UnityEngine.Object) null)
      return;
    this.rb.MovePosition(this.rb.position + new Vector2(this.vx, this.vy) * Time.deltaTime);
  }

  private void BounceChild()
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

  private void OnCollisionEnter2D(Collision2D collision)
  {
    this.FacingAngle += (float) (180 + UnityEngine.Random.Range(-20, 20));
    this.FacingAngle %= 360f;
    this.vx = this.Speed * Mathf.Cos(this.FacingAngle * ((float) Math.PI / 180f));
    this.vy = this.Speed * Mathf.Sin(this.FacingAngle * ((float) Math.PI / 180f));
  }
}
