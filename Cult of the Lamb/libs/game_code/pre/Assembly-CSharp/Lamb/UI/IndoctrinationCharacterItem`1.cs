// Decompiled with JetBrains decompiler
// Type: Lamb.UI.IndoctrinationCharacterItem`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using Spine.Unity;
using System;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI;

public abstract class IndoctrinationCharacterItem<T> : MonoBehaviour
{
  private const string kNormalLayerID = "Normal";
  private const string kSelectedLayerID = "Selected";
  private const string kLockedLayerID = "Locked";
  public Action<T> OnItemSelected;
  [SerializeField]
  private MMButton _button;
  [SerializeField]
  private Animator _animator;
  [SerializeField]
  protected SkeletonGraphic _spine;
  [SerializeField]
  private RectTransform _container;
  protected WorshipperData.SkinAndData _skinAndData;
  private bool _locked;
  private bool _selected;
  private Vector2 _containerOrigin;

  public MMButton Button => this._button;

  public bool Locked => this._locked;

  public WorshipperData.DropLocation DropLocation => this._skinAndData.DropLocation;

  public string Skin => this._skinAndData.Skin[0].Skin;

  public void Awake()
  {
    this._containerOrigin = this._container.anchoredPosition;
    this._button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
    this._button.OnConfirmDenied += new System.Action(this.Shake);
    this.UpdateState();
  }

  public virtual void Configure(WorshipperData.SkinAndData skinAndData)
  {
    this._skinAndData = skinAndData;
    this._spine.ConfigureFollowerSkin(skinAndData);
  }

  private void OnButtonClicked() => this.OnButtonClickedImpl();

  public void Shake()
  {
    this._container.DOKill();
    this._container.anchoredPosition = this._containerOrigin;
    this._container.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
  }

  protected abstract void OnButtonClickedImpl();

  public void SetAsDefault()
  {
    this._selected = false;
    this._locked = !DataManager.GetFollowerSkinUnlocked(this._skinAndData.Skin[0].Skin) && !this._skinAndData.Invariant;
    this._button.Confirmable = !this._locked;
    if (this._locked)
      this.transform.SetAsLastSibling();
    this.UpdateState();
  }

  public void SetAsSelected()
  {
    this._selected = true;
    this._locked = false;
    this.UpdateState();
  }

  private void UpdateState()
  {
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Normal"), this._locked || this._selected ? 0.0f : 1f);
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Selected"), this._locked || !this._selected ? 0.0f : 1f);
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Locked"), !this._locked || this._selected ? 0.0f : 1f);
  }
}
