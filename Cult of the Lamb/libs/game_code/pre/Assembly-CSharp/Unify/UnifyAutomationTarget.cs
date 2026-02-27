// Decompiled with JetBrains decompiler
// Type: Unify.UnifyAutomationTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Pathfinding;
using System;
using UnityEngine;

#nullable disable
namespace Unify;

public class UnifyAutomationTarget : MonoBehaviour
{
  public GameObject Player;
  public string PlayerObjectName;
  private Path Path;
  private bool ActiveTarget;
  private bool ReachedTarget;
  private Seeker seeker;
  private Vector3 CurrentTarget;
  private int CurrentTargetIndex;
  private float RadiusToTarget = 0.5f;

  private void Start()
  {
    this.seeker = this.GetComponent<Seeker>();
    this.Path = (Path) null;
    this.ActiveTarget = false;
    this.ReachedTarget = false;
    if (!string.IsNullOrEmpty(this.PlayerObjectName))
      return;
    this.PlayerObjectName = "Player - Prisoner(Clone)";
  }

  private void Update()
  {
    if ((UnityEngine.Object) this.Player == (UnityEngine.Object) null)
    {
      this.Player = GameObject.FindWithTag("Player");
    }
    else
    {
      if (!this.ActiveTarget)
        return;
      if ((double) Vector3.Distance(this.transform.position, this.Player.transform.position) < 1.0)
      {
        Debug.Log((object) "Reached Unify Navigation Target");
        this.ReachedTarget = true;
        UnifyManager.Instance.AutomationClient.SendGameEvent("LeftStick:0.0;0.0");
        UnifyManager.Instance.AutomationClient.SendGameEvent("GAME_TARGET");
      }
      else if (this.Path == null)
      {
        this.seeker.StartPath(this.Player.transform.position, this.transform.position, new OnPathDelegate(this.OnPathComplete));
      }
      else
      {
        if (!this.Path.IsDone())
          return;
        this.SendInputToAutomation(this.Path);
      }
    }
  }

  internal bool GetActive() => this.ActiveTarget;

  internal void SetActive(bool active)
  {
    this.ActiveTarget = active;
    Debug.Log((object) ("Unify Automation Target active: " + active.ToString()));
  }

  public bool PlayerReachedTarget() => this.ReachedTarget;

  public void OnPathComplete(Path p)
  {
    if (p.error)
    {
      Debug.LogError((object) "Unify Autamtion Target no path to player.");
    }
    else
    {
      this.Path = p;
      this.CurrentTargetIndex = 0;
      this.CurrentTarget = this.Path.vectorPath[this.CurrentTargetIndex];
    }
  }

  private void SendInputToAutomation(Path Path)
  {
    if ((double) Vector3.Distance(this.CurrentTarget, this.Player.transform.position) >= (double) this.RadiusToTarget || Path.vectorPath.Count <= this.CurrentTargetIndex)
      return;
    this.CurrentTarget = Path.vectorPath[++this.CurrentTargetIndex];
    Vector2 vector2 = (Vector2) (this.CurrentTarget - this.Player.transform.position);
    vector2.Normalize();
    UnifyManager.Instance.AutomationClient.SendGameEvent($"LeftStick:{vector2.x};{(ValueType) (float) ((double) vector2.y * -1.0)}");
  }
}
