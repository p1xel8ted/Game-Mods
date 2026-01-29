// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UpgradeTreeNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI.Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UpgradeTreeNode : MonoBehaviour, ISelectHandler, IEventSystemHandler
{
  public const string kLockedStateLayer = "Locked";
  public const string kUnavailableStateLayer = "Unavailable";
  public const string kAvailableStateLayer = "Available";
  public const string kUnlockedStateLayer = "Unlocked";
  public Action<UpgradeTreeNode> OnUpgradeNodeSelected;
  public Action<UpgradeTreeNode> OnStateDidChange;
  [Header("Components")]
  [SerializeField]
  public RectTransform _rectTransform;
  [SerializeField]
  public MMButton _button;
  [SerializeField]
  public TextMeshProUGUI _title;
  [SerializeField]
  public Localize _localize;
  [SerializeField]
  public Animator _animator;
  [Header("Upgrade Category")]
  [SerializeField]
  public TextMeshProUGUI _categoryText;
  [SerializeField]
  public UpgradeCategoryIconMapping _categoryIconMapping;
  [Header("Upgrade")]
  [SerializeField]
  public UpgradeTreeConfiguration _treeConfig;
  [SerializeField]
  public UpgradeSystem.Type _upgrade;
  [SerializeField]
  public StructureBrain.TYPES _requiresBuiltStructure;
  [SerializeField]
  public Image _upgradeIcon;
  [SerializeField]
  public UpgradeTypeMapping _upgradeMapping;
  [Header("Availability Highlight")]
  [SerializeField]
  public bool _highlightWhenAvailable;
  [SerializeField]
  public GameObject _availabilityAlert;
  [Header("Tier")]
  [SerializeField]
  public UpgradeTreeNode.TreeTier _nodeTier;
  [SerializeField]
  public UpgradeTreeNode[] _prerequisiteNodes;
  [SerializeField]
  public List<UpgradeTreeNode> _nodeConnections = new List<UpgradeTreeNode>();
  [Header("Tree Appearance Modifiers")]
  [SerializeField]
  public bool _nonCenteredStem;
  [SerializeField]
  public UpgradeSystem.Type requiresUpgrade;
  public UpgradeTreeNode.NodeState _state;
  public float _lockedWeight;
  public float _unavailableWeight;
  public float _availableWeight;
  public float _unlockedWeight;
  public float _hiddenWeight;
  public bool _configured;
  public UpgradeTreeConfiguration.TreeTierConfig _tierConfig;

  public UpgradeSystem.Type RequiresUpgrade => this.requiresUpgrade;

  public RectTransform RectTransform => this._rectTransform;

  public MMButton Button => this._button;

  public UpgradeSystem.Type Upgrade => this._upgrade;

  public StructureBrain.TYPES RequiresBuiltStructure => this._requiresBuiltStructure;

  public UpgradeTreeNode.NodeState State => this._state;

  public float LockedWeight => this._lockedWeight;

  public float UnavailableWeight => this._unavailableWeight;

  public float AvailableWeight => this._availableWeight;

  public float UnlockedWeight => this._unlockedWeight;

  public float HiddenWeight => this._hiddenWeight;

  public UpgradeTreeNode.TreeTier NodeTier => this._nodeTier;

  public UpgradeTreeConfiguration.TreeTierConfig TierConfig => this._tierConfig;

  public UpgradeTreeConfiguration TreeConfig => this._treeConfig;

  public UpgradeTreeNode[] PrerequisiteNodes => this._prerequisiteNodes;

  public bool NonCenteredStem => this._nonCenteredStem;

  public bool HighlightWhenAvailable => this._highlightWhenAvailable;

  public bool Configured
  {
    get => this._configured;
    set => this._configured = value;
  }

  public List<UpgradeTreeNode> NodeConnections => this._nodeConnections;

  public void Start() => this._button.onClick.AddListener(new UnityAction(this.OnButtonClicked));

  public void OnEnable()
  {
    if (this.IsAvailable())
      return;
    this.gameObject.SetActive(false);
    ITreeMenuController componentInParent = this.GetComponentInParent<ITreeMenuController>();
    if (componentInParent != null)
    {
      foreach (NodeConnectionLine connectionLine in componentInParent.GetConnectionLines())
      {
        if (connectionLine.Nodes.Contains(this))
        {
          int num = 0;
          foreach (UpgradeTreeNode node in connectionLine.Nodes)
          {
            if (node.RequiresUpgrade != this.RequiresUpgrade)
              ++num;
          }
          if (num < 1)
            connectionLine.gameObject.SetActive(false);
        }
      }
    }
    this.Button.Interactable = false;
  }

  public void Configure(UpgradeTreeNode.TreeTier currentTreeTier, bool forceReconfigure = false)
  {
    UpgradeTreeNode.NodeState state = this._state;
    if (forceReconfigure)
      this._configured = false;
    this._availabilityAlert.SetActive(false);
    if (this._tierConfig == null)
      this._tierConfig = this._treeConfig.GetConfigForTier(this._nodeTier);
    if (!this._configured)
    {
      if (!this.IsAvailable())
      {
        this._state = UpgradeTreeNode.NodeState.Hidden;
        this._configured = true;
      }
      else if (this._requiresBuiltStructure != StructureBrain.TYPES.NONE && !DataManager.Instance.HistoryOfStructures.Contains(this.RequiresBuiltStructure))
      {
        this._state = UpgradeTreeNode.NodeState.Locked;
        this._configured = true;
      }
      else if (this._upgrade == UpgradeSystem.Type.Building_Furnace_2 && DataManager.Instance.DLCUpgradeTreeSnowIncrement <= 0)
      {
        this._state = UpgradeTreeNode.NodeState.Locked;
        this._configured = true;
      }
      else if (this._upgrade == UpgradeSystem.Type.Building_Furnace_3 && DataManager.Instance.DLCUpgradeTreeSnowIncrement <= 1)
      {
        this._state = UpgradeTreeNode.NodeState.Locked;
        this._configured = true;
      }
      else if (UpgradeSystem.GetUnlocked(this._upgrade))
      {
        this._state = UpgradeTreeNode.NodeState.Unlocked;
        this._configured = true;
      }
      else if (this._tierConfig.RequiresCentralTier && this._tierConfig.CentralNode == this.Upgrade && this._treeConfig.NumUnlockedUpgrades() >= this._treeConfig.NumRequiredNodesForTier(this._nodeTier) && this.UnlockedPrerequisites())
      {
        this._state = UpgradeTreeNode.NodeState.Available;
        this._configured = true;
      }
      else if (currentTreeTier < this._nodeTier)
      {
        this._state = UpgradeTreeNode.NodeState.Locked;
        this._configured = true;
      }
      else
      {
        this._state = UpgradeTreeNode.NodeState.Unavailable;
        if (this._prerequisiteNodes.Length != 0)
        {
          foreach (UpgradeTreeNode prerequisiteNode in this._prerequisiteNodes)
          {
            if (!prerequisiteNode._configured)
              prerequisiteNode.Configure(currentTreeTier);
            if (prerequisiteNode.State == UpgradeTreeNode.NodeState.Unlocked)
            {
              this._state = UpgradeTreeNode.NodeState.Available;
              break;
            }
          }
        }
        else
          this._state = UpgradeTreeNode.NodeState.Available;
        this._configured = true;
      }
      if (!forceReconfigure)
      {
        if (this._state == UpgradeTreeNode.NodeState.Locked)
          this._lockedWeight = 1f;
        else if (this._state == UpgradeTreeNode.NodeState.Unavailable)
          this._unavailableWeight = 1f;
        else if (this._state == UpgradeTreeNode.NodeState.Available)
          this._availableWeight = 1f;
        else if (this._state == UpgradeTreeNode.NodeState.Unlocked)
          this._unlockedWeight = 1f;
        else if (this._state == UpgradeTreeNode.NodeState.Hidden)
          this._hiddenWeight = 1f;
      }
    }
    foreach (UpgradeTreeNode nodeConnection in this._nodeConnections)
    {
      if (!nodeConnection._configured | forceReconfigure)
        nodeConnection.Configure(currentTreeTier, forceReconfigure);
    }
    if (!forceReconfigure && (this._state == UpgradeTreeNode.NodeState.Available || this._state == UpgradeTreeNode.NodeState.Unavailable) && this._highlightWhenAvailable)
      this._availabilityAlert.SetActive(true);
    if (!forceReconfigure)
      this.UpdateAnimationLayerStates();
    if (this._state == state)
      return;
    Action<UpgradeTreeNode> onStateDidChange = this.OnStateDidChange;
    if (onStateDidChange == null)
      return;
    onStateDidChange(this);
  }

  public bool UnlockedPrerequisites()
  {
    foreach (UpgradeTreeNode prerequisiteNode in this._prerequisiteNodes)
    {
      if (prerequisiteNode.State == UpgradeTreeNode.NodeState.Unlocked)
        return true;
    }
    return false;
  }

  public IEnumerator DoUpdateStateAnimation()
  {
    UpgradeTreeNode upgradeTreeNode = this;
    Vector2 scale = (Vector2) upgradeTreeNode._rectTransform.localScale;
    upgradeTreeNode.UpdateAnimationLayerStates();
    if (upgradeTreeNode.State == UpgradeTreeNode.NodeState.Unlocked)
      UIManager.PlayAudio("event:/unlock_building/unlock");
    else if (upgradeTreeNode.State == UpgradeTreeNode.NodeState.Available)
      UIManager.PlayAudio("event:/unlock_building/selection_flash");
    upgradeTreeNode._rectTransform.DOScale((Vector3) (scale * 1.5f), 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    upgradeTreeNode.StartCoroutine((IEnumerator) upgradeTreeNode.UpdateStateWeights(0.75f));
    upgradeTreeNode.transform.DOScale((Vector3) scale, 0.35f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    if (upgradeTreeNode._highlightWhenAvailable && (upgradeTreeNode._state == UpgradeTreeNode.NodeState.Available || upgradeTreeNode._state == UpgradeTreeNode.NodeState.Unavailable))
    {
      upgradeTreeNode._availabilityAlert.SetActive(true);
      upgradeTreeNode._availabilityAlert.transform.localScale = Vector3.zero;
      upgradeTreeNode._availabilityAlert.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      yield return (object) new WaitForSecondsRealtime(0.6f);
    }
  }

  public IEnumerator UpdateStateWeights(float duration)
  {
    float t = 0.0f;
    while ((double) t < (double) duration)
    {
      t += Time.unscaledDeltaTime;
      if ((double) this._lockedWeight < 1.0 && this._state == UpgradeTreeNode.NodeState.Locked)
        this._lockedWeight = t / duration;
      else if ((double) this._lockedWeight > 0.0)
        this._lockedWeight = (float) (1.0 - (double) t / (double) duration);
      if ((double) this._unavailableWeight < 1.0 && this._state == UpgradeTreeNode.NodeState.Unavailable)
        this._unavailableWeight = t / duration;
      else if ((double) this._unavailableWeight > 0.0)
        this._unavailableWeight = (float) (1.0 - (double) t / (double) duration);
      if ((double) this._availableWeight < 1.0 && this._state == UpgradeTreeNode.NodeState.Available)
        this._availableWeight = t / duration;
      else if ((double) this._availableWeight > 0.0)
        this._availableWeight = (float) (1.0 - (double) t / (double) duration);
      if ((double) this._unlockedWeight < 1.0 && this._state == UpgradeTreeNode.NodeState.Unlocked)
        this._unlockedWeight = t / duration;
      else if ((double) this._unlockedWeight > 0.0)
        this._unlockedWeight = (float) (1.0 - (double) t / (double) duration);
      if ((double) this._hiddenWeight < 1.0 && this._state == UpgradeTreeNode.NodeState.Hidden)
        this._hiddenWeight = t / duration;
      else if ((double) this._hiddenWeight > 0.0)
        this._hiddenWeight = (float) (1.0 - (double) t / (double) duration);
      yield return (object) null;
    }
  }

  public void ForceState(UpgradeTreeNode.NodeState state) => this._state = state;

  public void OnButtonClicked()
  {
    Action<UpgradeTreeNode> upgradeNodeSelected = this.OnUpgradeNodeSelected;
    if (upgradeNodeSelected == null)
      return;
    upgradeNodeSelected(this);
  }

  public void UpdateAnimationLayerStates() => this.SetAnimationLayerState(this._state);

  public void SetAnimationLayerState(UpgradeTreeNode.NodeState nodeState)
  {
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Locked"), nodeState == UpgradeTreeNode.NodeState.Locked ? 1f : 0.0f);
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Unavailable"), nodeState == UpgradeTreeNode.NodeState.Unavailable ? 1f : 0.0f);
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Available"), nodeState == UpgradeTreeNode.NodeState.Available ? 1f : 0.0f);
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Unlocked"), nodeState == UpgradeTreeNode.NodeState.Unlocked ? 1f : 0.0f);
  }

  public void OnValidate()
  {
    if ((UnityEngine.Object) this._upgradeMapping != (UnityEngine.Object) null)
    {
      UpgradeTypeMapping.SpriteItem spriteItem = this._upgradeMapping.GetItem(this._upgrade);
      if (spriteItem != null)
      {
        if ((UnityEngine.Object) this._categoryText != (UnityEngine.Object) null && (UnityEngine.Object) this._categoryIconMapping != (UnityEngine.Object) null)
        {
          this._categoryText.text = UpgradeCategoryIconMapping.GetIcon(spriteItem.Category);
          this._categoryText.color = this._categoryIconMapping.GetColor(spriteItem.Category);
        }
        if ((UnityEngine.Object) this._upgradeIcon != (UnityEngine.Object) null)
          this._upgradeIcon.sprite = spriteItem.Sprite;
      }
    }
    if (!((UnityEngine.Object) this._title != (UnityEngine.Object) null))
      return;
    this._title.text = UpgradeSystem.GetLocalizedName(this._upgrade);
    this._localize.Term = $"UpgradeSystem/{this._upgrade}/Name";
  }

  public void OnSelect(BaseEventData eventData)
  {
    UIManager.PlayAudio("event:/upgrade_statue/upgrade_statue_scroll");
  }

  public bool IsAvailable()
  {
    return this.requiresUpgrade == UpgradeSystem.Type.Combat_ExtraHeart1 || UpgradeSystem.GetUnlocked(this.requiresUpgrade);
  }

  public void ShowNode(bool configure = true)
  {
    this.gameObject.SetActive(true);
    CanvasGroup component = this.GetComponent<CanvasGroup>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      component.alpha = 0.0f;
      component.DOFade(1f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    }
    foreach (NodeConnectionLine connectionLine in this.GetComponentInParent<ITreeMenuController>().GetConnectionLines())
    {
      if (connectionLine.Nodes.Contains(this))
      {
        connectionLine.Line.DOFade(0.0f, 0.0f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
        connectionLine.Line.DOFade(1f, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
        connectionLine.gameObject.SetActive(true);
      }
    }
    this.Button.Interactable = true;
    if (!configure)
      return;
    this.Configure(this.TierConfig.Tier, true);
  }

  public enum NodeState
  {
    Hidden,
    Locked,
    Unavailable,
    Available,
    Unlocked,
  }

  public enum TreeTier
  {
    Tier1,
    Tier2,
    Tier3,
    Tier4,
    Tier5,
    Tier6,
  }
}
