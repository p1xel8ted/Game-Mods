// Decompiled with JetBrains decompiler
// Type: Pathfinding.GraphCollision
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Pathfinding;

[Serializable]
public class GraphCollision
{
  public ColliderType type = ColliderType.Capsule;
  public float diameter = 1f;
  public float height = 2f;
  public float collisionOffset;
  public RayDirection rayDirection = RayDirection.Both;
  public LayerMask mask;
  public LayerMask heightMask = (LayerMask) -1;
  public float fromHeight = 100f;
  public bool thickRaycast;
  public float thickRaycastDiameter = 1f;
  public bool unwalkableWhenNoGround = true;
  public bool use2D;
  public bool collisionCheck = true;
  public bool heightCheck = true;
  public Vector3 up;
  public Vector3 upheight;
  public float finalRadius;
  public float finalRaycastRadius;
  public const float RaycastErrorMargin = 0.005f;

  public void Initialize(Matrix4x4 matrix, float scale)
  {
    this.up = matrix.MultiplyVector(Vector3.up);
    this.upheight = this.up * this.height;
    this.finalRadius = (float) ((double) this.diameter * (double) scale * 0.5);
    this.finalRaycastRadius = (float) ((double) this.thickRaycastDiameter * (double) scale * 0.5);
  }

  public bool Check(Vector3 position)
  {
    if (!this.collisionCheck)
      return true;
    if (this.use2D)
    {
      switch (this.type)
      {
        case ColliderType.Sphere:
          return (UnityEngine.Object) Physics2D.OverlapCircle((Vector2) position, this.finalRadius, (int) this.mask) == (UnityEngine.Object) null;
        case ColliderType.Capsule:
          throw new Exception("Capsule mode cannot be used with 2D since capsules don't exist in 2D. Please change the Physics Testing -> Collider Type setting.");
        default:
          return (UnityEngine.Object) Physics2D.OverlapPoint((Vector2) position, (int) this.mask) == (UnityEngine.Object) null;
      }
    }
    else
    {
      position += this.up * this.collisionOffset;
      switch (this.type)
      {
        case ColliderType.Sphere:
          return !Physics.CheckSphere(position, this.finalRadius, (int) this.mask);
        case ColliderType.Capsule:
          return !Physics.CheckCapsule(position, position + this.upheight, this.finalRadius, (int) this.mask);
        default:
          switch (this.rayDirection)
          {
            case RayDirection.Up:
              return !Physics.Raycast(position, this.up, this.height, (int) this.mask);
            case RayDirection.Both:
              return !Physics.Raycast(position, this.up, this.height, (int) this.mask) && !Physics.Raycast(position + this.upheight, -this.up, this.height, (int) this.mask);
            default:
              return !Physics.Raycast(position + this.upheight, -this.up, this.height, (int) this.mask);
          }
      }
    }
  }

  public Vector3 CheckHeight(Vector3 position)
  {
    return this.CheckHeight(position, out RaycastHit _, out bool _);
  }

  public Vector3 CheckHeight(Vector3 position, out RaycastHit hit, out bool walkable)
  {
    walkable = true;
    if (!this.heightCheck || this.use2D)
    {
      hit = new RaycastHit();
      return position;
    }
    if (this.thickRaycast)
    {
      Ray ray = new Ray(position + this.up * this.fromHeight, -this.up);
      if (Physics.SphereCast(ray, this.finalRaycastRadius, out hit, this.fromHeight + 0.005f, (int) this.heightMask))
        return VectorMath.ClosestPointOnLine(ray.origin, ray.origin + ray.direction, hit.point);
      walkable &= !this.unwalkableWhenNoGround;
    }
    else
    {
      if (Physics.Raycast(position + this.up * this.fromHeight, -this.up, out hit, this.fromHeight + 0.005f, (int) this.heightMask))
        return hit.point;
      walkable &= !this.unwalkableWhenNoGround;
    }
    return position;
  }

  public Vector3 Raycast(Vector3 origin, out RaycastHit hit, out bool walkable)
  {
    walkable = true;
    if (!this.heightCheck || this.use2D)
    {
      hit = new RaycastHit();
      return origin - this.up * this.fromHeight;
    }
    if (this.thickRaycast)
    {
      Ray ray = new Ray(origin, -this.up);
      if (Physics.SphereCast(ray, this.finalRaycastRadius, out hit, this.fromHeight + 0.005f, (int) this.heightMask))
        return VectorMath.ClosestPointOnLine(ray.origin, ray.origin + ray.direction, hit.point);
      walkable &= !this.unwalkableWhenNoGround;
    }
    else
    {
      if (Physics.Raycast(origin, -this.up, out hit, this.fromHeight + 0.005f, (int) this.heightMask))
        return hit.point;
      walkable &= !this.unwalkableWhenNoGround;
    }
    return origin - this.up * this.fromHeight;
  }

  public RaycastHit[] CheckHeightAll(Vector3 position)
  {
    if (!this.heightCheck || this.use2D)
      return new RaycastHit[1]
      {
        new RaycastHit() { point = position, distance = 0.0f }
      };
    if (this.thickRaycast)
      return new RaycastHit[0];
    List<RaycastHit> raycastHitList = new List<RaycastHit>();
    Vector3 origin = position + this.up * this.fromHeight;
    Vector3 vector3 = Vector3.zero;
    int num = 0;
    do
    {
      RaycastHit hit;
      this.Raycast(origin, out hit, out bool _);
      if (!((UnityEngine.Object) hit.transform == (UnityEngine.Object) null))
      {
        if (hit.point != vector3 || raycastHitList.Count == 0)
        {
          origin = hit.point - this.up * 0.005f;
          vector3 = hit.point;
          num = 0;
          raycastHitList.Add(hit);
        }
        else
        {
          origin -= this.up * (1f / 1000f);
          ++num;
        }
      }
      else
        goto label_10;
    }
    while (num <= 10);
    Debug.LogError((object) $"Infinite Loop when raycasting. Please report this error (arongranberg.com)\n{origin.ToString()} : {vector3.ToString()}");
label_10:
    return raycastHitList.ToArray();
  }

  public void DeserializeSettingsCompatibility(GraphSerializationContext ctx)
  {
    this.type = (ColliderType) ctx.reader.ReadInt32();
    this.diameter = ctx.reader.ReadSingle();
    this.height = ctx.reader.ReadSingle();
    this.collisionOffset = ctx.reader.ReadSingle();
    this.rayDirection = (RayDirection) ctx.reader.ReadInt32();
    this.mask = (LayerMask) ctx.reader.ReadInt32();
    this.heightMask = (LayerMask) ctx.reader.ReadInt32();
    this.fromHeight = ctx.reader.ReadSingle();
    this.thickRaycast = ctx.reader.ReadBoolean();
    this.thickRaycastDiameter = ctx.reader.ReadSingle();
    this.unwalkableWhenNoGround = ctx.reader.ReadBoolean();
    this.use2D = ctx.reader.ReadBoolean();
    this.collisionCheck = ctx.reader.ReadBoolean();
    this.heightCheck = ctx.reader.ReadBoolean();
  }
}
