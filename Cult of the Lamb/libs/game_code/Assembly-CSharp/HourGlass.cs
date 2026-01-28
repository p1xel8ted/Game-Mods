// Decompiled with JetBrains decompiler
// Type: HourGlass
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class HourGlass : BaseMonoBehaviour
{
  public SkeletonAnimation SpineAnimation;
  public TrackEntry Track;
  public float SpineDuration;
  public Transform SpawnPosition;
  public List<GameObject> Doors = new List<GameObject>();
  public List<SimpleSFX> DoorsSFX = new List<SimpleSFX>();
  public HourGlass.State _CurrentState;

  public HourGlass.State CurrentState
  {
    set
    {
      if (this._CurrentState != value)
      {
        switch (value)
        {
          case HourGlass.State.idle:
            this.SpineAnimation.AnimationState.SetAnimation(0, "idle", true);
            break;
          case HourGlass.State.turning:
            this.SpineAnimation.AnimationState.SetAnimation(0, "turn", false);
            break;
          case HourGlass.State.timeup:
            this.SpineAnimation.AnimationState.SetAnimation(0, "time-up", true);
            break;
        }
      }
      this._CurrentState = value;
    }
    get => this._CurrentState;
  }

  public void Start() => this.CurrentState = HourGlass.State.idle;

  public void TurnHouseglass() => this.StartCoroutine((IEnumerator) this.OpenDoors());

  public IEnumerator DoTurn()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    HourGlass hourGlass = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      HUD_Timer.Timer = 120f;
      HUD_Timer.TimerRunning = true;
      hourGlass.StartCoroutine((IEnumerator) hourGlass.DoActive());
      hourGlass.StartCoroutine((IEnumerator) hourGlass.OpenDoors());
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    hourGlass.CurrentState = HourGlass.State.turning;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(1.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator OpenDoors()
  {
    yield return (object) new WaitForSeconds(1f);
    float Progress = 0.0f;
    float Duration = 3f;
    foreach (SimpleSFX simpleSfx in this.DoorsSFX)
    {
      if ((Object) simpleSfx != (Object) null)
        simpleSfx.Play("stone_door_sliding");
    }
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      CameraManager.shakeCamera(Random.Range(0.15f, 0.2f), (float) Random.Range(0, 360));
      foreach (GameObject door in this.Doors)
      {
        Vector3 localPosition = door.transform.localPosition with
        {
          z = (float) (2.0 * ((double) Progress / (double) Duration))
        };
        door.transform.localPosition = localPosition;
      }
      yield return (object) null;
    }
    foreach (GameObject door in this.Doors)
    {
      Collider2D componentInChildren = door.GetComponentInChildren<Collider2D>();
      if ((Object) componentInChildren != (Object) null)
        componentInChildren.enabled = false;
    }
  }

  public IEnumerator DoActive()
  {
    this.CurrentState = HourGlass.State.active;
    while (this.CurrentState == HourGlass.State.active)
    {
      this.Track = this.SpineAnimation.AnimationState.SetAnimation(0, "countdown", false);
      this.SpineDuration = this.SpineAnimation.Skeleton.Data.FindAnimation("countdown").Duration;
      this.SpineAnimation.timeScale = 0.0f;
      this.Track.Animation.Apply(this.SpineAnimation.Skeleton, 0.0f, this.SpineDuration * HUD_Timer.Progress, false, (ExposedList<Spine.Event>) null, 1f, MixBlend.Replace, MixDirection.In);
      if (HUD_Timer.IsTimeUp)
      {
        this.SpineAnimation.timeScale = 1f;
        this.CurrentState = HourGlass.State.timeup;
        break;
      }
      yield return (object) null;
    }
  }

  public enum State
  {
    idle,
    turning,
    active,
    timeup,
  }
}
