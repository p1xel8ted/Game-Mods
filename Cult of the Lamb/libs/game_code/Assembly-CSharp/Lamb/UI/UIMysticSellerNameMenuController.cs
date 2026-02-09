// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIMysticSellerNameMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
