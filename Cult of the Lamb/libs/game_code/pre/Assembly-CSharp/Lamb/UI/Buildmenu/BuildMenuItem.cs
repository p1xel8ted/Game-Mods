// Decompiled with JetBrains decompiler
// Type: Lamb.UI.BuildMenu.BuildMenuItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI.Alerts;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.BuildMenu;

public class BuildMenuItem : MonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler
{
  public Action<StructureBrain.TYPES> OnStructureSelected;
  [SerializeField]
  private RectTransform _rectTransform;
  [SerializeField]
  private RectTransform _container;
  [SerializeField]
  private CanvasGroup _canvasGroup;
  [SerializeField]
  private MMButton _button;
  [SerializeField]
  private Image _icon;
  [SerializeField]
  private Image _canAffordIcon;
  [SerializeField]
  private Image _cantAffordIcon;
  [SerializeField]
  private Image _selectedIcon;
  [SerializeField]
  private Image _flashIcon;
  [SerializeField]
  private TextMeshProUGUI _costText;
  [SerializeField]
  private StructureAlert _alert;
  [SerializeField]
  private Material _blackAndWhiteMaterial;
  [SerializeField]
  private GameObject _lockedContainer;
  private Material _blackAndWhiteMaterialInstance;
  public StructureBrain.TYPES _structureType;
  private bool _alreadyBuilt;
  private Vector2 _anchoredOrigin;
  private bool _locked;

  public MMButton Button => this._button;

  public StructureBrain.TYPES Structure => this._structureType;

  public bool Locked => this._locked;

  public RectTransform RectTransform => this._rectTransform;

  private bool _clickable
  {
    get => this._button.Confirmable;
    set => this._button.Confirmable = value;
  }

  public void Configure(StructureBrain.TYPES structureType)
  {
    this._anchoredOrigin = this._container.anchoredPosition;
    this._structureType = structureType;
    this._icon.sprite = TypeAndPlacementObjects.GetByType(this._structureType)?.IconImage;
    this._costText.text = StructuresData.ItemCost.GetCostString(StructuresData.GetCost(structureType));
    this._button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
    this._button.OnConfirmDenied += new System.Action(this.OnButtonClickDenied);
    this._button.Confirmable = false;
    this._alert.Configure(this._structureType);
    this._selectedIcon.enabled = false;
    this._clickable = false;
    this._locked = !StructuresData.GetUnlocked(structureType);
    this._lockedContainer.SetActive(this._locked);
    if (this._locked)
    {
      this.SetBlackAndWhite();
      this._costText.text = "";
      this._locked = true;
    }
    else if (StructuresData.RequiresTempleToBuild(this._structureType) && !StructuresData.HasTemple())
    {
      this.SetBlackAndWhite();
      this._costText.text = "-";
    }
    else if (StructuresData.GetBuildOnlyOne(this._structureType) && (StructureManager.IsBuilt(this._structureType) || StructureManager.IsBuilding(this._structureType) || StructureManager.IsAnyUpgradeBuiltOrBuilding(this._structureType)))
    {
      this.SetBlackAndWhite();
      this._costText.text = "-";
    }
    else if (StructuresData.IsUpgradeStructure(this._structureType) && StructureManager.GetAllStructuresOfType(StructuresData.GetUpgradePrerequisite(this._structureType)).Count <= 0)
      this.SetBlackAndWhite();
    else if (!this.CheckCanAfford(this._structureType))
      this.SetBlackAndWhite();
    else
      this._clickable = true;
  }

  public void SetBlackAndWhite()
  {
    this._blackAndWhiteMaterialInstance = new Material(this._blackAndWhiteMaterial);
    this._blackAndWhiteMaterialInstance.SetFloat("_GrayscaleLerpFade", 1f);
    this._icon.material = this._blackAndWhiteMaterialInstance;
    this._cantAffordIcon.material = this._blackAndWhiteMaterialInstance;
    this._canAffordIcon.gameObject.SetActive(this._clickable);
    this._cantAffordIcon.gameObject.SetActive(!this._clickable);
  }

  public void ForceIncognitoState()
  {
    this.SetBlackAndWhite();
    this._alert.gameObject.SetActive(false);
    this._costText.text = "";
    this._cantAffordIcon.gameObject.SetActive(true);
    this._canAffordIcon.gameObject.SetActive(false);
  }

  public void ForceLockedState()
  {
    this._alert.gameObject.SetActive(false);
    this._lockedContainer.SetActive(true);
    this._costText.text = "";
  }

  public IEnumerator DoUnlock()
  {
    this._container.DOScale(Vector3.one * 0.75f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    this.Configure(this._structureType);
    if ((UnityEngine.Object) this._blackAndWhiteMaterialInstance != (UnityEngine.Object) null)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this._blackAndWhiteMaterialInstance);
      this._icon.material = (Material) null;
      this._cantAffordIcon.material = (Material) null;
    }
    this._flashIcon.gameObject.SetActive(true);
    this._alert.gameObject.SetActive(false);
    UIManager.PlayAudio("event:/unlock_building/unlock");
    this._container.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    DOTweenModuleUI.DOColor(this._flashIcon, new Color(1f, 1f, 1f, 0.0f), 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    yield return (object) new WaitForSecondsRealtime(0.1f);
    Vector3 endValue = new Vector3(0.3f, 0.3f, 1f);
    this._alert.transform.localScale = Vector3.zero;
    this._alert.transform.DOScale(endValue, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this._alert.gameObject.SetActive(true);
    yield return (object) new WaitForSecondsRealtime(0.5f);
  }

  private bool CheckCanAfford(StructureBrain.TYPES type)
  {
    if (CheatConsole.BuildingsFree)
      return true;
    foreach (StructuresData.ItemCost itemCost in StructuresData.GetCost(type))
    {
      if (!itemCost.CanAfford())
        return false;
    }
    return true;
  }

  private void OnButtonClicked()
  {
    Action<StructureBrain.TYPES> structureSelected = this.OnStructureSelected;
    if (structureSelected == null)
      return;
    structureSelected(this._structureType);
  }

  private void OnButtonClickDenied()
  {
    this._container.DOKill();
    this._container.anchoredPosition = this._anchoredOrigin;
    this._container.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
  }

  private void OnDisable()
  {
    this._canvasGroup.alpha = 1f;
    this.transform.localScale = Vector3.one;
    this._flashIcon.gameObject.SetActive(false);
  }

  private void OnDestroy()
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) this._blackAndWhiteMaterialInstance);
    this._blackAndWhiteMaterialInstance = (Material) null;
  }

  public void OnSelect(BaseEventData eventData)
  {
    this._selectedIcon.enabled = true;
    this._alert.TryRemoveAlert();
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.DoSelected(this.transform.localScale.x, 1.2f));
  }

  public void OnDeselect(BaseEventData eventData)
  {
    this._selectedIcon.enabled = false;
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.DoDeSelected());
  }

  private IEnumerator DoSelected(float starting, float target)
  {
    BuildMenuItem buildMenuItem = this;
    buildMenuItem._canvasGroup.alpha = 1f;
    buildMenuItem.transform.localScale = Vector3.one;
    float progress = 0.0f;
    float duration = 0.1f;
    while ((double) (progress += Time.unscaledDeltaTime) < (double) duration)
    {
      float num = Mathf.SmoothStep(starting, target, progress / duration);
      buildMenuItem.transform.localScale = Vector3.one * num;
      yield return (object) null;
    }
    float num1 = target;
    buildMenuItem.transform.localScale = Vector3.one * num1;
  }

  private IEnumerator DoDeSelected()
  {
    BuildMenuItem buildMenuItem = this;
    float progress = 0.0f;
    float duration = 0.3f;
    float startingScale = buildMenuItem.transform.localScale.x;
    float targetScale = 1f;
    while ((double) (progress += Time.unscaledDeltaTime) < (double) duration)
    {
      float num = Mathf.SmoothStep(startingScale, targetScale, progress / duration);
      buildMenuItem.transform.localScale = Vector3.one * num;
      yield return (object) null;
    }
    float num1 = targetScale;
    buildMenuItem.transform.localScale = Vector3.one * num1;
  }
}
