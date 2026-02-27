// Decompiled with JetBrains decompiler
// Type: src.UI.ButtonHighlightController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace src.UI;

public class ButtonHighlightController : MonoBehaviour
{
  [Header("Components")]
  [SerializeField]
  private Image _image;
  [Header("Textures")]
  [SerializeField]
  private Sprite _blackSprite;
  [SerializeField]
  private Sprite _redSprite;

  public Image Image => this._image;

  public void SetAsBlack() => this._image.sprite = this._blackSprite;

  public void SetAsRed() => this._image.sprite = this._redSprite;
}
