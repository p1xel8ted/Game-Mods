// Decompiled with JetBrains decompiler
// Type: CapturedFollowerChain
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class CapturedFollowerChain : MonoBehaviour
{
  [SerializeField]
  public Transform fervourDropPosition;
  public FollowerManager.SpawnedFollower follower;
  [CompilerGenerated]
  public bool \u003CDroppingFervour\u003Ek__BackingField;

  public bool DroppingFervour
  {
    get => this.\u003CDroppingFervour\u003Ek__BackingField;
    set => this.\u003CDroppingFervour\u003Ek__BackingField = value;
  }

  public void Init(FollowerManager.SpawnedFollower follower)
  {
    if (!(bool) (Object) follower.Follower)
      return;
    this.follower = follower;
    follower.Follower.gameObject.SetActive(false);
    follower.Follower.transform.parent = this.transform;
    follower.Follower.transform.localPosition = new Vector3(0.0f, -0.8f, 0.0f);
    follower.Follower.transform.localScale = new Vector3(Random.Range(0, 2) == 0 ? 1f : -1f, 1f, 1f);
    double num = (double) follower.Follower.SetBodyAnimation("FinalBoss/appear", false);
    follower.Follower.AddBodyAnimation("FinalBoss/idle", true, 0.0f);
    follower.Follower.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.AnimationState_Event);
    this.StartCoroutine((IEnumerator) this.FrameDelay());
  }

  public IEnumerator FrameDelay()
  {
    CapturedFollowerChain capturedFollowerChain = this;
    yield return (object) new WaitForEndOfFrame();
    capturedFollowerChain.follower.Follower.gameObject.SetActive(true);
    while ((Object) EnemyDeathCatBoss.Instance == (Object) null || (Object) EnemyDeathCatBoss.Instance.Spine == (Object) null)
      yield return (object) null;
    EnemyDeathCatBoss.Instance.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(capturedFollowerChain.AnimationState_Event);
  }

  public void DropFervour()
  {
    if ((Object) this.follower.Follower != (Object) null)
    {
      double num = (double) this.follower.Follower.SetBodyAnimation("FinalBoss/give-fervour", false);
      this.follower.Follower.AddBodyAnimation("FinalBoss/idle", true, 0.0f);
    }
    else
      this.StartCoroutine((IEnumerator) this.SpawnFervourIE());
    this.DroppingFervour = true;
  }

  public IEnumerator SpawnFervourIE()
  {
    int Rewards = 20;
    if ((Object) this.follower.Follower == (Object) null)
      Rewards /= 2;
    int i = -1;
    while (++i < Rewards)
    {
      BlackSoul blackSoul = InventoryItem.SpawnBlackSoul(1, this.fervourDropPosition.position, simulated: true);
      if ((Object) blackSoul != (Object) null)
        blackSoul.SetAngle((float) Random.Range(0, 360), new Vector2(2f, 3f));
      yield return (object) new WaitForSeconds(0.05f);
    }
    this.DroppingFervour = false;
  }

  public void AnimationState_Event(TrackEntry trackEntry, Spine.Event e)
  {
    if (e.Data.Name == "fervour")
    {
      this.StartCoroutine((IEnumerator) this.SpawnFervourIE());
    }
    else
    {
      if (!(e.Data.Name == "slam") || this.DroppingFervour)
        return;
      double num = (double) this.follower.Follower.SetBodyAnimation("FinalBoss/shake", false);
      this.follower.Follower.AddBodyAnimation("FinalBoss/idle", true, 0.0f);
    }
  }
}
