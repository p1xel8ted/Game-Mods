// Decompiled with JetBrains decompiler
// Type: Soul
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class Soul : BaseMonoBehaviour
{
  private GameObject Target;
  private float Delay = 0.5f;
  private float Speed = -15f;
  private float Angle;
  private float TargetAngle;
  public GameObject Image;
  private float ImageZ;
  private float ImageZSpeed;
  private float TurnSpeed = 7f;

  private void Start()
  {
    if ((UnityEngine.Object) this.Target == (UnityEngine.Object) null)
      this.Target = GameObject.FindGameObjectWithTag("Player");
    this.Angle = Utils.GetAngle(this.transform.position, this.Target.transform.position);
  }

  private void Update()
  {
    if ((UnityEngine.Object) this.Target == (UnityEngine.Object) null)
      this.Target = GameObject.FindGameObjectWithTag("Player");
    if ((UnityEngine.Object) this.Target == (UnityEngine.Object) null)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
    else
    {
      this.ImageZSpeed += (float) ((-1.0 - (double) this.ImageZ) * 0.30000001192092896);
      this.ImageZ += (this.ImageZSpeed *= 0.7f);
      this.Image.transform.localPosition = Vector3.zero + Vector3.forward * this.ImageZ;
      if ((double) (this.Delay -= Time.deltaTime) > 0.0)
        return;
      if ((double) this.Delay < -1.0)
        this.TurnSpeed = Mathf.Lerp(this.TurnSpeed, 2f, 10f * Time.deltaTime);
      this.TargetAngle = Utils.GetAngle(this.transform.position, this.Target.transform.position);
      this.Angle += (float) ((double) Mathf.Atan2(Mathf.Sin((float) (((double) this.TargetAngle - (double) this.Angle) * (Math.PI / 180.0))), Mathf.Cos((float) (((double) this.TargetAngle - (double) this.Angle) * (Math.PI / 180.0)))) * 57.295780181884766 / ((double) this.TurnSpeed * (double) GameManager.DeltaTime));
      if ((double) this.Speed < 20.0)
        ++this.Speed;
      this.transform.position = this.transform.position + new Vector3(this.Speed * Mathf.Cos(this.Angle * ((float) Math.PI / 180f)) * Time.deltaTime, this.Speed * Mathf.Sin(this.Angle * ((float) Math.PI / 180f)) * Time.deltaTime);
      if ((double) Vector2.Distance((Vector2) this.transform.position, (Vector2) this.Target.transform.position) >= (double) this.Speed * (double) Time.deltaTime)
        return;
      this.CollectMe();
    }
  }

  private void CollectMe()
  {
    PlayerFarming.Instance.GetSoul();
    BiomeConstants.Instance.EmitHitVFXSoul(this.Image.gameObject.transform.position, Quaternion.identity);
    CameraManager.shakeCamera(0.2f, this.Angle);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }
}
