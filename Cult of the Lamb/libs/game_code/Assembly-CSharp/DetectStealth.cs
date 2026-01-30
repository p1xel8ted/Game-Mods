// Decompiled with JetBrains decompiler
// Type: DetectStealth
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

#nullable disable
public class DetectStealth : BaseMonoBehaviour
{
  public UnitObject unitObject;
  public bool Sway = true;
  public float SwayAngle = 30f;
  public float SwaySpeed = 1f;
  public float DetectEvenIfStealth = 3f;
  public float _LookingAngle;
  public SpriteShapeController spriteShapeController;
  public StateMachine state;
  public SpriteRenderer spriteRenderer;
  public Image alertRadial;
  public float VisionRage = 8f;
  public float VisionConeAngle = 40f;
  public float CloseConeAngle = 120f;
  public float _AlertLevel;
  public float AlertLimit = 3f;
  public int VisibleEnemies;
  public ContactFilter2D c;
  public List<RaycastHit2D> Results;
  public float Distance;
  public float WaitToHideUI;
  public Health EnemyHealth;
  public float Swing;

  public float LookingAngle
  {
    get => this._LookingAngle;
    set
    {
      this._LookingAngle = value;
      if ((double) this._LookingAngle < 0.0)
        this._LookingAngle += 360f;
      if ((double) this._LookingAngle <= 360.0)
        return;
      this._LookingAngle -= 360f;
    }
  }

  public float AlertLevel
  {
    get => this._AlertLevel;
    set => this._AlertLevel = Mathf.Clamp(value, 0.0f, this.AlertLimit);
  }

  public void Start()
  {
    this.spriteShapeController.spline.Clear();
    this.UpdateSpriteShape();
    this.LookingAngle = this.state.facingAngle;
    this.c = new ContactFilter2D();
    this.c.layerMask = (LayerMask) LayerMask.GetMask("Player");
    this.spriteShapeController.enabled = false;
  }

  public void Update()
  {
    if (!this.unitObject.health.Unaware)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
    else
    {
      if (this.Sway)
        this.DoSway();
      this.UpdateSpriteShape();
      this.VisibleEnemies = 0;
      if (!this.spriteShapeController.enabled)
      {
        foreach (Health health in Health.playerTeam)
        {
          if (health.state.CURRENT_STATE == StateMachine.State.Stealth)
          {
            this.spriteShapeController.enabled = true;
            break;
          }
        }
      }
      else
      {
        int num = 0;
        foreach (Health health in Health.playerTeam)
        {
          if (health.state.CURRENT_STATE == StateMachine.State.Stealth)
            ++num;
        }
        if (num <= 0)
          this.WaitToHideUI += Time.deltaTime;
        if ((double) this.WaitToHideUI > 1.0)
        {
          this.WaitToHideUI = 0.0f;
          this.spriteShapeController.enabled = false;
        }
      }
      foreach (Health health in Health.playerTeam)
      {
        this.Distance = Vector3.Distance(this.transform.position, health.transform.position);
        if ((UnityEngine.Object) health != (UnityEngine.Object) null && (double) this.Distance < (double) this.VisionRage)
        {
          if ((double) this.Distance < (double) this.DetectEvenIfStealth)
          {
            if ((double) Mathf.Abs(Utils.GetAngle(this.transform.position, health.transform.position) - this.LookingAngle) <= (double) this.CloseConeAngle / 2.0 || (double) Mathf.Abs(Utils.GetAngle(this.transform.position, health.transform.position) - this.LookingAngle) >= 360.0 - (double) this.CloseConeAngle / 2.0)
            {
              this.Results = new List<RaycastHit2D>();
              Physics2D.Raycast((Vector2) this.transform.position, (Vector2) (health.transform.position - this.transform.position), this.c, (List<RaycastHit2D>) this.Results, this.VisionRage);
              if ((UnityEngine.Object) this.Results[0].collider.gameObject == (UnityEngine.Object) health.gameObject)
              {
                ++this.VisibleEnemies;
                this.AlertLevel += Time.deltaTime * 10f;
                this.EnemyHealth = health;
              }
            }
          }
          else if ((double) Mathf.Abs(Utils.GetAngle(this.transform.position, health.transform.position) - this.LookingAngle) <= (double) this.VisionConeAngle / 2.0 || (double) Mathf.Abs(Utils.GetAngle(this.transform.position, health.transform.position) - this.LookingAngle) >= 360.0 - (double) this.VisionConeAngle / 2.0)
          {
            this.Results = new List<RaycastHit2D>();
            Physics2D.Raycast((Vector2) this.transform.position, (Vector2) (health.transform.position - this.transform.position), this.c, (List<RaycastHit2D>) this.Results, this.VisionRage);
            if ((UnityEngine.Object) this.Results[0].collider.gameObject == (UnityEngine.Object) health.gameObject)
            {
              ++this.VisibleEnemies;
              this.EnemyHealth = health;
              if (health.state.CURRENT_STATE == StateMachine.State.Stealth && (double) this.Distance > (double) this.DetectEvenIfStealth)
                this.AlertLevel += Time.deltaTime * 2f;
              else
                this.AlertLevel += Time.deltaTime * 10f;
            }
          }
        }
      }
      if (this.VisibleEnemies <= 0 && (double) this.AlertLevel < (double) this.AlertLimit)
        this.AlertLevel -= Time.deltaTime * 2f;
      this.alertRadial.fillAmount = this.AlertLevel / this.AlertLimit;
      if ((double) this.AlertLevel < (double) this.AlertLimit)
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
  }

  public void DoSway()
  {
    this.LookingAngle = this.state.facingAngle + (float) ((double) this.SwayAngle * (double) Mathf.Cos((this.Swing += Time.deltaTime * this.SwaySpeed) * ((float) Math.PI / 180f)) % 360.0);
  }

  public void BtnUpdateSpriteShape()
  {
    this.LookingAngle = this.state.facingAngle;
    this.UpdateSpriteShape();
  }

  public void UpdateSpriteShape()
  {
    while (this.spriteShapeController.spline.GetPointCount() < 7)
      this.spriteShapeController.spline.InsertPointAt(0, Vector3.one * (float) (this.spriteShapeController.spline.GetPointCount() + 1) * 30f);
    this.spriteShapeController.spline.SetPosition(0, Vector3.zero);
    float f1 = (float) (((double) this.LookingAngle + (double) this.CloseConeAngle / 2.0) * (Math.PI / 180.0));
    Vector3 point = new Vector3(this.DetectEvenIfStealth * Mathf.Cos(f1), this.DetectEvenIfStealth * Mathf.Sin(f1));
    this.spriteShapeController.spline.SetPosition(1, point);
    float f2 = (float) (((double) this.LookingAngle + (double) this.VisionConeAngle / 2.0) * (Math.PI / 180.0));
    point = new Vector3(this.DetectEvenIfStealth * Mathf.Cos(f2), this.DetectEvenIfStealth * Mathf.Sin(f2));
    this.spriteShapeController.spline.SetPosition(2, point);
    float f3 = (float) (((double) this.LookingAngle + (double) this.VisionConeAngle / 2.0) * (Math.PI / 180.0));
    point = new Vector3(this.VisionRage * Mathf.Cos(f3), this.VisionRage * Mathf.Sin(f3));
    this.spriteShapeController.spline.SetPosition(3, point);
    float f4 = (float) (((double) this.LookingAngle - (double) this.VisionConeAngle / 2.0) * (Math.PI / 180.0));
    point = new Vector3(this.VisionRage * Mathf.Cos(f4), this.VisionRage * Mathf.Sin(f4));
    this.spriteShapeController.spline.SetPosition(4, point);
    float f5 = (float) (((double) this.LookingAngle - (double) this.VisionConeAngle / 2.0) * (Math.PI / 180.0));
    point = new Vector3(this.DetectEvenIfStealth * Mathf.Cos(f5), this.DetectEvenIfStealth * Mathf.Sin(f5));
    this.spriteShapeController.spline.SetPosition(5, point);
    float f6 = (float) (((double) this.LookingAngle - (double) this.CloseConeAngle / 2.0) * (Math.PI / 180.0));
    point = new Vector3(this.DetectEvenIfStealth * Mathf.Cos(f6), this.DetectEvenIfStealth * Mathf.Sin(f6));
    this.spriteShapeController.spline.SetPosition(6, point);
  }

  public void OnDrawGizmos()
  {
    if (Application.isEditor && !Application.isPlaying)
      this.LookingAngle = this.state.facingAngle;
    float f1 = (float) (((double) this.LookingAngle + (double) this.VisionConeAngle / 2.0) * (Math.PI / 180.0));
    Vector3 vector3 = new Vector3(this.VisionRage * Mathf.Cos(f1), this.VisionRage * Mathf.Sin(f1));
    Utils.DrawLine(this.transform.position, this.transform.position + vector3, Color.white);
    float f2 = (float) (((double) this.LookingAngle - (double) this.VisionConeAngle / 2.0) * (Math.PI / 180.0));
    vector3 = new Vector3(this.VisionRage * Mathf.Cos(f2), this.VisionRage * Mathf.Sin(f2));
    Utils.DrawLine(this.transform.position, this.transform.position + vector3, Color.white);
    float f3 = (float) (((double) this.LookingAngle + (double) this.CloseConeAngle / 2.0) * (Math.PI / 180.0));
    vector3 = new Vector3(this.DetectEvenIfStealth * Mathf.Cos(f3), this.DetectEvenIfStealth * Mathf.Sin(f3));
    Utils.DrawLine(this.transform.position, this.transform.position + vector3, Color.red);
    float f4 = (float) (((double) this.LookingAngle - (double) this.CloseConeAngle / 2.0) * (Math.PI / 180.0));
    vector3 = new Vector3(this.DetectEvenIfStealth * Mathf.Cos(f4), this.DetectEvenIfStealth * Mathf.Sin(f4));
    Utils.DrawLine(this.transform.position, this.transform.position + vector3, Color.red);
    if (this.Sway)
    {
      float f5 = (float) (((double) this.state.facingAngle + (double) this.VisionConeAngle / 2.0 + (double) this.SwayAngle) * (Math.PI / 180.0));
      vector3 = new Vector3(this.VisionRage * Mathf.Cos(f5), this.VisionRage * Mathf.Sin(f5));
      Utils.DrawLine(this.transform.position, this.transform.position + vector3, Color.white);
      float f6 = (float) (((double) this.state.facingAngle - (double) this.VisionConeAngle / 2.0 - (double) this.SwayAngle) * (Math.PI / 180.0));
      vector3 = new Vector3(this.VisionRage * Mathf.Cos(f6), this.VisionRage * Mathf.Sin(f6));
      Utils.DrawLine(this.transform.position, this.transform.position + vector3, Color.white);
      vector3 = new Vector3(this.VisionRage * Mathf.Cos(this.LookingAngle * ((float) Math.PI / 180f)), this.VisionRage * Mathf.Sin(this.LookingAngle * ((float) Math.PI / 180f)));
      Utils.DrawLine(this.transform.position, this.transform.position + vector3, Color.yellow);
    }
    Utils.DrawCircleXY(this.transform.position, this.DetectEvenIfStealth / 2f, Color.red);
  }
}
