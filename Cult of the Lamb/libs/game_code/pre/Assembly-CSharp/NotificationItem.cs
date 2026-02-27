// Decompiled with JetBrains decompiler
// Type: NotificationItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class NotificationItem : NotificationBase
{
  private InventoryItem.ITEM_TYPE _itemType;
  private int _delta;
  private float _timer;
  [SerializeField]
  private GameObject backgroundGO;
  private Image background;
  [SerializeField]
  private Sprite negativeSprite;
  [SerializeField]
  private Sprite positiveSprite;

  protected override float _onScreenDuration => 3f;

  protected override float _showHideDuration => 0.4f;

  public InventoryItem.ITEM_TYPE ItemType => this._itemType;

  public Image backgroundImage()
  {
    if ((UnityEngine.Object) this.background == (UnityEngine.Object) null)
    {
      try
      {
        this.background = this.backgroundGO.GetComponent<Image>();
      }
      catch
      {
        return (Image) null;
      }
    }
    return this.background;
  }

  public void Configure(InventoryItem.ITEM_TYPE itemType, int delta, NotificationBase.Flair flair = NotificationBase.Flair.None)
  {
    this._itemType = itemType;
    this._delta = delta;
    this.Configure(flair);
    this._contentRectTransform.anchoredPosition = this._offScreenPosition;
    this.Localize();
    this.Show(andThen: (System.Action) (() => this.background = this.backgroundImage()));
    if (!((UnityEngine.Object) this.backgroundImage() != (UnityEngine.Object) null))
      return;
    this.backgroundImage().sprite = delta >= 0 ? this.positiveSprite : this.negativeSprite;
  }

  public void UpdateDelta(int delta)
  {
    if (this.Shown)
    {
      this._contentRectTransform.DOKill();
      this._contentRectTransform.anchoredPosition = this._onScreenPosition;
      this._contentRectTransform.DOPunchPosition(new Vector3(25f, 0.0f), 1f, 5).SetEase<Tweener>(Ease.InExpo);
    }
    else
      this.Show(andThen: (System.Action) (() => this.background = this.backgroundImage()));
    if ((UnityEngine.Object) this.backgroundImage() != (UnityEngine.Object) null)
      this.backgroundImage().sprite = delta >= 0 ? this.positiveSprite : this.negativeSprite;
    this._timer = this._onScreenDuration;
    this._delta += delta;
    if (this._delta == 0)
      this.Hide(true);
    this.Localize();
  }

  protected override IEnumerator HoldOnScreen()
  {
    NotificationItem notificationItem = this;
    notificationItem._timer = notificationItem._onScreenDuration;
    while ((double) notificationItem._timer > 0.0)
    {
      if ((UnityEngine.Object) HUD_Manager.Instance != (UnityEngine.Object) null && !HUD_Manager.Instance.Hidden && !LetterBox.IsPlaying)
        notificationItem._timer -= Time.deltaTime;
      yield return (object) null;
    }
  }

  protected override void Localize()
  {
    this._description.text = $"<size=30>{FontImageNames.GetIconByType(this._itemType)} {(this._delta > 0 ? (object) "+" : (object) "")}{(object) this._delta}</size> {InventoryItem.LocalizedName(this._itemType)}  <size=25><color=#6E6666>{(object) Inventory.GetItemQuantity((int) this._itemType)}</color></size>";
  }
}
