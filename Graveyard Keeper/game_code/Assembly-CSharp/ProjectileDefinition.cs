// Decompiled with JetBrains decompiler
// Type: ProjectileDefinition
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class ProjectileDefinition : BalanceBaseObject
{
  public string prefab_name;
  public float speed;
  public float max_dist_lower_limit;
  public float max_dist_upper_limit;
  public float max_time;
  public float damage;
  public int pierce;
  public bool can_damage_mobs;
  public EventDefinition on_start;
  public EventDefinition on_hit_combat;
  public EventDefinition on_hit_non_combat;
  public EventDefinition on_out_of_screen;
  public EventDefinition on_max_dist_reached;

  public float GetMaxDist()
  {
    float num = this.max_dist_upper_limit - this.max_dist_lower_limit;
    return (double) num < 0.0099999997764825821 ? this.max_dist_upper_limit : this.max_dist_lower_limit + UnityEngine.Random.value * num;
  }
}
