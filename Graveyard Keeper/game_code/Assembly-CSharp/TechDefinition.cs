// Decompiled with JetBrains decompiler
// Type: TechDefinition
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class TechDefinition : BalanceBaseObject
{
  public const int MAX_VISIBLE_UNLOCKS = 3;
  public int branch_type;
  public GameRes price;
  [NonSerialized]
  public List<TechDefinition> parents = new List<TechDefinition>();
  [NonSerialized]
  public List<TechDefinition> children = new List<TechDefinition>();
  public List<string> crafts = new List<string>();
  public List<string> works = new List<string>();
  public List<string> phrases = new List<string>();
  public List<string> perks = new List<string>();
  [SerializeField]
  public string[] _parents;
  public int x;
  public float y;
  public string icon;
  public string flowscript;
  public bool hidden;
  public bool invisible;
  public DLCEngine.DLCVersion requires_dlc;
  public static bool _initialized;
  public List<TechUnlock> _unlocks_list;
  public static List<string> TECH_POINTS = new List<string>()
  {
    "r",
    "g",
    "b",
    "v",
    "gratitude_points"
  };

  public List<TechUnlock> GetUnlocksList()
  {
    if (this._unlocks_list == null)
    {
      this._unlocks_list = new List<TechUnlock>();
      foreach (string craft in this.crafts)
        this._unlocks_list.Add(new TechUnlock(craft, TechUnlock.TechUnlockType.Craft));
      foreach (string work in this.works)
        this._unlocks_list.Add(new TechUnlock(work, TechUnlock.TechUnlockType.Work));
      foreach (string phrase in this.phrases)
        this._unlocks_list.Add(new TechUnlock(phrase, TechUnlock.TechUnlockType.Phrase));
      foreach (string perk in this.perks)
        this._unlocks_list.Add(new TechUnlock(perk, TechUnlock.TechUnlockType.Perk));
    }
    return this._unlocks_list;
  }

  public List<TechUnlock> GetVisibleUnlocksList()
  {
    List<TechUnlock> unlocksList = this.GetUnlocksList();
    List<TechUnlock> visibleUnlocksList = new List<TechUnlock>();
    foreach (TechUnlock techUnlock in unlocksList)
    {
      if (techUnlock.visible)
      {
        if (visibleUnlocksList.Count < 3)
        {
          visibleUnlocksList.Add(techUnlock);
        }
        else
        {
          Debug.LogError((object) $"Can't draw more than {3.ToString()} tech unlocks (MAX_VISIBLE_UNLOCKS const), tech_id = {this.id}");
          break;
        }
      }
    }
    return visibleUnlocksList;
  }

  public void InitParentsAndChildren()
  {
    foreach (string parent in this._parents)
    {
      if (!string.IsNullOrEmpty(parent))
      {
        TechDefinition data = GameBalance.me.GetData<TechDefinition>(parent);
        if (data == null)
        {
          Debug.LogError((object) ("no tech definition with id: " + parent));
        }
        else
        {
          data.AddChild(this);
          this.AddParent(data);
        }
      }
    }
  }

  public void AddParent(TechDefinition tech)
  {
    if (this.parents.Contains(tech))
      return;
    this.parents.Add(tech);
  }

  public void AddChild(TechDefinition tech)
  {
    if (this.children.Contains(tech))
      return;
    this.children.Add(tech);
  }

  public void ApplyTech()
  {
    foreach (string perk in this.perks)
    {
      if (perk[0] == '@')
        MainGame.me.save.UnlockPerk(perk.Substring(1));
      else
        MainGame.me.save.UnlockPerk(perk);
    }
    if (string.IsNullOrEmpty(this.flowscript))
      return;
    GS.RunFlowScript(this.flowscript);
  }

  public TechDefinition.TechState GetState()
  {
    if (!DLCEngine.IsDLCAvailable(this.requires_dlc) || this.invisible && !MainGame.me.save.visible_techs.Contains(this.id))
      return TechDefinition.TechState.Invisible;
    if (this.hidden && !MainGame.me.save.revealed_techs.Contains(this.id))
      return TechDefinition.TechState.Hidden;
    foreach (TechDefinition parent in this.parents)
    {
      if (parent.GetState() == TechDefinition.TechState.Hidden)
        return TechDefinition.TechState.Hidden;
    }
    if (MainGame.me.save.unlocked_techs.Contains(this.id))
      return TechDefinition.TechState.Purchased;
    return !MainGame.me.save.CanBuyTech(this.id) ? TechDefinition.TechState.Unavailable : TechDefinition.TechState.AvailableForPurchase;
  }

  public static void LinkTechs()
  {
    if (TechDefinition._initialized)
      return;
    foreach (TechDefinition techDefinition in GameBalance.me.techs_data)
    {
      techDefinition.InitParentsAndChildren();
      techDefinition.GetUnlocksList();
    }
    TechDefinition._initialized = true;
  }

  public void ResetLanguageCache()
  {
    if (this._unlocks_list == null)
      return;
    foreach (TechUnlock unlocks in this._unlocks_list)
      unlocks.ResetLanguageCache();
  }

  public enum TechState
  {
    Purchased,
    Unavailable,
    AvailableForPurchase,
    Hidden,
    Invisible,
  }
}
