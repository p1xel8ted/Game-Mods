// Decompiled with JetBrains decompiler
// Type: Lamb.UI.SermonWheelOverlay.UISermonWheelController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Lamb.UI.SermonWheelOverlay;

public class UISermonWheelController : UIRadialMenuBase<SermonWheelCategory, SermonCategory>
{
  public InventoryItem.ITEM_TYPE _currency;
  public Material _material;
  [SerializeField]
  public RectTransform container;
  [SerializeField]
  public GameObject _wheelItemsContainer;
  [SerializeField]
  public GameObject _crystalItemsContainer;
  [SerializeField]
  public GameObject _pentagram;
  [SerializeField]
  public GameObject _sixSidedStar;
  [SerializeField]
  public GameObject _sevenSidedStar;
  [SerializeField]
  public GameObject _cancelPrompt;
  [SerializeField]
  public Transform[] _sixSidedPositions;
  [SerializeField]
  public Transform[] _sevenSidedPositions;
  [SerializeField]
  public MMButton[] _pleasureOptions;
  [SerializeField]
  public MMButton[] _winterOptions;
  [SerializeField]
  public List<SermonWheelCategory> _crystalItems;
  public Vector3 pos;
  public Animator animator;
  public bool cancellable = true;

  public override bool SelectOnHighlight => false;

  public void Show(InventoryItem.ITEM_TYPE currency, bool instant = false)
  {
    this.animator = this.GetComponent<Animator>();
    this._currency = currency;
    if (this._currency == InventoryItem.ITEM_TYPE.CRYSTAL_DOCTRINE_STONE)
    {
      this._wheelItems = this._crystalItems;
      this._wheelItemsContainer.SetActive(false);
    }
    else
      this._crystalItemsContainer.SetActive(false);
    foreach (SermonWheelCategory wheelItem in this._wheelItems)
      wheelItem.Configure(this._currency);
    if (DataManager.Instance.WinterDoctrineEnabled)
      this.SetSevenSided();
    else if (DataManager.Instance.PleasureDoctrineOnboarded)
      this.SetSixSided();
    else
      this.SetFiveSided();
    foreach (MMButton pleasureOption in this._pleasureOptions)
    {
      pleasureOption.Interactable = DataManager.Instance.PleasureDoctrineOnboarded;
      pleasureOption.gameObject.SetActive(DataManager.Instance.PleasureDoctrineOnboarded);
    }
    foreach (MMButton winterOption in this._winterOptions)
    {
      winterOption.Interactable = DataManager.Instance.WinterDoctrineEnabled;
      winterOption.gameObject.SetActive(DataManager.Instance.WinterDoctrineEnabled);
    }
    this.Show(instant);
  }

  public override void OnShowStarted()
  {
    if (this._currency == InventoryItem.ITEM_TYPE.CRYSTAL_DOCTRINE_STONE)
      this._radialMaterialInstance.SetColor("_HighlightColor", StaticColors.BlueColourHex.ColourFromHex());
    base.OnShowStarted();
    if (!DataManager.Instance.PleasureDoctrineOnboarded && DataManager.Instance.HasBuiltPleasureShrine)
    {
      this.StartCoroutine((IEnumerator) this.OnboardPleasureDoctrineIE());
    }
    else
    {
      if (DataManager.Instance.WinterDoctrineEnabled || this._currency == InventoryItem.ITEM_TYPE.CRYSTAL_DOCTRINE_STONE || !DataManager.Instance.WinterDoctrineAvailable)
        return;
      this.StartCoroutine((IEnumerator) this.OnboardWinterDoctrineIE());
    }
  }

  public IEnumerator OnboardPleasureDoctrineIE()
  {
    UISermonWheelController sermonWheelController = this;
    foreach (SermonWheelCategory wheelItem in sermonWheelController._wheelItems)
    {
      if (wheelItem.SermonCategory != SermonCategory.Pleasure)
        wheelItem.Button.Interactable = false;
    }
    sermonWheelController.cancellable = false;
    sermonWheelController._cancelPrompt.gameObject.SetActive(false);
    yield return (object) new WaitForEndOfFrame();
    sermonWheelController.SetActiveStateForMenu(false);
    DataManager.Instance.PleasureDoctrineOnboarded = true;
    MonoSingleton<UINavigatorNew>.Instance.LockInput = MonoSingleton<UINavigatorNew>.Instance.LockNavigation = true;
    sermonWheelController.cursorEnabled = false;
    sermonWheelController.pos = sermonWheelController.container.localPosition;
    yield return (object) new WaitForSecondsRealtime(0.25f);
    EventInstance loopingSound = AudioManager.Instance.CreateLoop("event:/hearts_of_the_faithful/draw_power_loop", true);
    float time = 0.0f;
    while ((double) time < 2.0)
    {
      time += Time.unscaledDeltaTime;
      sermonWheelController.container.localPosition = sermonWheelController.pos + (Vector3) UnityEngine.Random.insideUnitCircle * time * 2f;
      int num = (int) loopingSound.setParameterByName("power", time / 2f);
      yield return (object) null;
    }
    UIManager.PlayAudio("event:/hearts_of_the_faithful/draw_power_end");
    AudioManager.Instance.StopLoop(loopingSound);
    sermonWheelController.SetSixSided();
    sermonWheelController.animator.enabled = false;
    sermonWheelController.container.DOPunchScale(Vector3.one * 0.15f, 0.2f).SetUpdate<Tweener>(true);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    foreach (MMButton pleasureOption in sermonWheelController._pleasureOptions)
    {
      pleasureOption.Interactable = true;
      pleasureOption.gameObject.SetActive(DataManager.Instance.PleasureDoctrineOnboarded);
      pleasureOption.transform.DOPunchScale(Vector3.one * 0.5f, 0.5f).SetUpdate<Tweener>(true);
      CanvasGroup component = pleasureOption.GetComponent<CanvasGroup>();
      component.alpha = 0.0f;
      component.DOFade(1f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    }
    foreach (SermonWheelCategory wheelItem in sermonWheelController._wheelItems)
    {
      if (wheelItem.SermonCategory != SermonCategory.Pleasure)
      {
        wheelItem.DoInactive();
        wheelItem.Button.Confirmable = wheelItem.Button.Interactable = false;
      }
    }
    yield return (object) new WaitForSecondsRealtime(1f);
    sermonWheelController.container.DOLocalMove(sermonWheelController.pos, 0.25f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    sermonWheelController.animator.enabled = true;
    yield return (object) new WaitForSecondsRealtime(0.25f);
    sermonWheelController.cursorEnabled = true;
    MonoSingleton<UINavigatorNew>.Instance.LockInput = MonoSingleton<UINavigatorNew>.Instance.LockNavigation = false;
    sermonWheelController.SetActiveStateForMenu(true);
    yield return (object) new WaitForEndOfFrame();
    foreach (SermonWheelCategory wheelItem in sermonWheelController._wheelItems)
    {
      if (wheelItem.SermonCategory != SermonCategory.Pleasure)
      {
        wheelItem.DoInactive();
        wheelItem.Button.Confirmable = wheelItem.Button.Interactable = false;
      }
    }
  }

  public IEnumerator OnboardWinterDoctrineIE()
  {
    UISermonWheelController sermonWheelController = this;
    sermonWheelController.cancellable = false;
    sermonWheelController._cancelPrompt.gameObject.SetActive(false);
    yield return (object) new WaitForEndOfFrame();
    DataManager.Instance.WinterDoctrineEnabled = true;
    MonoSingleton<UINavigatorNew>.Instance.LockInput = MonoSingleton<UINavigatorNew>.Instance.LockNavigation = true;
    sermonWheelController.cursorEnabled = false;
    sermonWheelController.SetActiveStateForMenu(false);
    sermonWheelController.pos = sermonWheelController.container.localPosition;
    yield return (object) new WaitForSecondsRealtime(0.25f);
    EventInstance loopingSound = AudioManager.Instance.CreateLoop("event:/hearts_of_the_faithful/draw_power_loop", true);
    MonoSingleton<UINavigatorNew>.Instance.LockInput = MonoSingleton<UINavigatorNew>.Instance.LockNavigation = true;
    sermonWheelController.SetActiveStateForMenu(false);
    float time = 0.0f;
    while ((double) time < 2.0)
    {
      time += Time.unscaledDeltaTime;
      sermonWheelController.container.localPosition = sermonWheelController.pos + (Vector3) UnityEngine.Random.insideUnitCircle * time * 2f;
      int num = (int) loopingSound.setParameterByName("power", time / 2f);
      yield return (object) null;
    }
    UIManager.PlayAudio("event:/hearts_of_the_faithful/draw_power_end");
    AudioManager.Instance.StopLoop(loopingSound);
    sermonWheelController.SetSevenSided();
    sermonWheelController.animator.enabled = false;
    sermonWheelController.container.DOPunchScale(Vector3.one * 0.15f, 0.2f).SetUpdate<Tweener>(true);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    foreach (MMButton winterOption in sermonWheelController._winterOptions)
    {
      winterOption.Interactable = true;
      winterOption.gameObject.SetActive(DataManager.Instance.WinterDoctrineEnabled);
      winterOption.transform.DOPunchScale(Vector3.one * 0.5f, 0.5f).SetUpdate<Tweener>(true);
      CanvasGroup component = winterOption.GetComponent<CanvasGroup>();
      component.alpha = 0.0f;
      component.DOFade(1f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    }
    yield return (object) new WaitForSecondsRealtime(1f);
    sermonWheelController.container.DOLocalMove(sermonWheelController.pos, 0.25f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    sermonWheelController.animator.enabled = true;
    yield return (object) new WaitForSecondsRealtime(0.25f);
    sermonWheelController.cursorEnabled = true;
    MonoSingleton<UINavigatorNew>.Instance.LockInput = MonoSingleton<UINavigatorNew>.Instance.LockNavigation = false;
    sermonWheelController.SetActiveStateForMenu(true);
  }

  public void SetFiveSided()
  {
    this._pentagram.gameObject.SetActive(true);
    this._sixSidedStar.gameObject.SetActive(false);
  }

  public void SetSixSided()
  {
    this._sixSidedStar.gameObject.SetActive(true);
    this._pentagram.gameObject.SetActive(false);
    for (int index = 0; index < 6; ++index)
    {
      this._wheelItems[index].transform.position = this._sixSidedPositions[index].position;
      this._crystalItems[index].transform.position = this._sixSidedPositions[index].position;
    }
  }

  public void SetSevenSided()
  {
    this._sevenSidedStar.gameObject.SetActive(true);
    this._sixSidedStar.gameObject.SetActive(false);
    this._pentagram.gameObject.SetActive(false);
    for (int index = 0; index < 7; ++index)
    {
      this._wheelItems[index].transform.position = this._sevenSidedPositions[index].position;
      this._crystalItems[index].transform.position = this._sevenSidedPositions[index].position;
    }
  }

  public override void OnChoiceFinalized()
  {
    this._finalizedSelection = true;
    this.Hide();
  }

  public override void MakeChoice(SermonWheelCategory item)
  {
    MonoSingleton<UINavigatorNew>.Instance.LockInput = MonoSingleton<UINavigatorNew>.Instance.LockNavigation = false;
    Action<SermonCategory> onItemChosen = this.OnItemChosen;
    if (onItemChosen == null)
      return;
    onItemChosen(item.SermonCategory);
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable || !this.cancellable)
      return;
    this._finalizedSelection = true;
    this.Hide();
  }
}
