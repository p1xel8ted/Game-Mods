// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeGamePieceBagMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using src.Extensions;
using src.UI.Items;
using src.UINavigator;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Flockade;

public class FlockadeGamePieceBagMenu : UIMenuBase
{
  public const string _OPEN_BAG_SOUND = "event:/dlc/ui/flockade_minigame/bag_open";
  public const string _CLOSE_BAG_SOUND = "event:/dlc/ui/flockade_minigame/bag_close";
  public const string _COUNT_PARAMETER_NAME = "COUNT";
  public const string _TOTAL_PARAMETER_NAME = "TOTAL";
  [SerializeField]
  public RectTransform _contentContainer;
  [SerializeField]
  public UIMenuControlPrompts _controlPrompts;
  [SerializeField]
  public LocalizationParamsManager _remainingParameters;
  public List<FlockadePieceItem> _items = new List<FlockadePieceItem>();

  public void Show(FlockadeGamePieceBag bag, FlockadePlayerBase player)
  {
    this._controlPrompts.HideAcceptButton();
    List<FlockadePieceType> flockadePieceTypeList = new List<FlockadePieceType>();
    foreach (FlockadeGamePieceConfiguration data in (IEnumerable<FlockadeGamePieceConfiguration>) bag.Content.OrderBy<FlockadeGamePieceConfiguration, FlockadePieceType>((Func<FlockadeGamePieceConfiguration, FlockadePieceType>) (gamePiece => gamePiece.Type)))
    {
      if (!flockadePieceTypeList.Contains(data.Type))
      {
        FlockadePieceItem flockadePieceItem = MonoSingleton<UIManager>.Instance.FlockadePieceItemTemplate.Instantiate<FlockadePieceItem>((Transform) this._contentContainer);
        flockadePieceItem.Configure(data, new Color?(player.Highlight));
        this._items.Add(flockadePieceItem);
        flockadePieceTypeList.Add(flockadePieceItem.Data.Type);
      }
    }
    this._remainingParameters.SetParameterValue("COUNT", this._items.Count.ToString(), false);
    this._remainingParameters.SetParameterValue("TOTAL", 36.ToString());
    this.Show();
  }

  public override void OnShowStarted()
  {
    UIManager.PlayAudio("event:/ui/open_menu");
    UIManager.PlayAudio("event:/dlc/ui/flockade_minigame/bag_open");
    this.OverrideDefault((Selectable) this._items[0].Selectable);
    this.ActivateNavigation();
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  public override void OnHideStarted()
  {
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    UIManager.PlayAudio("event:/ui/close_menu");
    UIManager.PlayAudio("event:/dlc/ui/flockade_minigame/bag_close");
  }

  public override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
}
