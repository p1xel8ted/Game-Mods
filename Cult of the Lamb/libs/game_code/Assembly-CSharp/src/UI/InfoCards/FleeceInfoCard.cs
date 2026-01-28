// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.FleeceInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using MMTools;
using src.UINavigator;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

#nullable disable
namespace src.UI.InfoCards;

public class FleeceInfoCard : UIInfoCardBase<int>
{
  [SerializeField]
  public TextMeshProUGUI _itemHeader;
  [SerializeField]
  public TextMeshProUGUI _itemDescription;
  [SerializeField]
  public TextMeshProUGUI _itemDLCDescription;
  [SerializeField]
  public TextMeshProUGUI _itemCustomisedFleece;
  [SerializeField]
  public FleeceItem _fleeceItem;
  [SerializeField]
  public FleeceItem _originalFleeceItem;
  [SerializeField]
  public GameObject _originalFleeceContainer;
  [SerializeField]
  public GameObject _costHeader;
  [SerializeField]
  public GameObject _costContainer;
  [SerializeField]
  public TextMeshProUGUI _costText;
  [SerializeField]
  public GameObject _arrowsContainer;
  [SerializeField]
  public MMButton _nextButton;
  [SerializeField]
  public MMButton _prevButton;
  [SerializeField]
  public MMControlPrompt _nextButtonPrompt;
  [SerializeField]
  public MMControlPrompt _prevButtonPrompt;
  public GameObject _redOutline;
  [SerializeField]
  public UIPlayerUpgradesMenuController _controller;
  [SerializeField]
  public bool _pauseDetailsMenu;
  public List<int> _fleeceItems = new List<int>();
  public int fleeceIndex;
  public int fleeceCache;
  public int originalFleeceCache;
  public CanvasGroup _canvasGroupArrows;
  public float selectionDelay;
  public Vector3 startPos;

  public void ShowPrompts()
  {
    if ((Object) this._canvasGroupArrows == (Object) null)
      this._canvasGroupArrows = this._arrowsContainer.GetComponent<CanvasGroup>();
    this._arrowsContainer.gameObject.SetActive(true);
    this._canvasGroupArrows.alpha = 0.0f;
    this._canvasGroupArrows.DOKill();
    this._canvasGroupArrows.DOFade(1f, 0.33f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
  }

  public void HidePrompts()
  {
    if ((Object) this._canvasGroupArrows == (Object) null)
      this._canvasGroupArrows = this._arrowsContainer.GetComponent<CanvasGroup>();
    this._canvasGroupArrows.DOKill();
    this._canvasGroupArrows.DOFade(0.0f, 0.33f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
    {
      this._canvasGroupArrows.alpha = 0.0f;
      this._arrowsContainer.gameObject.SetActive(false);
    }));
  }

  public void UpdateCustomOptions()
  {
    bool flag = false;
    List<Vector2> vector2List = new List<Vector2>();
    PlayerFleeceManager.FleeceType x = PlayerFleeceManager.FleeceType.Default;
    PlayerFleeceManager.FleeceType y = PlayerFleeceManager.FleeceType.Default;
    for (int index = 0; index < this._fleeceItems.Count; ++index)
    {
      if (this._fleeceItems[index] == this.originalFleeceCache)
        x = (PlayerFleeceManager.FleeceType) this._fleeceItems[index];
      if (this._fleeceItems[index] == this.fleeceCache)
        y = (PlayerFleeceManager.FleeceType) this._fleeceItems[index];
    }
    foreach (Vector2 customisedFleeceOption in DataManager.Instance.CustomisedFleeceOptions)
    {
      if ((double) customisedFleeceOption.x == (double) this.originalFleeceCache)
      {
        vector2List.Add(new Vector2((float) x, (float) y));
        flag = true;
        Debug.Log((object) $"Found it, Original Fleece Cache: {this.originalFleeceCache.ToString()} Fleece Cache: {this.fleeceCache.ToString()}");
      }
      else
        vector2List.Add(customisedFleeceOption);
    }
    if (!flag)
    {
      vector2List.Add(new Vector2((float) x, (float) y));
      Debug.Log((object) $"Didn't find it, Created Original Fleece Cache: {this.originalFleeceCache.ToString()} Fleece Cache: {this.fleeceCache.ToString()}");
    }
    DataManager.Instance.CustomisedFleeceOptions = vector2List;
    this._originalFleeceContainer.gameObject.SetActive(this.fleeceCache != this.originalFleeceCache);
    int index1 = -1;
    for (int index2 = 0; index2 < this._fleeceItems.Count; ++index2)
    {
      if (this._fleeceItems[index2] == this.originalFleeceCache)
      {
        index1 = index2;
        break;
      }
    }
    this._controller.GetFleeceItem(index1).Configure(this.originalFleeceCache);
    Debug.Log((object) "Show Fleece Icon secondary Fleece");
  }

  public void NextFleece()
  {
    if ((double) this.CanvasGroup.alpha == 0.0)
      return;
    AudioManager.Instance.PlayOneShot("event:/ui/change_selection");
    this._nextButton.gameObject.transform.DOKill();
    this._nextButton.transform.localScale = Vector3.one * 0.3374532f;
    this._nextButton.gameObject.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f);
    this.selectionDelay = 0.2f;
    int index = this.CheckNextFleece();
    Debug.Log((object) ("Next Fleece: " + index.ToString()));
    if (index >= 0)
    {
      this.fleeceCache = this._controller.GetFleeceItem(index).Fleece;
      this._fleeceItem.Configure(this._fleeceItems[index]);
      this.UpdateCustomOptions();
      this.UpdateButtonPrompts();
      this.UpdatePlayer();
    }
    else
      Debug.Log((object) "Fleece not found -1");
  }

  public void PrevFleece()
  {
    if ((double) this.CanvasGroup.alpha == 0.0)
      return;
    AudioManager.Instance.PlayOneShot("event:/ui/change_selection");
    this._prevButton.gameObject.transform.DOKill();
    this._prevButton.transform.localScale = Vector3.one * 0.3374532f;
    this._prevButton.gameObject.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f);
    this.selectionDelay = 0.2f;
    int index = this.CheckPrevFleece();
    Debug.Log((object) ("Prev Fleece: " + index.ToString()));
    if (index != -1)
    {
      this.fleeceCache = this._controller.GetFleeceItem(index).Fleece;
      this._fleeceItem.Configure(this._fleeceItems[index]);
      this.UpdateCustomOptions();
      this.UpdateButtonPrompts();
      this.UpdatePlayer();
    }
    else
      Debug.Log((object) "Fleece not found -1");
  }

  public void UpdatePlayer()
  {
    if (DataManager.Instance.PlayerFleece != this.originalFleeceCache)
      return;
    DataManager.Instance.PlayerVisualFleece = this._fleeceItems[this.fleeceIndex];
    PlayerFarming.Instance.IsGoat = DataManager.Instance.PlayerVisualFleece == 1003;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if (player.isLamb)
      {
        if (DataManager.Instance.PlayerVisualFleece == 1003)
          player.SetSkin();
        else
          player.simpleSpineAnimator?.SetSkin("Lamb_" + DataManager.Instance.PlayerVisualFleece.ToString());
      }
    }
  }

  public void PunchPositionFleece()
  {
    this._fleeceItem.DOKill();
    this._fleeceItem.transform.localPosition = this.startPos;
    this._fleeceItem.transform.DOPunchPosition(new Vector3(0.0f, 5f, 0.0f), 0.5f);
  }

  public void UpdateButtonPrompts()
  {
    if ((Object) this._controller == (Object) null || !this.CheckIfCustomisable(this.originalFleeceCache, true))
      return;
    if (this.fleeceIndex == 0)
    {
      this._prevButton.gameObject.SetActive(false);
      this._prevButtonPrompt.gameObject.SetActive(false);
    }
    else
    {
      this._prevButton.gameObject.SetActive(true);
      this._prevButtonPrompt.gameObject.SetActive(true);
    }
    if (this.fleeceIndex == this.GetMaxFleeceIndex())
    {
      this._nextButton.gameObject.SetActive(false);
      this._nextButtonPrompt.gameObject.SetActive(false);
    }
    else
    {
      this._nextButton.gameObject.SetActive(true);
      this._nextButtonPrompt.gameObject.SetActive(true);
    }
    if (this.fleeceCache == this.originalFleeceCache)
    {
      this._originalFleeceContainer.gameObject.SetActive(false);
      this._itemCustomisedFleece.gameObject.SetActive(false);
    }
    else
    {
      this._originalFleeceContainer.gameObject.SetActive(true);
      this._itemCustomisedFleece.gameObject.SetActive(true);
    }
  }

  public int CheckNextFleece()
  {
    int index1 = Mathf.Clamp(this.fleeceIndex + 1, 0, this._fleeceItems.Count - 1);
    while (this._fleeceItems[index1] == 0 || !DataManager.Instance.UnlockedFleeces.Contains(this._fleeceItems[index1]) || this._fleeceItems[index1] == 1003 && CoopManager.CoopActive)
    {
      ++index1;
      if (index1 >= this._fleeceItems.Count - 1)
        break;
    }
    int index2 = Mathf.Clamp(index1, 0, this._fleeceItems.Count - 1);
    this.PunchPositionFleece();
    Debug.Log((object) $"Current Fleece: {this.fleeceCache.ToString()} Next Fleece: {index1.ToString()} Unlocked Fleeces: {DataManager.Instance.UnlockedFleeces.Count.ToString()}");
    if (this._fleeceItems[index2] == 0 && index2 != 0)
      return this.fleeceIndex;
    this.fleeceIndex = index2;
    return index2;
  }

  public int CheckPrevFleece()
  {
    int index1 = this.fleeceIndex - 1;
    if (index1 <= 0)
    {
      this.fleeceIndex = 0;
      return 0;
    }
    while (this._fleeceItems[index1] == 0 || !DataManager.Instance.UnlockedFleeces.Contains(this._fleeceItems[index1]) || this._fleeceItems[index1] == 1003 && CoopManager.CoopActive)
    {
      --index1;
      if (index1 <= 0)
        break;
    }
    int index2 = Mathf.Clamp(index1, 0, this._fleeceItems.Count - 1);
    this.PunchPositionFleece();
    Debug.Log((object) $"Current Fleece: {this.fleeceCache.ToString()} Next Fleece: {index1.ToString()} Unlocked Fleeces: {DataManager.Instance.UnlockedFleeces.Count.ToString()}");
    if (this._fleeceItems[index2] == 0 && index2 != 0)
      return this.fleeceIndex;
    this.fleeceIndex = index2;
    return index2;
  }

  public void TurnOff()
  {
    this._prevButton.gameObject.SetActive(false);
    this._prevButtonPrompt.gameObject.SetActive(false);
    this._nextButton.gameObject.SetActive(false);
    this._nextButtonPrompt.gameObject.SetActive(false);
    this._originalFleeceContainer.gameObject.SetActive(false);
  }

  public bool CheckIfCustomisable(int fleece, bool turnoff = false)
  {
    if (this._pauseDetailsMenu)
      return true;
    FleeceItemBuyable fleeceItem = this._controller.GetFleeceItem(this._controller.GetFleeceIndex(fleece));
    if ((Object) fleeceItem == (Object) null)
    {
      this.TurnOff();
      this._controller._customiseFleeceButton.SetActive(false);
      return false;
    }
    if (PlayerFleeceManager.NOT_CUSTOMISABLE_FLEECES.Contains((PlayerFleeceManager.FleeceType) fleece) || (Object) this._controller == (Object) null || fleece == 1003 && CoopManager.CoopActive)
    {
      if (turnoff)
      {
        this.TurnOff();
        this._controller._customiseFleeceButton.SetActive(false);
      }
      return false;
    }
    if (fleeceItem.postGameFleece && !DataManager.Instance.PostGameFleecesOnboarded || fleece == 676 && !DataManager.Instance.CowboyFleeceOnboarded || fleece == 1003 && !DataManager.Instance.GoatFleeceOnboarded)
    {
      if (turnoff)
      {
        this.TurnOff();
        this._controller._customiseFleeceButton.SetActive(false);
      }
      return false;
    }
    if (fleeceItem.ForcedFleeceIndex == -1 || DataManager.Instance.UnlockedFleeces.Contains(fleeceItem.ForcedFleeceIndex))
      return true;
    if (turnoff)
    {
      this.TurnOff();
      this._controller._customiseFleeceButton.SetActive(false);
    }
    return false;
  }

  public void Update()
  {
    if ((double) this.CanvasGroup.alpha == 0.0 || !this._arrowsContainer.gameObject.activeSelf || !this.CheckIfCustomisable(this.originalFleeceCache, true) || !DataManager.Instance.UnlockedFleeces.Contains(this.originalFleeceCache))
      return;
    this.selectionDelay -= Time.deltaTime;
    if ((double) InputManager.UI.GetHorizontalAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) < -0.3 && (double) this.selectionDelay < 0.0)
      this.PrevFleece();
    if ((double) InputManager.UI.GetHorizontalAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) <= 0.3 || (double) this.selectionDelay >= 0.0 || this.fleeceIndex >= this.GetMaxFleeceIndex())
      return;
    this.NextFleece();
  }

  public void OnEnable() => this._arrowsContainer.gameObject.SetActive(false);

  public void Start()
  {
    this.startPos = this._fleeceItem.transform.localPosition;
    if (!((Object) this._controller != (Object) null))
      return;
    this._fleeceItems = this._controller.GetFleeces();
    foreach (int fleeceItem in this._fleeceItems)
      Debug.Log((object) ("Fleece Items: " + fleeceItem.ToString()));
  }

  public override void Configure(int fleece)
  {
    this.fleeceCache = fleece;
    this.originalFleeceCache = fleece;
    this._redOutline.SetActive(false);
    this._fleeceItem.Configure(this.fleeceCache);
    if ((Object) this._controller != (Object) null)
      this._controller._customiseFleeceButton.SetActive(false);
    this.HidePrompts();
    this._itemHeader.text = LocalizationManager.GetTranslation($"TarotCards/Fleece{fleece}/Name");
    this._itemDescription.text = LocalizationManager.GetTranslation($"TarotCards/Fleece{fleece}/Description");
    if (PlayerFleeceManager.SinfulPackFleeces.Contains((PlayerFleeceManager.FleeceType) fleece))
    {
      this._itemDLCDescription.gameObject.SetActive(true);
      this._itemDLCDescription.text = LocalizationManager.GetTranslation("UI/DLC/SinfulEdition");
    }
    else if (PlayerFleeceManager.HereticPackFleeces.Contains((PlayerFleeceManager.FleeceType) fleece))
    {
      this._itemDLCDescription.gameObject.SetActive(true);
      this._itemDLCDescription.text = LocalizationManager.GetTranslation("UI/DLC/HereticEdition");
    }
    else if (PlayerFleeceManager.PilgrimPackFleeces.Contains((PlayerFleeceManager.FleeceType) fleece))
    {
      this._itemDLCDescription.gameObject.SetActive(true);
      this._itemDLCDescription.text = LocalizationManager.GetTranslation("UI/DLC/PilgrimPack");
    }
    else if (PlayerFleeceManager.WoolhavenPackFleeces.Contains((PlayerFleeceManager.FleeceType) fleece))
    {
      this._itemDLCDescription.gameObject.SetActive(true);
      this._itemDLCDescription.text = LocalizationManager.GetTranslation("UI/DLC/Name");
    }
    else
      this._itemDLCDescription.gameObject.SetActive(false);
    if (this.CheckIfCustomisable(fleece) && DataManager.Instance.UnlockedFleeces.Count > 1 && (Object) this._controller != (Object) null)
      this._controller._customiseFleeceButton.SetActive(true);
    if (!DataManager.Instance.UnlockedFleeces.Contains(fleece))
    {
      Debug.Log((object) "Fleece locked");
      this._costText.text = this._controller.GetFleeceItem(this._controller.GetFleeceIndex(fleece)).Cost.ToStringShowQuantity();
      this._costHeader.SetActive(true);
      this._costContainer.SetActive(true);
      this._originalFleeceContainer.gameObject.SetActive(false);
      this._nextButton.gameObject.SetActive(false);
      this._prevButton.gameObject.SetActive(false);
      this._nextButtonPrompt.gameObject.SetActive(false);
      this._prevButtonPrompt.gameObject.SetActive(false);
      this._controller._customiseFleeceButton.SetActive(false);
    }
    else
    {
      Debug.Log((object) "Fleece unlocked");
      this._originalFleeceContainer.gameObject.SetActive(true);
      this.UpdateButtonPrompts();
      foreach (Vector2 customisedFleeceOption in DataManager.Instance.CustomisedFleeceOptions)
      {
        if ((double) customisedFleeceOption.x == (double) this.originalFleeceCache)
        {
          this.fleeceCache = (int) customisedFleeceOption.y;
          Debug.Log((object) $"Update FLeece Cache & original fleece: {this.fleeceCache.ToString()} {this.originalFleeceCache.ToString()}");
          break;
        }
      }
      if (this._pauseDetailsMenu)
        this._fleeceItem.Configure(this.fleeceCache);
      if (this.fleeceCache == this.originalFleeceCache)
      {
        this._originalFleeceContainer.gameObject.SetActive(false);
        this._itemCustomisedFleece.gameObject.SetActive(false);
      }
      else
      {
        this._originalFleeceContainer.gameObject.SetActive(true);
        this._itemCustomisedFleece.gameObject.SetActive(true);
      }
      this._originalFleeceItem.Configure(this.originalFleeceCache);
      this._costHeader.SetActive(false);
      this._costContainer.SetActive(false);
    }
    int index1 = 0;
    for (int index2 = 0; index2 < this._fleeceItems.Count; ++index2)
    {
      if (this._fleeceItems[index2] == this.fleeceCache)
      {
        index1 = index2;
        break;
      }
    }
    this.fleeceIndex = index1;
    if (this._fleeceItems.Count > 0 && DataManager.Instance.UnlockedFleeces.Contains(this.originalFleeceCache))
    {
      this._fleeceItem.Configure(this._fleeceItems[index1]);
      this.UpdateCustomOptions();
      this.UpdateButtonPrompts();
    }
    else
    {
      this._prevButton.gameObject.SetActive(false);
      this._prevButtonPrompt.gameObject.SetActive(false);
      this._nextButton.gameObject.SetActive(false);
      this._nextButtonPrompt.gameObject.SetActive(false);
    }
  }

  public int GetMaxFleeceIndex()
  {
    for (int index = this._fleeceItems.Count - 1; index >= 0; --index)
    {
      if (this._fleeceItems[index] != 0 && DataManager.Instance.UnlockedFleeces.Contains(this._fleeceItems[index]))
        return index;
    }
    return 0;
  }

  [CompilerGenerated]
  public void \u003CHidePrompts\u003Eb__24_0()
  {
    this._canvasGroupArrows.alpha = 0.0f;
    this._arrowsContainer.gameObject.SetActive(false);
  }
}
