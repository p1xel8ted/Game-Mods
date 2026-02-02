// Decompiled with JetBrains decompiler
// Type: LogisticsConnectionItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class LogisticsConnectionItem : MonoBehaviour
{
  [SerializeField]
  public Image background;
  [SerializeField]
  public Image rootImage;
  [SerializeField]
  public Image targetImage;
  [SerializeField]
  public TMP_Text rootHeader;
  [SerializeField]
  public TMP_Text targetHeader;
  [SerializeField]
  public MMButton button;
  [SerializeField]
  public RectTransform purchased;
  [SerializeField]
  public RectTransform locked;
  [SerializeField]
  public RectTransform empty;
  [SerializeField]
  public TMP_Text lockedText;
  [SerializeField]
  public Image buyOverlay;
  [CompilerGenerated]
  public StructureBrain.TYPES \u003CRootStructureType\u003Ek__BackingField;
  [CompilerGenerated]
  public StructureBrain.TYPES \u003CTargetStructureType\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CIsLocked\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CIsEmpty\u003Ek__BackingField;
  public Action<LogisticsConnectionItem> OnHighlighted;
  public Action<LogisticsConnectionItem> OnSelected;

  public StructureBrain.TYPES RootStructureType
  {
    get => this.\u003CRootStructureType\u003Ek__BackingField;
    set => this.\u003CRootStructureType\u003Ek__BackingField = value;
  }

  public StructureBrain.TYPES TargetStructureType
  {
    get => this.\u003CTargetStructureType\u003Ek__BackingField;
    set => this.\u003CTargetStructureType\u003Ek__BackingField = value;
  }

  public bool IsLocked
  {
    get => this.\u003CIsLocked\u003Ek__BackingField;
    set => this.\u003CIsLocked\u003Ek__BackingField = value;
  }

  public bool IsEmpty
  {
    get => this.\u003CIsEmpty\u003Ek__BackingField;
    set => this.\u003CIsEmpty\u003Ek__BackingField = value;
  }

  public MMButton MMButton => this.button;

  public void Awake()
  {
    if ((UnityEngine.Object) this.MMButton != (UnityEngine.Object) null)
    {
      this.MMButton.onClick.AddListener(new UnityAction(this.OnItemSelected));
      this.MMButton.OnSelected += new System.Action(this.OnItemHighlighted);
      this.MMButton.OnDeselected += new System.Action(this.OnItemUnhighlighted);
    }
    DOTweenModuleUI.DOFade(this.buyOverlay, 0.0f, 0.0f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
  }

  public void OnDestroy()
  {
    if (!((UnityEngine.Object) this.MMButton != (UnityEngine.Object) null))
      return;
    this.MMButton.onClick.RemoveAllListeners();
    this.MMButton.OnSelected -= new System.Action(this.OnItemHighlighted);
    this.MMButton.OnDeselected -= new System.Action(this.OnItemUnhighlighted);
  }

  public void OnItemUnhighlighted()
  {
    this.StopAllCoroutines();
    StructureBrain.TYPES types = this.RootStructureType;
    string str1 = types.ToString();
    types = this.TargetStructureType;
    string str2 = types.ToString();
    Debug.Log((object) $"LogisticsConnectionItem unhighlighted: {str1} -> {str2}");
  }

  public void OnItemHighlighted()
  {
    Action<LogisticsConnectionItem> onHighlighted = this.OnHighlighted;
    if (onHighlighted != null)
      onHighlighted(this);
    Debug.Log((object) $"LogisticsConnectionItem highlighted: {this.RootStructureType.ToString()} -> {this.TargetStructureType.ToString()}");
  }

  public void OnDisable()
  {
    this.StopAllCoroutines();
    this.transform.localScale = Vector3.one;
  }

  public void OnItemSelected()
  {
    Action<LogisticsConnectionItem> onSelected = this.OnSelected;
    if (onSelected == null)
      return;
    onSelected(this);
  }

  public void Configure(
    StructureBrain.TYPES rootStructureType,
    StructureBrain.TYPES targetStructureType)
  {
    this.purchased.gameObject.SetActive(true);
    this.empty.gameObject.SetActive(false);
    this.locked.gameObject.SetActive(false);
    this.IsEmpty = false;
    this.RootStructureType = rootStructureType;
    this.TargetStructureType = targetStructureType;
    this.rootImage.sprite = TypeAndPlacementObjects.GetByType(rootStructureType)?.IconImage;
    this.targetImage.sprite = TypeAndPlacementObjects.GetByType(targetStructureType)?.IconImage;
    this.rootHeader.text = StructuresData.LocalizedName(rootStructureType);
    this.targetHeader.text = StructuresData.LocalizedName(targetStructureType);
  }

  public void Configure(bool isAvailable, bool justBought = false)
  {
    if (justBought)
    {
      this.transform.DOComplete();
      this.transform.DOPunchScale(Vector3.one * 0.1f, 0.33f).SetUpdate<Tweener>(true);
      this.buyOverlay.color = StaticColors.GreenColor;
      this.buyOverlay.DOComplete();
      DOTweenModuleUI.DOFade(this.buyOverlay, 0.0f, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      UIManager.PlayAudio("event:/shop/buy");
      MMVibrate.Haptic(MMVibrate.HapticTypes.Success);
    }
    this.IsLocked = !isAvailable;
    this.IsEmpty = isAvailable;
    this.purchased.gameObject.SetActive(false);
    this.empty.gameObject.SetActive(isAvailable);
    this.locked.gameObject.SetActive(!isAvailable);
    this.lockedText.text = string.Format(LocalizationManager.GetTranslation("UI/Logistics/UnlockSlot"), (object) CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.YEW_HOLY, 5));
  }

  public void ShakeLocked()
  {
    this.transform.DOComplete();
    this.transform.DOShakePosition(0.33f, Vector3.right * 10f).SetUpdate<Tweener>(true);
    UIManager.PlayAudio("event:/ui/negative_feedback");
  }
}
