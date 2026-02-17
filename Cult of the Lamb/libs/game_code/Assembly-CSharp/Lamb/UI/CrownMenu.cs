// Decompiled with JetBrains decompiler
// Type: Lamb.UI.CrownMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public MMScrollRect _scrollRect;
  [Header("Tarot")]
  [SerializeField]
  public RectTransform _tarotCardContentContainer;
  [FormerlySerializedAs("_tarotCardItemTemplate")]
  [SerializeField]
  public TarotCardItem_Run _tarotCardItemRunTemplate;
  [SerializeField]
  public GameObject _noTarotText;
  public List<WeaponItem> _weaponItems = new List<WeaponItem>();
  public List<CurseItem> _curseItems = new List<CurseItem>();
  public List<TarotCardItem_Run> _tarotCardItems = new List<TarotCardItem_Run>();

  public override void OnShowStarted()
  {
    this._scrollRect.enabled = false;
    if (this._tarotCardItems.Count == 0 && PlayerFarming.Instance.RunTrinkets.Count > 0)
    {
      foreach (TarotCards.TarotCard runTrinket in PlayerFarming.Instance.RunTrinkets)
      {
        TarotCardItem_Run tarotCardItemRun = this._tarotCardItemRunTemplate.Instantiate<TarotCardItem_Run>((Transform) this._tarotCardContentContainer);
        tarotCardItemRun.Configure(runTrinket.CardType);
        this._tarotCardItems.Add(tarotCardItemRun);
      }
      this._noTarotText.SetActive(false);
    }
    this._scrollRect.enabled = true;
    this._scrollRect.normalizedPosition = Vector2.one;
  }
}
