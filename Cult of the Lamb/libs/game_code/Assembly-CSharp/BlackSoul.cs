// Decompiled with JetBrains decompiler
// Type: BlackSoul
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class BlackSoul : BaseMonoBehaviour
{
  public float SeperationRadius = 0.5f;
  public GameObject Target;
  public float Delay = 0.5f;
  public float Speed;
  public float Angle;
  public float TargetAngle;
  public GameObject BounceChild;
  public GameObject Image;
  public GameObject Trail;
  public float ImageZ;
  public float ImageZSpeed;
  public float TurnSpeed = 5f;
  public float Distance;
  public float ChaseTimer;
  public float LifeTime;
  public float LifeTimeDuration = 70f;
  public float Bobbing;
  public float BobbingArc = 0.1f;
  public float BobbingSpeed = 1f;
  public bool Moving;
  public float TargetHeight;
  public float MaxHeight;
  public float MinHeight;
  public float gravity = -9.8f;
  public bool Sin;
  public List<BlackSoul.BiomeColour> Colours;
  public float z;
  public float magnetiseDistance = 1.5f;
  public int Delta = 1;
  public bool Completed;
  public bool Simulated;
  [CompilerGenerated]
  public bool \u003CGiveXP\u003Ek__BackingField;
  public BlackSoulUpdater BlacksoulsupdaterInstance;
  public static bool CanPickUpIsUpdatedInThisFrame;
  public Vector3 seperator;

  public bool GiveXP
  {
    get => this.\u003CGiveXP\u003Ek__BackingField;
    set => this.\u003CGiveXP\u003Ek__BackingField = value;
  }

  public static void UpdateCanPickUp()
  {
    BlackSoul.CanPickUpIsUpdatedInThisFrame = true;
    bool flag = false;
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      PlayerFarming player = PlayerFarming.players[index];
      if (!flag)
        flag = (double) player.playerSpells.faithAmmo.Ammo < (double) player.playerSpells.faithAmmo.Total;
      player.CanPickupBlackSoul = (double) player.playerSpells.faithAmmo.Ammo < (double) player.playerSpells.faithAmmo.Total;
    }
    if (flag)
      return;
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      PlayerFarming player = PlayerFarming.players[index];
      player.CanPickupBlackSoul = player.CurrentWeaponInfo != null && (UnityEngine.Object) player.CurrentWeaponInfo.WeaponData != (UnityEngine.Object) null && player.CurrentWeaponInfo.WeaponData.ContainsAttachmentType(AttachmentEffect.Fervour);
    }
  }

  public void OnEnable()
  {
    if (PlayerFleeceManager.FleeceSwapsWeaponForCurse())
      this.LifeTimeDuration *= 2f;
    this.BlacksoulsupdaterInstance = BlackSoulUpdater.Instance;
    this.gameObject.SetActive(true);
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
    if ((UnityEngine.Object) EnemyDeathCatBoss.Instance != (UnityEngine.Object) null || (UnityEngine.Object) EnemyWolfBoss.Instance != (UnityEngine.Object) null)
    {
      this.Image.GetComponent<SpriteRenderer>().material.SetColor("_Tint", Color.black + color);
      this.Trail.GetComponent<TrailRenderer>().startColor = Color.black;
    }
    BlackSoulUpdater.BlackSouls.Add(this);
    this.magnetiseDistance = 1.5f;
    Interaction_Chest.OnChestRevealed += new Interaction_Chest.ChestEvent(this.OnChestRevealed);
    foreach (GameObject demon in Demon_Arrows.Demons)
    {
      if ((bool) (UnityEngine.Object) demon.GetComponent<Demon_Arrows>())
        this.magnetiseDistance = 100f;
    }
  }

  public void OnDisable()
  {
    BlackSoulUpdater.BlackSouls.Remove(this);
    Interaction_Chest.OnChestRevealed -= new Interaction_Chest.ChestEvent(this.OnChestRevealed);
  }

  public void OnChestRevealed() => this.magnetiseDistance = 100f;

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
    foreach (BlackSoul blackSoul in BlackSoulUpdater.BlackSouls)
    {
      if ((UnityEngine.Object) blackSoul != (UnityEngine.Object) this && (UnityEngine.Object) blackSoul != (UnityEngine.Object) null && !blackSoul.Moving)
      {
        float num2 = Vector3.Distance(blackSoul.gameObject.transform.position, this.transform.position);
        if ((double) num2 < (double) SeperationRadius)
        {
          float f = Utils.GetAngleR(blackSoul.gameObject.transform.position, this.transform.position) * ((float) Math.PI / 180f);
          float num3 = (float) (((double) SeperationRadius - (double) num2) / 2.0);
          this.seperator.x += num3 * Mathf.Cos(f) * num1;
          this.seperator.y += num3 * Mathf.Sin(f) * num1;
        }
      }
    }
    this.transform.position += this.seperator;
  }

  public void FixedUpdate() => this.FixedUpdateMethod();

  public void LateUpdate() => BlackSoul.CanPickUpIsUpdatedInThisFrame = false;

  public void FixedUpdateMethod()
  {
    if (!BlackSoul.CanPickUpIsUpdatedInThisFrame)
      BlackSoul.UpdateCanPickUp();
    PlayerFarming playerFarming = (PlayerFarming) null;
    float num1 = this.magnetiseDistance;
    this.Target = (GameObject) null;
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      if ((bool) (UnityEngine.Object) PlayerFarming.players[index] && PlayerFarming.players[index].CanPickupBlackSoul)
      {
        float num2 = Vector3.Distance(this.transform.position, PlayerFarming.players[index].transform.position);
        if ((double) num2 < (double) num1 || index == 0)
        {
          playerFarming = PlayerFarming.players[index];
          num1 = num2;
          this.Target = playerFarming.gameObject;
        }
      }
    }
    if ((UnityEngine.Object) this.Target == (UnityEngine.Object) null)
    {
      for (int index = 0; index < PlayerFarming.playersCount; ++index)
      {
        if ((bool) (UnityEngine.Object) PlayerFarming.players[index] && PlayerFarming.players[index].CanPickupBlackSoul)
        {
          playerFarming = PlayerFarming.players[index];
          this.Target = playerFarming.gameObject;
          break;
        }
      }
    }
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
      this.Distance = Vector3.Distance(this.transform.position, this.Target.transform.position);
      if (this.Moving && !playerFarming.CanPickupBlackSoul)
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
          this.CollectMe(playerFarming);
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
        this.TargetHeight = (double) this.Distance >= (double) this.magnetiseDistance + 0.5 || !playerFarming.CanPickupBlackSoul ? this.MinHeight : this.MaxHeight;
        if ((double) this.Distance >= (double) this.magnetiseDistance || !playerFarming.CanPickupBlackSoul)
          return;
        this.TargetHeight = this.MaxHeight;
        this.Moving = true;
        this.Speed = -10f;
        this.Image.SetActive(true);
      }
    }
  }

  public void DisableMe()
  {
    this.Completed = true;
    ObjectPool.Recycle(this.gameObject);
  }

  public void CollectMe(PlayerFarming playerFarming)
  {
    if ((UnityEngine.Object) playerFarming == (UnityEngine.Object) null)
      playerFarming = PlayerFarming.Instance;
    if (!playerFarming.CanPickupBlackSoul)
      return;
    if (!this.Sin)
    {
      playerFarming.GetBlackSoul(this.Delta, this.GiveXP);
      BlackSoul.UpdateCanPickUp();
    }
    BiomeConstants.Instance.EmitPickUpVFX(this.BounceChild.transform.position, "StarBurst_4");
    CameraManager.shakeCamera(0.2f, this.Angle);
    this.DisableMe();
    this.Delta = 1;
  }

  public float MagnitudeFindDistanceBetween(Vector2 a, Vector2 b)
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
