// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIDemonMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
