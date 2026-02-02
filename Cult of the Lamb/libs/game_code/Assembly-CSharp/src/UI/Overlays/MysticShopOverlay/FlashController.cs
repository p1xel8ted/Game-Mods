// Decompiled with JetBrains decompiler
// Type: src.UI.Overlays.MysticShopOverlay.FlashController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace src.UI.Overlays.MysticShopOverlay;

[ExecuteInEditMode]
public class FlashController : MonoBehaviour
{
  [SerializeField]
  [Range(0.0f, 1f)]
  public float _flash;
  [SerializeField]
  public MaskableGraphic[] _graphics;

  public float Flash
  {
    get => this._flash;
    set
    {
      this._flash = value;
      this.Update();
    }
  }

  public void Update()
  {
    Color white = Color.white with { a = this._flash };
    foreach (Graphic graphic in this._graphics)
      graphic.color = white;
  }
}
