// Decompiled with JetBrains decompiler
// Type: Unify.UnifyAutomationTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Pathfinding;
using System;
using UnityEngine;

#nullable disable
namespace Unify;

public class UnifyAutomationTarget : MonoBehaviour
{
  public GameObject Player;
  public string PlayerObjectName;
  public Path Path;
  public bool ActiveTarget;
  public bool ReachedTarget;
  public Seeker seeker;
  public Vector3 CurrentTarget;
  public int CurrentTargetIndex;
  public float RadiusToTarget = 0.5f;

  public void Start()
  {
    this.seeker = this.GetComponent<Seeker>();
    this.Path = (Path) null;
    this.ActiveTarget = false;
    this.ReachedTarget = false;
    if (!string.IsNullOrEmpty(this.PlayerObjectName))
      return;
    this.PlayerObjectName = "Player - Prisoner(Clone)";
  }

  public void Update()
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

  public bool GetActive() => this.ActiveTarget;

  public void SetActive(bool active)
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

  public void SendInputToAutomation(Path Path)
  {
    if ((double) Vector3.Distance(this.CurrentTarget, this.Player.transform.position) >= (double) this.RadiusToTarget || Path.vectorPath.Count <= this.CurrentTargetIndex)
      return;
    this.CurrentTarget = Path.vectorPath[++this.CurrentTargetIndex];
    Vector2 vector2 = (Vector2) (this.CurrentTarget - this.Player.transform.position);
    vector2.Normalize();
    UnifyManager.Instance.AutomationClient.SendGameEvent($"LeftStick:{vector2.x};{(ValueType) (float) ((double) vector2.y * -1.0)}");
  }
}
