// Decompiled with JetBrains decompiler
// Type: CoopAssignInputOption
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class CoopAssignInputOption : MonoBehaviour
{
  public GameObject IconContainer;
  public int selection = -1;
  public TextMeshProUGUI title;
  public TextMeshProUGUI padNumDisplay;
  public GameObject arrowDisplayObject;
  public float distanceFromCenterX = 150f;
  [HideInInspector]
  public bool inputLock;

  public void Start()
  {
    this.IconContainer.transform.localPosition = new Vector3((float) this.selection * this.distanceFromCenterX, 0.0f, 0.0f);
  }

  public void IsSelected() => UICoopAssignController.SelectedInputOption = this;

  public void IsUnSelected()
  {
    UICoopAssignController.SelectedInputOption = (CoopAssignInputOption) null;
  }

  public void OnEnable()
  {
    MMButton component = this.GetComponent<MMButton>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.OnSelected += new System.Action(this.IsSelected);
    component.OnDeselected += new System.Action(this.IsUnSelected);
    component.onClick.AddListener(new UnityAction(this.OnMouseClick));
  }

  public void SetpadNumDisplay(int value = 1)
  {
    if (value == -1)
    {
      this.padNumDisplay.transform.parent.gameObject.SetActive(false);
    }
    else
    {
      this.padNumDisplay.transform.parent.gameObject.SetActive(true);
      this.padNumDisplay.text = value.ToString() ?? "";
    }
  }

  public void OnMouseClick() => this.SetSelection(this.selection * -1);

  public void OnDisable()
  {
    MMButton component = this.GetComponent<MMButton>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      component.OnSelected -= new System.Action(this.IsSelected);
      component.OnDeselected -= new System.Action(this.IsUnSelected);
      component.onClick.RemoveListener(new UnityAction(this.OnMouseClick));
    }
    if (!((UnityEngine.Object) UICoopAssignController.SelectedInputOption == (UnityEngine.Object) this))
      return;
    UICoopAssignController.SelectedInputOption = (CoopAssignInputOption) null;
  }

  public void SetSelection(int selection = 1, bool instant = false)
  {
    if (this.inputLock)
      return;
    this.inputLock = true;
    DOTween.Kill((object) this.IconContainer);
    this.selection = selection;
    if (instant)
    {
      this.IconContainer.transform.localPosition = new Vector3((float) this.selection * this.distanceFromCenterX, 0.0f, 0.0f);
      this.arrowDisplayObject.transform.localEulerAngles = selection != 1 ? new Vector3(0.0f, 0.0f, 0.0f) : new Vector3(0.0f, 0.0f, 180f);
    }
    else
    {
      this.IconContainer.transform.DOLocalMoveX((float) selection * this.distanceFromCenterX, 0.075f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutQuad).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      this.arrowDisplayObject.transform.DOKill();
      this.arrowDisplayObject.transform.localScale = Vector3.one * 2f;
      this.arrowDisplayObject.transform.DOScale(1f, 0.2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutQuad).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      if (selection == 1)
        this.arrowDisplayObject.transform.DORotate(new Vector3(0.0f, 0.0f, 180f), 0.25f).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(Ease.InOutQuad).SetUpdate<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(true);
      else
        this.arrowDisplayObject.transform.DORotate(new Vector3(0.0f, 0.0f, 0.0f), 0.25f).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(Ease.InOutQuad).SetUpdate<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(true);
    }
    if (instant)
      return;
    if (selection == -1)
      UIManager.PlayAudio("event:/player/speak_to_follower_noBookPage");
    else
      AudioManager.Instance.PlayOneShot("event:/player/goat_player/goat_speak_to_follower_noBookPage");
  }
}
