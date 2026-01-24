// Decompiled with JetBrains decompiler
// Type: Particle_Chunk
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Particle_Chunk : BaseMonoBehaviour
{
  public float vz;
  public float childZ;
  public float vx;
  public float vy;
  public float Speed;
  public float Scale;
  public float FacingAngle;
  public GameObject child;
  public SpriteRenderer blood;
  public float BloodTimer;
  public float Timer;
  public bool scaleObjectOut = true;

  public void Start()
  {
    this.Scale = 2f;
    this.vz = UnityEngine.Random.Range(-0.15f, -0.1f);
    this.childZ = UnityEngine.Random.Range(-0.7f, -0.3f);
    this.Speed = (float) UnityEngine.Random.Range(3, 5);
    this.Timer = 0.0f;
  }

  public static void AddNew(
    Vector3 position,
    float FacingAngle,
    List<Sprite> frames = null,
    int frame = -1,
    bool scaleObjectOut = true)
  {
    Particle_Chunk particleChunk = BiomeConstants.Instance.SpawnParticleChunk(position);
    particleChunk.FacingAngle = FacingAngle;
    particleChunk.scaleObjectOut = scaleObjectOut;
    if (frames == null)
      return;
    particleChunk.gameObject.GetComponentInChildren<RandomFrame>().frames = frames;
    particleChunk.gameObject.GetComponentInChildren<RandomFrame>().frame = frame;
    particleChunk.gameObject.GetComponentInChildren<RandomFrame>().Start();
    particleChunk.Start();
  }

  public static void AddNew(
    Vector3 position,
    float FacingAngle,
    Color color,
    List<Sprite> frames = null,
    int frame = -1,
    bool scaleObjectOut = true)
  {
    Particle_Chunk particleChunk = BiomeConstants.Instance.SpawnParticleChunk(position);
    particleChunk.FacingAngle = FacingAngle;
    particleChunk.scaleObjectOut = scaleObjectOut;
    particleChunk.child.GetComponent<SpriteRenderer>().color = color;
    if (frames == null)
      return;
    particleChunk.gameObject.GetComponentInChildren<RandomFrame>().frames = frames;
    particleChunk.gameObject.GetComponentInChildren<RandomFrame>().frame = frame;
    particleChunk.Start();
  }

  public static void AddNewMat(
    Vector3 position,
    float FacingAngle,
    List<Sprite> frames = null,
    int frame = -1,
    Material mat = null,
    bool scaleObjectOut = true)
  {
    Particle_Chunk particleChunk = BiomeConstants.Instance.SpawnParticleChunk(position);
    particleChunk.FacingAngle = FacingAngle;
    particleChunk.FacingAngle = FacingAngle;
    if (frames == null)
      return;
    particleChunk.gameObject.GetComponentInChildren<RandomFrame>().mat = mat;
    particleChunk.gameObject.GetComponentInChildren<RandomFrame>().frames = frames;
    particleChunk.gameObject.GetComponentInChildren<RandomFrame>().frame = frame;
    particleChunk.Start();
  }

  public void FixedUpdate()
  {
    this.BounceChild();
    if (this.scaleObjectOut)
    {
      if ((double) (this.Timer += Time.fixedDeltaTime) > 1.0)
      {
        if ((double) (this.Scale -= (float) (0.10000000149011612 * (double) Time.fixedDeltaTime * 60.0)) <= 0.0)
          this.gameObject.Recycle();
      }
      else
        this.Scale += (float) ((1.0 - (double) this.Scale) / 4.0);
    }
    else if ((double) (this.Timer += Time.fixedDeltaTime) > 5.0)
      this.Speed += (float) ((0.0 - (double) this.Speed) / 50.0 / ((double) Time.fixedDeltaTime * 60.0));
    else
      this.Speed = 0.0f;
    this.child.transform.localScale = new Vector3(this.Scale, this.Scale, this.Scale);
    this.child.transform.Rotate(new Vector3(0.0f, 0.0f, (float) ((double) this.Speed * ((double) Time.fixedDeltaTime * 60.0) * 6.0 * ((double) this.FacingAngle >= 90.0 || (double) this.FacingAngle <= -90.0 ? 1.0 : -1.0))));
  }

  public void Update()
  {
    this.vx = this.Speed * Mathf.Cos(this.FacingAngle * ((float) Math.PI / 180f));
    this.vy = this.Speed * Mathf.Sin(this.FacingAngle * ((float) Math.PI / 180f));
    this.transform.position = this.transform.position + new Vector3(this.vx, this.vy) * Time.deltaTime;
  }

  public void BounceChild()
  {
    if ((double) this.childZ >= 0.0)
    {
      if ((double) this.vz > 0.05000000074505806)
      {
        this.Speed *= 0.7f;
        this.vz *= -0.6f;
      }
      else
        this.vz = 0.0f;
      this.childZ = 0.0f;
    }
    else
      this.vz += (float) (0.0099999997764825821 * (double) Time.fixedDeltaTime * 60.0);
    this.childZ += (float) ((double) this.vz * (double) Time.fixedDeltaTime * 60.0);
    this.child.transform.localPosition = new Vector3(0.0f, 0.0f, this.childZ);
  }
}
