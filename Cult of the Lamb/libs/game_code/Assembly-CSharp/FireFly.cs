// Decompiled with JetBrains decompiler
// Type: FireFly
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class FireFly : BaseMonoBehaviour
{
  public Vector3 InitialPosition;
  public float MovementRange = 5f;
  public float VerticalRange = 2f;
  public Vector3 Destination;
  public Vector3 Velocity;
  public float MaxSpeed = 5f;
  public float AvoidSpeed = 20f;
  public float AvoidDistance = 10f;
  public float Decay = 0.9f;
  public Gradient gradient;
  public SpriteRenderer Sprite;
  public float ColorDistance = 2f;

  public void Start()
  {
    this.InitialPosition = new Vector3(this.transform.position.x, this.transform.position.y, 0.0f);
    this.GetNewDestination();
  }

  public void Update()
  {
    this.Velocity += (this.Destination - this.transform.position) / this.MaxSpeed;
    if ((double) Vector3.Distance(this.transform.position, this.Destination) < 0.5)
      this.GetNewDestination();
    foreach (Health allUnit in Health.allUnits)
    {
      if (allUnit.team == Health.Team.PlayerTeam && (double) Vector2.Distance((Vector2) this.transform.position, (Vector2) allUnit.transform.position) < 2.0)
      {
        float f = Utils.GetAngle(allUnit.transform.position, this.transform.position) * ((float) Math.PI / 180f);
        this.Velocity += new Vector3(this.AvoidSpeed * Mathf.Cos(f), this.AvoidSpeed * Mathf.Sin(f), 0.0f);
        this.Destination = this.transform.position + new Vector3(this.AvoidSpeed * Mathf.Cos(f), this.AvoidSpeed * Mathf.Sin(f), 0.0f);
      }
    }
    this.Velocity *= this.Decay;
    this.transform.position += this.Velocity * Time.deltaTime;
    this.transform.forward = Camera.main.transform.forward;
    this.Sprite.color = this.gradient.Evaluate(Vector2.Distance((Vector2) this.transform.position, (Vector2) Vector3.zero) / this.ColorDistance);
  }

  public void GetNewDestination()
  {
    this.Destination = this.InitialPosition + new Vector3(UnityEngine.Random.Range(-this.MovementRange, this.MovementRange), UnityEngine.Random.Range(-this.MovementRange, this.MovementRange), -UnityEngine.Random.Range(0.5f, this.VerticalRange));
  }
}
