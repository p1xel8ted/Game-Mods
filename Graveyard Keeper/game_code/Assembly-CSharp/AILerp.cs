// Decompiled with JetBrains decompiler
// Type: AILerp
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
[HelpURL("http://arongranberg.com/astar/docs/class_a_i_lerp.php")]
[AddComponentMenu("Pathfinding/AI/AISimpleLerp (2D,3D generic)")]
[RequireComponent(typeof (Seeker))]
public class AILerp : MonoBehaviour
{
  public float repathRate = 0.5f;
  public Transform target;
  public bool canSearch = true;
  public bool canMove = true;
  public float speed = 3f;
  public bool enableRotation = true;
  public bool rotationIn2D;
  public float rotationSpeed = 10f;
  public bool interpolatePathSwitches = true;
  public float switchPathInterpolationSpeed = 5f;
  public Seeker seeker;
  public Transform tr;
  public float lastRepath = -9999f;
  public ABPath path;
  public int currentWaypointIndex;
  public float distanceAlongSegment;
  [CompilerGenerated]
  public bool \u003CtargetReached\u003Ek__BackingField;
  public bool canSearchAgain = true;
  public Vector3 previousMovementOrigin;
  public Vector3 previousMovementDirection;
  public float previousMovementStartTime = -9999f;
  public bool startHasRun;

  public bool targetReached
  {
    get => this.\u003CtargetReached\u003Ek__BackingField;
    set => this.\u003CtargetReached\u003Ek__BackingField = value;
  }

  public virtual void Awake()
  {
    this.tr = this.transform;
    this.seeker = this.GetComponent<Seeker>();
    this.seeker.startEndModifier.adjustStartPoint = (Func<Vector3>) (() => this.tr.position);
  }

  public virtual void Start()
  {
    this.startHasRun = true;
    this.OnEnable();
  }

  public virtual void OnEnable()
  {
    this.lastRepath = -9999f;
    this.canSearchAgain = true;
    if (!this.startHasRun)
      return;
    this.seeker.pathCallback += new OnPathDelegate(this.OnPathComplete);
    this.StartCoroutine(this.RepeatTrySearchPath());
  }

  public void OnDisable()
  {
    if ((UnityEngine.Object) this.seeker != (UnityEngine.Object) null && !this.seeker.IsDone())
      this.seeker.GetCurrentPath().Error();
    if (this.path != null)
      this.path.Release((object) this);
    this.path = (ABPath) null;
    this.seeker.pathCallback -= new OnPathDelegate(this.OnPathComplete);
  }

  public IEnumerator RepeatTrySearchPath()
  {
    while (true)
      yield return (object) new WaitForSeconds(this.TrySearchPath());
  }

  public float TrySearchPath()
  {
    if ((double) Time.time - (double) this.lastRepath >= (double) this.repathRate && this.canSearchAgain && this.canSearch && (UnityEngine.Object) this.target != (UnityEngine.Object) null)
    {
      this.SearchPath();
      return this.repathRate;
    }
    float num = this.repathRate - (Time.time - this.lastRepath);
    return (double) num >= 0.0 ? num : 0.0f;
  }

  public virtual void SearchPath() => this.ForceSearchPath();

  public virtual void ForceSearchPath()
  {
    if ((UnityEngine.Object) this.target == (UnityEngine.Object) null)
      throw new InvalidOperationException("Target is null");
    this.lastRepath = Time.time;
    Vector3 position = this.target.position;
    Vector3 feetPosition = this.GetFeetPosition();
    if (this.path != null && this.path.vectorPath.Count > 1)
      feetPosition = this.path.vectorPath[this.currentWaypointIndex];
    this.canSearchAgain = false;
    this.seeker.StartPath(feetPosition, position);
  }

  public virtual void OnTargetReached()
  {
  }

  public virtual void OnPathComplete(Path _p)
  {
    if (!(_p is ABPath abPath))
      throw new Exception("This function only handles ABPaths, do not use special path types");
    this.canSearchAgain = true;
    abPath.Claim((object) this);
    if (abPath.error)
    {
      abPath.Release((object) this);
    }
    else
    {
      if (this.interpolatePathSwitches)
        this.ConfigurePathSwitchInterpolation();
      if (this.path != null)
        this.path.Release((object) this);
      this.path = abPath;
      if (this.path.vectorPath != null && this.path.vectorPath.Count == 1)
        this.path.vectorPath.Insert(0, this.GetFeetPosition());
      this.targetReached = false;
      this.ConfigureNewPath();
    }
  }

  public virtual void ConfigurePathSwitchInterpolation()
  {
    int num1 = this.path == null || this.path.vectorPath == null ? 0 : (this.path.vectorPath.Count > 1 ? 1 : 0);
    bool flag = false;
    if (num1 != 0)
      flag = this.currentWaypointIndex == this.path.vectorPath.Count - 1 && (double) this.distanceAlongSegment >= (double) (this.path.vectorPath[this.path.vectorPath.Count - 1] - this.path.vectorPath[this.path.vectorPath.Count - 2]).magnitude;
    if (num1 != 0 && !flag)
    {
      List<Vector3> vectorPath = this.path.vectorPath;
      this.currentWaypointIndex = Mathf.Clamp(this.currentWaypointIndex, 1, vectorPath.Count - 1);
      Vector3 vector3 = vectorPath[this.currentWaypointIndex] - vectorPath[this.currentWaypointIndex - 1];
      float num2 = vector3.magnitude * Mathf.Clamp01(1f - this.distanceAlongSegment);
      for (int currentWaypointIndex = this.currentWaypointIndex; currentWaypointIndex < vectorPath.Count - 1; ++currentWaypointIndex)
        num2 += (vectorPath[currentWaypointIndex + 1] - vectorPath[currentWaypointIndex]).magnitude;
      this.previousMovementOrigin = this.GetFeetPosition();
      this.previousMovementDirection = vector3.normalized * num2;
      this.previousMovementStartTime = Time.time;
    }
    else
    {
      this.previousMovementOrigin = Vector3.zero;
      this.previousMovementDirection = Vector3.zero;
      this.previousMovementStartTime = -9999f;
    }
  }

  public virtual Vector3 GetFeetPosition() => this.tr.position;

  public virtual void ConfigureNewPath()
  {
    List<Vector3> vectorPath = this.path.vectorPath;
    Vector3 feetPosition = this.GetFeetPosition();
    float num1 = 0.0f;
    float num2 = float.PositiveInfinity;
    Vector3 vector3_1 = Vector3.zero;
    int num3 = 1;
    for (int index = 0; index < vectorPath.Count - 1; ++index)
    {
      float t = Mathf.Clamp01(VectorMath.ClosestPointOnLineFactor(vectorPath[index], vectorPath[index + 1], feetPosition));
      Vector3 vector3_2 = Vector3.Lerp(vectorPath[index], vectorPath[index + 1], t);
      float sqrMagnitude = (feetPosition - vector3_2).sqrMagnitude;
      if ((double) sqrMagnitude < (double) num2)
      {
        num2 = sqrMagnitude;
        vector3_1 = vectorPath[index + 1] - vectorPath[index];
        num1 = t * vector3_1.magnitude;
        num3 = index + 1;
      }
    }
    this.currentWaypointIndex = num3;
    this.distanceAlongSegment = num1;
    if (!this.interpolatePathSwitches || (double) this.switchPathInterpolationSpeed <= 0.0099999997764825821)
      return;
    this.distanceAlongSegment -= (float) ((double) this.speed * (double) Mathf.Max(-Vector3.Dot(this.previousMovementDirection.normalized, vector3_1.normalized), 0.0f) * (1.0 / (double) this.switchPathInterpolationSpeed));
  }

  public virtual void Update()
  {
    if (!this.canMove)
      return;
    Vector3 direction;
    Vector3 nextPosition = this.CalculateNextPosition(out direction);
    if (this.enableRotation && direction != Vector3.zero)
    {
      if (this.rotationIn2D)
      {
        float b = (float) ((double) Mathf.Atan2(direction.x, -direction.y) * 57.295780181884766 + 180.0);
        Vector3 eulerAngles = this.tr.eulerAngles;
        eulerAngles.z = Mathf.LerpAngle(eulerAngles.z, b, Time.deltaTime * this.rotationSpeed);
        this.tr.eulerAngles = eulerAngles;
      }
      else
        this.tr.rotation = Quaternion.Slerp(this.tr.rotation, Quaternion.LookRotation(direction), Time.deltaTime * this.rotationSpeed);
    }
    this.tr.position = nextPosition;
  }

  public virtual Vector3 CalculateNextPosition(out Vector3 direction)
  {
    if (this.path == null || this.path.vectorPath == null || this.path.vectorPath.Count == 0)
    {
      direction = Vector3.zero;
      return this.tr.position;
    }
    List<Vector3> vectorPath = this.path.vectorPath;
    this.currentWaypointIndex = Mathf.Clamp(this.currentWaypointIndex, 1, vectorPath.Count - 1);
    Vector3 vector3_1 = vectorPath[this.currentWaypointIndex] - vectorPath[this.currentWaypointIndex - 1];
    float num1 = vector3_1.magnitude;
    this.distanceAlongSegment += Time.deltaTime * this.speed;
    if ((double) this.distanceAlongSegment >= (double) num1 && this.currentWaypointIndex < vectorPath.Count - 1)
    {
      float num2 = this.distanceAlongSegment - num1;
      Vector3 vector3_2;
      float magnitude;
      while (true)
      {
        ++this.currentWaypointIndex;
        vector3_2 = vectorPath[this.currentWaypointIndex] - vectorPath[this.currentWaypointIndex - 1];
        magnitude = vector3_2.magnitude;
        if ((double) num2 > (double) magnitude && this.currentWaypointIndex != vectorPath.Count - 1)
          num2 -= magnitude;
        else
          break;
      }
      vector3_1 = vector3_2;
      num1 = magnitude;
      this.distanceAlongSegment = num2;
    }
    if ((double) this.distanceAlongSegment >= (double) num1 && this.currentWaypointIndex == vectorPath.Count - 1)
    {
      if (!this.targetReached)
        this.OnTargetReached();
      this.targetReached = true;
    }
    Vector3 b = vector3_1 * Mathf.Clamp01((double) num1 > 0.0 ? this.distanceAlongSegment / num1 : 1f) + vectorPath[this.currentWaypointIndex - 1];
    direction = vector3_1;
    return this.interpolatePathSwitches ? Vector3.Lerp(this.previousMovementOrigin + Vector3.ClampMagnitude(this.previousMovementDirection, this.speed * (Time.time - this.previousMovementStartTime)), b, this.switchPathInterpolationSpeed * (Time.time - this.previousMovementStartTime)) : b;
  }

  [CompilerGenerated]
  public Vector3 \u003CAwake\u003Eb__25_0() => this.tr.position;
}
