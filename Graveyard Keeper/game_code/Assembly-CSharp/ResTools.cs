// Decompiled with JetBrains decompiler
// Type: ResTools
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public static class ResTools
{
  public static string ParseResForCraft(this string res_srt)
  {
    return res_srt.Replace("[GameRes: ", "").Replace("]", "").Replace("=", " ");
  }

  public static int UsedInventoryCellCount(this GameRes res)
  {
    int num = 0;
    foreach (GameResAtom atom in res.ToAtomList())
      num += ResTools.CellsUsedForRes(atom, out int _);
    return num;
  }

  public static List<GameResAtom> ToUnstackedAtomList(this GameRes res)
  {
    List<GameResAtom> unstackedAtomList = new List<GameResAtom>();
    foreach (GameResAtom atom in res.ToAtomList())
    {
      int stack_count;
      int num = ResTools.CellsUsedForRes(atom, out stack_count);
      float v = atom.value;
      string type = atom.type;
      if (num == 1)
        unstackedAtomList.Add(new GameResAtom(type, v));
      else if (num > 1)
      {
        for (; (double) v / (double) stack_count > 0.0; v -= (float) stack_count)
          unstackedAtomList.Add(new GameResAtom(type, stack_count));
        if ((double) v > 0.0)
          unstackedAtomList.Add(new GameResAtom(type, v));
      }
    }
    return unstackedAtomList;
  }

  public static List<GameResAtom> ListOfResAtomsUntilFullStack(this GameRes res)
  {
    List<GameResAtom> gameResAtomList = new List<GameResAtom>();
    foreach (GameResAtom unstackedAtom in res.ToUnstackedAtomList())
    {
      ItemDefinition dataOrNull = GameBalance.me.GetDataOrNull<ItemDefinition>(unstackedAtom.type);
      if (dataOrNull != null)
      {
        float v = (float) dataOrNull.stack_count - unstackedAtom.value;
        if ((double) v > 0.0)
          gameResAtomList.Add(new GameResAtom(unstackedAtom.type, v));
      }
    }
    return gameResAtomList;
  }

  public static int CellsUsedForRes(GameResAtom res_atom, out int stack_count)
  {
    stack_count = 0;
    ItemDefinition dataOrNull = GameBalance.me.GetDataOrNull<ItemDefinition>(res_atom.type);
    if (dataOrNull == null)
      return 0;
    stack_count = dataOrNull.stack_count;
    return (int) Math.Ceiling((double) res_atom.value / (double) stack_count);
  }
}
