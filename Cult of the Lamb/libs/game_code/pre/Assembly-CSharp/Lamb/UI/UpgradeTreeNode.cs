// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UpgradeTreeNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private const string kLockedStateLayer = "Locked";
  private const string kUnavailableStateLayer = "Unavailable";
  private const string kAvailableStateLayer = "Available";
  private const string kUnlockedStateLayer = "Unlocked";
  public Action<UpgradeTreeNode> OnUpgradeNodeSelected;
  public Action<UpgradeTreeNode> OnStateDidChange;
  [Header("Components")]
  [SerializeField]
  private RectTransform _rectTransform;
  [SerializeField]
  private MMButton _button;
  [SerializeField]
  private TextMeshProUGUI _title;
  [SerializeField]
  private Localize _localize;
  [SerializeField]
  private Animator _animator;
  [Header("Upgrade Category")]
  [SerializeField]
  private TextMeshProUGUI _categoryText;
  [SerializeField]
  private UpgradeCategoryIconMapping _categoryIconMapping;
  [Header("Upgrade")]
  [SerializeField]
  private UpgradeTreeConfiguration _treeConfig;
  [SerializeField]
  private UpgradeSystem.Type _upgrade;
  [SerializeField]
  private StructureBrain.TYPES _requiresBuiltStructure;
  [SerializeField]
  private Image _upgradeIcon;
  [SerializeField]
  private UpgradeTypeMapping _upgradeMapping;
  [Header("Tier")]
  [SerializeField]
  private UpgradeTreeNode.TreeTier _nodeTier;
  [SerializeField]
  private UpgradeTreeNode[] _prerequisiteNodes;
  [SerializeField]
  private List<UpgradeTreeNode> _nodeConnections = new List<UpgradeTreeNode>();
  private UpgradeTreeNode.NodeState _state;
  private float _lockedWeight;
  private float _unavailableWeight;
  private float _availableWeight;
  private float _unlockedWeight;
  private bool _configured;
  private UpgradeTreeConfiguration.TreeTierConfig _tierConfig;

  public RectTransform RectTransform => this._rectTransform;

  public MMButton Button => this._button;

  public UpgradeSystem.Type Upgrade => this._upgrade;

  public StructureBrain.TYPES RequiresBuiltStructure => this._requiresBuiltStructure;

  public UpgradeTreeNode.NodeState State => this._state;

  public float LockedWeight => this._lockedWeight;

  public float UnavailableWeight => this._unavailableWeight;

  public float AvailableWeight => this._availableWeight;

  public float UnlockedWeight => this._unlockedWeight;

  public UpgradeTreeNode.TreeTier NodeTier => this._nodeTier;

  public UpgradeTreeConfiguration.TreeTierConfig TierConfig => this._tierConfig;

  public UpgradeTreeConfiguration TreeConfig => this._treeConfig;

  public UpgradeTreeNode[] PrerequisiteNodes => this._prerequisiteNodes;

  public List<UpgradeTreeNode> NodeConnections => this._nodeConnections;

  private void Start() => this._button.onClick.AddListener(new UnityAction(this.OnButtonClicked));

  public void Configure(UpgradeTreeNode.TreeTier currentTreeTier, bool forceReconfigure = false)
  {
    UpgradeTreeNode.NodeState state = this._state;
    if (forceReconfigure)
      this._configured = false;
    if (this._tierConfig == null)
      this._tierConfig = this._treeConfig.GetConfigForTier(this._nodeTier);
    if (!this._configured)
    {
      if (this._requiresBuiltStructure != StructureBrain.TYPES.NONE && !DataManager.Instance.HistoryOfStructures.Contains(this.RequiresBuiltStructure))
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
      }
    }
    foreach (UpgradeTreeNode nodeConnection in this._nodeConnections)
    {
      if (!nodeConnection._configured | forceReconfigure)
        nodeConnection.Configure(currentTreeTier, forceReconfigure);
    }
    if (!forceReconfigure)
      this.UpdateAnimationLayerStates();
    if (this._state == state)
      return;
    Action<UpgradeTreeNode> onStateDidChange = this.OnStateDidChange;
    if (onStateDidChange == null)
      return;
    onStateDidChange(this);
  }

  private bool UnlockedPrerequisites()
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
    upgradeTreeNode._rectTransform.DOScale((Vector3) (scale * 1.5f), 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    upgradeTreeNode.StartCoroutine((IEnumerator) upgradeTreeNode.UpdateStateWeights(0.75f));
    upgradeTreeNode.transform.DOScale((Vector3) scale, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.25f);
  }

  private IEnumerator UpdateStateWeights(float duration)
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
      yield return (object) null;
    }
  }

  private void OnButtonClicked()
  {
    Action<UpgradeTreeNode> upgradeNodeSelected = this.OnUpgradeNodeSelected;
    if (upgradeNodeSelected == null)
      return;
    upgradeNodeSelected(this);
  }

  public void UpdateAnimationLayerStates() => this.SetAnimationLayerState(this._state);

  private void SetAnimationLayerState(UpgradeTreeNode.NodeState nodeState)
  {
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Locked"), nodeState == UpgradeTreeNode.NodeState.Locked ? 1f : 0.0f);
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Unavailable"), nodeState == UpgradeTreeNode.NodeState.Unavailable ? 1f : 0.0f);
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Available"), nodeState == UpgradeTreeNode.NodeState.Available ? 1f : 0.0f);
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Unlocked"), nodeState == UpgradeTreeNode.NodeState.Unlocked ? 1f : 0.0f);
  }

  private void OnValidate()
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

  public enum NodeState
  {
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
