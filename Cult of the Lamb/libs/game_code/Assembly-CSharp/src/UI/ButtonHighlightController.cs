// Decompiled with JetBrains decompiler
// Type: src.UI.ButtonHighlightController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace src.UI;

public class ButtonHighlightController : MonoBehaviour
{
  [Header("Components")]
  [SerializeField]
  public Image _image;
  [Header("Textures")]
  [SerializeField]
  public Sprite _blackSprite;
  [SerializeField]
  public Sprite _redSprite;
  [SerializeField]
  public Sprite _twitchSprite;

  public Image Image => this._image;

  public void SetAsBlack() => this._image.sprite = this._blackSprite;

  public void SetAsRed() => this._image.sprite = this._redSprite;

  public void SetAsTwitch() => this._image.sprite = this._twitchSprite;
}
