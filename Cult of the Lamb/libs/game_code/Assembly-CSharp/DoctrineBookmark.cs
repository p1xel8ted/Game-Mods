// Decompiled with JetBrains decompiler
// Type: DoctrineBookmark
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using UnityEngine;

#nullable disable
public class DoctrineBookmark : MMTab
{
  public const string kLockedLayer = "Locked";
  [SerializeField]
  public SermonCategory _sermonCategory;
  [SerializeField]
  public GameObject _categoryIcon;
  [SerializeField]
  public GameObject _lockIcon;
  [SerializeField]
  public DoctrineCategoryNotification _notification;
  public float _lockedWeight;
  public bool _hasDoctrine;

  public override void Configure()
  {
    this._hasDoctrine = true;
    this._lockIcon.gameObject.SetActive(!this._hasDoctrine);
    if ((Object) this._categoryIcon != (Object) null)
      this._categoryIcon.gameObject.SetActive(this._hasDoctrine);
    if (this._sermonCategory != SermonCategory.None)
      this._notification.Configure(this._sermonCategory);
    if (!this._hasDoctrine)
    {
      this._animator.SetLayerWeight(this._animator.GetLayerIndex("Locked"), 1f);
      this._animator.SetLayerWeight(this._animator.GetLayerIndex("Active"), 0.0f);
      this._animator.SetLayerWeight(this._animator.GetLayerIndex("Inactive"), 0.0f);
    }
    this._button.interactable = this._hasDoctrine;
    this._button.enabled = this._hasDoctrine;
    if (this._sermonCategory == SermonCategory.Pleasure && !DataManager.Instance.PleasureEnabled)
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      if (this._sermonCategory != SermonCategory.Winter || DataManager.Instance.WinterDoctrineEnabled)
        return;
      this.gameObject.SetActive(false);
    }
  }

  public override void SetActive()
  {
    if (!this._hasDoctrine)
      return;
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Inactive"), 0.0f);
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Active"), 1f);
  }

  public override void SetInactive()
  {
    if (!this._hasDoctrine)
      return;
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Inactive"), 1f);
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Active"), 0.0f);
  }
}
