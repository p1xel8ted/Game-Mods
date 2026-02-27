// Decompiled with JetBrains decompiler
// Type: PracticeShrine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class PracticeShrine : MonoBehaviour
{
  public Health Health;
  public SkeletonAnimation Spine;
  public FollowerLocation Boss;
  [SpineSkin("", "", true, false, false, dataField = "Spine")]
  public string DefeatedSkin;

  private void OnEnable()
  {
    this.Health.OnHit += new Health.HitAction(this.OnHit);
    this.Health.OnDie += new Health.DieAction(this.OnDie);
    this.Health.OnPoisonedHit += new Health.HitAction(this.OnHit);
    if (DataManager.Instance.BossesCompleted.Contains(this.Boss))
    {
      Debug.Log((object) "USE SKIN!!");
      this.Spine.skeleton.SetSkin(this.DefeatedSkin);
      this.Spine.skeleton.SetSlotsToSetupPose();
      this.Spine.AnimationState.Apply(this.Spine.skeleton);
    }
    this.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
  }

  private void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (e.Data.Name == "piece")
    {
      AudioManager.Instance.PlayOneShot("event:/material/stone_impact", this.gameObject);
    }
    else
    {
      if (!(e.Data.Name == "final_piece"))
        return;
      AudioManager.Instance.PlayOneShot("event:/building/finished_stone", this.gameObject);
    }
  }

  private void OnDisable()
  {
    this.Health.OnHit -= new Health.HitAction(this.OnHit);
    this.Health.OnPoisonedHit -= new Health.HitAction(this.OnHit);
    this.Health.OnDie -= new Health.DieAction(this.OnDie);
    this.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
  }

  private void OnHit(
    GameObject attacker,
    Vector3 attacklocation,
    Health.AttackTypes attacktype,
    bool frombehind)
  {
    int message = Mathf.FloorToInt((float) ((1.0 - (double) this.Health.HP / (double) this.Health.totalHP) * 4.0));
    this.Spine.AnimationState.SetAnimation(0, message.ToString(), false);
    Debug.Log((object) message);
  }

  private void OnDie(
    GameObject attacker,
    Vector3 attacklocation,
    Health victim,
    Health.AttackTypes attacktype,
    Health.AttackFlags attackflags)
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.ReformRoutine());
  }

  private IEnumerator ReformRoutine()
  {
    yield return (object) null;
    this.Spine.AnimationState.SetAnimation(0, "4", false);
    yield return (object) new WaitForSeconds(2f);
    this.Spine.AnimationState.SetAnimation(0, "reform", false);
    this.Spine.AnimationState.AddAnimation(0, "0", false, 0.0f);
    yield return (object) new WaitForSeconds(6.133333f);
    this.Health.HP = this.Health.totalHP;
    this.Health.enabled = true;
  }
}
