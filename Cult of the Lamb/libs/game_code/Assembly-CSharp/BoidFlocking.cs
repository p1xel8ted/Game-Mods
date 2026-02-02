// Decompiled with JetBrains decompiler
// Type: BoidFlocking
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BoidFlocking : BaseMonoBehaviour
{
  public static List<BoidFlocking> Boids = new List<BoidFlocking>();
  public float LoopingLimit = 6f;
  public bool EnableMovement;
  public float MoveSpeed = 1f;
  public Vector3 CurrentDirection;
  public Vector3 Velocity;
  public bool EnableAlignment;
  public float AlignmentDistance = 1f;
  public Vector3 AlignmentDirection;
  public bool EnableAvoidance;
  public float AvoidanceDistance = 1f;
  public Vector3 AvoidanceDirection;
  public bool EnableCohesion;
  public float CohesionDistance = 1f;
  public Vector3 CohesionDirection;
  public GameObject AttractionObject;
  public Vector3 AttractionDirection;
  public float Distance;

  public void OnEnable() => BoidFlocking.Boids.Add(this);

  public void OnDisable() => BoidFlocking.Boids.Remove(this);

  public void Update()
  {
    this.AlignmentDirection = Vector3.zero;
    int num1 = 0;
    if (this.EnableAlignment)
    {
      foreach (BoidFlocking boid in BoidFlocking.Boids)
      {
        this.Distance = Vector3.Distance(boid.transform.position, this.transform.position);
        if ((Object) boid != (Object) this && (double) this.Distance < (double) this.AlignmentDistance)
        {
          this.AlignmentDirection += boid.Velocity;
          ++num1;
        }
      }
    }
    if (num1 > 0)
    {
      this.AlignmentDirection /= (float) num1;
      this.AlignmentDirection.Normalize();
    }
    this.CohesionDirection = Vector3.zero;
    int num2 = 0;
    if (this.EnableCohesion)
    {
      foreach (BoidFlocking boid in BoidFlocking.Boids)
      {
        this.Distance = Vector3.Distance(boid.transform.position, this.transform.position);
        if ((Object) boid != (Object) this && (double) this.Distance < (double) this.CohesionDistance)
        {
          this.CohesionDirection += boid.transform.position;
          ++num2;
        }
      }
    }
    if (num2 > 0)
    {
      this.CohesionDirection /= (float) num2;
      this.CohesionDirection -= this.transform.position;
      this.CohesionDirection.Normalize();
    }
    this.AvoidanceDirection = Vector3.zero;
    int num3 = 0;
    if (this.EnableAvoidance)
    {
      foreach (BoidFlocking boid in BoidFlocking.Boids)
      {
        this.Distance = Vector3.Distance(boid.transform.position, this.transform.position);
        if ((Object) boid != (Object) this && (double) this.Distance < (double) this.AvoidanceDistance)
        {
          this.AvoidanceDirection += boid.transform.position;
          ++num3;
        }
      }
    }
    if (num3 > 0)
    {
      this.AvoidanceDirection /= (float) num3;
      this.AvoidanceDirection -= this.transform.position;
      this.AvoidanceDirection *= -1f;
      this.AvoidanceDirection.Normalize();
    }
    this.CurrentDirection += this.CohesionDirection + this.AvoidanceDirection + this.AlignmentDirection;
    this.Velocity = this.CurrentDirection;
    this.CurrentDirection.Normalize();
    if (this.EnableMovement)
      this.transform.position += this.CurrentDirection * this.MoveSpeed * Time.deltaTime;
    if ((double) Vector3.Distance(Vector3.zero, this.transform.position) <= (double) this.LoopingLimit)
      return;
    this.transform.position -= this.transform.position * 2f;
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, this.AvoidanceDistance, Color.white);
    if (this.EnableAvoidance)
      Utils.DrawCircleXY(this.transform.position + this.AvoidanceDirection, 0.1f, Color.red);
    if (this.EnableCohesion)
      Utils.DrawCircleXY(this.transform.position + this.CohesionDirection, 0.1f, Color.blue);
    if (this.EnableAlignment)
      Utils.DrawCircleXY(this.transform.position + this.AlignmentDirection, 0.1f, Color.yellow);
    Utils.DrawCircleXY(this.transform.position + this.CurrentDirection, 0.1f, Color.green);
  }
}
