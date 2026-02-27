// Decompiled with JetBrains decompiler
// Type: StealDevotionShrine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class StealDevotionShrine : MonoBehaviour
{
  public Health Health;
  public SkeletonAnimation Spine;
  private bool BossBeatn;
  public bool droppedStone;
  public GameObject VortexObject;
  public GameObject ContainerToHide;

  private void OnEnable()
  {
    this.Health.OnHit += new Health.HitAction(this.OnHit);
    this.Health.OnDie += new Health.DieAction(this.OnDie);
    this.Health.OnPoisonedHit += new Health.HitAction(this.OnHit);
    LocationManager.OnPlayerLocationSet += new System.Action(this.OnPlayerLocationSet);
    this.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
  }

  private void OnDisable()
  {
    this.Health.OnHit -= new Health.HitAction(this.OnHit);
    this.Health.OnPoisonedHit -= new Health.HitAction(this.OnHit);
    this.Health.OnDie -= new Health.DieAction(this.OnDie);
    this.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    LocationManager.OnPlayerLocationSet -= new System.Action(this.OnPlayerLocationSet);
  }

  private void OnPlayerLocationSet()
  {
    LocationManager.OnPlayerLocationSet -= new System.Action(this.OnPlayerLocationSet);
    this.BossBeatn = DataManager.Instance.BossesCompleted.Contains(PlayerFarming.Location);
    string str = "";
    switch (PlayerFarming.Location)
    {
      case FollowerLocation.Dungeon1_1:
        str = "1";
        break;
      case FollowerLocation.Dungeon1_2:
        str = "2";
        break;
      case FollowerLocation.Dungeon1_3:
        str = "3";
        break;
      case FollowerLocation.Dungeon1_4:
        str = "4";
        break;
    }
    this.Spine.skeleton.SetSkin(str + (this.BossBeatn ? "_defeated" : ""));
    this.Spine.skeleton.SetSlotsToSetupPose();
    this.Spine.AnimationState.Apply(this.Spine.skeleton);
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
    if (!this.BossBeatn)
    {
      if (!this.droppedStone)
      {
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.STONE, UnityEngine.Random.Range(1, 3), this.transform.position);
        this.droppedStone = true;
      }
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.ReformRoutine());
    }
    else
    {
      this.GetComponent<CircleCollider2D>().enabled = false;
      this.ContainerToHide.SetActive(false);
      this.VortexObject.SetActive(true);
    }
    this.Spine.AnimationState.SetAnimation(0, "4", false);
    AudioManager.Instance.PlayOneShot("event:/player/Curses/explosive_shot", this.transform.position);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position);
    CameraManager.shakeCamera(5f);
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
