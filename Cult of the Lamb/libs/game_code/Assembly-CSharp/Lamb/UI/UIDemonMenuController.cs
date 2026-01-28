// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIDemonMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.FollowerSelect;

#nullable disable
namespace Lamb.UI;

public class UIDemonMenuController : UIFollowerSelectBase<DemonFollowerItem>
{
  public override bool AllowsVoting => false;

  public override DemonFollowerItem PrefabTemplate()
  {
    return MonoSingleton<UIManager>.Instance.DemonFollowerItemTemplate;
  }
}
