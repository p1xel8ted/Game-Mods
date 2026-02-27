// Decompiled with JetBrains decompiler
// Type: src.UI.Items.FollowerThoughtItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace src.UI.Items;

public class FollowerThoughtItem : BaseMonoBehaviour
{
  private const int kSpriteSizeReduction = 10;
  [SerializeField]
  private Image _icon;
  [SerializeField]
  private TextMeshProUGUI _thoughtDescription;
  [SerializeField]
  private Sprite _fiathDoubleDown;
  [SerializeField]
  private Sprite _faithDown;
  [SerializeField]
  private Sprite _faithUp;
  [SerializeField]
  private Sprite _faithDoubleUp;
  [SerializeField]
  private MMSelectable _selectable;

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
    string str = FollowerThoughts.GetLocalisedName(thoughtData.ThoughtType);
    if (str.Contains("<sprite"))
      str = str.Insert(str.IndexOf("<sprite", StringComparison.OrdinalIgnoreCase), $"<size=-{10}>") + "</size>";
    this._thoughtDescription.text = string.Join(": ", str.Bold(), FollowerThoughts.GetLocalisedDescription(thoughtData.ThoughtType));
  }
}
