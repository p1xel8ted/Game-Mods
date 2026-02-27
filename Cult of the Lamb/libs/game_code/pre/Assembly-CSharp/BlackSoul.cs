// Decompiled with JetBrains decompiler
// Type: BlackSoul
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BlackSoul : BaseMonoBehaviour
{
  public static List<BlackSoul> BlackSouls = new List<BlackSoul>();
  public float SeperationRadius = 0.5f;
  private GameObject Target;
  private float Delay = 0.5f;
  public float Speed;
  private float Angle;
  private float TargetAngle;
  public GameObject BounceChild;
  public GameObject Image;
  public GameObject Trail;
  private float ImageZ;
  private float ImageZSpeed;
  private float TurnSpeed = 5f;
  private float Distance;
  private float ChaseTimer;
  private float LifeTime;
  public float LifeTimeDuration = 7f;
  private float Bobbing;
  public float BobbingArc = 0.1f;
  public float BobbingSpeed = 1f;
  public bool Moving;
  private float TargetHeight;
  private float MaxHeight;
  private float MinHeight;
  private float gravity = -9.8f;
  public List<BlackSoul.BiomeColour> Colours;
  private float z;
  private float magnetiseDistance = 1.5f;
  public bool Completed;
  public bool Simulated;
  private BlackSoulUpdater BlacksoulsupdaterInstance;
  private Vector3 seperator;

  public bool GiveXP { get; set; }

  private bool CanPickUp
  {
    get
    {
      if ((double) FaithAmmo.Ammo < (double) FaithAmmo.Total)
        return true;
      return (UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null && PlayerFarming.Instance.playerWeapon.CurrentWeapon != null && (UnityEngine.Object) PlayerFarming.Instance.playerWeapon.CurrentWeapon.WeaponData != (UnityEngine.Object) null && PlayerFarming.Instance.playerWeapon.CurrentWeapon.WeaponData.ContainsAttachmentType(AttachmentEffect.Fervour);
    }
  }

  private void OnEnable()
  {
    this.BlacksoulsupdaterInstance = BlackSoulUpdater.Instance;
    this.gameObject.SetActive(true);
    if ((UnityEngine.Object) this.Target == (UnityEngine.Object) null && (UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
      this.Target = PlayerFarming.Instance.gameObject;
    this.Bobbing = (float) UnityEngine.Random.Range(0, 360);
    this.MaxHeight = UnityEngine.Random.Range(0.5f, 0.9f);
    this.MinHeight = UnityEngine.Random.Range(0.1f, 0.3f);
    this.TargetHeight = this.MinHeight;
    this.Completed = false;
    this.Delay = 0.5f;
    this.Moving = false;
    this.TurnSpeed = 5f;
    this.ImageZ = 0.0f;
    this.ImageZSpeed = 0.0f;
    this.LifeTime = 0.0f;
    this.Image.SetActive(true);
    Color color = new Color(UnityEngine.Random.Range(-0.25f, 0.25f), this.Colours[0].Color.g, this.Colours[0].Color.b, this.Colours[0].Color.a);
    this.Image.GetComponent<SpriteRenderer>().color = this.Colours[0].Color + color;
    this.Trail.GetComponent<TrailRenderer>().startColor = this.Colours[0].Color + color;
    foreach (BlackSoul.BiomeColour colour in this.Colours)
    {
      if (colour.Location == PlayerFarming.Location)
      {
        this.Image.GetComponent<SpriteRenderer>().material.SetColor("_Tint", colour.Color + color);
        this.Trail.GetComponent<TrailRenderer>().startColor = colour.Color + color;
        break;
      }
    }
    if ((UnityEngine.Object) EnemyDeathCatBoss.Instance != (UnityEngine.Object) null)
    {
      this.Image.GetComponent<SpriteRenderer>().material.SetColor("_Tint", Color.black + color);
      this.Trail.GetComponent<TrailRenderer>().startColor = Color.black;
    }
    BlackSoul.BlackSouls.Add(this);
    this.magnetiseDistance = 1.5f;
    Interaction_Chest.OnChestRevealed += new Interaction_Chest.ChestEvent(this.OnChestRevealed);
    foreach (GameObject demon in Demon_Arrows.Demons)
    {
      if ((bool) (UnityEngine.Object) demon.GetComponent<Demon_Arrows>())
        this.magnetiseDistance = 100f;
    }
  }

  private void OnDisable()
  {
    BlackSoul.BlackSouls.Remove(this);
    Interaction_Chest.OnChestRevealed -= new Interaction_Chest.ChestEvent(this.OnChestRevealed);
  }

  private void OnChestRevealed() => this.magnetiseDistance = 100f;

  public void SetAngle(float Angle)
  {
    this.Angle = Angle;
    this.Speed = UnityEngine.Random.Range(0.5f, 1.5f);
    this.transform.position = this.transform.position + new Vector3(this.Speed * Mathf.Cos(Angle * ((float) Math.PI / 180f)), this.Speed * Mathf.Sin(Angle * ((float) Math.PI / 180f))) * Time.fixedDeltaTime;
  }

  public void SetAngle(float Angle, Vector2 RandomSpeedRange)
  {
    this.Angle = Angle;
    this.Speed = UnityEngine.Random.Range(RandomSpeedRange.x, RandomSpeedRange.y);
    this.transform.position = this.transform.position + new Vector3(this.Speed * Mathf.Cos(Angle * ((float) Math.PI / 180f)), this.Speed * Mathf.Sin(Angle * ((float) Math.PI / 180f))) * Time.fixedDeltaTime;
  }

  public void Seperate(float SeperationRadius)
  {
    this.seperator = Vector3.zero;
    float num1 = Time.fixedDeltaTime * 60f;
    foreach (BlackSoul blackSoul in BlackSoul.BlackSouls)
    {
      if ((UnityEngine.Object) blackSoul != (UnityEngine.Object) this && (UnityEngine.Object) blackSoul != (UnityEngine.Object) null && !blackSoul.Moving)
      {
        float distanceBetween = this.MagnitudeFindDistanceBetween((Vector2) blackSoul.gameObject.transform.position, (Vector2) this.transform.position);
        if ((double) distanceBetween < (double) SeperationRadius)
        {
          float f = Utils.GetAngleR(blackSoul.gameObject.transform.position, this.transform.position) * ((float) Math.PI / 180f);
          float num2 = (float) (((double) SeperationRadius - (double) distanceBetween) / 2.0);
          this.seperator.x += num2 * Mathf.Cos(f) * num1;
          this.seperator.y += num2 * Mathf.Sin(f) * num1;
        }
      }
    }
    this.transform.position += this.seperator;
  }

  private void FixedUpdate() => this.FixedUpdateMethod();

  private void FixedUpdateMethod()
  {
    if ((UnityEngine.Object) this.Target == (UnityEngine.Object) null && (bool) (UnityEngine.Object) PlayerFarming.Instance)
      this.Target = PlayerFarming.Instance.transform.gameObject;
    if ((UnityEngine.Object) this.Target == (UnityEngine.Object) null)
    {
      this.DisableMe();
    }
    else
    {
      this.Trail.SetActive(this.Moving);
      if ((double) Time.fixedDeltaTime > 0.0 && (!this.Simulated || (double) this.transform.position.z >= 0.0))
      {
        this.ImageZSpeed += (float) ((-(double) this.TargetHeight - (double) this.ImageZ) * 0.30000001192092896 / ((double) Time.fixedDeltaTime * 60.0));
        this.ImageZ += (this.ImageZSpeed *= (float) (0.699999988079071 * ((double) Time.fixedDeltaTime * 60.0))) * (Time.fixedDeltaTime * 60f);
        this.BounceChild.transform.localPosition = Vector3.zero + Vector3.forward * this.ImageZ + Vector3.forward * this.BobbingArc * Mathf.Cos(this.Bobbing += this.BobbingSpeed * Time.fixedDeltaTime);
      }
      Vector3 vector3 = this.transform.position + new Vector3(this.Speed * Mathf.Cos(this.Angle * ((float) Math.PI / 180f)), this.Speed * Mathf.Sin(this.Angle * ((float) Math.PI / 180f))) * Time.fixedDeltaTime;
      if (!float.IsNaN(vector3.x) && !float.IsNaN(vector3.y) && !float.IsNaN(vector3.z))
        this.transform.position = new Vector3(vector3.x, vector3.y, !this.Simulated || (double) this.transform.position.z >= 0.0 ? 0.0f : this.transform.position.z + this.z * Time.deltaTime);
      this.z -= this.gravity * Time.deltaTime;
      if ((double) (this.Delay -= Time.fixedDeltaTime) > 0.0)
        return;
      this.Distance = this.MagnitudeFindDistanceBetween((Vector2) this.transform.position, (Vector2) this.Target.transform.position);
      if (this.Moving && !this.CanPickUp)
      {
        this.Speed = 0.0f;
        this.Moving = false;
      }
      if (this.Moving)
      {
        if ((UnityEngine.Object) this.Target == (UnityEngine.Object) null)
        {
          this.DisableMe();
        }
        else
        {
          if ((double) (this.ChaseTimer += Time.fixedDeltaTime) > 1.0)
            this.TurnSpeed = Mathf.Lerp(this.TurnSpeed, 1f, 10f * Time.fixedDeltaTime);
          this.TargetAngle = Utils.GetAngle(this.transform.position, this.Target.transform.position);
          this.Angle = Utils.SmoothAngle(this.Angle, this.TargetAngle, this.TurnSpeed);
          if ((double) this.Speed < 25.0)
            this.Speed += (float) (1.0 * ((double) Time.fixedDeltaTime * 60.0));
          if ((double) this.Distance >= (double) this.Speed * (double) Time.fixedDeltaTime)
            return;
          this.CollectMe();
        }
      }
      else
      {
        if ((double) this.Speed > 0.0)
        {
          this.Speed -= (float) (0.5 * (double) Time.fixedDeltaTime * 60.0);
          if ((double) this.Speed < 0.0)
            this.Speed = 0.0f;
        }
        if (!LetterBox.IsPlaying && (double) (this.LifeTime += Time.fixedDeltaTime) > (double) this.LifeTimeDuration - 2.0)
        {
          if (Time.frameCount % 5 == 0)
            this.Image.SetActive(!this.Image.activeSelf);
          if ((double) this.LifeTime > (double) this.LifeTimeDuration)
            this.DisableMe();
        }
        this.TargetHeight = (double) this.Distance >= (double) this.magnetiseDistance + 0.5 || !this.CanPickUp ? this.MinHeight : this.MaxHeight;
        if ((double) this.Distance >= (double) this.magnetiseDistance || !this.CanPickUp)
          return;
        this.TargetHeight = this.MaxHeight;
        this.Moving = true;
        this.Speed = -10f;
        this.Image.SetActive(true);
      }
    }
  }

  private void DisableMe()
  {
    this.Completed = true;
    ObjectPool.Recycle(this.gameObject);
  }

  private void CollectMe()
  {
    if (!this.CanPickUp)
      return;
    PlayerFarming.Instance.GetBlackSoul(giveXp: this.GiveXP);
    BiomeConstants.Instance.EmitPickUpVFX(this.BounceChild.transform.position, "StarBurst_4");
    CameraManager.shakeCamera(0.2f, this.Angle);
    this.DisableMe();
  }

  public static void Clear()
  {
    for (int index = BlackSoul.BlackSouls.Count - 1; index >= 0; --index)
      BlackSoul.BlackSouls[index].DisableMe();
    BlackSoul.BlackSouls.Clear();
  }

  private float MagnitudeFindDistanceBetween(Vector2 a, Vector2 b)
  {
    double num1 = (double) a.x - (double) b.x;
    float num2 = a.y - b.y;
    return (float) (num1 * num1 + (double) num2 * (double) num2);
  }

  [Serializable]
  public struct BiomeColour
  {
    public Color Color;
    public FollowerLocation Location;
  }
}
