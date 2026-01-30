// Decompiled with JetBrains decompiler
// Type: src.UI.Items.FollowerThoughtItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace src.UI.Items;

public class FollowerThoughtItem : BaseMonoBehaviour
{
  public const int kSpriteSizeReduction = 10;
  [SerializeField]
  public Image _icon;
  [SerializeField]
  public TextMeshProUGUI _thoughtDescription;
  [SerializeField]
  public Sprite _fiathDoubleDown;
  [SerializeField]
  public Sprite _faithDown;
  [SerializeField]
  public Sprite _faithUp;
  [SerializeField]
  public Sprite _faithDoubleUp;
  [SerializeField]
  public MMSelectable _selectable;

  public MMSelectable Selectable => this._selectable;

  public void Configure(ThoughtData thoughtData)
  {
    if ((double) thoughtData.Modifier <= -7.0)
      this._icon.sprite = this._fiathDoubleDown;
    else if ((double) thoughtData.Modifier < 0.0)
      this._icon.sprite = this._faithDown;
    else if ((double) thoughtData.Modifier >= 7.0)
      this._icon.sprite = this._faithDoubleUp;
    else if ((double) thoughtData.Modifier >= 0.0)
      this._icon.sprite = this._faithUp;
    string str = FollowerThoughts.GetLocalisedName(thoughtData.ThoughtType, thoughtData.FollowerID);
    if (str.Contains("<sprite"))
      str = str.Insert(str.IndexOf("<sprite", StringComparison.OrdinalIgnoreCase), $"<size=-{10}>") + "</size>";
    if (LocalizationManager.CurrentLanguage == "French (Canadian)")
      this._thoughtDescription.text = string.Join(" : ", str.Bold(), FollowerThoughts.GetLocalisedDescription(thoughtData.ThoughtType, thoughtData.FollowerID));
    else
      this._thoughtDescription.text = string.Join(": ", str.Bold(), FollowerThoughts.GetLocalisedDescription(thoughtData.ThoughtType, thoughtData.FollowerID));
  }
}
