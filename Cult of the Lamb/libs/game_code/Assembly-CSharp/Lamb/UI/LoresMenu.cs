// Decompiled with JetBrains decompiler
// Type: Lamb.UI.LoresMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.PauseDetails;
using src.Extensions;
using src.UI.InfoCards;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class LoresMenu : UISubmenuBase
{
  [Header("Templates")]
  [SerializeField]
  public LoreItem _loreItemTemplate;
  [Header("Lore Menu")]
  [SerializeField]
  public MMScrollRect _scrollRect;
  [SerializeField]
  public RectTransform _unlockedLoreContent;
  [SerializeField]
  public GameObject _unlockedLoreHeader;
  [SerializeField]
  public RectTransform _lockedLoreContent;
  [SerializeField]
  public GameObject _lockedLoreHeader;
  [SerializeField]
  public LoreInfoCardController loreInfoCardController;
  public List<LoreItem> _unlockedLoreItems = new List<LoreItem>();
  public List<LoreItem> _lockedLoreItems = new List<LoreItem>();

  public override void OnShowStarted()
  {
    this._scrollRect.enabled = false;
    this._scrollRect.normalizedPosition = (Vector2) Vector3.one;
    this._unlockedLoreItems.Clear();
    this._lockedLoreItems.Clear();
    this._unlockedLoreContent.transform.DestroyAllChildren();
    this._lockedLoreContent.transform.DestroyAllChildren();
    foreach (int unlockedLore in LoreSystem.GetUnlockedLoreList())
    {
      LoreItem loreItem = this._loreItemTemplate.Instantiate<LoreItem>((Transform) this._unlockedLoreContent);
      loreItem.Configure(unlockedLore);
      this._unlockedLoreItems.Add(loreItem);
    }
    foreach (int lockedLore in LoreSystem.GetLockedLoreList())
    {
      LoreItem loreItem = this._loreItemTemplate.Instantiate<LoreItem>((Transform) this._lockedLoreContent);
      loreItem.Configure(lockedLore);
      this._lockedLoreItems.Add(loreItem);
    }
    this._lockedLoreHeader.SetActive(this._lockedLoreItems.Count > 0);
    this._lockedLoreContent.gameObject.SetActive(this._lockedLoreItems.Count > 0);
    if (this._unlockedLoreItems.Count > 0)
    {
      this.OverrideDefault((Selectable) this._unlockedLoreItems[0].GetComponent<MMButton>());
      this.loreInfoCardController.ShowCardWithParam(this._unlockedLoreItems[0].LoreId);
    }
    else if (this._lockedLoreItems.Count > 0)
      this.OverrideDefault((Selectable) this._lockedLoreItems[0].GetComponent<MMButton>());
    this.ActivateNavigation();
    this._scrollRect.enabled = true;
    this._scrollRect.normalizedPosition = Vector2.one;
  }
}
