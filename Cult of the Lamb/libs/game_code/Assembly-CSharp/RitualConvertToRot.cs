// Decompiled with JetBrains decompiler
// Type: RitualConvertToRot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using UnityEngine;

#nullable disable
public class RitualConvertToRot : Ritual
{
  public FollowerBrain resurrectingFollower;
  public Light RitualLight;

  public override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_ConvertToRot;

  public override void Play()
  {
    base.Play();
    this.StartCoroutine((IEnumerator) this.RitualRoutine());
  }

  public IEnumerator RitualRoutine()
  {
    RitualConvertToRot ritualConvertToRot = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    AudioManager.Instance.PlayOneShot("event:/dlc/ritual/decay_start");
    yield return (object) ritualConvertToRot.StartCoroutine((IEnumerator) ritualConvertToRot.CentreAndAnimatePlayer());
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    Debug.Log((object) "Ritual sacrifice begin gather");
    yield return (object) ritualConvertToRot.StartCoroutine((IEnumerator) ritualConvertToRot.WaitFollowersFormCircle());
    Debug.Log((object) "Ritual sacrifice end gather");
    SimulationManager.Pause();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-start", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("rituals/ritual-loop", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(1f);
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    GameManager.GetInstance().StartCoroutine((IEnumerator) ritualConvertToRot.DoRessurectRoutine());
  }

  public IEnumerator DoRessurectRoutine()
  {
    RitualConvertToRot ritualConvertToRot = this;
    if ((Object) ritualConvertToRot.RitualLight != (Object) null)
    {
      ritualConvertToRot.RitualLight.gameObject.SetActive(true);
      ritualConvertToRot.RitualLight.color = Color.black;
      ritualConvertToRot.RitualLight.DOColor(Color.red, 1f);
      ritualConvertToRot.RitualLight.DOIntensity(1.5f, 1f);
    }
    ChurchFollowerManager.Instance.PlayOverlay(ChurchFollowerManager.OverlayType.Ritual, "resurrect");
    yield return (object) new WaitForSeconds(0.5f);
    AudioManager.Instance.PlayOneShot("event:/dlc/ritual/decay_portal");
    ritualConvertToRot.PlaySacrificePortalEffect();
    Interaction_TempleAltar.Instance.PulseDisplacementObject(Interaction_TempleAltar.Instance.PortalEffect.transform.position);
    BiomeConstants.Instance.ChromaticAbberationTween(1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 7f);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain != null && allBrain.CurrentTask != null && allBrain.CurrentTask is FollowerTask_AttendRitual currentTask)
        currentTask.WorshipTentacle();
    }
    yield return (object) new WaitForSeconds(0.5f);
    AudioManager.Instance.PlayOneShot("event:/rituals/resurrect");
    FollowerInfo info = FollowerInfo.NewCharacter(FollowerLocation.Church);
    ritualConvertToRot.resurrectingFollower = FollowerBrain.GetOrCreateBrain(info);
    FollowerTask_ManualControl nextTask = new FollowerTask_ManualControl();
    ritualConvertToRot.resurrectingFollower.HardSwapToTask((FollowerTask) nextTask);
    ritualConvertToRot.resurrectingFollower.Location = FollowerLocation.Church;
    ritualConvertToRot.resurrectingFollower.DesiredLocation = FollowerLocation.Church;
    ritualConvertToRot.resurrectingFollower.CurrentTask.Arrive();
    Follower revivedFollower = FollowerManager.CreateNewFollower(ritualConvertToRot.resurrectingFollower._directInfoAccess, ChurchFollowerManager.Instance.RitualCenterPosition.position);
    revivedFollower.AddTrait(FollowerTrait.TraitType.Mutated);
    revivedFollower.SetOutfit(FollowerOutfitType.Follower, false);
    revivedFollower.Brain.CheckChangeState();
    revivedFollower.HideAllFollowerIcons();
    revivedFollower.Interaction_FollowerInteraction.eventListener.SetPitchAndVibrator(ritualConvertToRot.resurrectingFollower._directInfoAccess.follower_pitch, ritualConvertToRot.resurrectingFollower._directInfoAccess.follower_vibrato, ritualConvertToRot.resurrectingFollower._directInfoAccess.ID);
    if (!revivedFollower.Brain.HasTrait(FollowerTrait.TraitType.ExistentialDread))
      Ritual.FollowerToAttendSermon.Add(revivedFollower.Brain);
    revivedFollower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    GameManager.GetInstance().OnConversationNext(revivedFollower.gameObject, 5f);
    revivedFollower.Spine.gameObject.SetActive(false);
    yield return (object) new WaitForSeconds(1f);
    revivedFollower.Spine.gameObject.SetActive(true);
    GameManager.GetInstance().OnConversationNext(revivedFollower.gameObject, 8f);
    double num1 = (double) revivedFollower.SetBodyAnimation("Sermons/resurrect", false);
    revivedFollower.AddBodyAnimation("Reactions/react-enlightened1", false, 0.0f);
    revivedFollower.AddBodyAnimation("idle", true, 0.0f);
    yield return (object) new WaitForSeconds(2f);
    Interaction_TempleAltar.Instance.PulseDisplacementObject(revivedFollower.transform.position);
    yield return (object) new WaitForSeconds(2f);
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-stop", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(0.6666667f);
    revivedFollower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_AttendTeaching());
    float num2 = 0.5f;
    foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
    {
      float Delay = Random.Range(0.1f, 0.5f);
      num2 += Delay;
      ritualConvertToRot.StartCoroutine((IEnumerator) ritualConvertToRot.DelayFollowerReaction(brain, Delay));
      brain.AddThought(Thought.ConvertToRot);
    }
    yield return (object) new WaitForSeconds(1f);
    BiomeConstants.Instance.ChromaticAbberationTween(0.25f, 7f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    ritualConvertToRot.EndRitual();
    yield return (object) new WaitForSeconds(0.5f);
    ritualConvertToRot.CompleteRitual(targetFollowerID_1: revivedFollower.Brain.Info.ID);
    yield return (object) new WaitForSeconds(1f);
    CultFaithManager.AddThought(Thought.Cult_ConvertToRot, revivedFollower.Brain.Info.ID);
  }

  public void EndRitual()
  {
    this.StopSacrificePortalEffect();
    ChurchFollowerManager.Instance.StopSacrificePortalEffect();
  }
}
