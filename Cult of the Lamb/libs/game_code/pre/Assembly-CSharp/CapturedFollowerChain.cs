// Decompiled with JetBrains decompiler
// Type: CapturedFollowerChain
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine;
using System.Collections;
using UnityEngine;

#nullable disable
public class CapturedFollowerChain : MonoBehaviour
{
  [SerializeField]
  private Transform fervourDropPosition;
  private FollowerManager.SpawnedFollower follower;

  public bool DroppingFervour { get; private set; }

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

  private IEnumerator FrameDelay()
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

  private IEnumerator SpawnFervourIE()
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

  private void AnimationState_Event(TrackEntry trackEntry, Spine.Event e)
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
