// Decompiled with JetBrains decompiler
// Type: RitualFeast
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RitualFeast : Ritual
{
  public override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_Feast;

  public override void Play()
  {
    base.Play();
    GameManager.GetInstance().StartCoroutine(this.RitualRoutine());
  }

  public IEnumerator RitualRoutine()
  {
    RitualFeast ritualFeast = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    Interaction_FeastTable feastTable = ChurchFollowerManager.Instance.FeastTable;
    feastTable.gameObject.SetActive(true);
    feastTable.transform.position = new Vector3(feastTable.transform.position.x, feastTable.transform.position.y, 2f);
    feastTable.transform.DOMoveZ(0.0f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    AudioManager.Instance.PlayOneShot("event:/ui/map_location_pan", feastTable.gameObject);
    AudioManager.Instance.PlayOneShot("event:/player/harvest_meat", PlayerFarming.Instance.gameObject);
    feastTable.GetComponent<BoxCollider2D>().enabled = true;
    Bounds bounds = feastTable.GetComponent<BoxCollider2D>().bounds;
    AstarPath.active.UpdateGraphs(bounds);
    PlayerFarming.Instance.GoToAndStop(feastTable.PlayerPosition.transform.position, GoToCallback: (System.Action) (() =>
    {
      Interaction_TempleAltar.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.Instance.simpleSpineAnimator.Animate("idle", 0, true);
      PlayerFarming.Instance.state.transform.DOMove(feastTable.PlayerPosition.transform.position, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    }));
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    while (feastTable.StructureInfo == null)
      yield return (object) null;
    yield return (object) ritualFeast.StartCoroutine(ritualFeast.WaitForFollowersToTakeSeat(feastTable.StructureInfo.ID));
    PlayerFarming.Instance.simpleSpineAnimator.Animate("build", 0, true);
    PlayerFarming.Instance.Spine.skeleton.FindBone("ritualring").Rotation += 60f;
    PlayerFarming.Instance.Spine.skeleton.UpdateWorldTransform();
    PlayerFarming.Instance.Spine.skeleton.Update(Time.deltaTime);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/feast-start", 0, false);
    if (!PlayerFarming.Instance.isLamb || PlayerFarming.Instance.IsGoat)
      AudioManager.Instance.PlayOneShot("event:/player/goat_player/goat_speak_to_follower_noBookPage");
    else
      AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower_noBookPage");
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("rituals/feast-eat", 0, true, 0.0f);
    Interaction_TempleAltar.Instance.RitualLighting.gameObject.SetActive(true);
    ChurchFollowerManager.Instance.PlayOverlay(ChurchFollowerManager.OverlayType.Ritual, "feast");
    BiomeConstants.Instance.ChromaticAbberationTween(2f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 1f);
    BiomeConstants.Instance.VignetteTween(2f, BiomeConstants.Instance.VignetteDefaultValue, 0.7f);
    DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 6.5f, 1f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutSine);
    yield return (object) new WaitForSeconds(1.2f);
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    Interaction_TempleAltar.Instance.CloseUpCamera.Play();
    yield return (object) new WaitForSeconds(1.2f);
    List<FollowerBrain> followers = new List<FollowerBrain>((IEnumerable<FollowerBrain>) Ritual.GetFollowersAvailableToAttendSermon());
    foreach (FollowerBrain followerBrain in followers)
    {
      if (followerBrain.CurrentTask is FollowerTask_ChangeLocation)
        followerBrain.CurrentTask.Arrive();
      Follower followerById = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
      if ((bool) (UnityEngine.Object) followerById)
      {
        followerById.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
        double num = (double) followerById.SetBodyAnimation("Food/feast-eat", true);
      }
    }
    feastTable.IsFeastActive = true;
    yield return (object) new WaitForSeconds(7f);
    feastTable.IsFeastActive = false;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/feast-end", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    foreach (FollowerBrain followerBrain in followers)
      followerBrain.CompleteCurrentTask();
    feastTable.transform.DOMoveZ(2f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    AudioManager.Instance.PlayOneShot("event:/ui/map_location_pan", feastTable.gameObject);
    AudioManager.Instance.PlayOneShot("event:/player/harvest_meat_done", PlayerFarming.Instance.gameObject);
    yield return (object) new WaitForSeconds(1f);
    feastTable.gameObject.SetActive(false);
    feastTable.GetComponent<BoxCollider2D>().enabled = false;
    AstarPath.active.UpdateGraphs(bounds);
    BiomeConstants.Instance.ChromaticAbberationTween(1f, 1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    BiomeConstants.Instance.VignetteTween(1f, 0.7f, BiomeConstants.Instance.VignetteDefaultValue);
    ChurchFollowerManager.Instance.EndRitualOverlay();
    GameManager.GetInstance().CamFollowTarget.targetDistance = 11f;
    Interaction_TempleAltar.Instance.RitualLighting.gameObject.SetActive(false);
    float num1 = 0.0f;
    foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
    {
      float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
      num1 += Delay;
      ritualFeast.StartCoroutine(ritualFeast.DelayFollowerReaction(brain, Delay));
    }
    yield return (object) new WaitForSeconds(1.5f);
    if (DataManager.Instance.SurvivalModeActive)
      DataManager.Instance.SurvivalMode_Hunger = 100f;
    Interaction_TempleAltar.Instance.CloseUpCamera.Reset();
    DataManager.Instance.LastFeastDeclared = TimeManager.TotalElapsedGameTime;
    ritualFeast.CompleteRitual();
    yield return (object) new WaitForSeconds(1f);
    CultFaithManager.AddThought(Thought.Cult_Feast);
    foreach (FollowerBrain followerBrain in followers)
    {
      followerBrain.Stats.Starvation = 0.0f;
      followerBrain.Stats.Satiation = 100f;
      followerBrain.AddThought(Thought.FeastTable);
    }
  }

  public IEnumerator MoveFollower(Follower follower, int index)
  {
    RitualFeast ritualFeast = this;
    List<Vector3> positions = new List<Vector3>()
    {
      Vector3.left,
      Vector3.up,
      Vector3.right,
      Vector3.down
    };
    bool waiting = true;
    follower.HoodOff(onComplete: (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    yield return (object) ritualFeast.StartCoroutine(follower.GoToRoutine(ChurchFollowerManager.Instance.RitualCenterPosition.position + positions[index]));
    double num = (double) follower.SetBodyAnimation("dance-hooded", true);
  }

  public IEnumerator WaitForFollowersToTakeSeat(int feastTableID)
  {
    GameManager gameManager = GameManager.GetInstance();
    Interaction_TempleAltar templeManager = Interaction_TempleAltar.Instance;
    if ((UnityEngine.Object) gameManager != (UnityEngine.Object) null && (UnityEngine.Object) templeManager != (UnityEngine.Object) null && (UnityEngine.Object) templeManager.RitualCameraPosition != (UnityEngine.Object) null)
      gameManager.OnConversationNext(templeManager.RitualCameraPosition, 12f);
    bool getFollowers = Ritual.FollowerToAttendSermon == null || Ritual.FollowerToAttendSermon.Count <= 0;
    if (getFollowers)
      Ritual.FollowerToAttendSermon = new List<FollowerBrain>();
    foreach (FollowerBrain followerBrain in Ritual.GetFollowersAvailableToAttendSermon())
    {
      if (followerBrain != null)
      {
        if (getFollowers)
          Ritual.FollowerToAttendSermon.Add(followerBrain);
        if (followerBrain.CurrentTask != null)
          followerBrain.CurrentTask.Abort();
        followerBrain.HardSwapToTask((FollowerTask) new FollowerTask_EatFeastTable(feastTableID));
        if (followerBrain.CurrentTaskType == FollowerTaskType.ChangeLocation)
          followerBrain.CurrentTask.Arrive();
        if (followerBrain.Info != null)
        {
          Follower followerById = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
          if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
            followerById.HideAllFollowerIcons();
        }
        yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.05f, 0.15f));
      }
    }
    yield return (object) null;
    Debug.Log((object) ("RECALCULATE! " + Ritual.FollowerToAttendSermon.Count.ToString()));
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain != null && followerBrain.CurrentTask != null)
        followerBrain.CurrentTask.RecalculateDestination();
    }
    float timer = 0.0f;
    while (!this.FollowersInPosition() && (double) (timer += Time.deltaTime) < 10.0)
      yield return (object) null;
    SimulationManager.Pause();
    if ((UnityEngine.Object) gameManager != (UnityEngine.Object) null && (UnityEngine.Object) templeManager != (UnityEngine.Object) null && (UnityEngine.Object) templeManager.PortalEffect != (UnityEngine.Object) null)
      gameManager.OnConversationNext(templeManager.PortalEffect.gameObject, 8f);
  }

  public new bool FollowersInPosition()
  {
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain.CurrentTaskType == FollowerTaskType.ChangeLocation || followerBrain.Location != PlayerFarming.Location || followerBrain.CurrentTaskType == FollowerTaskType.EatFeastTable && followerBrain.CurrentTask.State != FollowerTaskState.Doing)
      {
        if (followerBrain.CurrentTaskType != FollowerTaskType.EatFeastTable)
        {
          if (followerBrain.CurrentTask != null)
            followerBrain.CurrentTask.Abort();
          followerBrain.HardSwapToTask((FollowerTask) new FollowerTask_EatFeastTable(0));
          if (followerBrain.CurrentTaskType == FollowerTaskType.ChangeLocation)
            followerBrain.CurrentTask.Arrive();
        }
        if (followerBrain.Location != PlayerFarming.Location)
        {
          followerBrain.HardSwapToTask((FollowerTask) new FollowerTask_EatFeastTable(0));
          followerBrain.ShouldReconsiderTask = false;
          if (followerBrain.CurrentTaskType == FollowerTaskType.ChangeLocation)
            followerBrain.CurrentTask.Arrive();
        }
        return false;
      }
    }
    return true;
  }
}
