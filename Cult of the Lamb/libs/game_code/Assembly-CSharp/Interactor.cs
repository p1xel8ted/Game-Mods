// Decompiled with JetBrains decompiler
// Type: Interactor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMBiomeGeneration;
using MMTools;
using src.UINavigator;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

#nullable disable
public class Interactor : BaseMonoBehaviour
{
  public Interaction CurrentInteraction;
  public Interaction PreviousInteraction;
  public Interaction TempInteraction;
  public float CurrentDist;
  public float CurrentPriorityWeight;
  public float TestDist;
  public PlayerFarming playerFarming;
  public RectTransform IndicatorRT;
  public StateMachine state;
  public float Delay;
  public int defaultButton = 9;
  public int secondaryButton = 68;
  public int tertiaryButton = 67;
  public int quaternaryButton = 66;
  public int FrameIntervalOffset;
  public int UpdateInterval = 2;
  public Vector2 _axes;
  [CompilerGenerated]
  public static bool \u003COverriding\u003Ek__BackingField = false;
  public static bool UseInteractionCulling = true;
  public const float COOP_ACTIVATION_DISTANCE_MODIFIER = 0.25f;
  [CompilerGenerated]
  public Indicator \u003Cindicator\u003Ek__BackingField;
  public bool isPrisoner;
  public static float interactionFrame;
  public static bool UseRegions = false;
  public static HashSet<Interaction> allVisibleInteractions = new HashSet<Interaction>();
  public static FollowerLocation[] IgnoreCoopDistanceModifiers = new FollowerLocation[5]
  {
    FollowerLocation.DLC_ShrineRoom,
    FollowerLocation.DecorationShop_Inside,
    FollowerLocation.Flockade_Inside,
    FollowerLocation.Blacksmith_Inside,
    FollowerLocation.TarotShop_Inside
  };
  public float distanceModifier;
  public FollowerLocation lastLocationCheck;
  public static Dictionary<Vector3Int, List<Interaction>> InteractionRegions = new Dictionary<Vector3Int, List<Interaction>>();
  public const int RegionSize = 3;

  public static bool Overriding
  {
    get => Interactor.\u003COverriding\u003Ek__BackingField;
    set => Interactor.\u003COverriding\u003Ek__BackingField = value;
  }

  public Indicator indicator
  {
    get => this.\u003Cindicator\u003Ek__BackingField;
    set => this.\u003Cindicator\u003Ek__BackingField = value;
  }

  public void Start() => this.Init();

  public void ResetPrompts()
  {
    if (!(bool) (UnityEngine.Object) this.indicator)
      return;
    this.indicator.primaryControlPrompt.Category = 0;
    this.indicator.primaryControlPrompt.Action = this.defaultButton;
    this.indicator.secondaryControlPrompt.Category = 0;
    this.indicator.secondaryControlPrompt.Action = this.secondaryButton;
    this.indicator.thirdControlPrompt.Category = 0;
    this.indicator.thirdControlPrompt.Action = this.tertiaryButton;
    this.indicator.fourthControlPrompt.Category = 0;
    this.indicator.fourthControlPrompt.Action = this.quaternaryButton;
  }

  public void OnDestroy()
  {
    if ((bool) (UnityEngine.Object) this.indicator && this.isPrisoner)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.indicator.gameObject);
    Interactor.Overriding = false;
  }

  public void OnEnable() => this.Init();

  public void Init()
  {
    this.playerFarming = this.gameObject.GetComponent<PlayerFarming>();
    if ((UnityEngine.Object) this.state == (UnityEngine.Object) null)
      this.state = this.gameObject.GetComponent<StateMachine>();
    this.FrameIntervalOffset = UnityEngine.Random.Range(0, this.UpdateInterval);
    this.ResetPrompts();
    if ((bool) (UnityEngine.Object) this.playerFarming)
      this.indicator = this.playerFarming.indicator;
    else if ((UnityEngine.Object) this.indicator == (UnityEngine.Object) null && (UnityEngine.Object) HUD_Manager.Instance != (UnityEngine.Object) null)
      this.indicator = HUD_Manager.Instance.InitialiseIndicator();
    if ((bool) (UnityEngine.Object) this.indicator)
    {
      this.IndicatorRT = this.indicator.RectTransform;
      this.indicator.HideTopInfo();
    }
    Interactor.UseInteractionCulling = SceneManager.GetActiveScene().name == "Base Biome 1";
  }

  public void Update()
  {
    if ((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null && (UnityEngine.Object) this.playerFarming.PickedUpFollower != (UnityEngine.Object) null)
      return;
    if ((bool) (UnityEngine.Object) this.indicator && (UnityEngine.Object) HUD_Manager.Instance != (UnityEngine.Object) null)
    {
      this.indicator.transform.SetParent(HUD_Manager.Instance.transform.parent);
      this.indicator.primaryControlPrompt.PrioritizeMouse = (UnityEngine.Object) BiomeGenerator.Instance == (UnityEngine.Object) null;
      this.indicator.secondaryControlPrompt.PrioritizeMouse = (UnityEngine.Object) BiomeGenerator.Instance == (UnityEngine.Object) null;
      this.indicator.thirdControlPrompt.PrioritizeMouse = (UnityEngine.Object) BiomeGenerator.Instance == (UnityEngine.Object) null;
      this.indicator.fourthControlPrompt.PrioritizeMouse = (UnityEngine.Object) BiomeGenerator.Instance == (UnityEngine.Object) null;
    }
    if ((UnityEngine.Object) PlacementObject.Instance != (UnityEngine.Object) null && (UnityEngine.Object) this.playerFarming == (UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer)
    {
      this.indicator.Reset();
      this.indicator.SetGameObjectActive(true);
      this.indicator.Interactable = true;
      this.indicator.HideTopInfo();
      this.indicator.primaryControlPrompt.Category = 0;
      this.indicator.primaryControlPrompt.Action = 69;
      this.indicator.secondaryControlPrompt.Category = 0;
      this.indicator.secondaryControlPrompt.Action = 70;
      this.indicator.thirdControlPrompt.Category = 0;
      this.indicator.thirdControlPrompt.Action = 67;
      this.indicator.HasFourthInteraction = true;
      this.indicator.fourthControlPrompt.Category = 1;
      this.indicator.fourthControlPrompt.Action = 39;
      if ((UnityEngine.Object) PlacementObject.Instance != (UnityEngine.Object) null)
        this.indicator.Fourthtext.text = ScriptLocalization.Interactions.Done;
      else
        this.indicator.Fourthtext.text = ScriptLocalization.Interactions.Cancel;
      this._axes.x = InputManager.Gameplay.GetHorizontalAxis(this.playerFarming);
      this._axes.y = InputManager.Gameplay.GetVerticalAxis(this.playerFarming);
      this.indicator.primaryControlPrompt.PrioritizeMouse = (double) this._axes.magnitude <= 0.0 && this.playerFarming.canUseKeyboard && InputManager.General.MouseInputActive;
      this.indicator.secondaryControlPrompt.PrioritizeMouse = (double) this._axes.magnitude <= 0.0 && this.playerFarming.canUseKeyboard && InputManager.General.MouseInputActive;
      this.indicator.thirdControlPrompt.PrioritizeMouse = (double) this._axes.magnitude <= 0.0 && this.playerFarming.canUseKeyboard && InputManager.General.MouseInputActive;
      this.indicator.fourthControlPrompt.PrioritizeMouse = (double) this._axes.magnitude <= 0.0 && this.playerFarming.canUseKeyboard && InputManager.General.MouseInputActive;
      if (PlacementObject.Instance.StructureType == StructureBrain.TYPES.EDIT_BUILDINGS)
      {
        Structure hoveredStructure = PlacementRegion.Instance.GetHoveredStructure();
        this.indicator.HasSecondaryInteraction = true;
        this.indicator.HasFourthInteraction = true;
        this.indicator.HasThirdInteraction = true;
        if ((UnityEngine.Object) hoveredStructure != (UnityEngine.Object) null && hoveredStructure.Brain.Data.Type == StructureBrain.TYPES.VOLCANIC_SPA)
        {
          hoveredStructure.Brain.Data.CanBeMoved = true;
          hoveredStructure.Brain.Data.isDeletable = true;
          foreach (Interaction_VolcanicSpa healingBay in Interaction_VolcanicSpa.HealingBays)
          {
            if (healingBay.currentSpaOccupants.Count > 0)
            {
              hoveredStructure.Brain.Data.CanBeMoved = false;
              hoveredStructure.Brain.Data.isDeletable = false;
              break;
            }
          }
        }
        if ((UnityEngine.Object) hoveredStructure != (UnityEngine.Object) null && (hoveredStructure.Brain.Data.CanBeMoved || hoveredStructure.Brain.Data.IsDeletable))
        {
          StructureBrain.TYPES Type = hoveredStructure.Type;
          switch (Type)
          {
            case StructureBrain.TYPES.TEMPLE_II:
            case StructureBrain.TYPES.TEMPLE_III:
            case StructureBrain.TYPES.TEMPLE_IV:
              Type = StructureBrain.TYPES.TEMPLE;
              break;
          }
          this.indicator.text.text = hoveredStructure.Brain.Data.CanBeMoved ? $"{ScriptLocalization.Interactions.MoveStructure} <color=yellow>{StructuresData.LocalizedName(Type)}</color>" : "";
          this.indicator.SecondaryText.text = hoveredStructure.Brain.Data.IsDeletable ? $"{ScriptLocalization.Interactions.Remove} <color=yellow>{StructuresData.LocalizedName(Type)}</color>" : "";
          if (PlacementRegion.Instance.GetPathAtPosition() != StructureBrain.TYPES.NONE)
            this.indicator.Thirdtext.text = $"{ScriptLocalization.Interactions.Remove} <color=yellow>{StructuresData.LocalizedName(PlacementRegion.Instance.GetPathAtPosition())}</color>";
          else
            this.indicator.Thirdtext.text = string.Empty;
        }
        else if (PlacementRegion.Instance.GetPathAtPosition() != StructureBrain.TYPES.NONE)
        {
          this.indicator.SecondaryText.text = "";
          this.indicator.Thirdtext.text = $"{ScriptLocalization.Interactions.Remove} <color=yellow>{StructuresData.LocalizedName(PlacementRegion.Instance.GetPathAtPosition())}</color>";
        }
        else
        {
          this.indicator.text.text = "";
          this.indicator.SecondaryText.text = "";
          this.indicator.Thirdtext.text = "";
        }
      }
      else
      {
        if (StructureBrain.IsPath(PlacementObject.Instance.StructureType))
        {
          if (PlacementRegion.Instance.GetPathAtPosition() != StructureBrain.TYPES.NONE)
          {
            this.indicator.HasSecondaryInteraction = true;
            this.indicator.SecondaryText.text = $"{ScriptLocalization.Interactions.Remove} <color=yellow>{StructuresData.LocalizedName(PlacementRegion.Instance.GetPathAtPosition())}</color>";
          }
          else
          {
            this.indicator.HasSecondaryInteraction = false;
            this.indicator.SecondaryText.text = string.Empty;
          }
        }
        else if (PlacementObject.Instance.StructureType == StructureBrain.TYPES.RANCH_FENCE)
        {
          Structure hoveredStructure = PlacementRegion.Instance.GetHoveredStructure();
          if ((UnityEngine.Object) hoveredStructure != (UnityEngine.Object) null && hoveredStructure.Type == StructureBrain.TYPES.RANCH_FENCE)
          {
            this.indicator.SecondaryInteractable = true;
            this.indicator.SecondaryText.text = $"{ScriptLocalization.Interactions.Remove} <color=yellow>{StructuresData.LocalizedName(StructureBrain.TYPES.RANCH_FENCE)}</color>";
          }
          else
          {
            this.indicator.SecondaryInteractable = false;
            this.indicator.SecondaryText.text = "";
          }
        }
        else
        {
          this.indicator.SecondaryInteractable = StructuresData.CanBeFlipped(PlacementObject.Instance.StructureType);
          this.indicator.SecondaryText.text = StructuresData.CanBeFlipped(PlacementObject.Instance.StructureType) ? ScriptLocalization.Interactions.Flip : "";
          if (PlacementObject.Instance.StructureType == StructureBrain.TYPES.SLEEPING_BAG || PlacementObject.Instance.StructureType == StructureBrain.TYPES.BED || PlacementObject.Instance.StructureType == StructureBrain.TYPES.BED_2 || PlacementObject.Instance.StructureType == StructureBrain.TYPES.BED_3 || PlacementObject.Instance.StructureType == StructureBrain.TYPES.SHARED_HOUSE)
            this.indicator.ShowTopInfo($"<sprite name=\"icon_House\"> {StructureManager.GetTotalHomesCount(true).ToString()} <sprite name=\"icon_Followers\">{DataManager.Instance.Followers.Count.ToString()}");
        }
        this.indicator.HasSecondaryInteraction = true;
        if (PlacementRegion.Instance.CurrentMode == PlacementRegion.Mode.Upgrading && PlacementObject.Instance.StructureType != StructureBrain.TYPES.SHRINE)
        {
          this.indicator.text.text = ScriptLocalization.Interactions.UpgradeBuilding.Replace("{0}", StructuresData.LocalizedName(PlacementObject.Instance.StructureType));
        }
        else
        {
          StructureBrain.TYPES Type = PlacementRegion.Instance.StructureType;
          switch (Type)
          {
            case StructureBrain.TYPES.TEMPLE_II:
            case StructureBrain.TYPES.TEMPLE_III:
            case StructureBrain.TYPES.TEMPLE_IV:
              Type = StructureBrain.TYPES.TEMPLE;
              break;
          }
          this.indicator.text.text = $"{ScriptLocalization.Interactions.PlaceBuilding} <color=yellow>{StructuresData.LocalizedName(Type)}";
        }
      }
    }
    else
    {
      this.ResetPrompts();
      if (this.state.CURRENT_STATE == StateMachine.State.Idle_CarryingBody || this.state.CURRENT_STATE == StateMachine.State.Moving_CarryingBody)
      {
        this.indicator.SetGameObjectActive(true);
        this.indicator.Interactable = true;
        this.indicator.HasSecondaryInteraction = false;
        this.indicator.SecondaryText.text = "";
        if (this.playerFarming.NearGrave)
        {
          if (this.playerFarming.NearStructure != null && this.playerFarming.NearStructure.IsFull)
          {
            this.indicator.text.text = ScriptLocalization.Interactions.Full;
            this.indicator.Interactable = false;
          }
          else if (this.playerFarming.CarryingDeadFollowerID == -1)
          {
            if (this.playerFarming.CarryingEgg)
              this.indicator.text.text = ScriptLocalization.Interactions.PlaceBuilding;
            else
              this.indicator.text.text = LocalizationManager.GetTranslation("Interactions/Bury");
          }
          else
            this.indicator.text.text = ScriptLocalization.Interactions.BuryBody;
        }
        else if (this.playerFarming.NearCompostBody)
          this.indicator.text.text = ScriptLocalization.Interactions.CompostBody;
        else if (this.playerFarming.NearFurnace)
        {
          if (this.playerFarming.CarryingDeadFollowerID != -1)
            this.indicator.text.text = LocalizationManager.GetTranslation("Interactions/BurnBody");
          else if ((UnityEngine.Object) this.playerFarming.GetLeashingAnimal() != (UnityEngine.Object) null)
            this.indicator.text.text = LocalizationManager.GetTranslation("Interactions/BurnAnimal");
        }
        else if (this.playerFarming.CarryingSnowball)
          this.indicator.text.text = LocalizationManager.GetTranslation("UI/Settings/Controls/HoldToAim");
        else if (!((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null) || !((UnityEngine.Object) this.playerFarming.PuzzlePieceCarried != (UnityEngine.Object) null))
          this.indicator.text.text = ScriptLocalization.Interactions.Drop;
        this.indicator.Reset();
      }
      else if (this.state.CURRENT_STATE == StateMachine.State.ChargingSnowball)
        this.indicator.Reset();
      else if ((UnityEngine.Object) this.state != (UnityEngine.Object) null && (UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null && !this.IsInStateToInteract() && !Interactor.Overriding)
      {
        if (!((UnityEngine.Object) this.indicator != (UnityEngine.Object) null) || !((UnityEngine.Object) this.indicator.gameObject != (UnityEngine.Object) null) || !this.indicator.IsActive)
          return;
        this.indicator.Deactivate();
        if (!((UnityEngine.Object) this.CurrentInteraction != (UnityEngine.Object) null))
          return;
        this.CurrentInteraction.OnBecomeNotCurrent(this.playerFarming);
        this.PreviousInteraction = (Interaction) null;
        this.CurrentInteraction = (Interaction) null;
      }
      else
      {
        if ((double) Time.timeScale == 0.0)
          return;
        if (((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null || (UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null && !this.playerFarming.IsKnockedOut) && (Time.frameCount + this.FrameIntervalOffset) % this.UpdateInterval == 0)
          this.GetClosestInteraction<Interaction>();
        if ((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null && this.playerFarming.IsKnockedOut && (Time.frameCount + this.FrameIntervalOffset) % this.UpdateInterval == 0)
          this.GetClosestInteraction<Interaction_HeartPickupBase>();
        if ((UnityEngine.Object) this.CurrentInteraction != (UnityEngine.Object) null && (this.CurrentInteraction.LambOnly && !this.playerFarming.isLamb || this.CurrentInteraction.GoatOnly && this.playerFarming.isLamb))
          this.CurrentInteraction = (Interaction) null;
        bool flag = (UnityEngine.Object) this.PreviousInteraction != (UnityEngine.Object) this.CurrentInteraction || (UnityEngine.Object) this.CurrentInteraction != (UnityEngine.Object) null && this.CurrentInteraction.HasChanged || PlayerFollowerSelection.IsPlaying;
        if (PlayerFollowerSelection.IsPlaying)
          this.CurrentInteraction = (Interaction) null;
        if (flag)
        {
          if ((UnityEngine.Object) this.PreviousInteraction != (UnityEngine.Object) null)
            this.PreviousInteraction.OnBecomeNotCurrent(this.playerFarming);
          if ((UnityEngine.Object) this.CurrentInteraction != (UnityEngine.Object) null)
            this.CurrentInteraction.OnBecomeCurrent(this.playerFarming);
          if ((bool) (UnityEngine.Object) this.indicator)
            this.indicator.Reset();
        }
        if (PlayerFollowerSelection.IsPlaying)
        {
          this.indicator.SetGameObjectActive(true);
          this.indicator.Interactable = false;
          this.indicator.HasSecondaryInteraction = false;
          this.indicator.SecondaryText.text = "";
          this.indicator.HasThirdInteraction = false;
          this.indicator.Thirdtext.text = "";
          this.indicator.HasFourthInteraction = false;
          this.indicator.Fourthtext.text = "";
          this.indicator.text.text = PlayerFollowerSelection.Instance.SelectedFollowers.Count > 0 ? "Release to issue your <color=red>Command</color>" : "Select <color=yellow>Followers</color> to Command";
          this.indicator.Reset();
        }
        else if ((UnityEngine.Object) this.CurrentInteraction == (UnityEngine.Object) null)
        {
          if ((UnityEngine.Object) this.PreviousInteraction != (UnityEngine.Object) null && this.PreviousInteraction.HoldToInteract)
            this.PreviousInteraction.OnHoldProgressStop();
          this.PreviousInteraction = (Interaction) null;
          if (!(bool) (UnityEngine.Object) this.indicator || !this.indicator.IsActive)
            return;
          this.indicator.Deactivate();
        }
        else
        {
          if ((UnityEngine.Object) this.CurrentInteraction == (UnityEngine.Object) this.PreviousInteraction && !this.indicator.IsActive)
            this.indicator.SetGameObjectActive(true);
          this.indicator.SetPosition((UnityEngine.Object) this.CurrentInteraction.LockPosition == (UnityEngine.Object) null ? this.CurrentInteraction.transform.position + this.CurrentInteraction.Offset : this.CurrentInteraction.LockPosition.transform.position);
          if (this.CurrentInteraction.ContinuouslyHold)
          {
            this.indicator.text.text = this.CurrentInteraction.Label;
            if (string.IsNullOrWhiteSpace(this.indicator.text.text) && string.IsNullOrWhiteSpace(this.CurrentInteraction.SecondaryLabel) && string.IsNullOrWhiteSpace(this.CurrentInteraction.ThirdLabel) && string.IsNullOrWhiteSpace(this.CurrentInteraction.FourthLabel))
              this.indicator.ContainerImage.enabled = false;
            this.indicator.Interactable = this.CurrentInteraction.Interactable;
            this.indicator.HasSecondaryInteraction = this.CurrentInteraction.HasSecondaryInteraction;
            this.indicator.SecondaryText.text = this.CurrentInteraction.SecondaryLabel;
            this.indicator.HasThirdInteraction = this.CurrentInteraction.HasThirdInteraction;
            this.indicator.Thirdtext.text = this.CurrentInteraction.ThirdLabel;
            this.indicator.HasFourthInteraction = this.CurrentInteraction.HasFourthInteraction;
            this.indicator.Fourthtext.text = this.CurrentInteraction.FourthLabel;
            this.indicator.Reset();
          }
          if (flag)
          {
            if ((UnityEngine.Object) this.PreviousInteraction != (UnityEngine.Object) null && this.PreviousInteraction.HoldToInteract)
              this.PreviousInteraction.OnHoldProgressStop();
            this.indicator.SetGameObjectActive(false);
            this.indicator.HoldToInteract = this.CurrentInteraction.HoldToInteract && SettingsManager.Settings.Accessibility.HoldActions;
            this.indicator.Interactable = this.CurrentInteraction.Interactable;
            this.indicator.HasSecondaryInteraction = this.CurrentInteraction.HasSecondaryInteraction;
            this.indicator.SecondaryInteractable = this.CurrentInteraction.SecondaryInteractable;
            this.indicator.HasThirdInteraction = this.CurrentInteraction.HasThirdInteraction;
            this.indicator.ThirdInteractable = this.CurrentInteraction.ThirdInteractable;
            this.indicator.HasFourthInteraction = this.CurrentInteraction.HasFourthInteraction;
            this.indicator.FourthInteractable = this.CurrentInteraction.FourthInteractable;
            this.indicator.text.text = this.CurrentInteraction.AutomaticallyInteract ? "" : this.CurrentInteraction.Label;
            this.indicator.SetGameObjectActive(true);
            if (string.IsNullOrWhiteSpace(this.indicator.text.text) && string.IsNullOrWhiteSpace(this.CurrentInteraction.SecondaryLabel) && string.IsNullOrWhiteSpace(this.CurrentInteraction.ThirdLabel) && string.IsNullOrWhiteSpace(this.CurrentInteraction.FourthLabel))
              this.indicator.ContainerImage.enabled = false;
            this.indicator.SecondaryText.text = this.CurrentInteraction.SecondaryLabel;
            this.indicator.Thirdtext.text = this.CurrentInteraction.ThirdLabel;
            this.indicator.Fourthtext.text = this.CurrentInteraction.FourthLabel;
            this.PreviousInteraction = this.CurrentInteraction;
            this.indicator.Reset();
          }
          if (this.CurrentInteraction.Interactable)
          {
            if (this.CurrentInteraction.HoldToInteract && SettingsManager.Settings.Accessibility.HoldActions)
            {
              if (InputManager.Gameplay.GetInteractButtonDown(this.playerFarming))
                this.CurrentInteraction.OnHoldProgressDown(this.indicator, this.playerFarming);
              if (InputManager.Gameplay.GetInteractButtonUp(this.playerFarming))
                this.CurrentInteraction.OnHoldProgressRelease();
              if (InputManager.Gameplay.GetInteractButtonHeld(this.playerFarming))
              {
                this.CurrentInteraction.HoldProgress += 1f * Time.deltaTime;
                if (!this.CurrentInteraction.HoldBegun && this.CurrentInteraction.FreezeCoopPlayersOnHoldToInteract)
                  PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive, PlayerNotToInclude: this.playerFarming);
                this.CurrentInteraction.HoldBegun = true;
                this.indicator.Progress = this.CurrentInteraction.HoldProgress / 1f;
                if ((double) this.CurrentInteraction.HoldProgress >= 1.0)
                {
                  this.CurrentInteraction.OnHoldProgressStop();
                  this.Interact(this.CurrentInteraction);
                  this.Delay = 0.25f;
                }
              }
              else
              {
                if (!this.CurrentInteraction.HoldBegun && (double) this.CurrentInteraction.HoldProgress > 0.0)
                {
                  this.CurrentInteraction.HoldProgress -= 0.5f * Time.deltaTime;
                  this.indicator.Progress = this.CurrentInteraction.HoldProgress / 1f;
                }
                else if (this.CurrentInteraction.HoldBegun && (double) this.CurrentInteraction.HoldProgress < 0.20000000298023224)
                {
                  this.CurrentInteraction.HoldProgress += 1f * Time.deltaTime;
                  this.indicator.Progress = this.CurrentInteraction.HoldProgress / 1f;
                  if ((double) this.CurrentInteraction.HoldProgress >= 0.20000000298023224)
                    this.CurrentInteraction.HoldBegun = false;
                }
                if ((double) this.CurrentInteraction.HoldProgress >= 0.20000000298023224)
                  this.CurrentInteraction.HoldBegun = false;
              }
            }
            else if (this.CurrentInteraction.AutomaticallyInteract)
            {
              this.Interact(this.CurrentInteraction);
              this.HideIndicator();
              this.Delay = 0.25f;
            }
            else if (InputManager.Gameplay.GetInteractButtonDown(this.playerFarming) && (this.IsInStateToInteract() || Interactor.Overriding) && (bool) (UnityEngine.Object) this.CurrentInteraction && (!this.CurrentInteraction.LambOnly || this.playerFarming.isLamb))
            {
              if (!this.CurrentInteraction.ContinuouslyHold)
              {
                this.StartCoroutine((IEnumerator) this.WaitForEndOfFrameAndInteract());
                this.Delay = 0.25f;
              }
              else
              {
                this.Interact(this.CurrentInteraction);
                this.indicator.text.text = this.CurrentInteraction.Label;
              }
            }
          }
          if (this.CurrentInteraction.HasSecondaryInteraction && this.CurrentInteraction.SecondaryInteractable && InputManager.Gameplay.GetInteract2ButtonDown(this.playerFarming) && this.IsInStateToInteract())
          {
            MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = this.playerFarming;
            this.CurrentInteraction.OnSecondaryInteract(this.state);
            this.Delay = 0.25f;
          }
          if (this.CurrentInteraction.HasThirdInteraction && this.CurrentInteraction.ThirdInteractable && InputManager.Gameplay.GetInteract3ButtonDown(this.playerFarming) && this.IsInStateToInteract())
          {
            MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = this.playerFarming;
            this.CurrentInteraction.OnThirdInteract(this.state);
            this.Delay = 0.25f;
          }
          if (this.CurrentInteraction.HasFourthInteraction && this.CurrentInteraction.FourthInteractable && InputManager.Gameplay.GetInteract4ButtonDown(this.playerFarming) && this.IsInStateToInteract())
          {
            MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = this.playerFarming;
            this.CurrentInteraction.OnFourthInteract(this.state);
            this.Delay = 0.25f;
          }
          int num = SettingsManager.Settings.Accessibility.DyslexicFont ? 500 : 800;
          if (this.indicator.text.text.Length <= num)
            return;
          for (int index = num; index < this.indicator.text.text.Length; ++index)
          {
            if (this.indicator.text.text[index] == '>')
            {
              int startIndex = index + 1;
              this.indicator.text.text = this.indicator.text.text.Remove(startIndex, this.indicator.text.text.Length - startIndex) + "<color=grey>...</color>";
              break;
            }
          }
        }
      }
    }
  }

  public void Interact(Interaction interaction)
  {
    if ((double) Mathf.Abs((float) Time.frameCount - Interactor.interactionFrame) < 15.0)
      return;
    Interactor.interactionFrame = (float) Time.frameCount;
    MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = this.playerFarming;
    if (PlayerFarming.playersCount <= 1)
      interaction.OnInteract(this.state);
    else
      GameManager.GetInstance().WaitForSeconds(0.05f, (System.Action) (() =>
      {
        if (MMConversation.isPlaying && !MMConversation.isBark || MMTransition.IsPlaying || !this.IsInStateToInteract())
          return;
        interaction.OnInteract(this.state);
      }));
  }

  public bool IsInStateToInteract()
  {
    return !PlayerFarming.AnyPlayerGotoAndStopping() && this.state.CURRENT_STATE != StateMachine.State.InActive && this.state.CURRENT_STATE != StateMachine.State.CustomAnimation && this.state.CURRENT_STATE != StateMachine.State.Heal && this.state.CURRENT_STATE != StateMachine.State.Meditate && this.state.CURRENT_STATE != StateMachine.State.Dodging && this.state.CURRENT_STATE != StateMachine.State.Map && this.state.CURRENT_STATE != StateMachine.State.Teleporting && this.state.CURRENT_STATE != StateMachine.State.Grapple && this.state.CURRENT_STATE != StateMachine.State.CustomAction0 && !LetterBox.IsPlaying && this.state.CURRENT_STATE != StateMachine.State.TimedAction && this.state.CURRENT_STATE != StateMachine.State.Idle_CarryingBody && this.state.CURRENT_STATE != StateMachine.State.Moving_CarryingBody && this.state.CURRENT_STATE != StateMachine.State.Aiming && this.state.CURRENT_STATE != StateMachine.State.Casting && this.state.CURRENT_STATE != StateMachine.State.HitThrown;
  }

  public IEnumerator WaitForEndOfFrameAndInteract()
  {
    yield return (object) new WaitForEndOfFrame();
    this.Interact(this.CurrentInteraction);
  }

  public IEnumerator HideAndRevealIndicator()
  {
    this.HideIndicator();
    yield return (object) new WaitForSeconds(0.3f);
    if ((UnityEngine.Object) this.CurrentInteraction != (UnityEngine.Object) null && !string.IsNullOrWhiteSpace(this.CurrentInteraction.Label))
      this.PreviousInteraction = (Interaction) null;
  }

  public void HideIndicator()
  {
    if (!this.indicator.IsActive)
      return;
    this.indicator.Deactivate();
  }

  public static void Add(Interaction go)
  {
    if (Interactor.allVisibleInteractions.Contains(go))
      return;
    Interactor.allVisibleInteractions.Add(go);
  }

  public static void Remove(Interaction go)
  {
    if (!Interactor.allVisibleInteractions.Contains(go))
      return;
    Interactor.allVisibleInteractions.Remove(go);
  }

  public void GetClosestInteraction<T>() where T : Interaction
  {
    if (InputManager.Gameplay.GetInteractButtonDown(this.playerFarming))
      return;
    this.CurrentInteraction = (Interaction) null;
    this.CurrentDist = (float) int.MaxValue;
    this.CurrentPriorityWeight = 0.0f;
    if ((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null && !this.playerFarming.IsPlayerWithinScreenView())
      return;
    Interaction interaction = (Interaction) null;
    if (Interactor.UseRegions)
    {
      Vector3 position = this.playerFarming.transform.position;
      if (Interactor.InteractionRegions.ContainsKey(Interactor.PositionToRegions(position)))
      {
        foreach (Interaction i in Interactor.InteractionRegions[Interactor.PositionToRegions(position)])
        {
          if (i.gameObject.activeInHierarchy && i is T)
          {
            bool flag1 = false;
            foreach (PlayerFarming player in PlayerFarming.players)
            {
              if ((UnityEngine.Object) player != (UnityEngine.Object) this.playerFarming && (UnityEngine.Object) player.interactor.CurrentInteraction == (UnityEngine.Object) i && !Interactor.CanBothPlayersInteraction(i))
                flag1 = true;
            }
            if (!flag1)
            {
              this.TestDist = Vector3.Distance(i.gameObject.transform.position + i.ActivatorOffset, this.gameObject.transform.position);
              if ((double) this.TestDist < (double) i.ActivateDistance - (double) this.GetActivationDistanceModifier())
              {
                if ((UnityEngine.Object) interaction == (UnityEngine.Object) null || (double) i.PriorityWeight >= (double) interaction.PriorityWeight)
                  interaction = i;
                if ((double) this.TestDist < (double) this.CurrentDist)
                {
                  bool flag2 = false;
                  if (!i.allowMultipleInteractors)
                  {
                    for (int index = 0; index < PlayerFarming.playersCount; ++index)
                    {
                      if ((UnityEngine.Object) PlayerFarming.players[index] != (UnityEngine.Object) this.playerFarming && (UnityEngine.Object) i == (UnityEngine.Object) PlayerFarming.players[index].interactor.CurrentInteraction)
                        flag2 = true;
                    }
                  }
                  bool flag3 = i.AllowInteractionWithoutLabel || !string.IsNullOrWhiteSpace(i.Label) || !string.IsNullOrWhiteSpace(i.SecondaryLabel) || !string.IsNullOrWhiteSpace(i.ThirdLabel) || !string.IsNullOrWhiteSpace(i.FourthLabel);
                  if (!flag2 & flag3)
                  {
                    this.CurrentInteraction = i;
                    this.CurrentDist = this.TestDist;
                    this.CurrentPriorityWeight = i.PriorityWeight;
                  }
                }
              }
            }
          }
        }
        if ((UnityEngine.Object) this.CurrentInteraction != (UnityEngine.Object) null)
        {
          if ((UnityEngine.Object) interaction != (UnityEngine.Object) null && (double) interaction.PriorityWeight > (double) this.CurrentInteraction.PriorityWeight)
            this.CurrentInteraction = interaction;
          if (this.CurrentInteraction is interaction_FollowerInteraction && (double) this.playerFarming.playerController.speed <= 0.0 && (UnityEngine.Object) this.PreviousInteraction != (UnityEngine.Object) null)
            this.CurrentInteraction = this.PreviousInteraction;
        }
      }
    }
    else
    {
      List<Interaction> interactionList = Interaction.interactions;
      if (Interactor.UseInteractionCulling)
        interactionList = Interactor.allVisibleInteractions.ToList<Interaction>();
      foreach (Interaction i in interactionList)
      {
        if ((UnityEngine.Object) i != (UnityEngine.Object) null && i.gameObject.activeInHierarchy && i is T)
        {
          bool flag4 = false;
          foreach (PlayerFarming player in PlayerFarming.players)
          {
            if ((UnityEngine.Object) player != (UnityEngine.Object) this.playerFarming && (UnityEngine.Object) player.interactor != (UnityEngine.Object) null && (UnityEngine.Object) player.interactor.CurrentInteraction == (UnityEngine.Object) i && !player.interactor.CurrentInteraction.CanMultiplePlayersInteract && !Interactor.CanBothPlayersInteraction(i))
              flag4 = true;
          }
          if (!flag4)
          {
            this.TestDist = Vector3.Distance(i.gameObject.transform.position + i.ActivatorOffset, this.gameObject.transform.position);
            if (i is Interaction_Woodcutting && !this.playerFarming.isLamb && this.playerFarming.playerID == 2)
              Debug.Log((object) $"{this.TestDist.ToString()}  {this.playerFarming.name} ActivateDistance {i.ActivateDistance.ToString()}");
            if ((double) this.TestDist < (double) i.ActivateDistance - (double) this.GetActivationDistanceModifier())
            {
              this.TempInteraction = i;
              if ((i.AllowInteractionWithoutLabel || !string.IsNullOrWhiteSpace(i.Label) || !string.IsNullOrWhiteSpace(i.SecondaryLabel) || !string.IsNullOrWhiteSpace(i.ThirdLabel) ? 1 : (!string.IsNullOrWhiteSpace(i.FourthLabel) ? 1 : 0)) != 0)
              {
                if ((UnityEngine.Object) interaction == (UnityEngine.Object) null || (double) i.PriorityWeight >= (double) interaction.PriorityWeight)
                  interaction = i;
                if ((double) this.TestDist < (double) this.CurrentDist)
                {
                  bool flag5 = false;
                  if (!i.allowMultipleInteractors)
                  {
                    for (int index = 0; index < PlayerFarming.playersCount; ++index)
                    {
                      if ((UnityEngine.Object) PlayerFarming.players[index] != (UnityEngine.Object) this.playerFarming && (UnityEngine.Object) i == (UnityEngine.Object) PlayerFarming.players[index].interactor.CurrentInteraction)
                        flag5 = true;
                    }
                  }
                  if (!flag5)
                  {
                    this.CurrentInteraction = i;
                    this.CurrentDist = this.TestDist;
                    this.CurrentPriorityWeight = i.PriorityWeight;
                  }
                }
              }
            }
          }
        }
      }
      if ((UnityEngine.Object) this.CurrentInteraction != (UnityEngine.Object) null)
      {
        if ((UnityEngine.Object) interaction != (UnityEngine.Object) null && (double) interaction.PriorityWeight > (double) this.CurrentInteraction.PriorityWeight)
          this.CurrentInteraction = interaction;
        if (this.CurrentInteraction is interaction_FollowerInteraction && (double) this.playerFarming.playerController.speed <= 0.0 && (UnityEngine.Object) this.PreviousInteraction != (UnityEngine.Object) null)
          this.CurrentInteraction = this.PreviousInteraction;
      }
    }
    this.TempInteraction = (Interaction) null;
  }

  public float GetActivationDistanceModifier()
  {
    if (PlayerFarming.Location == this.lastLocationCheck)
      return this.distanceModifier;
    this.lastLocationCheck = PlayerFarming.Location;
    this.distanceModifier = 0.0f;
    if (PlayerFarming.players.Count > 1 && !Interactor.IgnoreCoopDistanceModifiers.Contains<FollowerLocation>(PlayerFarming.Location))
      this.distanceModifier = 0.25f;
    return this.distanceModifier;
  }

  public static bool CanBothPlayersInteraction(Interaction i)
  {
    switch (i)
    {
      case Interaction_PlayerBuildProject _:
      case Interaction_PlayerBuild _:
      case Interaction_PlayerClearRubble _:
      case Interaction_Woodcutting _:
      case Interaction_WoodcuttingRubble _:
        return true;
      default:
        return i is Interaction_Berries;
    }
  }

  public static void AddToRegion(Interaction l)
  {
    Vector3Int regions = Interactor.PositionToRegions(l.Position);
    if (!Interactor.InteractionRegions.ContainsKey(regions))
      Interactor.InteractionRegions.Add(regions, new List<Interaction>()
      {
        l
      });
    else
      Interactor.InteractionRegions[regions].Add(l);
  }

  public static void RemoveFromRegion(Interaction l)
  {
    Vector3Int regions = Interactor.PositionToRegions(l.Position);
    if (!Interactor.InteractionRegions.ContainsKey(regions))
      return;
    Interactor.InteractionRegions[regions].Remove(l);
  }

  public static Vector3Int PositionToRegions(Vector3 Position)
  {
    return Vector3Int.FloorToInt(Position) / 3;
  }
}
