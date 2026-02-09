// Decompiled with JetBrains decompiler
// Type: ObjectCraftDefinition
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class ObjectCraftDefinition : CraftDefinition
{
  public string out_obj = "";
  public ObjectCraftDefinition.BuildType build_type;
  public List<string> builder_ids = new List<string>();
  public List<string> locked_builders_ids = new List<string>();
  public bool enabled = true;
  public string sub_zone_id = "";
  public bool is_remove_without_hp_work;
  public bool is_destroy_worker_on_remove;
  public bool wait_script_callback;
  public bool has_variations;

  public enum BuildType
  {
    Put,
    Remove,
    None,
  }
}
