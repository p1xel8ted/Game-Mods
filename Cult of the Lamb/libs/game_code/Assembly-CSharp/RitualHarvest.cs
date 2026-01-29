// Decompiled with JetBrains decompiler
// Type: RitualHarvest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RitualHarvest : Ritual
{
  public Vector3 averagePosition;

  public override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_HarvestRitual;

  public override void Play()
  {
    base.Play();
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.RitualRoutine());
  }

  public IEnumerator RitualRoutine()
  {
    RitualHarvest ritualHarvest = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    PlayerFarming.Instance.GoToAndStop(ChurchFollowerManager.Instance.RitualCenterPosition.position, GoToCallback: (System.Action) (() =>
    {
      Interaction_TempleAltar.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.Instance.simpleSpineAnimator.Animate("idle", 0, true);
      PlayerFarming.Instance.state.transform.DOMove(ChurchFollowerManager.Instance.RitualCenterPosition.position, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    }));
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    yield return (object) ritualHarvest.StartCoroutine((IEnumerator) ritualHarvest.WaitFollowersFormCircle());
    yield return (object) new WaitForSeconds(1f);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("build", 0, true);
    PlayerFarming.Instance.Spine.skeleton.FindBone("ritualring").Rotation += 60f;
    PlayerFarming.Instance.Spine.skeleton.UpdateWorldTransform();
    PlayerFarming.Instance.Spine.skeleton.Update(Time.deltaTime);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-start", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("rituals/ritual-loop", 0, true, 0.0f);
    Interaction_TempleAltar.Instance.RitualLighting.gameObject.SetActive(true);
    ChurchFollowerManager.Instance.StartRitualOverlay();
    BiomeConstants.Instance.ChromaticAbberationTween(2f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 1f);
    BiomeConstants.Instance.VignetteTween(2f, BiomeConstants.Instance.VignetteDefaultValue, 0.7f);
    DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 6.5f, 1f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutSine);
    yield return (object) new WaitForSeconds(1.2f);
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    Interaction_TempleAltar.Instance.CloseUpCamera.Play();
    AudioManager.Instance.PlayOneShot("event:/Stings/harvest");
    ChurchFollowerManager.Instance.FarmMud.gameObject.SetActive(true);
    ChurchFollowerManager.Instance.FarmMud.transform.DOMoveZ(1f, 0.0f);
    ChurchFollowerManager.Instance.FarmMud.transform.DOMoveZ(0.0f, 1.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
    List<FollowerBrain> followerBrainList = new List<FollowerBrain>((IEnumerable<FollowerBrain>) Ritual.GetFollowersAvailableToAttendSermon());
    for (int index = 0; index < 2 && followerBrainList.Count != 0; ++index)
    {
      FollowerBrain followerBrain = followerBrainList[UnityEngine.Random.Range(0, followerBrainList.Count)];
      followerBrainList.Remove(followerBrain);
      Follower followerById = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
      if ((bool) (UnityEngine.Object) followerById)
        ritualHarvest.StartCoroutine((IEnumerator) ritualHarvest.MoveFollower(followerById, index));
    }
    foreach (FollowerBrain followerBrain in followerBrainList)
    {
      if (followerBrain.CurrentTask is FollowerTask_AttendRitual)
        ((FollowerTask_AttendRitual) followerBrain.CurrentTask).Pray2();
    }
    yield return (object) new WaitForSeconds(6f);
    bool waiting = true;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.4f, "", (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    PlayerFarming.Instance.Spine.gameObject.SetActive(false);
    BaseLocationManager.Instance.Activatable = false;
    ChurchLocationManager.Instance.Activatable = false;
    BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 0.0f, 0.0f);
    BiomeBaseManager.Instance.ActivateRoom(false);
    Vector3 camPosition = GameManager.GetInstance().CamFollowTarget.transform.position;
    List<Structures_FarmerPlot> plots = StructureManager.GetAllStructuresOfType<Structures_FarmerPlot>(FollowerLocation.Base);
    for (int index = plots.Count - 1; index >= 0; --index)
    {
      if (plots[index].Data.Withered || !plots[index].CanGrow(SeasonsManager.CurrentSeason))
        plots.RemoveAt(index);
    }
    if (plots.Count > 0)
    {
      Structures_FarmerPlot structuresFarmerPlot = plots[UnityEngine.Random.Range(0, plots.Count)];
      foreach (FarmPlot farmPlot1 in FarmPlot.FarmPlots)
      {
        if (farmPlot1.StructureInfo.ID == structuresFarmerPlot.Data.ID)
        {
          ritualHarvest.averagePosition = Vector3.zero;
          int num = 0;
          foreach (FarmPlot farmPlot2 in FarmPlot.FarmPlots)
          {
            if ((double) Vector3.Distance(farmPlot2.transform.position, farmPlot1.transform.position) < 5.0)
            {
              ritualHarvest.averagePosition += farmPlot2.transform.position;
              ++num;
            }
          }
          ritualHarvest.averagePosition /= (float) num;
          GameManager.GetInstance().CamFollowTarget.ResetTargetCamera(0.0f);
          yield return (object) new WaitForEndOfFrame();
          GameManager.GetInstance().CamFollowTarget.ClearAllTargets();
          GameManager.GetInstance().CamFollowTarget.SnapTo(ritualHarvest.averagePosition);
          GameManager.GetInstance().CamFollowTarget.transform.localRotation = Quaternion.Euler(-45f, 0.0f, 0.0f);
          break;
        }
      }
    }
    ChurchFollowerManager.Instance.Sparkles.gameObject.SetActive(true);
    Vector3 SparklesStartPos = ChurchFollowerManager.Instance.Sparkles.transform.position;
    ChurchFollowerManager.Instance.Sparkles.transform.parent = BiomeConstants.Instance.transform;
    ChurchFollowerManager.Instance.Sparkles.transform.position = ritualHarvest.averagePosition;
    ChurchFollowerManager.Instance.Sparkles.Play();
    AudioManager.Instance.PlayOneShot("event:/player/Curses/charm_curse", AudioManager.Instance.Listener);
    yield return (object) new WaitForSeconds(1.5f);
    AudioManager.Instance.PlayOneShot("event:/building/finished_farmplot", AudioManager.Instance.Listener);
    foreach (Structures_FarmerPlot structuresFarmerPlot in plots)
    {
      structuresFarmerPlot.ForceFullyGrown();
      yield return (object) null;
    }
    yield return (object) new WaitForSeconds(2.5f);
    waiting = true;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.4f, "", (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    ChurchFollowerManager.Instance.Sparkles.transform.position = SparklesStartPos;
    ChurchFollowerManager.Instance.Sparkles.transform.parent = ChurchFollowerManager.Instance.transform;
    ChurchFollowerManager.Instance.Sparkles.gameObject.SetActive(false);
    GameManager.GetInstance().CamFollowTarget.ClearAllTargets();
    GameManager.GetInstance().CamFollowTarget.ForceSnapTo(camPosition);
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 1f, 200f);
    BiomeBaseManager.Instance.ActivateChurch();
    PlayerFarming.Instance.Spine.gameObject.SetActive(true);
    BaseLocationManager.Instance.Activatable = true;
    ChurchLocationManager.Instance.Activatable = true;
    foreach (FollowerBrain followerBrain in Ritual.GetFollowersAvailableToAttendSermon())
    {
      if (followerBrain != null)
        FollowerManager.FindFollowerByID(followerBrain.Info.ID)?.HoodOn("pray", true);
    }
    yield return (object) new WaitForSeconds(2f);
    ChurchFollowerManager.Instance.FarmMud.transform.DOMoveZ(1f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuart);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-stop", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain != null && followerBrain.CurrentTask is FollowerTask_AttendRitual)
        (followerBrain.CurrentTask as FollowerTask_AttendRitual).Cheer();
    }
    yield return (object) new WaitForSeconds(0.5f);
    ChurchFollowerManager.Instance.FarmMud.gameObject.SetActive(false);
    BiomeConstants.Instance.ChromaticAbberationTween(1f, 1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    BiomeConstants.Instance.VignetteTween(1f, 0.7f, BiomeConstants.Instance.VignetteDefaultValue);
    ChurchFollowerManager.Instance.EndRitualOverlay();
    GameManager.GetInstance().CamFollowTarget.targetDistance = 11f;
    Interaction_TempleAltar.Instance.RitualLighting.gameObject.SetActive(false);
    float num1 = 0.0f;
    foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
    {
      if (brain != null)
      {
        float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
        num1 += Delay;
        ritualHarvest.StartCoroutine((IEnumerator) ritualHarvest.DelayFollowerReaction(brain, Delay));
      }
    }
    yield return (object) new WaitForSeconds(1.5f);
    Interaction_TempleAltar.Instance.CloseUpCamera.Reset();
    ritualHarvest.CompleteRitual();
    yield return (object) new WaitForSeconds(1f);
    CultFaithManager.AddThought(Thought.Cult_HarvestRitual);
  }

  public IEnumerator MoveFollower(Follower follower, int index)
  {
    RitualHarvest ritualHarvest = this;
    List<Vector3> positions = new List<Vector3>()
    {
      Vector3.left,
      Vector3.right
    };
    string[] anims = new string[4]
    {
      "Farming/add-seed-mushroom",
      "Farming/add-seed-pumpkin",
      "Farming/add-seed-redflower",
      "Farming/add-seed"
    };
    bool waiting = true;
    follower.HoodOff(onComplete: (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    yield return (object) ritualHarvest.StartCoroutine((IEnumerator) follower.GoToRoutine(PlayerFarming.Instance.transform.position + positions[index]));
    follower.State.facingAngle = Utils.GetAngle(follower.transform.position, PlayerFarming.Instance.transform.position);
    double num = (double) follower.SetBodyAnimation(anims[UnityEngine.Random.Range(0, anims.Length)], false);
    follower.AddBodyAnimation(anims[UnityEngine.Random.Range(0, anims.Length)], false, 0.0f);
    follower.AddBodyAnimation(anims[UnityEngine.Random.Range(0, anims.Length)], false, 0.0f);
    follower.AddBodyAnimation("idle", true, 0.0f);
  }
}
