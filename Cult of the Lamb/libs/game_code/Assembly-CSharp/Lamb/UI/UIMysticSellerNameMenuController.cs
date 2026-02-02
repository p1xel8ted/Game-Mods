// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIMysticSellerNameMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;

#nullable disable
namespace Lamb.UI;

public class UIMysticSellerNameMenuController : UICultNameMenuController
{
  public override void Awake()
  {
    base.Awake();
    this._nameInputField.text = LocalizationManager.GetTranslation("NAMES/MysticShopSellerDefault");
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    MonoSingleton<UIManager>.Instance.UnloadMysticSellerNameMenuAssets();
  }
}
