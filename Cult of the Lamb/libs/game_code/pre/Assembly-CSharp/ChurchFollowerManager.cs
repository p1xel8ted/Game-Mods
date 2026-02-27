// Decompiled with JetBrains decompiler
// Type: ChurchFollowerManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ChurchFollowerManager : BaseMonoBehaviour
{
  public static ChurchFollowerManager Instance;
  public DevotionCounterOverlay devotionCounterOverlay;
  public Transform DoorPosition;
  public Transform RitualCenterPosition;
  public Transform WatchSermonPosition;
  public Transform AltarPosition;
  public GameObject Altar;
  public GameObject RitualCameraPosition;
  public Gradient ColorGradient;
  public Interaction_FeastTable FeastTable;
  public Interaction_FireDancePit FirePit;
  public SkeletonGraphic RitualOverlay;
  public SkeletonGraphic SermonOverlay;
  public SkeletonGraphic SacrificeOverlay;
  public SkeletonAnimation RitualEffect;
  public SkeletonAnimation SermonEffect;
  public SkeletonAnimation PortalEffect;
  public GameObject Light;
  public GameObject GodRays;
  public GameObject Mushrooms;
  public Animator Goop;
  public GameObject Water;
  public GameObject FarmMud;
  public ParticleSystem Sparkles;
  public CanvasGroup RedOverlay;
  [Space]
  [SerializeField]
  private GameObject[] studySlots;
  private int randomNumber;
  private List<int> _audienceMemberBrainIDs = new List<int>();
  private Vector3[] _audiencePositions = new Vector3[22]
  {
    new Vector3(-0.5f, 2.5f),
    new Vector3(0.5f, 2.5f),
    new Vector3(-1.5f, 2.5f),
    new Vector3(1.5f, 2.5f),
    new Vector3(0.0f, 1.5f),
    new Vector3(-1f, 1.5f),
    new Vector3(1f, 1.5f),
    new Vector3(-2f, 1.5f),
    new Vector3(2f, 1.5f),
    new Vector3(-0.5f, 0.5f),
    new Vector3(0.5f, 0.5f),
    new Vector3(-1.5f, 0.5f),
    new Vector3(1.5f, 0.5f),
    new Vector3(0.0f, -0.5f),
    new Vector3(-1f, -0.5f),
    new Vector3(1f, -0.5f),
    new Vector3(-2f, -0.5f),
    new Vector3(2f, -0.5f),
    new Vector3(-0.5f, -1.5f),
    new Vector3(0.5f, -1.5f),
    new Vector3(-1.5f, -1.5f),
    new Vector3(1.5f, -1.5f)
  };

  public GameObject[] StudySlots => this.studySlots;

  private void Awake() => ChurchFollowerManager.Instance = this;

  private void OnDestroy() => ChurchFollowerManager.Instance = (ChurchFollowerManager) null;

  private void Start()
  {
    this.DisableAllOverlays();
    this.DisableAllEffects();
    this.PortalEffect.gameObject.SetActive(false);
    this.GodRays.SetActive(false);
  }

  public void DisableAllOverlays()
  {
    this.RitualOverlay.gameObject.SetActive(false);
    this.SermonOverlay.gameObject.SetActive(false);
    this.SacrificeOverlay.gameObject.SetActive(false);
    this.GodRays.gameObject.SetActive(false);
  }

  private void DisableAllEffects()
  {
    this.RitualEffect.gameObject.SetActive(false);
    this.SermonEffect.gameObject.SetActive(false);
  }

  public void PlayOverlay(ChurchFollowerManager.OverlayType overlayType, string AnimationName)
  {
    SkeletonGraphic skeletonGraphic = (SkeletonGraphic) null;
    switch (overlayType)
    {
      case ChurchFollowerManager.OverlayType.Ritual:
        skeletonGraphic = this.RitualOverlay;
        break;
      case ChurchFollowerManager.OverlayType.Sermon:
        skeletonGraphic = this.SermonOverlay;
        break;
      case ChurchFollowerManager.OverlayType.Sacrifice:
        skeletonGraphic = this.SacrificeOverlay;
        break;
    }
    skeletonGraphic.gameObject.SetActive(true);
    skeletonGraphic.AnimationState.SetAnimation(0, AnimationName, false);
    skeletonGraphic.AnimationState.Complete += new Spine.AnimationState.TrackEntryDelegate(this.OverlayComplete);
  }

  public void StartRitualOverlay()
  {
    this.RitualOverlay.gameObject.SetActive(true);
    this.RitualOverlay.AnimationState.SetAnimation(0, "ritual-in", false);
    this.RitualOverlay.AnimationState.AddAnimation(0, "ritual-loop", true, 0.0f);
  }

  public void EndRitualOverlay()
  {
    this.RitualOverlay.AnimationState.AddAnimation(0, "ritual-out", false, 0.0f).Complete += new Spine.AnimationState.TrackEntryDelegate(this.OverlayComplete);
  }

  public void StartSermonEffect()
  {
    this.randomNumber = UnityEngine.Random.Range(1, 5);
    this.SermonEffect.gameObject.SetActive(true);
    this.SermonEffect.AnimationState.SetAnimation(0, $"sermons/{(object) this.randomNumber}-in", false);
    this.SermonEffect.AnimationState.AddAnimation(0, $"sermons/{(object) this.randomNumber}-loop", true, 0.0f);
    this.SermonOverlay.gameObject.SetActive(true);
    this.SermonOverlay.AnimationState.SetAnimation(0, "sermon-start", false);
    this.SermonOverlay.AnimationState.AddAnimation(0, "sermon-loop", true, 0.0f);
  }

  public void EndSermonEffect()
  {
    this.SermonEffect.AnimationState.AddAnimation(0, $"sermons/{(object) this.randomNumber}-out", false, 0.0f).Complete += new Spine.AnimationState.TrackEntryDelegate(this.OverlayComplete);
    this.SermonOverlay.AnimationState.AddAnimation(0, "sermon-stop", false, 0.0f);
  }

  public void StartSermonEffectClean()
  {
    this.SermonEffect.gameObject.SetActive(true);
    this.SermonEffect.AnimationState.SetAnimation(0, "clean-in", false);
    this.SermonEffect.AnimationState.AddAnimation(0, "clean-loop", true, 0.0f);
    this.SermonOverlay.gameObject.SetActive(true);
    this.SermonOverlay.AnimationState.SetAnimation(0, "sermon-start", false);
    this.SermonOverlay.AnimationState.AddAnimation(0, "sermon-loop", true, 0.0f);
  }

  public void EndSermonEffectClean()
  {
    this.SermonEffect.AnimationState.AddAnimation(0, "clean-out", false, 0.0f).Complete += new Spine.AnimationState.TrackEntryDelegate(this.OverlayComplete);
    this.SermonOverlay.AnimationState.AddAnimation(0, "sermon-stop", false, 0.0f);
  }

  public void StartSermonOverlay()
  {
    this.SermonOverlay.gameObject.SetActive(true);
    this.SermonOverlay.AnimationState.SetAnimation(0, "1", true);
  }

  public void EndSermonOverlay()
  {
    this.SermonOverlay.AnimationState.AddAnimation(0, "1", false, 0.0f).Complete += new Spine.AnimationState.TrackEntryDelegate(this.OverlayComplete);
  }

  public void PlayEffect(ChurchFollowerManager.EffectType effectType, string AnimationName)
  {
    SkeletonAnimation skeletonAnimation = (SkeletonAnimation) null;
    switch (effectType)
    {
      case ChurchFollowerManager.EffectType.Ritual:
        skeletonAnimation = this.RitualEffect;
        break;
      case ChurchFollowerManager.EffectType.Sermon:
        skeletonAnimation = this.SermonEffect;
        break;
    }
    skeletonAnimation.gameObject.SetActive(true);
    skeletonAnimation.AnimationState.SetAnimation(0, AnimationName, false);
    skeletonAnimation.AnimationState.Complete += new Spine.AnimationState.TrackEntryDelegate(this.EffectComplete);
  }

  public void UpdateChurch()
  {
    int index = -1;
    while (++index < this.studySlots.Length)
    {
      if (index <= 1)
        this.studySlots[index].SetActive(DataManager.Instance.HasBuiltTemple4);
      else if (index > 1 && index <= 3)
        this.studySlots[index].SetActive(UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Temple_MonksUpgrade));
    }
  }

  public void PlayPortalEffect()
  {
    this.PortalEffect.gameObject.SetActive(true);
    this.PortalEffect.AnimationState.SetAnimation(0, "start-ritual", false);
    this.PortalEffect.AnimationState.AddAnimation(0, "loop-ritual", true, 0.0f);
  }

  public void StopPortalEffect()
  {
    this.PortalEffect.AnimationState.SetAnimation(0, "stop-ritual", false);
    this.PortalEffect.AnimationState.Complete += new Spine.AnimationState.TrackEntryDelegate(this.PortalEffectComplete);
  }

  public void PlaySacrificePortalEffect()
  {
    this.PortalEffect.gameObject.SetActive(true);
    this.PortalEffect.AnimationState.SetAnimation(0, "start", false);
    this.PortalEffect.AnimationState.AddAnimation(0, "animation", true, 0.0f);
  }

  public void StopSacrificePortalEffect()
  {
    this.PortalEffect.AnimationState.SetAnimation(0, "stop", false);
    this.PortalEffect.AnimationState.Complete += new Spine.AnimationState.TrackEntryDelegate(this.PortalEffectComplete);
  }

  private void OverlayComplete(TrackEntry trackEntry)
  {
    trackEntry.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.OverlayComplete);
  }

  private void EffectComplete(TrackEntry trackEntry)
  {
    trackEntry.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.EffectComplete);
    this.DisableAllEffects();
  }

  private void PortalEffectComplete(TrackEntry trackEntry)
  {
    this.PortalEffect.AnimationState.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.PortalEffectComplete);
    trackEntry.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.EffectComplete);
    this.PortalEffect.gameObject.SetActive(false);
  }

  public Vector3 GetSlotPosition(int index) => this.studySlots[index].transform.position;

  public int GetClosestSlotIndex(Vector3 pos)
  {
    GameObject studySlot = this.studySlots[0];
    int closestSlotIndex = 0;
    for (int index = 0; index < this.studySlots.Length; ++index)
    {
      if ((double) Vector3.Distance(this.studySlots[index].transform.position, pos) < (double) Vector3.Distance(this.studySlots[index].transform.position, studySlot.transform.position))
      {
        studySlot = this.studySlots[index];
        closestSlotIndex = index;
      }
    }
    return closestSlotIndex;
  }

  public void ExitAllFollowers()
  {
  }

  public void AddBrainToAudience(FollowerBrain brain)
  {
    if (this._audienceMemberBrainIDs.Contains(brain.Info.ID))
      return;
    this._audienceMemberBrainIDs.Add(brain.Info.ID);
  }

  public Vector3 GetAudienceMemberPosition(FollowerBrain brain)
  {
    int index = this._audienceMemberBrainIDs.IndexOf(brain.Info.ID);
    if (index == -1)
    {
      this.AddBrainToAudience(brain);
      index = this._audienceMemberBrainIDs.IndexOf(brain.Info.ID);
    }
    return this.RitualCenterPosition.position + (index == -1 || index >= this._audiencePositions.Length ? this._audiencePositions[this._audiencePositions.Length - 1] : this._audiencePositions[index]) + new Vector3(0.0f, -0.5f);
  }

  public void ClearAudienceBrains() => this._audienceMemberBrainIDs.Clear();

  public void RemoveBrainFromAudience(FollowerBrain brain)
  {
    if (!this._audienceMemberBrainIDs.Contains(brain.Info.ID))
      return;
    this._audienceMemberBrainIDs.Remove(brain.Info.ID);
  }

  public Vector3 GetCirclePosition(FollowerBrain brain)
  {
    int num1 = this._audienceMemberBrainIDs.IndexOf(brain.Info.ID);
    if (this._audienceMemberBrainIDs.Count <= 12)
    {
      float num2 = 2f;
      float f = (float) ((double) num1 * (360.0 / (double) this._audienceMemberBrainIDs.Count) * (Math.PI / 180.0));
      return this.RitualCenterPosition.position + new Vector3(num2 * Mathf.Cos(f), num2 * Mathf.Sin(f));
    }
    int b = 8;
    float num3;
    float f1;
    if (num1 < b)
    {
      num3 = 2f;
      f1 = (float) ((double) num1 * (360.0 / (double) Mathf.Min(this._audienceMemberBrainIDs.Count, b)) * (Math.PI / 180.0));
    }
    else
    {
      num3 = 3f;
      f1 = (float) ((double) (num1 - b) * (360.0 / (double) (this._audienceMemberBrainIDs.Count - b)) * (Math.PI / 180.0));
    }
    return this.RitualCenterPosition.position + new Vector3(num3 * Mathf.Cos(f1), num3 * Mathf.Sin(f1));
  }

  public IEnumerator DoSacrificeRoutine(
    Interaction interaction,
    int sacrificeID,
    System.Action onComplete)
  {
    yield return (object) null;
  }

  public enum OverlayType
  {
    Ritual,
    Sermon,
    Sacrifice,
  }

  public enum EffectType
  {
    Ritual,
    Sermon,
  }
}
