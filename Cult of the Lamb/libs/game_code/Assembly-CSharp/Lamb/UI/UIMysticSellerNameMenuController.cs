// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIMysticSellerNameMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
