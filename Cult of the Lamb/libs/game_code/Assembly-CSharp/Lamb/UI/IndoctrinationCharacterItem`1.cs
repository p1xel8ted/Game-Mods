// Decompiled with JetBrains decompiler
// Type: Lamb.UI.IndoctrinationCharacterItem`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using Spine.Unity;
using System;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI;

public abstract class IndoctrinationCharacterItem<T> : MonoBehaviour
{
  public const string kNormalLayerID = "Normal";
  public const string kSelectedLayerID = "Selected";
  public const string kLockedLayerID = "Locked";
  public Action<T> OnItemSelected;
  public Action<T> OnItemHighlighted;
  [SerializeField]
  public MMButton _button;
  [SerializeField]
  public Animator _animator;
  [SerializeField]
  public SkeletonGraphic _spine;
  [SerializeField]
  public RectTransform _container;
  [SerializeField]
  public bool _sortLockedItems = true;
  public WorshipperData.SkinAndData _skinAndData;
  public bool _locked;
  public bool _selected;
  public Vector2 _containerOrigin;

  public MMButton Button => this._button;

  public bool Locked => this._locked;

  public WorshipperData.DropLocation DropLocation => this._skinAndData.DropLocation;

  public string Skin => this._skinAndData.Skin[0].Skin;

  public void Awake()
  {
    this._containerOrigin = this._container.anchoredPosition;
    this._button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
    this._button.OnSelected += new System.Action(this.OnButtonSelected);
    this._button.OnConfirmDenied += new System.Action(this.Shake);
    this.UpdateState();
  }

  public virtual void Configure(WorshipperData.SkinAndData skinAndData)
  {
    this._skinAndData = skinAndData;
    this._spine.ConfigureFollowerSkin(skinAndData);
  }

  public void OnButtonClicked() => this.OnButtonClickedImpl();

  public void OnButtonSelected() => this.OnButtonHighlightedImpl();

  public void Shake()
  {
    this._container.DOKill();
    this._container.anchoredPosition = this._containerOrigin;
    this._container.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
  }

  public abstract void OnButtonClickedImpl();

  public abstract void OnButtonHighlightedImpl();

  public void SetLocked()
  {
    this._locked = true;
    this._button.Confirmable = false;
    this.transform.SetAsLastSibling();
    this.UpdateState();
  }

  public void SetAsDefault()
  {
    this._selected = false;
    if (this._skinAndData != null)
      this._locked = !DataManager.GetFollowerSkinUnlocked(this._skinAndData.Skin[0].Skin) && !this._skinAndData.Invariant;
    this._button.Confirmable = !this._locked;
    if (this._locked && this._sortLockedItems)
      this.transform.SetAsLastSibling();
    this.UpdateState();
  }

  public virtual void SetAsSelected()
  {
    this._selected = true;
    this._locked = false;
    this.UpdateState();
  }

  public virtual void SetAsDeselected()
  {
    this._selected = false;
    this._locked = false;
    this.UpdateState();
  }

  public void UpdateState()
  {
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Normal"), this._locked || this._selected ? 0.0f : 1f);
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Selected"), this._locked || !this._selected ? 0.0f : 1f);
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Locked"), !this._locked || this._selected ? 0.0f : 1f);
  }
}
