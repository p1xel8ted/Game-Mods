// Decompiled with JetBrains decompiler
// Type: Interactor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using MMBiomeGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interactor : BaseMonoBehaviour
{
  public static Interaction CurrentInteraction;
  public static Interaction PreviousInteraction;
  private float CurrentDist;
  private float CurrentPriorityWeight;
  private float TestDist;
  private PlayerFarming player;
  private Indicator Indicator;
  private RectTransform IndicatorRT;
  private StateMachine state;
  private float Delay;
  private int defaultButton = 9;
  private int secondaryButton = 68;
  private int tertiaryButton = 67;
  private int quaternaryButton = 66;
  private int FrameIntervalOffset;
  private int UpdateInterval = 2;
  private Vector2 _axes;
  public static bool UseRegions = false;
  public static Dictionary<Vector3Int, List<Interaction>> InteractionRegions = new Dictionary<Vector3Int, List<Interaction>>();
  public const int RegionSize = 3;

  private void Start()
  {
    this.state = this.gameObject.GetComponent<StateMachine>();
    this.player = this.gameObject.GetComponent<PlayerFarming>();
    this.StartCoroutine((IEnumerator) this.WaitForIndicator());
    this.FrameIntervalOffset = Random.Range(0, this.UpdateInterval);
  }

  private IEnumerator WaitForIndicator()
  {
    while ((Object) this.Indicator == (Object) null)
    {
      this.Indicator = MonoSingleton<Indicator>.Instance;
      yield return (object) null;
    }
    this.IndicatorRT = this.Indicator.RectTransform;
    this.ResetPrompts();
  }

  private void ResetPrompts()
  {
    this.Indicator.primaryControlPrompt.Category = 0;
    this.Indicator.primaryControlPrompt.Action = this.defaultButton;
    this.Indicator.secondaryControlPrompt.Category = 0;
    this.Indicator.secondaryControlPrompt.Action = this.secondaryButton;
    this.Indicator.thirdControlPrompt.Category = 0;
    this.Indicator.thirdControlPrompt.Action = this.tertiaryButton;
    this.Indicator.fourthControlPrompt.Category = 0;
    this.Indicator.fourthControlPrompt.Action = this.quaternaryButton;
  }

  private void OnEnable()
  {
    if ((Object) this.state == (Object) null)
      this.state = this.gameObject.GetComponent<StateMachine>();
    if (!((Object) this.player == (Object) null))
      return;
    this.player = this.gameObject.GetComponent<PlayerFarming>();
  }

  private void Update()
  {
    if ((Object) this.player != (Object) null && (Object) this.player.PickedUpFollower != (Object) null)
      return;
    this.Indicator.primaryControlPrompt.PrioritizeMouse = (Object) BiomeGenerator.Instance == (Object) null;
    this.Indicator.secondaryControlPrompt.PrioritizeMouse = (Object) BiomeGenerator.Instance == (Object) null;
    this.Indicator.thirdControlPrompt.PrioritizeMouse = (Object) BiomeGenerator.Instance == (Object) null;
    this.Indicator.fourthControlPrompt.PrioritizeMouse = (Object) BiomeGenerator.Instance == (Object) null;
    if ((Object) PlacementObject.Instance != (Object) null)
    {
      this.Indicator.Reset();
      this.Indicator.gameObject.SetActive(true);
      this.Indicator.Interactable = true;
      this.Indicator.HideTopInfo();
      this.Indicator.primaryControlPrompt.Category = 0;
      this.Indicator.primaryControlPrompt.Action = 69;
      this.Indicator.secondaryControlPrompt.Category = 0;
      this.Indicator.secondaryControlPrompt.Action = 70;
      this.Indicator.thirdControlPrompt.Category = 0;
      this.Indicator.thirdControlPrompt.Action = 67;
      this.Indicator.HasFourthInteraction = true;
      this.Indicator.fourthControlPrompt.Category = 1;
      this.Indicator.fourthControlPrompt.Action = 39;
      this.Indicator.Fourthtext.text = ScriptLocalization.Interactions.Cancel;
      this._axes.x = InputManager.Gameplay.GetHorizontalAxis();
      this._axes.y = InputManager.Gameplay.GetVerticalAxis();
      this.Indicator.primaryControlPrompt.PrioritizeMouse = (double) this._axes.magnitude <= 0.0 && InputManager.General.MouseInputActive;
      this.Indicator.secondaryControlPrompt.PrioritizeMouse = (double) this._axes.magnitude <= 0.0 && InputManager.General.MouseInputActive;
      this.Indicator.thirdControlPrompt.PrioritizeMouse = (double) this._axes.magnitude <= 0.0 && InputManager.General.MouseInputActive;
      this.Indicator.fourthControlPrompt.PrioritizeMouse = (double) this._axes.magnitude <= 0.0 && InputManager.General.MouseInputActive;
      if (PlacementObject.Instance.StructureType == StructureBrain.TYPES.EDIT_BUILDINGS)
      {
        Structure hoveredStructure = PlacementRegion.Instance.GetHoveredStructure();
        this.Indicator.HasSecondaryInteraction = true;
        this.Indicator.HasFourthInteraction = true;
        this.Indicator.HasThirdInteraction = true;
        if ((Object) hoveredStructure != (Object) null && (hoveredStructure.Brain.Data.CanBeMoved || hoveredStructure.Brain.Data.IsDeletable))
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
          this.Indicator.text.text = hoveredStructure.Brain.Data.CanBeMoved ? $"{ScriptLocalization.Interactions.MoveStructure} <color=yellow>{StructuresData.LocalizedName(Type)}</color>" : "";
          this.Indicator.SecondaryText.text = hoveredStructure.Brain.Data.IsDeletable ? $"{ScriptLocalization.Interactions.Remove} <color=yellow>{StructuresData.LocalizedName(Type)}</color>" : "";
          if (PlacementRegion.Instance.GetPathAtPosition() != StructureBrain.TYPES.NONE)
            this.Indicator.Thirdtext.text = $"{ScriptLocalization.Interactions.Remove} <color=yellow>{StructuresData.LocalizedName(PlacementRegion.Instance.GetPathAtPosition())}</color>";
          else
            this.Indicator.Thirdtext.text = string.Empty;
        }
        else if (PlacementRegion.Instance.GetPathAtPosition() != StructureBrain.TYPES.NONE)
        {
          this.Indicator.SecondaryText.text = "";
          this.Indicator.Thirdtext.text = $"{ScriptLocalization.Interactions.Remove} <color=yellow>{StructuresData.LocalizedName(PlacementRegion.Instance.GetPathAtPosition())}</color>";
        }
        else
        {
          this.Indicator.text.text = "";
          this.Indicator.SecondaryText.text = "";
          this.Indicator.Thirdtext.text = "";
        }
      }
      else
      {
        if (StructureBrain.IsPath(PlacementObject.Instance.StructureType))
        {
          if (PlacementRegion.Instance.GetPathAtPosition() != StructureBrain.TYPES.NONE)
          {
            this.Indicator.HasSecondaryInteraction = true;
            this.Indicator.SecondaryText.text = $"{ScriptLocalization.Interactions.Remove} <color=yellow>{StructuresData.LocalizedName(PlacementRegion.Instance.GetPathAtPosition())}</color>";
          }
          else
          {
            this.Indicator.HasSecondaryInteraction = false;
            this.Indicator.SecondaryText.text = string.Empty;
          }
        }
        else
        {
          this.Indicator.SecondaryInteractable = StructuresData.CanBeFlipped(PlacementObject.Instance.StructureType);
          this.Indicator.SecondaryText.text = StructuresData.CanBeFlipped(PlacementObject.Instance.StructureType) ? ScriptLocalization.Interactions.Flip : "";
          if (PlacementObject.Instance.StructureType == StructureBrain.TYPES.SLEEPING_BAG || PlacementObject.Instance.StructureType == StructureBrain.TYPES.BED || PlacementObject.Instance.StructureType == StructureBrain.TYPES.BED_2 || PlacementObject.Instance.StructureType == StructureBrain.TYPES.BED_3)
            this.Indicator.ShowTopInfo($"<sprite name=\"icon_House\">{StructureManager.GetTotalHomesCount(true).ToString()} <sprite name=\"icon_Followers\">{DataManager.Instance.Followers.Count.ToString()}");
        }
        this.Indicator.HasSecondaryInteraction = true;
        if (PlacementRegion.Instance.CurrentMode == PlacementRegion.Mode.Upgrading && PlacementObject.Instance.StructureType != StructureBrain.TYPES.SHRINE)
        {
          this.Indicator.text.text = ScriptLocalization.Interactions.UpgradeBuilding.Replace("{0}", StructuresData.LocalizedName(PlacementObject.Instance.StructureType));
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
          this.Indicator.text.text = $"{ScriptLocalization.Interactions.PlaceBuilding} <color=yellow>{StructuresData.LocalizedName(Type)}";
        }
      }
    }
    else
    {
      this.ResetPrompts();
      if (this.state.CURRENT_STATE == StateMachine.State.Idle_CarryingBody || this.state.CURRENT_STATE == StateMachine.State.Moving_CarryingBody)
      {
        this.Indicator.gameObject.SetActive(true);
        this.Indicator.Interactable = true;
        this.Indicator.HasSecondaryInteraction = false;
        this.Indicator.SecondaryText.text = "";
        if (PlayerFarming.Instance.NearGrave)
        {
          if (PlayerFarming.Instance.CarryingDeadFollowerID == -1)
            this.Indicator.text.text = LocalizationManager.GetTranslation("Interactions/Bury");
          else
            this.Indicator.text.text = ScriptLocalization.Interactions.BuryBody;
        }
        else if (PlayerFarming.Instance.NearCompostBody)
          this.Indicator.text.text = ScriptLocalization.Interactions.CompostBody;
        else
          this.Indicator.text.text = ScriptLocalization.Interactions.Drop;
        this.Indicator.Reset();
      }
      else if ((Object) this.state != (Object) null && (Object) this.player != (Object) null && (this.state.CURRENT_STATE == StateMachine.State.InActive || this.state.CURRENT_STATE == StateMachine.State.CustomAnimation || this.state.CURRENT_STATE == StateMachine.State.Heal || this.state.CURRENT_STATE == StateMachine.State.Meditate || this.state.CURRENT_STATE == StateMachine.State.Map || this.state.CURRENT_STATE == StateMachine.State.Teleporting || this.state.CURRENT_STATE == StateMachine.State.Grapple || this.state.CURRENT_STATE == StateMachine.State.CustomAction0 || LetterBox.IsPlaying || this.state.CURRENT_STATE == StateMachine.State.TimedAction || this.state.CURRENT_STATE == StateMachine.State.Idle_CarryingBody || this.state.CURRENT_STATE == StateMachine.State.Moving_CarryingBody || this.player.GoToAndStopping || this.state.CURRENT_STATE == StateMachine.State.Aiming))
      {
        if (!((Object) this.Indicator != (Object) null) || !((Object) this.Indicator.gameObject != (Object) null) || !this.Indicator.gameObject.activeSelf)
          return;
        this.Indicator.Deactivate();
      }
      else
      {
        if ((double) Time.timeScale == 0.0)
          return;
        if ((Time.frameCount + this.FrameIntervalOffset) % this.UpdateInterval == 0)
          this.GetClosestInteraction();
        bool flag = (Object) Interactor.PreviousInteraction != (Object) Interactor.CurrentInteraction || (Object) Interactor.CurrentInteraction != (Object) null && Interactor.CurrentInteraction.HasChanged || PlayerFollowerSelection.IsPlaying;
        if (PlayerFollowerSelection.IsPlaying)
          Interactor.CurrentInteraction = (Interaction) null;
        if (flag)
        {
          if ((Object) Interactor.PreviousInteraction != (Object) null)
            Interactor.PreviousInteraction.OnBecomeNotCurrent();
          if ((Object) Interactor.CurrentInteraction != (Object) null)
            Interactor.CurrentInteraction.OnBecomeCurrent();
          this.Indicator.Reset();
        }
        if (PlayerFollowerSelection.IsPlaying)
        {
          this.Indicator.gameObject.SetActive(true);
          this.Indicator.Interactable = false;
          this.Indicator.HasSecondaryInteraction = false;
          this.Indicator.SecondaryText.text = "";
          this.Indicator.HasThirdInteraction = false;
          this.Indicator.Thirdtext.text = "";
          this.Indicator.HasFourthInteraction = false;
          this.Indicator.Fourthtext.text = "";
          this.Indicator.text.text = PlayerFollowerSelection.Instance.SelectedFollowers.Count > 0 ? "Release to issue your <color=red>Command</color>" : "Select <color=yellow>Followers</color> to Command";
          this.Indicator.Reset();
        }
        else if ((Object) Interactor.CurrentInteraction == (Object) null)
        {
          if ((Object) Interactor.PreviousInteraction != (Object) null)
            Interactor.PreviousInteraction.OnHoldProgressStop();
          Interactor.PreviousInteraction = (Interaction) null;
          if (!this.Indicator.gameObject.activeSelf)
            return;
          this.Indicator.Deactivate();
        }
        else
        {
          if ((Object) Interactor.CurrentInteraction == (Object) Interactor.PreviousInteraction && !this.Indicator.gameObject.activeSelf)
            this.Indicator.gameObject.SetActive(true);
          this.Indicator.SetPosition((Object) Interactor.CurrentInteraction.LockPosition == (Object) null ? Interactor.CurrentInteraction.transform.position + Interactor.CurrentInteraction.Offset : Interactor.CurrentInteraction.LockPosition.transform.position);
          if (Interactor.CurrentInteraction.ContinuouslyHold)
          {
            this.Indicator.text.text = Interactor.CurrentInteraction.Label;
            if (string.IsNullOrWhiteSpace(Interactor.CurrentInteraction.Label) && string.IsNullOrWhiteSpace(Interactor.CurrentInteraction.SecondaryLabel) && string.IsNullOrWhiteSpace(Interactor.CurrentInteraction.ThirdLabel) && string.IsNullOrWhiteSpace(Interactor.CurrentInteraction.FourthLabel))
              this.Indicator.ContainerImage.enabled = false;
            this.Indicator.Interactable = Interactor.CurrentInteraction.Interactable;
            this.Indicator.HasSecondaryInteraction = Interactor.CurrentInteraction.HasSecondaryInteraction;
            this.Indicator.SecondaryText.text = Interactor.CurrentInteraction.SecondaryLabel;
            this.Indicator.HasThirdInteraction = Interactor.CurrentInteraction.HasThirdInteraction;
            this.Indicator.Thirdtext.text = Interactor.CurrentInteraction.ThirdLabel;
            this.Indicator.HasFourthInteraction = Interactor.CurrentInteraction.HasFourthInteraction;
            this.Indicator.Fourthtext.text = Interactor.CurrentInteraction.FourthLabel;
            this.Indicator.Reset();
          }
          if (flag)
          {
            if ((Object) Interactor.PreviousInteraction != (Object) null)
              Interactor.PreviousInteraction.OnHoldProgressStop();
            this.Indicator.gameObject.SetActive(false);
            this.Indicator.HoldToInteract = Interactor.CurrentInteraction.HoldToInteract && SettingsManager.Settings.Accessibility.HoldActions;
            this.Indicator.Interactable = Interactor.CurrentInteraction.Interactable;
            this.Indicator.HasSecondaryInteraction = Interactor.CurrentInteraction.HasSecondaryInteraction;
            this.Indicator.SecondaryInteractable = Interactor.CurrentInteraction.SecondaryInteractable;
            this.Indicator.HasThirdInteraction = Interactor.CurrentInteraction.HasThirdInteraction;
            this.Indicator.ThirdInteractable = Interactor.CurrentInteraction.ThirdInteractable;
            this.Indicator.HasFourthInteraction = Interactor.CurrentInteraction.HasFourthInteraction;
            this.Indicator.FourthInteractable = Interactor.CurrentInteraction.FourthInteractable;
            this.Indicator.text.text = Interactor.CurrentInteraction.Label;
            this.Indicator.gameObject.SetActive(true);
            if (string.IsNullOrWhiteSpace(Interactor.CurrentInteraction.Label) && string.IsNullOrWhiteSpace(Interactor.CurrentInteraction.SecondaryLabel) && string.IsNullOrWhiteSpace(Interactor.CurrentInteraction.ThirdLabel) && string.IsNullOrWhiteSpace(Interactor.CurrentInteraction.FourthLabel))
              this.Indicator.ContainerImage.enabled = false;
            this.Indicator.SecondaryText.text = Interactor.CurrentInteraction.SecondaryLabel;
            this.Indicator.Thirdtext.text = Interactor.CurrentInteraction.ThirdLabel;
            this.Indicator.Fourthtext.text = Interactor.CurrentInteraction.FourthLabel;
            Interactor.PreviousInteraction = Interactor.CurrentInteraction;
            this.Indicator.Reset();
          }
          if (Interactor.CurrentInteraction.Interactable)
          {
            if (Interactor.CurrentInteraction.HoldToInteract && SettingsManager.Settings.Accessibility.HoldActions)
            {
              if (InputManager.Gameplay.GetInteractButtonDown())
                Interactor.CurrentInteraction.OnHoldProgressDown();
              if (InputManager.Gameplay.GetInteractButtonUp())
                Interactor.CurrentInteraction.OnHoldProgressRelease();
              if (InputManager.Gameplay.GetInteractButtonHeld())
              {
                Interactor.CurrentInteraction.HoldProgress += 1f * Time.deltaTime;
                Interactor.CurrentInteraction.HoldBegun = true;
                this.Indicator.Progress = Interactor.CurrentInteraction.HoldProgress / 1f;
                if ((double) Interactor.CurrentInteraction.HoldProgress >= 1.0)
                {
                  Interactor.CurrentInteraction.OnHoldProgressStop();
                  Interactor.CurrentInteraction.OnInteract(this.state);
                  this.Delay = 0.25f;
                }
              }
              else
              {
                if (!Interactor.CurrentInteraction.HoldBegun && (double) Interactor.CurrentInteraction.HoldProgress > 0.0)
                {
                  Interactor.CurrentInteraction.HoldProgress -= 0.5f * Time.deltaTime;
                  this.Indicator.Progress = Interactor.CurrentInteraction.HoldProgress / 1f;
                }
                else if (Interactor.CurrentInteraction.HoldBegun && (double) Interactor.CurrentInteraction.HoldProgress < 0.20000000298023224)
                {
                  Interactor.CurrentInteraction.HoldProgress += 1f * Time.deltaTime;
                  this.Indicator.Progress = Interactor.CurrentInteraction.HoldProgress / 1f;
                  if ((double) Interactor.CurrentInteraction.HoldProgress >= 0.20000000298023224)
                    Interactor.CurrentInteraction.HoldBegun = false;
                }
                if ((double) Interactor.CurrentInteraction.HoldProgress >= 0.20000000298023224)
                  Interactor.CurrentInteraction.HoldBegun = false;
              }
            }
            else if (Interactor.CurrentInteraction.AutomaticallyInteract)
            {
              Interactor.CurrentInteraction.OnInteract(this.state);
              this.HideIndicator();
              this.Delay = 0.25f;
            }
            else if (InputManager.Gameplay.GetInteractButtonDown())
            {
              if (!Interactor.CurrentInteraction.ContinuouslyHold)
              {
                this.StartCoroutine((IEnumerator) this.WaitForEndOfFrameAndInteract());
                this.Delay = 0.25f;
              }
              else
              {
                Interactor.CurrentInteraction.OnInteract(this.state);
                this.Indicator.text.text = Interactor.CurrentInteraction.Label;
              }
            }
          }
          if (!Interactor.CurrentInteraction.HasSecondaryInteraction || !Interactor.CurrentInteraction.SecondaryInteractable || !InputManager.Gameplay.GetInteract2ButtonDown())
            return;
          Interactor.CurrentInteraction.OnSecondaryInteract(this.state);
          this.Delay = 0.25f;
        }
      }
    }
  }

  private IEnumerator WaitForEndOfFrameAndInteract()
  {
    yield return (object) new WaitForEndOfFrame();
    Interactor.CurrentInteraction.OnInteract(this.state);
  }

  private IEnumerator HideAndRevealIndicator()
  {
    this.HideIndicator();
    yield return (object) new WaitForSeconds(0.3f);
    if ((Object) Interactor.CurrentInteraction != (Object) null && !string.IsNullOrWhiteSpace(Interactor.CurrentInteraction.Label))
      Interactor.PreviousInteraction = (Interaction) null;
  }

  public void HideIndicator()
  {
    if (!this.Indicator.gameObject.activeSelf)
      return;
    this.Indicator.Deactivate();
  }

  private void GetClosestInteraction()
  {
    if (InputManager.Gameplay.GetInteractButtonDown())
      return;
    Interactor.CurrentInteraction = (Interaction) null;
    this.CurrentDist = (float) int.MaxValue;
    this.CurrentPriorityWeight = 0.0f;
    Interaction interaction1 = (Interaction) null;
    if (Interactor.UseRegions)
    {
      Vector3 position = PlayerFarming.Instance.transform.position;
      if (!Interactor.InteractionRegions.ContainsKey(Interactor.PositionToRegions(position)))
        return;
      foreach (Interaction interaction2 in Interactor.InteractionRegions[Interactor.PositionToRegions(position)])
      {
        if (interaction2.gameObject.activeInHierarchy)
        {
          this.TestDist = Vector3.Distance(interaction2.gameObject.transform.position + interaction2.ActivatorOffset, this.gameObject.transform.position);
          if ((double) this.TestDist < (double) interaction2.ActivateDistance)
          {
            if ((Object) interaction1 == (Object) null || (double) interaction2.PriorityWeight >= (double) interaction1.PriorityWeight)
              interaction1 = interaction2;
            if ((double) this.TestDist < (double) this.CurrentDist && (!string.IsNullOrWhiteSpace(interaction2.Label) || !string.IsNullOrWhiteSpace(interaction2.SecondaryLabel)))
            {
              Interactor.CurrentInteraction = interaction2;
              this.CurrentDist = this.TestDist;
              this.CurrentPriorityWeight = interaction2.PriorityWeight;
            }
          }
        }
      }
      if (!((Object) Interactor.CurrentInteraction != (Object) null))
        return;
      if ((Object) interaction1 != (Object) null && (double) interaction1.PriorityWeight > (double) Interactor.CurrentInteraction.PriorityWeight)
        Interactor.CurrentInteraction = interaction1;
      if (!(Interactor.CurrentInteraction is interaction_FollowerInteraction) || (double) PlayerFarming.Instance.playerController.speed > 0.0 || !((Object) Interactor.PreviousInteraction != (Object) null))
        return;
      Interactor.CurrentInteraction = Interactor.PreviousInteraction;
    }
    else
    {
      foreach (Interaction interaction3 in Interaction.interactions)
      {
        if ((Object) interaction3 != (Object) null && interaction3.gameObject.activeInHierarchy)
        {
          this.TestDist = Vector3.Distance(interaction3.gameObject.transform.position + interaction3.ActivatorOffset, this.gameObject.transform.position);
          if ((double) this.TestDist < (double) interaction3.ActivateDistance)
          {
            if ((Object) interaction1 == (Object) null || (double) interaction3.PriorityWeight >= (double) interaction1.PriorityWeight)
              interaction1 = interaction3;
            if ((double) this.TestDist < (double) this.CurrentDist && (!string.IsNullOrWhiteSpace(interaction3.Label) || !string.IsNullOrWhiteSpace(interaction3.SecondaryLabel)))
            {
              Interactor.CurrentInteraction = interaction3;
              this.CurrentDist = this.TestDist;
              this.CurrentPriorityWeight = interaction3.PriorityWeight;
            }
          }
        }
      }
      if (!((Object) Interactor.CurrentInteraction != (Object) null))
        return;
      if ((Object) interaction1 != (Object) null && (double) interaction1.PriorityWeight > (double) Interactor.CurrentInteraction.PriorityWeight)
        Interactor.CurrentInteraction = interaction1;
      if (!(Interactor.CurrentInteraction is interaction_FollowerInteraction) || (double) PlayerFarming.Instance.playerController.speed > 0.0 || !((Object) Interactor.PreviousInteraction != (Object) null))
        return;
      Interactor.CurrentInteraction = Interactor.PreviousInteraction;
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

  private static Vector3Int PositionToRegions(Vector3 Position)
  {
    return Vector3Int.FloorToInt(Position) / 3;
  }
}
