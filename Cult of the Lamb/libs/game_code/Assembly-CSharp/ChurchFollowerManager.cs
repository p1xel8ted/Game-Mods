// Decompiled with JetBrains decompiler
// Type: ChurchFollowerManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
  public LightingManagerVolume RedLightingVolume;
  public LightingManagerVolume NormalLightingVolume;
  public Interaction_FeastTable FeastTable;
  public Interaction_FeastTable DrinkTable;
  public Interaction_FireDancePit FirePit;
  public SkeletonGraphic RitualOverlay;
  public SkeletonGraphic SermonOverlay;
  public SkeletonGraphic SacrificeOverlay;
  public GameObject FireOverlay;
  public SkeletonAnimation RitualEffect;
  public SkeletonAnimation SermonEffect;
  public SkeletonAnimation PortalEffect;
  public SkeletonAnimation SacrificeTentacles;
  public GameObject Light;
  public GameObject GodRays;
  public GameObject Mushrooms;
  public Animator Goop;
  public GameObject Water;
  public GameObject FarmMud;
  public GameObject Snow;
  public ParticleSystem Sparkles;
  public GameObject Ring;
  public GameObject RanchableAnimal;
  public CanvasGroup RedOverlay;
  [Space]
  [SerializeField]
  public GameObject[] studySlots;
  public int randomNumber;
  public List<int> _audienceMemberBrainIDs = new List<int>();
  public Vector3[] _audiencePositions = new Vector3[36]
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
    new Vector3(1.5f, -1.5f),
    new Vector3(-0.0f, -2.5f),
    new Vector3(-1f, -2.5f),
    new Vector3(1f, -2.5f),
    new Vector3(-2f, -2.5f),
    new Vector3(2f, -2.5f),
    new Vector3(-0.5f, -3.5f),
    new Vector3(0.5f, -3.5f),
    new Vector3(-1.5f, -3.5f),
    new Vector3(1.5f, -3.5f),
    new Vector3(-0.0f, -4.5f),
    new Vector3(-1f, -4.5f),
    new Vector3(1f, -4.5f),
    new Vector3(-2f, -4.5f),
    new Vector3(2f, -4.5f)
  };

  public GameObject[] StudySlots => this.studySlots;

  public void Awake() => ChurchFollowerManager.Instance = this;

  public void OnDestroy() => ChurchFollowerManager.Instance = (ChurchFollowerManager) null;

  public void Start()
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
    this.FireOverlay.gameObject.SetActive(false);
  }

  public void DisableAllEffects()
  {
    this.RitualEffect.gameObject.SetActive(false);
    this.SermonEffect.gameObject.SetActive(false);
    this.FireOverlay.gameObject.SetActive(false);
  }

  public void PlayOverlay(
    ChurchFollowerManager.OverlayType overlayType,
    string AnimationName,
    string addAnim = "",
    bool useUnscaledTime = false)
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
    if (!string.IsNullOrEmpty(addAnim))
      skeletonGraphic.AnimationState.AddAnimation(0, addAnim, true, 0.0f);
    skeletonGraphic.unscaledTime = useUnscaledTime;
    skeletonGraphic.AnimationState.Complete += new Spine.AnimationState.TrackEntryDelegate(this.OverlayComplete);
  }

  public void StartFireOverlay()
  {
    this.FireOverlay.gameObject.SetActive(true);
    foreach (SpriteRenderer componentsInChild in this.FireOverlay.GetComponentsInChildren<SpriteRenderer>())
    {
      componentsInChild.color = new Color(1f, 0.0f, 0.0f, 0.0f);
      componentsInChild.DOColor(new Color(1f, 0.0f, 0.0f, 1f), 1f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    }
  }

  public void EndFireOverlay()
  {
    foreach (SpriteRenderer componentsInChild in this.FireOverlay.GetComponentsInChildren<SpriteRenderer>())
      componentsInChild.DOColor(new Color(1f, 0.0f, 0.0f, 0.0f), 1f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() => this.FireOverlay.gameObject.SetActive(false)));
  }

  public void SetOverlayCanvasOrder(int order)
  {
    this.RitualOverlay.GetComponentInParent<Canvas>().sortingOrder = order;
  }

  public void StartRitualOverlay()
  {
    this.RitualOverlay.gameObject.SetActive(true);
    this.RitualOverlay.AnimationState.SetAnimation(0, "ritual-in", false);
    this.RitualOverlay.AnimationState.AddAnimation(0, "ritual-loop", true, 0.0f);
  }

  public void EndRitualOverlay()
  {
    if (!((UnityEngine.Object) this.RitualOverlay != (UnityEngine.Object) null) || this.RitualOverlay.AnimationState == null)
      return;
    this.RitualOverlay.AnimationState.AddAnimation(0, "ritual-out", false, 0.0f).Complete += new Spine.AnimationState.TrackEntryDelegate(this.OverlayComplete);
  }

  public void StartSermonEffect()
  {
    this.randomNumber = UnityEngine.Random.Range(1, 5);
    this.SermonEffect.gameObject.SetActive(true);
    this.SermonEffect.AnimationState.SetAnimation(0, $"sermons/{this.randomNumber.ToString()}-in", false);
    this.SermonEffect.AnimationState.AddAnimation(0, $"sermons/{this.randomNumber.ToString()}-loop", true, 0.0f);
    this.SermonOverlay.gameObject.SetActive(true);
    this.SermonOverlay.AnimationState.SetAnimation(0, "sermon-start", false);
    this.SermonOverlay.AnimationState.AddAnimation(0, "sermon-loop", true, 0.0f);
  }

  public void EndSermonEffect()
  {
    if ((UnityEngine.Object) this.SermonEffect != (UnityEngine.Object) null && this.SermonEffect.AnimationState != null)
      this.SermonEffect.AnimationState.AddAnimation(0, $"sermons/{this.randomNumber.ToString()}-out", false, 0.0f).Complete += new Spine.AnimationState.TrackEntryDelegate(this.OverlayComplete);
    if (!((UnityEngine.Object) this.SermonOverlay != (UnityEngine.Object) null) || this.SermonOverlay.AnimationState == null)
      return;
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

  public void OverlayComplete(TrackEntry trackEntry)
  {
    trackEntry.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.OverlayComplete);
  }

  public void EffectComplete(TrackEntry trackEntry)
  {
    trackEntry.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.EffectComplete);
    this.DisableAllEffects();
  }

  public void PortalEffectComplete(TrackEntry trackEntry)
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
    int index1 = this._audienceMemberBrainIDs.IndexOf(brain.Info.ID);
    if (index1 == -1)
    {
      this.AddBrainToAudience(brain);
      index1 = this._audienceMemberBrainIDs.IndexOf(brain.Info.ID);
    }
    Vector3 vector3;
    if (index1 != -1 && index1 < this._audiencePositions.Length)
    {
      vector3 = this._audiencePositions[index1];
    }
    else
    {
      int index2 = index1 - this._audiencePositions.Length - 1;
      if (index2 != -1 && index2 < this._audiencePositions.Length)
      {
        Vector3 audiencePosition = this._audiencePositions[index2];
        vector3 = (Vector3) new Vector2(audiencePosition.x + UnityEngine.Random.Range(-0.25f, 0.25f), audiencePosition.y - 0.5f);
      }
      else
        vector3 = (Vector3) new Vector2(UnityEngine.Random.Range(-2.5f, 2.5f), UnityEngine.Random.Range(-4.5f, 2.5f));
    }
    return this.RitualCenterPosition.position + vector3 + new Vector3(0.0f, -0.5f);
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

  [CompilerGenerated]
  public void \u003CEndFireOverlay\u003Eb__44_0() => this.FireOverlay.gameObject.SetActive(false);

  public enum OverlayType
  {
    Ritual,
    Sermon,
    Sacrifice,
    Fire,
  }

  public enum EffectType
  {
    Ritual,
    Sermon,
  }
}
