// Decompiled with JetBrains decompiler
// Type: LogisticsMenuItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class LogisticsMenuItem : MonoBehaviour
{
  [SerializeField]
  public Image outline;
  [SerializeField]
  public Image image;
  [SerializeField]
  public Image category;
  [SerializeField]
  public MMButton button;
  [CompilerGenerated]
  public StructureBrain.TYPES \u003CStructureType\u003Ek__BackingField;
  public Action<LogisticsMenuItem> OnHighlighted;
  public Action<LogisticsMenuItem> OnSelected;

  public StructureBrain.TYPES StructureType
  {
    get => this.\u003CStructureType\u003Ek__BackingField;
    set => this.\u003CStructureType\u003Ek__BackingField = value;
  }

  public MMButton MMButton => this.button;

  public Image Image => this.image;

  public void Awake()
  {
    if ((UnityEngine.Object) this.MMButton != (UnityEngine.Object) null)
    {
      this.MMButton.onClick.AddListener(new UnityAction(this.OnItemSelected));
      this.MMButton.OnSelected += new System.Action(this.OnItemHighlighted);
      this.MMButton.OnDeselected += new System.Action(this.OnItemUnhighlighted);
    }
    this.outline.DOComplete();
    this.outline.transform.DOScale(Vector3.one * 0.9f, 0.0f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
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
    this.outline.color = StaticColors.GreyColor;
    this.StopAllCoroutines();
    this.outline.DOComplete();
    this.outline.transform.DOScale(Vector3.one * 0.9f, 0.25f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this.transform.DOComplete();
    this.transform.DOScale(Vector3.one, 0.25f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
  }

  public void OnItemHighlighted()
  {
    this.outline.color = StaticColors.OffWhiteColor;
    Action<LogisticsMenuItem> onHighlighted = this.OnHighlighted;
    if (onHighlighted != null)
      onHighlighted(this);
    this.StopAllCoroutines();
    this.outline.DOComplete();
    this.outline.transform.DOScale(Vector3.one * 1.05f, 0.25f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this.transform.DOComplete();
    this.transform.DOScale(Vector3.one * 1.05f, 0.25f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
  }

  public void OnDisable()
  {
    this.StopAllCoroutines();
    this.transform.localScale = Vector3.one;
    this.outline.color = StaticColors.GreyColor;
  }

  public void OnItemSelected()
  {
    Action<LogisticsMenuItem> onSelected = this.OnSelected;
    if (onSelected == null)
      return;
    onSelected(this);
  }

  public void Configure(StructureBrain.TYPES structureType, bool disabled = false)
  {
    this.StructureType = structureType;
    this.image.sprite = TypeAndPlacementObjects.GetByType(structureType)?.IconImage;
    if (!disabled)
      return;
    this.image.color = new Color(1f, 1f, 1f, 0.5f);
    this.button.Interactable = false;
  }
}
