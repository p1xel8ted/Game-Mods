// Decompiled with JetBrains decompiler
// Type: Lamb.UI.JobBoardMenuItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI.Assets;
using Spine.Unity;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class JobBoardMenuItem : MonoBehaviour, IPoolListener
{
  public Action<JobBoardMenuItem> OnSelected;
  public Action<JobBoardMenuItem> OnHighlighted;
  [SerializeField]
  public MMButton button;
  [SerializeField]
  public Image outline;
  [Header("Details")]
  [SerializeField]
  public TextMeshProUGUI objectiveText;
  [SerializeField]
  public GameObject tracked;
  [SerializeField]
  public GameObject disabled;
  [Header("Containers")]
  [SerializeField]
  public GameObject requirementContainer;
  [SerializeField]
  public Transform rewardsContainer;
  [Header("Variants")]
  [SerializeField]
  public SkeletonGraphic spine;
  [SerializeField]
  public SkeletonGraphic ghostSpine;
  [SerializeField]
  public SkeletonGraphic tarot;
  [SerializeField]
  public Image iconImage;
  [SerializeField]
  public GameObject decoContainer;
  [SerializeField]
  public Image decoImage;
  [SerializeField]
  public TextMeshProUGUI rewardText;
  [SerializeField]
  public LayoutElement mainLayoutElement;
  [SerializeField]
  public Image completedAlreadyClaimedOverlay;
  [Header("Decoration Shop")]
  [SerializeField]
  public Transform decorationToBuildContainer;
  [SerializeField]
  public Image decorationToBuild;
  [SerializeField]
  public Transform decorationToPlaceContainer;
  [Header("Mapping")]
  [SerializeField]
  public FleeceIconMapping fleeceIconMapping;
  [SerializeField]
  public InventoryIconMapping iconMapping;
  [Header("Reward")]
  [SerializeField]
  public GameObject completedRewardAvailable;
  [SerializeField]
  public GameObject completedRewardAvailableOutline;
  [Header("polish")]
  [SerializeField]
  public Image trackFlash;
  [SerializeField]
  public Transform trackContainer;
  [SerializeField]
  public Transform trackParentContainer;
  [SerializeField]
  public Transform RewardContainer;
  [CompilerGenerated]
  public Interaction_JobBoard.JobData \u003CJobData\u003Ek__BackingField;
  [CompilerGenerated]
  public ObjectivesData \u003CObjective\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CCompleted\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CCompletedClaimedReward\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CIndex\u003Ek__BackingField;
  [CompilerGenerated]
  public UIJobBoardMenuController \u003CJobBoardMenuController\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CIsDisabled\u003Ek__BackingField;
  public bool _oldTrackstate;

  public MMButton MMButton => this.button;

  public Interaction_JobBoard.JobData JobData
  {
    get => this.\u003CJobData\u003Ek__BackingField;
    set => this.\u003CJobData\u003Ek__BackingField = value;
  }

  public ObjectivesData Objective
  {
    get => this.\u003CObjective\u003Ek__BackingField;
    set => this.\u003CObjective\u003Ek__BackingField = value;
  }

  public bool Completed
  {
    get => this.\u003CCompleted\u003Ek__BackingField;
    set => this.\u003CCompleted\u003Ek__BackingField = value;
  }

  public bool CompletedClaimedReward
  {
    get => this.\u003CCompletedClaimedReward\u003Ek__BackingField;
    set => this.\u003CCompletedClaimedReward\u003Ek__BackingField = value;
  }

  public int Index
  {
    get => this.\u003CIndex\u003Ek__BackingField;
    set => this.\u003CIndex\u003Ek__BackingField = value;
  }

  public UIJobBoardMenuController JobBoardMenuController
  {
    get => this.\u003CJobBoardMenuController\u003Ek__BackingField;
    set => this.\u003CJobBoardMenuController\u003Ek__BackingField = value;
  }

  public bool IsDisabled
  {
    get => this.\u003CIsDisabled\u003Ek__BackingField;
    set => this.\u003CIsDisabled\u003Ek__BackingField = value;
  }

  public void Awake()
  {
    this.MMButton.onClick.AddListener(new UnityAction(this.OnItemSelected));
    this.MMButton.OnSelected += new System.Action(this.OnItemHighlighted);
    this.MMButton.OnDeselected += new System.Action(this.OnItemUnhighlighted);
  }

  public void OnDestroy()
  {
    if (!((UnityEngine.Object) this.MMButton != (UnityEngine.Object) null))
      return;
    this.MMButton.onClick.RemoveAllListeners();
    this.MMButton.OnSelected -= new System.Action(this.OnItemHighlighted);
    this.MMButton.OnDeselected -= new System.Action(this.OnItemUnhighlighted);
  }

  public void Configure(
    Interaction_JobBoard.JobData jobData,
    UIJobBoardMenuController jobBoardMenuController,
    bool active,
    int index)
  {
    DOTweenModuleUI.DOFade(this.outline, 0.0f, 0.0f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this.JobData = jobData;
    this.JobBoardMenuController = jobBoardMenuController;
    this.Index = index;
    this.Objective = CreateObjective.GetObjective(jobData.Objective, jobData.GroupTitle);
    this.objectiveText.text = this.Objective.Text;
    this.spine.gameObject.SetActive(false);
    this.ghostSpine.gameObject.SetActive(false);
    this.iconImage.gameObject.SetActive(false);
    this.decoContainer.gameObject.SetActive(false);
    this.tarot.gameObject.SetActive(false);
    this.requirementContainer.SetActive(false);
    this.decorationToBuildContainer.gameObject.SetActive(false);
    if (this.Objective is Objectives_GetAnimal objective6)
    {
      this.requirementContainer.SetActive(true);
      this.spine.gameObject.SetActive(true);
      this.spine.ConfigureAnimal(new StructuresData.Ranchable_Animal()
      {
        Type = objective6.AnimalType,
        GrowthStage = 10,
        Age = 10,
        Level = 3
      });
      if (objective6.AnimalType == InventoryItem.ITEM_TYPE.ANIMAL_LLAMA)
        this.spine.transform.localScale = Vector3.one * 0.75f;
      else
        this.spine.transform.localScale = Vector3.one;
    }
    else if (this.Objective is Objectives_BuildStructure objective5)
    {
      Debug.Log((object) " Decorations Build Objective");
      Debug.Log((object) $"Objective: {objective5.Text}, Type: {objective5.GetType().Name}");
      this.requirementContainer.gameObject.SetActive(true);
      this.decorationToBuildContainer.gameObject.SetActive(true);
      this.decorationToBuild.sprite = TypeAndPlacementObjects.GetByType(objective5.StructureType).IconImage;
    }
    else if (this.Objective is Objectives_PlaceStructure objective4)
    {
      Debug.Log((object) " Decorations Place Objective");
      Debug.Log((object) $"Objective: {objective4.Text}, Type: {objective4.GetType().Name}");
      this.rewardsContainer.gameObject.SetActive(false);
      this.decorationToPlaceContainer.gameObject.SetActive(true);
    }
    else if (this.Objective is Objectives_LegendaryWeaponRun objective3)
    {
      this.rewardsContainer.gameObject.SetActive(true);
      this.requirementContainer.SetActive(true);
      this.iconImage.gameObject.SetActive(true);
      this.iconImage.sprite = EquipmentManager.GetEquipmentData(objective3.LegendaryWeapon).WorldSprite;
      this.mainLayoutElement.preferredWidth = 50f;
    }
    else if (this.Objective is Objectives_WinFlockadeBet objective2)
    {
      this.requirementContainer.SetActive(true);
      this.ghostSpine.gameObject.SetActive(true);
      if (objective2.OpponentTermId.Contains("NAMES/FlockadeNPC"))
      {
        this.ghostSpine.Skeleton.SetSkin("Flockade/LambWar");
        this.ghostSpine.AnimationState.SetAnimation(0, "Flockade/LambWar/idle", true);
      }
      else if (objective2.OpponentTermId.Contains("NAMES/Rancher"))
      {
        this.ghostSpine.Skeleton.SetSkin("Flockade/Farmer");
        this.ghostSpine.AnimationState.SetAnimation(0, "Flockade/Farmer/idle", true);
      }
      else if (objective2.OpponentTermId.Contains("NAMES/TarotNPC"))
      {
        this.ghostSpine.Skeleton.SetSkin("Flockade/TarotReader");
        this.ghostSpine.AnimationState.SetAnimation(0, "Flockade/TarotReader/idle", true);
      }
      else if (objective2.OpponentTermId.Contains("NAMES/BlacksmithNPC"))
      {
        this.ghostSpine.Skeleton.SetSkin("Flockade/Blacksmith");
        this.ghostSpine.AnimationState.SetAnimation(0, "Flockade/Blacksmith/idle", true);
      }
      else if (objective2.OpponentTermId.Contains("NAMES/DecoNPC"))
      {
        this.ghostSpine.Skeleton.SetSkin("Flockade/Merchant");
        this.ghostSpine.AnimationState.SetAnimation(0, "Flockade/Merchant/idle", true);
      }
      else if (objective2.OpponentTermId.Contains("NAMES/GraveyardNPC"))
      {
        this.ghostSpine.Skeleton.SetSkin("Flockade/Priest");
        this.ghostSpine.AnimationState.SetAnimation(0, "Flockade/Priest/idle", true);
      }
      this.ghostSpine.Skeleton.SetSlotsToSetupPose();
      this.ghostSpine.Skeleton.UpdateCache();
    }
    else if (this.Objective is Objectives_ShowFleece objective1)
    {
      this.requirementContainer.SetActive(true);
      this.iconImage.gameObject.SetActive(true);
      this.fleeceIconMapping.GetImage((int) objective1.FleeceType, this.iconImage);
      if (!DataManager.Instance.UnlockedFleeces.Contains((int) objective1.FleeceType))
      {
        Debug.Log((object) $"Fleece for Church Job is not yet unlocked: {objective1.FleeceType}");
        this.iconImage.color = Color.black;
        this.SetDisabled();
      }
    }
    ObjectivesData objective7 = (ObjectivesData) null;
    jobBoardMenuController.TrackingObjective(this, out objective7, true, true);
    if (objective7 != null)
    {
      if (objective7.IsComplete)
      {
        this.Completed = true;
        this.objectiveText.text = "<s>" + this.objectiveText.text;
      }
    }
    else if (this.Objective != null && this.Objective.TryComplete())
    {
      this.Completed = true;
      this.objectiveText.text = $"<s>{this.objectiveText.text}</s>";
    }
    else if (active)
    {
      ObjectivesDataFinalized objective8 = (ObjectivesDataFinalized) null;
      jobBoardMenuController.CompletedObjective(this, out objective8);
      if (objective8 != null)
      {
        this.Completed = true;
        this.objectiveText.text = "<s>" + this.objectiveText.text;
      }
    }
    this.completedRewardAvailable.gameObject.SetActive(this.Completed);
    this.completedRewardAvailableOutline.gameObject.SetActive(this.Completed);
    this.trackParentContainer.gameObject.SetActive(!this.Completed);
    this.RewardContainer.gameObject.SetActive(this.Completed);
    this.UpdateTracked();
    if (!active)
      this.SetDisabled();
    if (this.CompletedClaimedReward)
    {
      this.RewardContainer.gameObject.SetActive(false);
      this.completedRewardAvailable.gameObject.SetActive(false);
      DOTweenModuleUI.DOFade(this.completedAlreadyClaimedOverlay, 0.5f, 0.1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    }
    else
      DOTweenModuleUI.DOFade(this.completedAlreadyClaimedOverlay, 0.0f, 0.1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    if (!this.Completed && !this.CompletedClaimedReward)
      return;
    this.objectiveText.text = "<s>" + this.objectiveText.text;
    if (this.Objective is Objectives_WinFlockadeBet)
    {
      this.objectiveText.text = this.objectiveText.text.Replace("0", jobData.Objective.MinimumWoolBet.ToString());
    }
    else
    {
      if (!(this.Objective is Objectives_PlaceStructure))
        return;
      this.objectiveText.text = this.objectiveText.text.Replace("0", jobData.Objective.DecorationCount.ToString());
    }
  }

  public void SetNotCompleted()
  {
    this.objectiveText.text = this.Objective.Text;
    this.completedRewardAvailable.gameObject.SetActive(false);
    this.completedRewardAvailableOutline.gameObject.SetActive(false);
    this.trackParentContainer.gameObject.SetActive(true);
    this.RewardContainer.gameObject.SetActive(false);
  }

  public void SetCompleted()
  {
    this.objectiveText.text = $"<s>{this.objectiveText.text}</s>";
    this.completedRewardAvailable.gameObject.SetActive(true);
    this.completedRewardAvailableOutline.gameObject.SetActive(true);
    this.trackParentContainer.gameObject.SetActive(false);
    this.RewardContainer.gameObject.SetActive(true);
  }

  public void OnRecycled()
  {
    this.OnSelected = (Action<JobBoardMenuItem>) null;
    this.OnHighlighted = (Action<JobBoardMenuItem>) null;
    this.IsDisabled = false;
  }

  public void OnItemSelected()
  {
    if (!this.IsDisabled)
    {
      Action<JobBoardMenuItem> onSelected = this.OnSelected;
      if (onSelected != null)
        onSelected(this);
      this.UpdateTracked();
    }
    else
    {
      Action<JobBoardMenuItem> onSelected = this.OnSelected;
      if (onSelected == null)
        return;
      onSelected(this);
    }
  }

  public void OnItemUnhighlighted()
  {
    DOTweenModuleUI.DOFade(this.outline, 0.0f, 0.2f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this.StopAllCoroutines();
  }

  public void OnItemHighlighted()
  {
    DOTweenModuleUI.DOFade(this.outline, 1f, 0.2f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    Action<JobBoardMenuItem> onHighlighted = this.OnHighlighted;
    if (onHighlighted == null)
      return;
    onHighlighted(this);
  }

  public void UpdateTracked()
  {
    ObjectivesData objective = (ObjectivesData) null;
    bool tracked = this.JobBoardMenuController.TrackingObjective(this, out objective, true, true) && !this.Completed;
    this.tracked.gameObject.SetActive(tracked);
    this.JobBoardMenuController.InfoCardController.CurrentCard?.UpdateTracked(tracked);
    if (tracked == this._oldTrackstate)
      return;
    this._oldTrackstate = tracked;
    this.trackFlash.color = Color.white;
    this.trackFlash.DOKill();
    DOTweenModuleUI.DOFade(this.trackFlash, 0.0f, 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this.trackContainer.DOComplete();
    this.trackContainer.DOPunchPosition(new Vector3(0.0f, -10f, 0.0f), 0.25f).SetUpdate<Tweener>(true);
    this.trackContainer.DOPunchRotation(new Vector3(0.0f, 0.0f, 6f), 0.25f).SetUpdate<Tweener>(true);
  }

  public void SetDisabled()
  {
    this.OnItemUnhighlighted();
    this.IsDisabled = true;
    this.disabled.SetActive(true);
    this.MMButton.Interactable = false;
    this.iconImage.color = Color.black;
    this.tarot.color = Color.black;
    this.spine.color = Color.black;
    this.decoImage.color = Color.black;
    this.ghostSpine.color = Color.black;
  }

  public IEnumerator UnlockSequenceIE()
  {
    UIManager.PlayAudio("event:/ui/task_completed");
    this.SetCompleted();
    this.trackFlash.color = StaticColors.GreenColor;
    this.trackFlash.DOKill();
    DOTweenModuleUI.DOFade(this.trackFlash, 0.0f, 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true).SetDelay<TweenerCore<Color, Color, ColorOptions>>(0.2f);
    yield return (object) new WaitForSeconds(0.2f);
  }
}
