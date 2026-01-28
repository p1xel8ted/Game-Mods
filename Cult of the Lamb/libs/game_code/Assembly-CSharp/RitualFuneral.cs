// Decompiled with JetBrains decompiler
// Type: RitualFuneral
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using MMTools;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RitualFuneral : Ritual
{
  public override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_Funeral;

  public override void Play()
  {
    base.Play();
    this.StartCoroutine((IEnumerator) this.RitualRoutine());
  }

  public IEnumerator RitualRoutine()
  {
    RitualFuneral ritualFuneral = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    yield return (object) ritualFuneral.StartCoroutine((IEnumerator) ritualFuneral.WaitFollowersFormCircle());
    yield return (object) new WaitForSeconds(1f);
    FollowerLocation location = FollowerLocation.Base;
    List<Structures_Grave> graves = StructureManager.GetAllStructuresOfType<Structures_Grave>(in location);
    location = FollowerLocation.Base;
    List<Structures_Crypt> crypts = StructureManager.GetAllStructuresOfType<Structures_Crypt>(in location);
    List<FollowerInfo> followerInfoList = new List<FollowerInfo>((IEnumerable<FollowerInfo>) DataManager.Instance.Followers_Dead);
    for (int index = followerInfoList.Count - 1; index >= 0; --index)
    {
      if (followerInfoList[index].HadFuneral)
      {
        followerInfoList.RemoveAt(index);
      }
      else
      {
        bool flag = false;
        foreach (StructureBrain structureBrain in graves)
        {
          if (structureBrain.Data.FollowerID == followerInfoList[index].ID)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          foreach (StructureBrain structureBrain in crypts)
          {
            if (structureBrain.Data.MultipleFollowerIDs.Contains(followerInfoList[index].ID))
            {
              flag = true;
              break;
            }
          }
        }
        if (!flag)
          followerInfoList.RemoveAt(index);
      }
    }
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    foreach (FollowerInfo followerInfo in followerInfoList)
      followerSelectEntries.Add(new FollowerSelectEntry(followerInfo));
    UIFollowerSelectMenuController followerSelectInstance = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    followerSelectInstance.VotingType = TwitchVoting.VotingType.RITUAL_FUNERAL;
    followerSelectInstance.Show(followerSelectEntries, followerSelectionType: ritualFuneral.RitualType);
    UIFollowerSelectMenuController selectMenuController1 = followerSelectInstance;
    selectMenuController1.OnFollowerSelected = selectMenuController1.OnFollowerSelected + (Action<FollowerInfo>) (followerInfo =>
    {
      AudioManager.Instance.PlayOneShot("event:/ritual_sacrifice/select_follower", PlayerFarming.Instance.gameObject);
      this.loopedSound = AudioManager.Instance.CreateLoop("event:/sermon/preach_loop", PlayerFarming.Instance.gameObject, true, false);
      StructureBrain grave = (StructureBrain) null;
      foreach (Structures_Grave structuresGrave in graves)
      {
        if (structuresGrave.Data.FollowerID == followerInfo.ID)
        {
          grave = (StructureBrain) structuresGrave;
          break;
        }
      }
      if (grave == null)
      {
        foreach (Structures_Crypt structuresCrypt in crypts)
        {
          if (structuresCrypt.Data.MultipleFollowerIDs.Contains(followerInfo.ID))
          {
            grave = (StructureBrain) structuresCrypt;
            break;
          }
        }
      }
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.ContinueRitual(followerInfo, grave));
    });
    UIFollowerSelectMenuController selectMenuController2 = followerSelectInstance;
    selectMenuController2.OnCancel = selectMenuController2.OnCancel + (System.Action) (() =>
    {
      AudioManager.Instance.StopLoop(this.loopedSound);
      this.EndRitual();
      Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
      Interaction_TempleAltar.Instance.CloseUpCamera.Reset();
      this.CancelFollowers();
      this.CompleteRitual(true);
    });
    UIFollowerSelectMenuController selectMenuController3 = followerSelectInstance;
    selectMenuController3.OnHidden = selectMenuController3.OnHidden + (System.Action) (() => followerSelectInstance = (UIFollowerSelectMenuController) null);
  }

  public IEnumerator ContinueRitual(FollowerInfo deadFollowerInfo, StructureBrain grave)
  {
    RitualFuneral ritualFuneral = this;
    AudioManager.Instance.PlayOneShot("event:/ritual_sacrifice/select_follower", PlayerFarming.Instance.gameObject);
    Interaction_TempleAltar.Instance.CloseUpCamera.Play();
    yield return (object) new WaitForSeconds(1f);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-start", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("rituals/ritual-loop", 0, true, 0.0f);
    foreach (FollowerBrain followerBrain in Ritual.GetFollowersAvailableToAttendSermon())
    {
      Follower followerById = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
      ritualFuneral.StartCoroutine((IEnumerator) ritualFuneral.FollowerMoveRoutine(followerById));
    }
    Interaction_TempleAltar.Instance.RitualLighting.gameObject.SetActive(true);
    ChurchFollowerManager.Instance.StartRitualOverlay();
    BiomeConstants.Instance.ChromaticAbberationTween(2f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 1f);
    BiomeConstants.Instance.VignetteTween(2f, BiomeConstants.Instance.VignetteDefaultValue, 0.7f);
    yield return (object) new WaitForSeconds(1.5f);
    FollowerBrain deadFollower = FollowerBrain.GetOrCreateBrain(deadFollowerInfo);
    DeadWorshipper deadWorshipper = (DeadWorshipper) null;
    StructuresData structure = StructuresData.GetInfoByType(StructureBrain.TYPES.DEAD_WORSHIPPER, 0);
    structure.FollowerID = deadFollower.Info.ID;
    StructureManager.BuildStructure(FollowerLocation.Church, structure, ChurchFollowerManager.Instance.RitualCenterPosition.position + Vector3.left * 0.4f, Vector2Int.one, false, (Action<GameObject>) (g =>
    {
      g.GetComponent<Interaction_HarvestMeat>().enabled = false;
      deadWorshipper = g.GetComponent<DeadWorshipper>();
      deadWorshipper.ItemIndicator.SetActive(false);
      deadWorshipper.RottenParticles.gameObject.SetActive(false);
      deadWorshipper.Spine.AnimationState.SetAnimation(0, "dead-funeral", true);
      deadWorshipper.Spine.skeleton.A = 0.0f;
      DOTween.To((DOGetter<float>) (() => deadWorshipper.Spine.skeleton.A), (DOSetter<float>) (x => deadWorshipper.Spine.skeleton.A = x), 1f, 0.5f);
    }), emitParticles: false);
    yield return (object) new WaitForSeconds(0.5f);
    AudioManager.Instance.PlayOneShot("event:/player/body_drop", ChurchFollowerManager.Instance.RitualCenterPosition.position);
    yield return (object) new WaitForSeconds(1.5f);
    Interaction_TempleAltar.Instance.CloseUpCamera.Reset();
    GameManager.GetInstance().OnConversationNext(ChurchFollowerManager.Instance.RitualCenterPosition.gameObject, 6f);
    GameManager.GetInstance().CamFollowTarget.targetDistance = 6.5f;
    yield return (object) new WaitForSeconds(1f);
    List<FollowerBrain> prayingFollowers = new List<FollowerBrain>();
    foreach (FollowerBrain followerBrain in Ritual.GetFollowersAvailableToAttendSermon())
    {
      if (followerBrain.Info.GetOrCreateRelationship(deadFollower.Info.ID).CurrentRelationshipState == IDAndRelationship.RelationshipState.Friends)
        prayingFollowers.Add(followerBrain);
    }
    bool waiting = true;
    List<FollowerBrain> followers = new List<FollowerBrain>((IEnumerable<FollowerBrain>) prayingFollowers);
    for (int i = 0; i < UnityEngine.Random.Range(2, 4); ++i)
    {
      if (followers.Count == 0)
      {
        followers = new List<FollowerBrain>((IEnumerable<FollowerBrain>) Ritual.GetFollowersAvailableToAttendSermon());
        for (int index = followers.Count - 1; index >= 0; --index)
        {
          if (prayingFollowers.Contains(followers[index]))
            followers.Remove(followers[index]);
        }
      }
      else
      {
        FollowerBrain b = followers[UnityEngine.Random.Range(0, followers.Count)];
        followers.Remove(b);
        Follower f = FollowerManager.FindFollowerByID(b.Info.ID);
        Vector3 startingPosition = b.LastPosition;
        float angle = Utils.GetAngle(b.LastPosition, ChurchFollowerManager.Instance.RitualCenterPosition.position);
        if (!((UnityEngine.Object) f == (UnityEngine.Object) null))
        {
          f.HoodOff();
          yield return (object) new WaitForSeconds(0.5f);
          waiting = true;
          ((FollowerTask_ManualControl) b.CurrentTask).GoToAndStop(f, deadWorshipper.transform.position - (Vector3) Utils.DegreeToVector2(angle) * UnityEngine.Random.Range(0.75f, 1f), (System.Action) (() => waiting = false));
          while (waiting)
            yield return (object) null;
          f.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
          double num = (double) f.SetBodyAnimation("action", true);
          AudioManager.Instance.PlayOneShot("event:/player/layer_clothes", f.gameObject);
          yield return (object) new WaitForSeconds(0.33f);
          AudioManager.Instance.PlayOneShot("event:/player/layer_clothes", f.gameObject);
          yield return (object) new WaitForSeconds(0.33f);
          AudioManager.Instance.PlayOneShot("event:/player/layer_clothes", f.gameObject);
          yield return (object) new WaitForSeconds(0.33f);
          deadWorshipper.Flowers[i].gameObject.SetActive(true);
          deadWorshipper.Flowers[i].transform.DOPunchScale(Vector3.one * 0.2f, 0.25f);
          AudioManager.Instance.PlayOneShot("event:/player/tall_grass_push", deadWorshipper.Flowers[i]);
          waiting = true;
          ((FollowerTask_ManualControl) b.CurrentTask).GoToAndStop(f, startingPosition, (System.Action) (() =>
          {
            waiting = false;
            f.State.CURRENT_STATE = StateMachine.State.Idle;
            f.State.facingAngle = Utils.GetAngle(f.transform.position, ChurchFollowerManager.Instance.RitualCenterPosition.position);
          }));
          while (waiting)
            yield return (object) null;
          f.HoodOn("pray", false);
          yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.2f, 0.5f));
          b = (FollowerBrain) null;
          startingPosition = new Vector3();
        }
      }
    }
    yield return (object) new WaitForSeconds(0.8333333f);
    DeadWorshipper ghost = (DeadWorshipper) null;
    StructureManager.BuildStructure(FollowerLocation.Church, structure, ChurchFollowerManager.Instance.RitualCenterPosition.position, Vector2Int.one, false, (Action<GameObject>) (g =>
    {
      g.GetComponent<Interaction_HarvestMeat>().enabled = false;
      ghost = g.GetComponent<DeadWorshipper>();
      ghost.SetOutfit(FollowerOutfitType.Worshipper);
      ghost.RottenParticles.gameObject.SetActive(false);
      ghost.Spine.AnimationState.SetAnimation(0, "ascend", false);
      ghost.ItemIndicator.SetActive(false);
      ghost.Rotted.gameObject.SetActive(false);
      ghost.Spine.gameObject.SetActive(true);
      AudioManager.Instance.PlayOneShot("event:/rituals/funeral_ghost", ghost.gameObject);
      ghost.Spine.skeleton.A = 0.5f;
    }), emitParticles: false);
    while ((UnityEngine.Object) ghost == (UnityEngine.Object) null)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNext(ghost.gameObject, 12f);
    yield return (object) new WaitForSeconds(0.5f);
    Interaction_TempleAltar.Instance.PulseDisplacementObject(Interaction_TempleAltar.Instance.PortalEffect.transform.position - Vector3.back);
    yield return (object) new WaitForSeconds(4f);
    float t = 0.0f;
    while ((double) t < 1.0)
    {
      t += Time.deltaTime;
      ghost.Spine.skeleton.A = Mathf.Lerp(0.5f, 0.0f, t / 1f);
      yield return (object) null;
    }
    Interaction_TempleAltar.Instance.CloseUpCamera.Play();
    UnityEngine.Object.Destroy((UnityEngine.Object) ghost.gameObject);
    waiting = true;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.4f, "", (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    PlayerFarming.Instance.Spine.gameObject.SetActive(false);
    BaseLocationManager.Instance.Activatable = false;
    ChurchLocationManager.Instance.Activatable = false;
    BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 0.0f, 0.0f);
    BiomeBaseManager.Instance.ActivateRoom(false);
    Vector3 camPosition = GameManager.GetInstance().CamFollowTarget.transform.position;
    GameManager.GetInstance().CamFollowTarget.ResetTargetCamera(0.0f);
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().CamFollowTarget.ClearAllTargets();
    Vector3 position = grave.Data.Position - GameManager.GetInstance().CamFollowTarget.transform.forward * 3f;
    if (grave is Structures_Crypt)
      position -= Vector3.forward;
    GameManager.GetInstance().CamFollowTarget.ForceSnapTo(position);
    GameManager.GetInstance().CamFollowTarget.transform.localRotation = Quaternion.Euler(-45f, 0.0f, 0.0f);
    ChurchFollowerManager.Instance.Sparkles.gameObject.SetActive(true);
    Vector3 SparklesStartPos = ChurchFollowerManager.Instance.Sparkles.transform.position;
    ChurchFollowerManager.Instance.Sparkles.transform.parent = BiomeConstants.Instance.transform;
    ChurchFollowerManager.Instance.Sparkles.transform.position = grave.Data.Position;
    ChurchFollowerManager.Instance.Sparkles.Play();
    AudioManager.Instance.PlayOneShot("event:/player/Curses/charm_curse", AudioManager.Instance.Listener);
    yield return (object) new WaitForSeconds(1.5f);
    deadFollower._directInfoAccess.HadFuneral = true;
    foreach (Grave grave1 in Grave.Graves)
    {
      if (grave1.StructureInfo.ID == grave.Data.ID)
      {
        grave1.SetGameObjects();
        AudioManager.Instance.PlayOneShot("event:/player/tall_grass_push", AudioManager.Instance.Listener);
        break;
      }
    }
    foreach (Interaction_Crypt crypt in Interaction_Crypt.Crypts)
    {
      if (crypt.StructureInfo.ID == grave.Data.ID)
      {
        crypt.SetGameObjects();
        AudioManager.Instance.PlayOneShot("event:/player/tall_grass_push", AudioManager.Instance.Listener);
        break;
      }
    }
    yield return (object) new WaitForSeconds(2.5f);
    waiting = true;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.4f, "", (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 1f, 200f);
    Transform transform = ChurchFollowerManager.Instance.Sparkles.transform;
    transform.position = SparklesStartPos;
    transform.parent = ChurchFollowerManager.Instance.transform;
    ChurchFollowerManager.Instance.Sparkles.gameObject.SetActive(false);
    GameManager.GetInstance().CamFollowTarget.targetDistance = 11f;
    GameManager.GetInstance().CamFollowTarget.ClearAllTargets();
    GameManager.GetInstance().CamFollowTarget.ForceSnapTo(camPosition);
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    BiomeBaseManager.Instance.ActivateChurch();
    PlayerFarming.Instance.Spine.gameObject.SetActive(true);
    BaseLocationManager.Instance.Activatable = true;
    ChurchLocationManager.Instance.Activatable = true;
    deadWorshipper.Spine.AnimationState.SetAnimation(0, "dead-funeral", true);
    deadWorshipper.ItemIndicator.gameObject.SetActive(false);
    FollowerBrain.AllBrains.Remove(deadFollower);
    foreach (FollowerBrain followerBrain in Ritual.GetFollowersAvailableToAttendSermon())
      FollowerManager.FindFollowerByID(followerBrain.Info.ID)?.HoodOn("pray", true);
    yield return (object) new WaitForSeconds(1f);
    foreach (FollowerBrain followerBrain in Ritual.GetFollowersAvailableToAttendSermon())
    {
      Follower followerById = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
      if ((bool) (UnityEngine.Object) followerById)
        ritualFuneral.StartCoroutine((IEnumerator) ritualFuneral.FollowerMoveRoutine(followerById));
    }
    AudioManager.Instance.PlayOneShot("event:/player/body_wrap");
    yield return (object) new WaitForSeconds(1f);
    deadWorshipper.transform.DOScale(0.0f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    DOTween.To((DOGetter<float>) (() => deadWorshipper.Spine.skeleton.A), (DOSetter<float>) (x => deadWorshipper.Spine.skeleton.A = x), 0.0f, 0.5f);
    yield return (object) new WaitForSeconds(0.5f);
    StructureManager.RemoveStructure((StructureBrain) StructureManager.GetAllStructuresOfType<Structures_DeadWorshipper>(FollowerLocation.Church)[0]);
    BiomeConstants.Instance.ChromaticAbberationTween(1f, 1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    BiomeConstants.Instance.VignetteTween(1f, 0.7f, BiomeConstants.Instance.VignetteDefaultValue);
    ChurchFollowerManager.Instance.EndRitualOverlay();
    GameManager.GetInstance().CamFollowTarget.targetDistance = 11f;
    Interaction_TempleAltar.Instance.RitualLighting.gameObject.SetActive(false);
    yield return (object) new WaitForSeconds(2f);
    float EndingDelay = 0.0f;
    yield return (object) null;
    foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
    {
      float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
      EndingDelay += Delay;
      ritualFuneral.StartCoroutine((IEnumerator) ritualFuneral.DelayFollowerReaction(brain, Delay));
      IDAndRelationship relationship = brain.Info.GetOrCreateRelationship(deadFollower.Info.ID);
      if (relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Enemies)
        brain.AddThought(Thought.AttendedEnemyFuneral);
      else if (relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Friends)
        brain.AddThought(Thought.AttendedFriendFuneral);
      else
        brain.AddThought(Thought.AttendedStrangerFuneral);
    }
    BiomeConstants.Instance.ChromaticAbberationTween(1f, 1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    BiomeConstants.Instance.VignetteTween(1f, 0.7f, BiomeConstants.Instance.VignetteDefaultValue);
    yield return (object) new WaitForSeconds(1f);
    ritualFuneral.EndRitual();
    ritualFuneral.CompleteRitual(targetFollowerID_1: deadFollower.Info.ID);
    yield return (object) new WaitForSeconds(1f);
    CultFaithManager.AddThought(Thought.Cult_Funeral, deadFollower.Info.ID);
  }

  public IEnumerator FollowerMoveRoutine(Follower follower)
  {
    Vector3 startingPosition = follower.Brain.LastPosition;
    float angle = Utils.GetAngle(follower.Brain.LastPosition, ChurchFollowerManager.Instance.RitualCenterPosition.position);
    FollowerTask_ManualControl task = new FollowerTask_ManualControl();
    follower.Brain.HardSwapToTask((FollowerTask) task);
    bool waiting = true;
    task.GoToAndStop(follower, ChurchFollowerManager.Instance.RitualCenterPosition.position - (Vector3) Utils.DegreeToVector2(angle) * UnityEngine.Random.Range(0.5f, 0.75f), (System.Action) (() => waiting = false));
    follower.SetOutfit(FollowerOutfitType.Worshipper, true);
    while (waiting)
      yield return (object) null;
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) follower.SetBodyAnimation("action", true);
    yield return (object) new WaitForSeconds(UnityEngine.Random.Range(1.5f, 2f));
    task.GoToAndStop(follower, startingPosition, (System.Action) (() =>
    {
      follower.State.facingAngle = Utils.GetAngle(follower.transform.position, ChurchFollowerManager.Instance.RitualCenterPosition.position);
      follower.State.CURRENT_STATE = StateMachine.State.Idle;
    }));
  }

  public void EndRitual()
  {
    AudioManager.Instance.StopLoop(this.loopedSound);
    Interaction_TempleAltar.Instance.RitualLighting.gameObject.SetActive(false);
    ChurchFollowerManager.Instance.EndRitualOverlay();
    GameManager.GetInstance().CamFollowTarget.targetDistance = 11f;
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
  }
}
