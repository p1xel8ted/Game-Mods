// Decompiled with JetBrains decompiler
// Type: BuildingSubZoneConfiguration
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BuildingSubZoneConfiguration : ScriptableObject
{
  public float z_build_grid_sorting = 1900f;
  public string build_cell_sorting_layer = "on_ground_3";
  public bool is_cell_ground_object = true;
  public bool override_grid_cell_sorting_order;
  public bool sort_floating_over_everything;
  public int grid_cell_sorting_order;
}
