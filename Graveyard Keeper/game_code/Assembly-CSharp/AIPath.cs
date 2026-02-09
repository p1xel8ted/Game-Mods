// Decompiled with JetBrains decompiler
// Type: AIPath
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding;
using Pathfinding.RVO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (Seeker))]
[AddComponentMenu("Pathfinding/AI/AIPath (3D)")]
[HelpURL("http://arongranberg.com/astar/docs/class_a_i_path.php")]
public class AIPath : MonoBehaviour
{
  public float repathRate = 0.5f;
  public Transform target;
  public bool canSearch = true;
  public bool canMove = true;
  public float speed = 3f;
  public float turningSpeed = 5f;
  public float slowdownDistance = 0.6f;
  public float pickNextWaypointDist = 2f;
  public float forwardLook = 1f;
  public float endReachedDistance = 0.2f;
  public bool closestOnPathCheck = true;
  public float minMoveScale = 0.05f;
  public Seeker seeker;
  public Transform tr;
  public float lastRepath = -9999f;
  public Path path;
  public CharacterController controller;
  public RVOController rvoController;
  public Rigidbody rigid;
  public int currentWaypointIndex;
  public bool targetReached;
  public bool canSearchAgain = true;
  public Vector3 lastFoundWaypointPosition;
  public float lastFoundWaypointTime = -9999f;
  public bool startHasRun;
  public Vector3 targetPoint;
  public Vector3 targetDirection;

  public bool TargetReached => this.targetReached;

  public virtual void Awake()
  {
    this.seeker = this.GetComponent<Seeker>();
    this.tr = this.transform;
    this.controller = this.GetComponent<CharacterController>();
    this.rvoController = this.GetComponent<RVOController>();
    if ((UnityEngine.Object) this.rvoController != (UnityEngine.Object) null)
      this.rvoController.enableRotation = false;
    this.rigid = this.GetComponent<Rigidbody>();
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
    this.lastFoundWaypointPosition = this.GetFeetPosition();
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
    this.path = (Path) null;
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

  public virtual void SearchPath()
  {
    if ((UnityEngine.Object) this.target == (UnityEngine.Object) null)
      throw new InvalidOperationException("Target is null");
    this.lastRepath = Time.time;
    Vector3 position = this.target.position;
    this.canSearchAgain = false;
    this.seeker.StartPath(this.GetFeetPosition(), position);
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
      if (this.path != null)
        this.path.Release((object) this);
      this.path = (Path) abPath;
      this.currentWaypointIndex = 0;
      this.targetReached = false;
      if (!this.closestOnPathCheck)
        return;
      Vector3 currentPosition = (double) Time.time - (double) this.lastFoundWaypointTime < 0.30000001192092896 ? this.lastFoundWaypointPosition : abPath.originalStartPoint;
      Vector3 vector3_1 = this.GetFeetPosition() - currentPosition;
      float magnitude = vector3_1.magnitude;
      Vector3 vector3_2 = vector3_1 / magnitude;
      int num = (int) ((double) magnitude / (double) this.pickNextWaypointDist);
      for (int index = 0; index <= num; ++index)
      {
        this.CalculateVelocity(currentPosition);
        currentPosition += vector3_2;
      }
    }
  }

  public virtual Vector3 GetFeetPosition()
  {
    if ((UnityEngine.Object) this.rvoController != (UnityEngine.Object) null)
      return this.tr.position - Vector3.up * this.rvoController.height * 0.5f;
    return (UnityEngine.Object) this.controller != (UnityEngine.Object) null ? this.tr.position - Vector3.up * this.controller.height * 0.5f : this.tr.position;
  }

  public virtual void Update()
  {
    if (!this.canMove)
      return;
    Vector3 velocity = this.CalculateVelocity(this.GetFeetPosition());
    this.RotateTowards(this.targetDirection);
    if ((UnityEngine.Object) this.rvoController != (UnityEngine.Object) null)
      this.rvoController.Move(velocity);
    else if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null)
      this.controller.SimpleMove(velocity);
    else if ((UnityEngine.Object) this.rigid != (UnityEngine.Object) null)
      this.rigid.AddForce(velocity);
    else
      this.tr.Translate(velocity * Time.deltaTime, Space.World);
  }

  public float XZSqrMagnitude(Vector3 a, Vector3 b)
  {
    double num1 = (double) b.x - (double) a.x;
    float num2 = b.z - a.z;
    return (float) (num1 * num1 + (double) num2 * (double) num2);
  }

  public Vector3 CalculateVelocity(Vector3 currentPosition)
  {
    if (this.path == null || this.path.vectorPath == null || this.path.vectorPath.Count == 0)
      return Vector3.zero;
    List<Vector3> vectorPath = this.path.vectorPath;
    if (vectorPath.Count == 1)
      vectorPath.Insert(0, currentPosition);
    if (this.currentWaypointIndex >= vectorPath.Count)
      this.currentWaypointIndex = vectorPath.Count - 1;
    if (this.currentWaypointIndex <= 1)
      this.currentWaypointIndex = 1;
    for (; this.currentWaypointIndex < vectorPath.Count - 1 && (double) this.XZSqrMagnitude(vectorPath[this.currentWaypointIndex], currentPosition) < (double) this.pickNextWaypointDist * (double) this.pickNextWaypointDist; ++this.currentWaypointIndex)
    {
      this.lastFoundWaypointPosition = currentPosition;
      this.lastFoundWaypointTime = Time.time;
    }
    Vector3 vector3_1 = vectorPath[this.currentWaypointIndex] - vectorPath[this.currentWaypointIndex - 1];
    Vector3 targetPoint = this.CalculateTargetPoint(currentPosition, vectorPath[this.currentWaypointIndex - 1], vectorPath[this.currentWaypointIndex]);
    Vector3 vector3_2 = (targetPoint - currentPosition) with
    {
      y = 0.0f
    };
    float magnitude = vector3_2.magnitude;
    float num1 = Mathf.Clamp01(magnitude / this.slowdownDistance);
    this.targetDirection = vector3_2;
    this.targetPoint = targetPoint;
    if (this.currentWaypointIndex == vectorPath.Count - 1 && (double) magnitude <= (double) this.endReachedDistance)
    {
      if (!this.targetReached)
      {
        this.targetReached = true;
        this.OnTargetReached();
      }
      return Vector3.zero;
    }
    Vector3 forward = this.tr.forward;
    float num2 = this.speed * Mathf.Max(Vector3.Dot(vector3_2.normalized, forward), this.minMoveScale) * num1;
    if ((double) Time.deltaTime > 0.0)
      num2 = Mathf.Clamp(num2, 0.0f, magnitude / (Time.deltaTime * 2f));
    return forward * num2;
  }

  public virtual void RotateTowards(Vector3 dir)
  {
    if (dir == Vector3.zero)
      return;
    this.tr.rotation = Quaternion.Euler(Quaternion.Slerp(this.tr.rotation, Quaternion.LookRotation(dir), this.turningSpeed * Time.deltaTime).eulerAngles with
    {
      z = 0.0f,
      x = 0.0f
    });
  }

  public Vector3 CalculateTargetPoint(Vector3 p, Vector3 a, Vector3 b)
  {
    a.y = p.y;
    b.y = p.y;
    float magnitude = (a - b).magnitude;
    if ((double) magnitude == 0.0)
      return a;
    float num1 = Mathf.Clamp01(VectorMath.ClosestPointOnLineFactor(a, b, p));
    float num2 = Mathf.Clamp(Mathf.Clamp(this.forwardLook - ((b - a) * num1 + a - p).magnitude, 0.0f, this.forwardLook) / magnitude + num1, 0.0f, 1f);
    return (b - a) * num2 + a;
  }
}
