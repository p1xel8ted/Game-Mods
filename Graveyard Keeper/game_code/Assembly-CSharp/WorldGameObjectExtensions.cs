// Decompiled with JetBrains decompiler
// Type: WorldGameObjectExtensions
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

#nullable disable
public static class WorldGameObjectExtensions
{
  public static string[] GetStagesOfWGO(this string obj_id)
  {
    switch (obj_id)
    {
      case "flower_small_1":
      case "flower_small_2":
      case "flower_small_3":
      case "flower_small_4":
      case "flower_small_5":
      case "flower_small_6":
      case "flower_small_7":
      case "flower_small_8":
      case "flower_small_9":
        return new string[2]{ "flower_spawner", obj_id };
      case "flower_spawner":
        return new string[10]
        {
          "flower_spawner",
          "flower_small_1",
          "flower_small_2",
          "flower_small_3",
          "flower_small_4",
          "flower_small_5",
          "flower_small_6",
          "flower_small_7",
          "flower_small_8",
          "flower_small_9"
        };
      case "mushroom_1":
      case "mushroom_2":
        return new string[2]{ obj_id, "mushroom_spawner" };
      case "mushroom_spawner":
        return new string[3]
        {
          "mushroom_1",
          "mushroom_2",
          "mushroom_spawner"
        };
      case "tree_1_1":
      case "tree_1_1_stump":
        return new string[4]
        {
          "tree_1_1",
          "tree_1_1_stump",
          "tree_spawner",
          "tree_micro"
        };
      case "tree_1_2":
      case "tree_1_2_stump":
        return new string[4]
        {
          "tree_1_2",
          "tree_1_2_stump",
          "tree_spawner",
          "tree_micro"
        };
      case "tree_1_3":
      case "tree_1_3_stump":
        return new string[4]
        {
          "tree_1_3",
          "tree_1_3_stump",
          "tree_spawner",
          "tree_micro"
        };
      case "tree_1_4":
      case "tree_1_4_stump":
        return new string[4]
        {
          "tree_1_4",
          "tree_1_4_stump",
          "tree_spawner",
          "tree_micro"
        };
      case "tree_2_1":
      case "tree_2_1_stump":
        return new string[4]
        {
          "tree_2_1",
          "tree_2_1_stump",
          "tree_spawner",
          "tree_micro"
        };
      case "tree_2_2":
      case "tree_2_2_stump":
        return new string[4]
        {
          "tree_2_2",
          "tree_2_2_stump",
          "tree_spawner",
          "tree_micro"
        };
      case "tree_2_3":
      case "tree_2_3_stump":
        return new string[4]
        {
          "tree_2_3",
          "tree_2_3_stump",
          "tree_spawner",
          "tree_micro"
        };
      case "tree_2_4":
      case "tree_2_4_stump":
        return new string[4]
        {
          "tree_2_4",
          "tree_2_4_stump",
          "tree_spawner",
          "tree_micro"
        };
      case "tree_2_5":
      case "tree_2_5_stump":
        return new string[4]
        {
          "tree_2_5",
          "tree_2_5_stump",
          "tree_spawner",
          "tree_micro"
        };
      case "tree_2_6":
      case "tree_2_6_stump":
        return new string[4]
        {
          "tree_2_6",
          "tree_2_6_stump",
          "tree_spawner",
          "tree_micro"
        };
      case "tree_3_1":
      case "tree_3_1_stump":
        return new string[4]
        {
          "tree_3_1",
          "tree_3_1_stump",
          "tree_spawner",
          "tree_micro"
        };
      case "tree_3_1_bees":
      case "tree_3_1_bees_done":
        return new string[2]
        {
          "tree_3_1_bees",
          "tree_3_1_bees_done"
        };
      case "tree_3_2":
      case "tree_3_2_stump":
        return new string[4]
        {
          "tree_3_2",
          "tree_3_2_stump",
          "tree_spawner",
          "tree_micro"
        };
      case "tree_3_2_bees":
      case "tree_3_2_bees_done":
        return new string[2]
        {
          "tree_3_2_bees",
          "tree_3_2_bees_done"
        };
      case "tree_3_3":
      case "tree_3_3_stump":
        return new string[4]
        {
          "tree_3_3",
          "tree_3_3_stump",
          "tree_spawner",
          "tree_micro"
        };
      case "tree_3_3_bees":
      case "tree_3_3_bees_done":
        return new string[2]
        {
          "tree_3_3_bees",
          "tree_3_3_bees_done"
        };
      case "tree_3_4":
      case "tree_3_4_stump":
        return new string[4]
        {
          "tree_3_4",
          "tree_3_4_stump",
          "tree_spawner",
          "tree_micro"
        };
      case "tree_3_4_bees":
      case "tree_3_4_bees_done":
        return new string[2]
        {
          "tree_3_4_bees",
          "tree_3_4_bees_done"
        };
      case "tree_micro":
      case "tree_spawner":
        return new string[30]
        {
          "tree_spawner",
          "tree_micro",
          "tree_1_1",
          "tree_1_1_stump",
          "tree_1_2",
          "tree_1_2_stump",
          "tree_1_3",
          "tree_1_3_stump",
          "tree_1_4",
          "tree_1_4_stump",
          "tree_2_1",
          "tree_2_1_stump",
          "tree_2_2",
          "tree_2_2_stump",
          "tree_2_3",
          "tree_2_3_stump",
          "tree_2_4",
          "tree_2_4_stump",
          "tree_2_5",
          "tree_2_5_stump",
          "tree_2_6",
          "tree_2_6_stump",
          "tree_3_1",
          "tree_3_1_stump",
          "tree_3_2",
          "tree_3_2_stump",
          "tree_3_3",
          "tree_3_3_stump",
          "tree_3_4",
          "tree_3_4_stump"
        };
      case "tree_old_1":
      case "tree_old_1_stump":
        return new string[2]
        {
          "tree_old_1",
          "tree_old_1_stump"
        };
      case "tree_old_2":
      case "tree_old_2_stump":
        return new string[2]
        {
          "tree_old_2",
          "tree_old_2_stump"
        };
      case "tree_old_3":
      case "tree_old_3_stump":
        return new string[2]
        {
          "tree_old_3",
          "tree_old_3_stump"
        };
      default:
        return new string[1]{ obj_id };
    }
  }
}
