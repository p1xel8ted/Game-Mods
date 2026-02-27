// Decompiled with JetBrains decompiler
// Type: FollowPath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Pathfinding;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (Seeker))]
public class FollowPath : BaseMonoBehaviour
{
  private Seeker seeker;
  private StateMachine state;
  private Vector2 targetLocation;
  public float maxSpeed = 0.05f;
  private float speed;
  public float vx;
  public float vy;
  public LayerMask layerToCheck;
  private List<Vector3> pathToFollow;
  private int currentWaypoint;

  private void Start()
  {
  }

  private void Awake()
  {
    this.seeker = this.GetComponent<Seeker>();
    this.state = this.GetComponent<StateMachine>();
    if (!((UnityEngine.Object) this.state == (UnityEngine.Object) null))
      return;
    this.state = this.gameObject.AddComponent<StateMachine>();
  }

  public event FollowPath.Action NewPath;

  public void givePath(Vector3 targetLocation)
  {
    if (this.CheckLineOfSight(targetLocation, Vector2.Distance((Vector2) this.transform.position, (Vector2) targetLocation)))
    {
      this.state.CURRENT_STATE = StateMachine.State.Moving;
      this.pathToFollow = new List<Vector3>();
      this.pathToFollow.Add((Vector3) AstarPath.active.GetNearest(targetLocation).node.position);
      this.currentWaypoint = 0;
      MouseManager.placeTarget(targetLocation);
      this.state.facingAngle = Utils.GetAngle(this.transform.position, this.pathToFollow[this.currentWaypoint]);
    }
    else
      this.seeker.StartPath(this.transform.position, targetLocation);
    if (this.NewPath == null)
      return;
    this.NewPath();
  }

  private bool CheckLineOfSight(Vector3 pointToCheck, float distance)
  {
    RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2) this.transform.position, (Vector2) (pointToCheck - this.transform.position), distance, (int) this.layerToCheck);
    if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
    {
      Debug.DrawRay(this.transform.position, (raycastHit2D.collider.transform.position - this.transform.position) * Vector2.Distance((Vector2) this.transform.position, (Vector2) raycastHit2D.collider.transform.position), Color.yellow);
      return false;
    }
    Debug.DrawRay(this.transform.position, pointToCheck - this.transform.position, Color.green);
    return true;
  }

  public void startPath(Path p)
  {
    if (p.error)
      return;
    this.state.CURRENT_STATE = StateMachine.State.Moving;
    this.pathToFollow = new List<Vector3>();
    for (int index = 0; index < p.vectorPath.Count; ++index)
      this.pathToFollow.Add(p.vectorPath[index]);
    this.currentWaypoint = 0;
    MouseManager.placeTarget(this.pathToFollow[this.pathToFollow.Count - 1]);
    this.state.facingAngle = Utils.GetAngle(this.transform.position, this.pathToFollow[this.currentWaypoint]);
  }

  protected virtual void OnEnable()
  {
    this.seeker.pathCallback += new OnPathDelegate(this.startPath);
  }

  public void OnDisable()
  {
    this.seeker.CancelCurrentPathRequest();
    this.seeker.pathCallback -= new OnPathDelegate(this.startPath);
  }

  private void Update()
  {
    if (this.pathToFollow == null)
      return;
    if (this.currentWaypoint > this.pathToFollow.Count)
    {
      this.speed += (float) ((0.0 - (double) this.speed) / 4.0);
      this.move();
    }
    else if (this.currentWaypoint == this.pathToFollow.Count)
    {
      this.state.CURRENT_STATE = StateMachine.State.Idle;
      ++this.currentWaypoint;
      this.pathToFollow = (List<Vector3>) null;
    }
    else
    {
      if ((double) this.speed < (double) this.maxSpeed)
        this.speed += (float) (((double) this.maxSpeed - (double) this.speed) / 7.0);
      this.state.facingAngle = Utils.GetAngle(this.transform.position, this.pathToFollow[this.currentWaypoint]);
      this.move();
      if ((double) Vector2.Distance((Vector2) this.transform.position, (Vector2) this.pathToFollow[this.currentWaypoint]) > (this.currentWaypoint == this.pathToFollow.Count - 1 ? 0.10000000149011612 : 0.5))
        return;
      ++this.currentWaypoint;
    }
  }

  private void move()
  {
    this.transform.position = this.transform.position + new Vector3(this.speed * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), this.speed * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)), 0.0f);
  }

  private void FixedUpdate()
  {
    Rigidbody2D component = this.gameObject.GetComponent<Rigidbody2D>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    component.MovePosition(component.position + new Vector2(this.vx, this.vy) * Time.deltaTime);
  }

  public static Quaternion FaceObject(Vector2 startingPosition, Vector2 targetPosition)
  {
    Vector2 vector2 = targetPosition - startingPosition;
    return Quaternion.AngleAxis(Mathf.Atan2(vector2.y, vector2.x) * 57.29578f, Vector3.forward);
  }

  public delegate void Action();
}
