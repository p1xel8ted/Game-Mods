// Decompiled with JetBrains decompiler
// Type: CustomDrawers
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class CustomDrawers
{
  public WorldGameObject _wobj;
  public const string GRAVE_PATH_PREFIX = "objects/grave parts/";
  public GardenCustomDrawer _garden_drawer;
  public StoneStockpileCustomDrawer _stone_stockpile_drawer;
  public List<WOPPrefabName> _wop_prefabs_names;

  public int grave_stage_decay
  {
    get => Mathf.FloorToInt((float) ((double) this._wobj.GetDecayFactor() * 100.0 / 35.0)) + 1;
  }

  public WorldGameObject wobj => this._wobj;

  public void SetWobj(WorldGameObject obj) => this._wobj = obj;

  public void OnValidate()
  {
  }

  public void OnObjectRedraw(bool force_redraw = false)
  {
    this._garden_drawer = (GardenCustomDrawer) null;
    this._stone_stockpile_drawer = (StoneStockpileCustomDrawer) null;
    if ((UnityEngine.Object) this._wobj == (UnityEngine.Object) null)
      return;
    if (this._wobj.obj_def == null)
    {
      Debug.LogError((object) ("OnObjectRedraw: no obj definition for id = " + this._wobj.obj_id));
    }
    else
    {
      string id = this._wobj.obj_def.id;
      if (id == null)
        return;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(id))
      {
        case 17324106:
          if (!(id == "garden_onion"))
            return;
          goto label_92;
        case 33223163:
          if (!(id == "zombie_garden_desk_carrot"))
            return;
          goto label_92;
        case 50016300:
          if (!(id == "garden_wheat"))
            return;
          goto label_92;
        case 306844660:
          if (!(id == "refugee_camp_garden_bed_1"))
            return;
          goto label_92;
        case 326769621:
          if (!(id == "garden_grapes"))
            return;
          goto label_92;
        case 340399898:
          if (!(id == "refugee_camp_garden_bed_3"))
            return;
          goto label_92;
        case 357177517:
          if (!(id == "refugee_camp_garden_bed_2"))
            return;
          goto label_92;
        case 379524836:
          if (!(id == "zombie_garden_desk_pumpkin"))
            return;
          goto label_92;
        case 396158665:
          if (!(id == "refugee_garden_desk_beet"))
            return;
          goto label_92;
        case 428506710:
          if (!(id == "garden_lentils"))
            return;
          goto label_92;
        case 640521172:
          if (!(id == "tree_apple_garden_empty"))
            return;
          goto label_92;
        case 665634972:
          if (!(id == "zombie_vineyard_desk_1"))
            return;
          goto label_92;
        case 682412591:
          if (!(id == "zombie_vineyard_desk_0"))
            return;
          goto label_92;
        case 693445352:
          if (!(id == "grave_ground"))
            return;
          break;
        case 700626221:
          if (!(id == "zombie_garden_desk_onion"))
            return;
          goto label_92;
        case 715967829:
          if (!(id == "zombie_vineyard_desk_2"))
            return;
          goto label_92;
        case 839995526:
          if (!(id == "garden_cannabis"))
            return;
          goto label_92;
        case 997028443:
          if (!(id == "garden_pumpkin"))
            return;
          goto label_92;
        case 1004682244:
          if (!(id == "zombie_garden_desk_beet"))
            return;
          goto label_92;
        case 1010817924:
          if (!(id == "zombie_garden_desk_0"))
            return;
          goto label_92;
        case 1027595543:
          if (!(id == "zombie_garden_desk_1"))
            return;
          goto label_92;
        case 1044373162:
          if (!(id == "zombie_garden_desk_2"))
            return;
          goto label_92;
        case 1112571752:
          if (!(id == "refugee_garden_desk_wheat"))
            return;
          goto label_92;
        case 1248768869:
          if (!(id == "garden_beet"))
            return;
          goto label_92;
        case 1337347697:
          if (!(id == "zombie_garden_desk_lentils"))
            return;
          goto label_92;
        case 1371892463:
          if (!(id == "working_table"))
            return;
          this.OnDrawSawmill();
          return;
        case 1470254187:
          if (!(id == "zombie_garden_desk_cabbage"))
            return;
          goto label_92;
        case 1680328836:
          if (!(id == "garden_hop"))
            return;
          goto label_92;
        case 1686372103:
          if (!(id == "refugee_garden_desk_pumpkin"))
            return;
          goto label_92;
        case 2183778938:
          if (!(id == "refugee_garden_desk_carrot"))
            return;
          goto label_92;
        case 2196835653:
          if (!(id == "autopsi_table"))
            return;
          this.OnDrawAutopsy();
          return;
        case 2237231266:
          if (!(id == "tree_apple_garden"))
            return;
          goto label_92;
        case 2449922508:
          if (!(id == "garden_cabbage"))
            return;
          goto label_92;
        case 2456363296:
          if (!(id == "refugee_garden_desk_cabbage"))
            return;
          goto label_92;
        case 2632379662:
          if (!(id == "refugee_garden_desk_onion"))
            return;
          goto label_92;
        case 2639747405:
          if (!(id == "mf_stones_1"))
            return;
          this.OnDrawStoneStockpile();
          return;
        case 2694761518:
          if (!(id == "garden_carrot"))
            return;
          goto label_92;
        case 3041770895:
          if (!(id == "zombie_garden_desk_wheat"))
            return;
          goto label_92;
        case 3221162032:
          if (!(id == "bush_berry_garden"))
            return;
          goto label_92;
        case 3427968194:
          if (!(id == "refugee_garden_desk_lentils"))
            return;
          goto label_92;
        case 4208184612:
          if (!(id == "grave_empty"))
            return;
          break;
        default:
          return;
      }
      this.OnDrawGrave(force_redraw);
      return;
label_92:
      this.OnDrawGarden();
    }
  }

  public void OnDrawAutopsy()
  {
    foreach (WorldObjectPart additionalWop in this._wobj.additional_wops)
    {
      if (!((UnityEngine.Object) additionalWop == (UnityEngine.Object) null))
        GJCommons.Destroy((UnityEngine.Object) additionalWop.gameObject);
    }
    this._wobj.additional_wops.Clear();
    foreach (Item obj in this._wobj.data.inventory)
    {
      if (obj.definition.type == ItemDefinition.ItemType.Body)
      {
        WorldObjectPart o = (WorldObjectPart) null;
        this._wobj.RedrawPart(ref o, "body_autopsi", "objects/grave parts/", 0.0f);
        if (!((UnityEngine.Object) o == (UnityEngine.Object) null))
          this._wobj.additional_wops.Add(o);
      }
    }
  }

  public bool NeedRedrawGrave()
  {
    if ((UnityEngine.Object) this._wobj == (UnityEngine.Object) null || this._wobj.obj_def == null || this._wobj.obj_def.id != "grave_ground")
      return false;
    if (this._wop_prefabs_names == null)
      this._wop_prefabs_names = new List<WOPPrefabName>();
    List<WOPPrefabName> newWopPrefabsNames = this.GetNewWOPPrefabsNames();
    if (this._wop_prefabs_names.Count != newWopPrefabsNames.Count)
    {
      this._wop_prefabs_names = newWopPrefabsNames;
      return true;
    }
    for (int index = 0; index < this._wop_prefabs_names.Count; ++index)
    {
      if (this._wop_prefabs_names[index] == null)
      {
        this._wop_prefabs_names = newWopPrefabsNames;
        return true;
      }
      if (!this._wop_prefabs_names[index].EqualsTo(newWopPrefabsNames[index]))
      {
        this._wop_prefabs_names = newWopPrefabsNames;
        return true;
      }
    }
    this._wop_prefabs_names = newWopPrefabsNames;
    return false;
  }

  public List<WOPPrefabName> GetNewWOPPrefabsNames()
  {
    List<WOPPrefabName> newWopPrefabsNames = new List<WOPPrefabName>();
    List<Item> objList = new List<Item>();
    foreach (Item obj in this._wobj.data.inventory)
    {
      if (obj.definition == null)
      {
        Debug.LogError((object) ("WGO has an item without a definition, id = " + obj.id), (UnityEngine.Object) this._wobj);
        objList.Add(obj);
      }
      else if (obj.definition.type == ItemDefinition.ItemType.GraveCover || obj.definition.type == ItemDefinition.ItemType.GraveFence || obj.definition.type == ItemDefinition.ItemType.GraveStone)
      {
        if ((double) this._wobj.GetParam(obj.id) < 1.0)
        {
          switch (obj.definition.type)
          {
            case ItemDefinition.ItemType.GraveStone:
              newWopPrefabsNames.Add(new WOPPrefabName()
              {
                part_1 = "grave_top_building_1_stg_1"
              });
              continue;
            case ItemDefinition.ItemType.GraveFence:
              newWopPrefabsNames.Add(new WOPPrefabName()
              {
                part_1 = "grave_bot_building_1_stg_1"
              });
              continue;
            default:
              continue;
          }
        }
        else
        {
          int graveStageDecay = this.grave_stage_decay;
          int num = 4 - Mathf.CeilToInt(obj.durability * 3f);
          if (num > 3)
            num = 3;
          WOPPrefabName wopPrefabName = new WOPPrefabName()
          {
            part_1 = obj.id + "_stg_",
            stage = num,
            part_2 = "",
            need_herb = false
          };
          if (this._wobj.GetParamInt("vegetation") != 0)
            wopPrefabName.need_herb = true;
          newWopPrefabsNames.Add(wopPrefabName);
        }
      }
    }
    foreach (Item obj in objList)
      this._wobj.data.inventory.Remove(obj);
    return newWopPrefabsNames;
  }

  public void OnDrawGrave(bool forced = false)
  {
    if (this._wobj.obj_def.id != "grave_ground")
      return;
    this.ForceNonNegativeParamValue("decay");
    this.ForceNonNegativeParamValue("vegetation");
    if (forced)
      this._wop_prefabs_names = this.GetNewWOPPrefabsNames();
    else if (!this.NeedRedrawGrave())
      return;
    this.RemoveAllWOPs();
    foreach (WOPPrefabName wopPrefabsName in this._wop_prefabs_names)
    {
      WorldObjectPart o1 = (WorldObjectPart) null;
      if (wopPrefabsName.stage <= 0)
      {
        this._wobj.RedrawPart(ref o1, wopPrefabsName.GetName(), "objects/grave parts/", 0.0f);
      }
      else
      {
        for (; wopPrefabsName.stage > 0; --wopPrefabsName.stage)
        {
          this._wobj.RedrawPart(ref o1, wopPrefabsName.GetName(), "objects/grave parts/", 0.0f);
          if (!((UnityEngine.Object) o1 == (UnityEngine.Object) null))
            break;
        }
      }
      if ((UnityEngine.Object) o1 != (UnityEngine.Object) null)
      {
        this._wobj.additional_wops.Add(o1);
        if (wopPrefabsName.need_herb)
        {
          WorldObjectPart o2 = (WorldObjectPart) null;
          this._wobj.RedrawPart(ref o2, wopPrefabsName.GetName() + "_herb", "objects/grave parts/", 0.0f);
          if ((UnityEngine.Object) o2 != (UnityEngine.Object) null)
            this._wobj.additional_wops.Add(o2);
        }
      }
    }
  }

  public void RemoveAllWOPs()
  {
    if ((UnityEngine.Object) this._wobj == (UnityEngine.Object) null || this._wobj.additional_wops == null || this._wobj.additional_wops.Count == 0)
      return;
    foreach (WorldObjectPart additionalWop in this._wobj.additional_wops)
    {
      if (!((UnityEngine.Object) additionalWop == (UnityEngine.Object) null))
        GJCommons.Destroy((UnityEngine.Object) additionalWop.gameObject);
    }
    this._wobj.additional_wops.Clear();
  }

  public void ForceNonNegativeParamValue(string param_name)
  {
    if (this._wobj.GetParamInt(param_name) >= 0)
      return;
    this._wobj.SetParam(param_name, 0.0f);
  }

  public void OnDrawSawmill()
  {
  }

  public void OnDrawGarden()
  {
    WorldObjectPart[] componentsInChildren = this._wobj.GetComponentsInChildren<WorldObjectPart>();
    WorldObjectPart worldObjectPart1 = (WorldObjectPart) null;
    if (componentsInChildren.Length == 1)
    {
      worldObjectPart1 = componentsInChildren[0];
    }
    else
    {
      foreach (WorldObjectPart worldObjectPart2 in componentsInChildren)
      {
        if (!((UnityEngine.Object) worldObjectPart2 == (UnityEngine.Object) null) && worldObjectPart2.enabled && (worldObjectPart2.name.Contains("garden") || worldObjectPart2.name.Contains("vineyard")))
        {
          worldObjectPart1 = worldObjectPart2;
          break;
        }
      }
    }
    if ((UnityEngine.Object) worldObjectPart1 == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "OnDrawGarden: WOP to draw is null!");
    }
    else
    {
      if ((UnityEngine.Object) this._garden_drawer == (UnityEngine.Object) null)
      {
        this._garden_drawer = worldObjectPart1.GetComponentInChildren<GardenCustomDrawer>();
        if ((UnityEngine.Object) this._garden_drawer == (UnityEngine.Object) null)
        {
          Debug.LogError((object) "GardenCastomDrawer is null!");
          return;
        }
      }
      this._garden_drawer.Redraw(this._wobj);
    }
  }

  public void FastRedraw()
  {
    if ((UnityEngine.Object) this._wobj == (UnityEngine.Object) null || !((UnityEngine.Object) this._garden_drawer != (UnityEngine.Object) null))
      return;
    this._garden_drawer.Redraw(this._wobj);
  }

  public void OnDrawStoneStockpile()
  {
    WorldObjectPart[] componentsInChildren = this._wobj.GetComponentsInChildren<WorldObjectPart>();
    WorldObjectPart worldObjectPart1 = (WorldObjectPart) null;
    if (componentsInChildren.Length == 1)
    {
      worldObjectPart1 = componentsInChildren[0];
    }
    else
    {
      foreach (WorldObjectPart worldObjectPart2 in componentsInChildren)
      {
        if (!((UnityEngine.Object) worldObjectPart2 == (UnityEngine.Object) null) && worldObjectPart2.enabled)
        {
          worldObjectPart1 = worldObjectPart2;
          break;
        }
      }
    }
    if ((UnityEngine.Object) worldObjectPart1 == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "OnDrawStoneStockpile: WOP to draw is null!");
    }
    else
    {
      if ((UnityEngine.Object) this._stone_stockpile_drawer == (UnityEngine.Object) null)
      {
        this._stone_stockpile_drawer = worldObjectPart1.GetComponentInChildren<StoneStockpileCustomDrawer>();
        if ((UnityEngine.Object) this._stone_stockpile_drawer == (UnityEngine.Object) null)
        {
          Debug.LogError((object) "StoneStockpileCustomDrawer is null!");
          return;
        }
      }
      this._stone_stockpile_drawer.Redraw(this._wobj);
    }
  }
}
