// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SmartRes
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[ParadoxNotion.Design.Icon("Stack", false, "")]
[Color("00ff00")]
[Category("Game Functions")]
[Name("Smart Res", 0)]
public class Flow_SmartRes : PureFunctionNode<SmartRes, SmartRes.ResType, string, float>
{
  public override SmartRes Invoke(SmartRes.ResType res_type, string id, float v)
  {
    SmartRes smartRes = new SmartRes()
    {
      res_type = res_type
    };
    switch (res_type)
    {
      case SmartRes.ResType.Item:
        smartRes.item = new Item(id, Mathf.RoundToInt(v));
        if (string.IsNullOrEmpty(id))
        {
          Debug.LogError((object) ("ERROR: SmartRes - item id is empty, v = " + v.ToString()));
          break;
        }
        break;
      case SmartRes.ResType.GameRes:
        smartRes.res = new GameResAtom(id, v);
        if (string.IsNullOrEmpty(id))
        {
          Debug.LogError((object) ("ERROR: SmartRes - res id is empty, v = " + v.ToString()));
          break;
        }
        break;
    }
    return smartRes;
  }
}
