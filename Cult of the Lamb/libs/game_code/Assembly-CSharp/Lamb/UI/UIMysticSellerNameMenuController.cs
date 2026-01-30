// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIMysticSellerNameMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
