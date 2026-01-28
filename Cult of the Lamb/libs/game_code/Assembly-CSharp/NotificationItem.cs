// Decompiled with JetBrains decompiler
// Type: NotificationItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class NotificationItem : NotificationBase
{
  public const float DELAY_BETWEEN_UPDATES = 0.2f;
  public InventoryItem.ITEM_TYPE _itemType;
  public int _delta;
  public float _timer;
  public float _updateDelay;
  public bool _pendingUpdate;
  [SerializeField]
  public GameObject backgroundGO;
  public Image background;
  [SerializeField]
  public Sprite negativeSprite;
  [SerializeField]
  public Sprite positiveSprite;
  public string localizedName;

  public override float _onScreenDuration => 3f;

  public override float _showHideDuration => 0.4f;

  public InventoryItem.ITEM_TYPE ItemType => this._itemType;

  public Image backgroundImage()
  {
    if ((UnityEngine.Object) this.background == (UnityEngine.Object) null)
    {
      try
      {
        if ((UnityEngine.Object) this.backgroundGO != (UnityEngine.Object) null)
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

  public void Update()
  {
    this._updateDelay += Time.deltaTime;
    if (!this._pendingUpdate || (double) this._updateDelay < 0.40000000596046448)
      return;
    this._updateDelay = 0.0f;
    this._pendingUpdate = false;
    this.DoPunchTween();
    this.UpdateLocalizedText();
  }

  public void DoPunchTween()
  {
    this._contentRectTransform.DOKill();
    this._contentRectTransform.anchoredPosition = this._onScreenPosition;
    this._contentRectTransform.DOPunchPosition(new Vector3(25f, 0.0f), 1f, 5).SetEase<Tweener>(Ease.InExpo);
  }

  public void UpdateDelta(int delta)
  {
    if (!this.Shown)
      this.Show(andThen: (System.Action) (() => this.background = this.backgroundImage()));
    if ((UnityEngine.Object) this.backgroundImage() != (UnityEngine.Object) null)
      this.backgroundImage().sprite = delta >= 0 ? this.positiveSprite : this.negativeSprite;
    this._timer = (double) this._overrideScreenDuration != -1.0 ? this._overrideScreenDuration : this._onScreenDuration;
    this._delta += delta;
    if (this._delta == 0)
      this.Hide(true);
    if ((double) this._updateDelay >= 0.20000000298023224)
    {
      if (this.Shown)
        this.DoPunchTween();
      this._updateDelay = 0.0f;
      this._pendingUpdate = false;
      this.UpdateLocalizedText();
    }
    else
      this._pendingUpdate = true;
  }

  public override IEnumerator HoldOnScreen()
  {
    NotificationItem notificationItem = this;
    notificationItem._timer = (double) notificationItem._overrideScreenDuration != -1.0 ? notificationItem._overrideScreenDuration : notificationItem._onScreenDuration;
    while ((double) notificationItem._timer > 0.0)
    {
      if ((UnityEngine.Object) HUD_Manager.Instance != (UnityEngine.Object) null && !HUD_Manager.Instance.Hidden && !LetterBox.IsPlaying)
        notificationItem._timer -= Time.deltaTime;
      yield return (object) null;
    }
  }

  public void UpdateLocalizedText()
  {
    if (LocalizeIntegration.IsArabic())
    {
      string str1 = LocalizeIntegration.ReverseText(this._delta.ToString());
      string str2 = LocalizeIntegration.ReverseText(Inventory.GetItemQuantity((int) this._itemType).ToString());
      this._description.isRightToLeftText = true;
      this._description.text = $"<size=30>{FontImageNames.GetIconByType(this._itemType)} {str1}{(this._delta > 0 ? "+" : "")}</size> {this.localizedName}  <size=25><color=#6E6666>{str2}</color></size>";
    }
    else
    {
      this._description.isRightToLeftText = false;
      this._description.text = $"<size=30>{FontImageNames.GetIconByType(this._itemType)} {(this._delta > 0 ? "+" : "")}{this._delta.ToString()}</size> {this.localizedName}  <size=25><color=#6E6666>{Inventory.GetItemQuantity((int) this._itemType).ToString()}</color></size>";
    }
  }

  public override void Localize()
  {
    this.localizedName = InventoryItem.LocalizedName(this._itemType);
    if (LocalizeIntegration.IsArabic())
    {
      string str1 = LocalizeIntegration.ReverseText(this._delta.ToString());
      string str2 = LocalizeIntegration.ReverseText(Inventory.GetItemQuantity((int) this._itemType).ToString());
      this._description.text = $"<size=30>{FontImageNames.GetIconByType(this._itemType)} {str1}{(this._delta > 0 ? "+" : "")}</size> {this.localizedName}  <size=25><color=#6E6666>{str2}</color></size>";
    }
    else
      this._description.text = $"<size=30>{FontImageNames.GetIconByType(this._itemType)} {(this._delta > 0 ? "+" : "")}{this._delta.ToString()}</size> {this.localizedName}  <size=25><color=#6E6666>{Inventory.GetItemQuantity((int) this._itemType).ToString()}</color></size>";
  }

  [CompilerGenerated]
  public void \u003CConfigure\u003Eb__18_0() => this.background = this.backgroundImage();

  [CompilerGenerated]
  public void \u003CUpdateDelta\u003Eb__21_0() => this.background = this.backgroundImage();
}
