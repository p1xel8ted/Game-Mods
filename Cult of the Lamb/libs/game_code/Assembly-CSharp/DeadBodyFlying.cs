// Decompiled with JetBrains decompiler
// Type: DeadBodyFlying
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMODUnity;
using System;
using UnityEngine;

#nullable disable
public class DeadBodyFlying : BaseMonoBehaviour
{
  public Transform ObjectToRotate;
  public Vector2 DirectionalSpeed;
  public float Speed = 5f;
  public bool RightFacing;
  public Rigidbody2D rb2D;
  public float velocity;
  [EventRef]
  [SerializeField]
  public string deathSound;
  public float Z = -0.1f;
  public float ZSpeed = -10f;
  public Vector2 ScaleX = new Vector2(2f, 0.0f);
  public Vector2 ScaleY = new Vector2(2f, 0.0f);
  public float DestroyTimer;

  public void Start() => this.rb2D = this.GetComponent<Rigidbody2D>();

  public void Init(float Angle)
  {
    this.DirectionalSpeed = new Vector2(this.Speed * Mathf.Cos(Angle * ((float) Math.PI / 180f)), this.Speed * Mathf.Sin(Angle * ((float) Math.PI / 180f)));
    this.RightFacing = (double) this.DirectionalSpeed.x > 0.0;
    if (this.RightFacing)
      this.ObjectToRotate.localScale = new Vector3(-1f, 1f, 1f);
    else
      this.ObjectToRotate.localScale = new Vector3(1f, 1f, 1f);
  }

  public void Update()
  {
    this.ObjectToRotate.transform.parent.Rotate(new Vector3(0.0f, this.Speed * (this.RightFacing ? -2f : 2f) * Time.timeScale, 0.0f));
    this.BounceMe();
    this.Speed = Mathf.SmoothDamp(this.Speed, 0.0f, ref this.velocity, 0.5f);
    if ((double) (this.DestroyTimer += Time.deltaTime) <= 0.40000000596046448)
      return;
    CameraManager.shakeCamera(0.5f, (float) UnityEngine.Random.Range(0, 360), false);
    AudioManager.Instance.PlayOneShot(this.deathSound, this.gameObject);
    this.gameObject.Recycle();
  }

  public void FixedUpdate()
  {
    this.rb2D.MovePosition(this.rb2D.position + this.DirectionalSpeed * Time.fixedDeltaTime);
  }

  public void BounceMe()
  {
    this.ZSpeed += 0.5f * GameManager.DeltaTime;
    this.Z += this.ZSpeed * Time.deltaTime;
    this.ScaleX.y += (float) ((1.0 - (double) this.ScaleX.x) * 0.30000001192092896);
    this.ScaleX.x += (this.ScaleX.y *= 0.8f);
    this.ScaleY.y += (float) ((1.0 - (double) this.ScaleY.x) * 0.30000001192092896);
    this.ScaleY.x += (this.ScaleY.y *= 0.8f);
    this.ObjectToRotate.transform.parent.transform.parent.localPosition = Vector3.forward * this.Z;
    this.ObjectToRotate.transform.parent.transform.parent.localScale = new Vector3(this.ScaleX.x, this.ScaleY.y, 1f);
  }

  public bool OnGround(Vector3 Position)
  {
    LayerMask mask = (LayerMask) LayerMask.GetMask("Island");
    return Physics.Raycast(Position, Vector3.forward, out RaycastHit _, 10f, (int) mask);
  }
}
