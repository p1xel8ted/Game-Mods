// Decompiled with JetBrains decompiler
// Type: DoctrineBookmark
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI;
using UnityEngine;

#nullable disable
public class DoctrineBookmark : MMTab
{
  protected const string kLockedLayer = "Locked";
  [SerializeField]
  private SermonCategory _sermonCategory;
  [SerializeField]
  private GameObject _categoryIcon;
  [SerializeField]
  private GameObject _lockIcon;
  [SerializeField]
  private DoctrineCategoryNotification _notification;
  private float _lockedWeight;
  private bool _hasDoctrine;

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
  }

  protected override void SetActive()
  {
    if (!this._hasDoctrine)
      return;
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Inactive"), 0.0f);
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Active"), 1f);
  }

  protected override void SetInactive()
  {
    if (!this._hasDoctrine)
      return;
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Inactive"), 1f);
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Active"), 0.0f);
  }
}
