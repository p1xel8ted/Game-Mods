// Decompiled with JetBrains decompiler
// Type: Pathfinding.RichAI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.RVO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Pathfinding;

[RequireComponent(typeof (Seeker))]
[AddComponentMenu("Pathfinding/AI/RichAI (3D, for navmesh)")]
[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_rich_a_i.php")]
public class RichAI : MonoBehaviour
{
  public Transform target;
  public bool drawGizmos = true;
  public bool repeatedlySearchPaths;
  public float repathRate = 0.5f;
  public float maxSpeed = 1f;
  public float acceleration = 5f;
  public float slowdownTime = 0.5f;
  public float rotationSpeed = 360f;
  public float endReachedDistance = 0.01f;
  public float wallForce = 3f;
  public float wallDist = 1f;
  public Vector3 gravity = new Vector3(0.0f, -9.82f, 0.0f);
  public bool raycastingForGroundPlacement = true;
  public LayerMask groundMask = (LayerMask) -1;
  public float centerOffset = 1f;
  public RichFunnel.FunnelSimplification funnelSimplification;
  public UnityEngine.Animation anim;
  public bool preciseSlowdown = true;
  public bool slowWhenNotFacingTarget = true;
  public Vector3 velocity;
  public RichPath rp;
  public Seeker seeker;
  public Transform tr;
  public CharacterController controller;
  public RVOController rvoController;
  public Vector3 lastTargetPoint;
  public Vector3 currentTargetDirection;
  public bool waitingForPathCalc;
  public bool canSearchPath;
  public bool delayUpdatePath;
  public bool traversingSpecialPath;
  public bool lastCorner;
  public float distanceToWaypoint = 999f;
  public List<Vector3> buffer = new List<Vector3>();
  public List<Vector3> wallBuffer = new List<Vector3>();
  public bool startHasRun;
  public float lastRepath = -9999f;
  public static float deltaTime;
  public static Color GizmoColorRaycast = new Color(0.4627451f, 0.807843149f, 0.4392157f);
  public static Color GizmoColorPath = new Color(0.03137255f, 0.305882365f, 0.7607843f);

  public Vector3 Velocity => this.velocity;

  public void Awake()
  {
    this.seeker = this.GetComponent<Seeker>();
    this.controller = this.GetComponent<CharacterController>();
    this.rvoController = this.GetComponent<RVOController>();
    if ((UnityEngine.Object) this.rvoController != (UnityEngine.Object) null)
      this.rvoController.enableRotation = false;
    this.tr = this.transform;
  }

  public virtual void Start()
  {
    this.startHasRun = true;
    this.OnEnable();
  }

  public virtual void OnEnable()
  {
    this.lastRepath = -9999f;
    this.waitingForPathCalc = false;
    this.canSearchPath = true;
    if (!this.startHasRun)
      return;
    this.seeker.pathCallback += new OnPathDelegate(this.OnPathComplete);
    this.StartCoroutine(this.SearchPaths());
  }

  public void OnDisable()
  {
    if ((UnityEngine.Object) this.seeker != (UnityEngine.Object) null && !this.seeker.IsDone())
      this.seeker.GetCurrentPath().Error();
    this.seeker.pathCallback -= new OnPathDelegate(this.OnPathComplete);
  }

  public virtual void UpdatePath()
  {
    this.canSearchPath = true;
    this.waitingForPathCalc = false;
    Path currentPath = this.seeker.GetCurrentPath();
    if (currentPath != null && !this.seeker.IsDone())
    {
      currentPath.Error();
      currentPath.Claim((object) this);
      currentPath.Release((object) this);
    }
    this.waitingForPathCalc = true;
    this.lastRepath = Time.time;
    this.seeker.StartPath(this.tr.position, this.target.position);
  }

  public IEnumerator SearchPaths()
  {
    while (true)
    {
      while (!this.repeatedlySearchPaths || this.waitingForPathCalc || !this.canSearchPath || (double) Time.time - (double) this.lastRepath < (double) this.repathRate)
        yield return (object) null;
      this.UpdatePath();
      yield return (object) null;
    }
  }

  public void OnPathComplete(Path p)
  {
    this.waitingForPathCalc = false;
    p.Claim((object) this);
    if (p.error)
    {
      p.Release((object) this);
    }
    else
    {
      if (this.traversingSpecialPath)
      {
        this.delayUpdatePath = true;
      }
      else
      {
        if (this.rp == null)
          this.rp = new RichPath();
        this.rp.Initialize(this.seeker, p, true, this.funnelSimplification);
      }
      p.Release((object) this);
    }
  }

  public bool TraversingSpecial => this.traversingSpecialPath;

  public Vector3 TargetPoint => this.lastTargetPoint;

  public bool ApproachingPartEndpoint => this.lastCorner;

  public bool ApproachingPathEndpoint
  {
    get => this.rp != null && this.ApproachingPartEndpoint && !this.rp.PartsLeft();
  }

  public float DistanceToNextWaypoint => this.distanceToWaypoint;

  public void NextPart()
  {
    this.rp.NextPart();
    this.lastCorner = false;
    if (this.rp.PartsLeft())
      return;
    this.OnTargetReached();
  }

  public virtual void OnTargetReached()
  {
  }

  public virtual Vector3 UpdateTarget(RichFunnel fn)
  {
    this.buffer.Clear();
    Vector3 position = this.tr.position;
    bool requiresRepath;
    Vector3 vector3 = fn.Update(position, this.buffer, 2, out this.lastCorner, out requiresRepath);
    if (requiresRepath && !this.waitingForPathCalc)
      this.UpdatePath();
    return vector3;
  }

  public virtual void Update()
  {
    RichAI.deltaTime = Mathf.Min(Time.smoothDeltaTime * 2f, Time.deltaTime);
    if (this.rp != null)
    {
      RichPathPart currentPart = this.rp.GetCurrentPart();
      if (currentPart is RichFunnel fn)
      {
        Vector3 vector3_1 = this.UpdateTarget(fn);
        if (Time.frameCount % 5 == 0 && (double) this.wallForce > 0.0 && (double) this.wallDist > 0.0)
        {
          this.wallBuffer.Clear();
          fn.FindWalls(this.wallBuffer, this.wallDist);
        }
        int index1 = 0;
        Vector3 vector3_2 = this.buffer[index1];
        if ((double) Vector3.Dot((vector3_2 - vector3_1) with
        {
          y = 0.0f
        }, this.currentTargetDirection) < 0.0 && this.buffer.Count - index1 > 1)
        {
          ++index1;
          vector3_2 = this.buffer[index1];
        }
        if (vector3_2 != this.lastTargetPoint)
        {
          this.currentTargetDirection = vector3_2 - vector3_1;
          this.currentTargetDirection.y = 0.0f;
          this.currentTargetDirection.Normalize();
          this.lastTargetPoint = vector3_2;
        }
        Vector3 vector3_3 = (vector3_2 - vector3_1) with
        {
          y = 0.0f
        };
        float magnitude = vector3_3.magnitude;
        this.distanceToWaypoint = magnitude;
        Vector3 vector3_4 = (double) magnitude == 0.0 ? Vector3.zero : vector3_3 / magnitude;
        Vector3 lhs = vector3_4;
        Vector3 vector3_5 = Vector3.zero;
        if ((double) this.wallForce > 0.0 && (double) this.wallDist > 0.0)
        {
          float val1_1 = 0.0f;
          float val1_2 = 0.0f;
          for (int index2 = 0; index2 < this.wallBuffer.Count; index2 += 2)
          {
            Vector3 vector3_6 = VectorMath.ClosestPointOnSegment(this.wallBuffer[index2], this.wallBuffer[index2 + 1], this.tr.position) - vector3_1;
            float sqrMagnitude = vector3_6.sqrMagnitude;
            if ((double) sqrMagnitude <= (double) this.wallDist * (double) this.wallDist)
            {
              vector3_6 = this.wallBuffer[index2 + 1] - this.wallBuffer[index2];
              Vector3 normalized = vector3_6.normalized;
              float val2 = Vector3.Dot(vector3_4, normalized) * (1f - Math.Max(0.0f, (float) (2.0 * ((double) sqrMagnitude / ((double) this.wallDist * (double) this.wallDist)) - 1.0)));
              if ((double) val2 > 0.0)
                val1_2 = Math.Max(val1_2, val2);
              else
                val1_1 = Math.Max(val1_1, -val2);
            }
          }
          vector3_5 = Vector3.Cross(Vector3.up, vector3_4) * (val1_2 - val1_1);
        }
        bool flag = this.lastCorner && this.buffer.Count - index1 == 1;
        Vector3 vector3_7;
        if (flag)
        {
          if ((double) this.slowdownTime < 1.0 / 1000.0)
            this.slowdownTime = 1f / 1000f;
          Vector3 vector3_8 = (vector3_2 - vector3_1) with
          {
            y = 0.0f
          };
          vector3_7 = Vector3.ClampMagnitude(!this.preciseSlowdown ? 2f * (vector3_8 - this.slowdownTime * this.velocity) / (this.slowdownTime * this.slowdownTime) : (6f * vector3_8 - 4f * this.slowdownTime * this.velocity) / (this.slowdownTime * this.slowdownTime), this.acceleration);
          vector3_5 *= Math.Min(magnitude / 0.5f, 1f);
          if ((double) magnitude < (double) this.endReachedDistance)
            this.NextPart();
        }
        else
          vector3_7 = vector3_4 * this.acceleration;
        this.velocity += (vector3_7 + vector3_5 * this.wallForce) * RichAI.deltaTime;
        if (this.slowWhenNotFacingTarget)
        {
          float a1 = (float) (((double) Vector3.Dot(lhs, this.tr.forward) + 0.5) * 0.66666668653488159);
          double a2 = (double) Mathf.Sqrt((float) ((double) this.velocity.x * (double) this.velocity.x + (double) this.velocity.z * (double) this.velocity.z));
          float y = this.velocity.y;
          this.velocity.y = 0.0f;
          double b = (double) this.maxSpeed * (double) Mathf.Max(a1, 0.2f);
          float num = Mathf.Min((float) a2, (float) b);
          this.velocity = Vector3.Lerp(this.tr.forward * num, this.velocity.normalized * num, Mathf.Clamp(flag ? magnitude * 2f : 0.0f, 0.5f, 1f));
          this.velocity.y = y;
        }
        else
        {
          float num = this.maxSpeed / Mathf.Sqrt((float) ((double) this.velocity.x * (double) this.velocity.x + (double) this.velocity.z * (double) this.velocity.z));
          if ((double) num < 1.0)
          {
            this.velocity.x *= num;
            this.velocity.z *= num;
          }
        }
        if (flag)
          this.RotateTowards(Vector3.Lerp(this.velocity, this.currentTargetDirection, Math.Max((float) (1.0 - (double) magnitude * 2.0), 0.0f)));
        else
          this.RotateTowards(this.velocity);
        this.velocity += RichAI.deltaTime * this.gravity;
        if ((UnityEngine.Object) this.rvoController != (UnityEngine.Object) null && this.rvoController.enabled)
        {
          this.tr.position = vector3_1;
          this.rvoController.Move(this.velocity);
        }
        else if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null && this.controller.enabled)
        {
          this.tr.position = vector3_1;
          int num = (int) this.controller.Move(this.velocity * RichAI.deltaTime);
        }
        else
        {
          float y = vector3_1.y;
          this.tr.position = this.RaycastPosition(vector3_1 + this.velocity * RichAI.deltaTime, y);
        }
      }
      else if ((UnityEngine.Object) this.rvoController != (UnityEngine.Object) null && this.rvoController.enabled)
        this.rvoController.Move(Vector3.zero);
      if (!(currentPart is RichSpecial) || this.traversingSpecialPath)
        return;
      this.StartCoroutine(this.TraverseSpecial(currentPart as RichSpecial));
    }
    else if ((UnityEngine.Object) this.rvoController != (UnityEngine.Object) null && this.rvoController.enabled)
    {
      this.rvoController.Move(Vector3.zero);
    }
    else
    {
      if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null && this.controller.enabled)
        return;
      this.tr.position = this.RaycastPosition(this.tr.position, this.tr.position.y);
    }
  }

  public Vector3 RaycastPosition(Vector3 position, float lasty)
  {
    if (this.raycastingForGroundPlacement)
    {
      float maxDistance = Mathf.Max(this.centerOffset, lasty - position.y + this.centerOffset);
      RaycastHit hitInfo;
      if (Physics.Raycast(position + Vector3.up * maxDistance, Vector3.down, out hitInfo, maxDistance, (int) this.groundMask) && (double) hitInfo.distance < (double) maxDistance)
      {
        position = hitInfo.point;
        this.velocity.y = 0.0f;
      }
    }
    return position;
  }

  public bool RotateTowards(Vector3 trotdir)
  {
    trotdir.y = 0.0f;
    if (!(trotdir != Vector3.zero))
      return false;
    Quaternion rotation = this.tr.rotation;
    Vector3 eulerAngles1 = Quaternion.LookRotation(trotdir).eulerAngles;
    Vector3 eulerAngles2 = rotation.eulerAngles;
    eulerAngles2.y = Mathf.MoveTowardsAngle(eulerAngles2.y, eulerAngles1.y, this.rotationSpeed * RichAI.deltaTime);
    this.tr.rotation = Quaternion.Euler(eulerAngles2);
    return (double) Mathf.Abs(eulerAngles2.y - eulerAngles1.y) < 5.0;
  }

  public void OnDrawGizmos()
  {
    if (!this.drawGizmos)
      return;
    if (this.raycastingForGroundPlacement)
    {
      Gizmos.color = RichAI.GizmoColorRaycast;
      Gizmos.DrawLine(this.transform.position, this.transform.position + Vector3.up * this.centerOffset);
      Gizmos.DrawLine(this.transform.position + Vector3.left * 0.1f, this.transform.position + Vector3.right * 0.1f);
      Gizmos.DrawLine(this.transform.position + Vector3.back * 0.1f, this.transform.position + Vector3.forward * 0.1f);
    }
    if (!((UnityEngine.Object) this.tr != (UnityEngine.Object) null) || this.buffer == null)
      return;
    Gizmos.color = RichAI.GizmoColorPath;
    Vector3 position = this.tr.position;
    for (int index = 0; index < this.buffer.Count; ++index)
    {
      Gizmos.DrawLine(position, this.buffer[index]);
      position = this.buffer[index];
    }
  }

  public IEnumerator TraverseSpecial(RichSpecial rs)
  {
    this.traversingSpecialPath = true;
    this.velocity = Vector3.zero;
    AnimationLink al = rs.nodeLink as AnimationLink;
    if ((UnityEngine.Object) al == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Unhandled RichSpecial");
    }
    else
    {
      while (!this.RotateTowards(rs.first.forward))
        yield return (object) null;
      this.tr.parent.position = this.tr.position;
      this.tr.parent.rotation = this.tr.rotation;
      this.tr.localPosition = Vector3.zero;
      this.tr.localRotation = Quaternion.identity;
      if (rs.reverse && al.reverseAnim)
      {
        this.anim[al.clip].speed = -al.animSpeed;
        this.anim[al.clip].normalizedTime = 1f;
        this.anim.Play(al.clip);
        this.anim.Sample();
      }
      else
      {
        this.anim[al.clip].speed = al.animSpeed;
        this.anim.Rewind(al.clip);
        this.anim.Play(al.clip);
      }
      this.tr.parent.position -= this.tr.position - this.tr.parent.position;
      yield return (object) new WaitForSeconds(Mathf.Abs(this.anim[al.clip].length / al.animSpeed));
      this.traversingSpecialPath = false;
      this.NextPart();
      if (this.delayUpdatePath)
      {
        this.delayUpdatePath = false;
        this.UpdatePath();
      }
    }
  }
}
