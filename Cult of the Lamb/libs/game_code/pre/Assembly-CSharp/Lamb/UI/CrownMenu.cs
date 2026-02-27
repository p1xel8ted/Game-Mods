// Decompiled with JetBrains decompiler
// Type: Lamb.UI.CrownMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI.PauseDetails;
using src.Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

#nullable disable
namespace Lamb.UI;

public class CrownMenu : UISubmenuBase
{
  [Header("Crown Menu")]
  [SerializeField]
  private MMScrollRect _scrollRect;
  [Header("Tarot")]
  [SerializeField]
  private RectTransform _tarotCardContentContainer;
  [FormerlySerializedAs("_tarotCardItemTemplate")]
  [SerializeField]
  private TarotCardItem_Run _tarotCardItemRunTemplate;
  [SerializeField]
  private GameObject _noTarotText;
  private List<WeaponItem> _weaponItems = new List<WeaponItem>();
  private List<CurseItem> _curseItems = new List<CurseItem>();
  private List<TarotCardItem_Run> _tarotCardItems = new List<TarotCardItem_Run>();

  protected override void OnShowStarted()
  {
    this._scrollRect.enabled = false;
    if (this._tarotCardItems.Count == 0 && DataManager.Instance.PlayerRunTrinkets.Count > 0)
    {
      foreach (TarotCards.TarotCard playerRunTrinket in DataManager.Instance.PlayerRunTrinkets)
      {
        TarotCardItem_Run tarotCardItemRun = this._tarotCardItemRunTemplate.Instantiate<TarotCardItem_Run>((Transform) this._tarotCardContentContainer);
        tarotCardItemRun.Configure(playerRunTrinket.CardType);
        this._tarotCardItems.Add(tarotCardItemRun);
      }
      this._noTarotText.SetActive(false);
    }
    this._scrollRect.enabled = true;
    this._scrollRect.normalizedPosition = Vector2.one;
  }
}
