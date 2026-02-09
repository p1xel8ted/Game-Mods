// Decompiled with JetBrains decompiler
// Type: GameBalanceBase
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GameBalanceBase : ScriptableObject
{
  [NonSerialized]
  public List<IList> _datas = new List<IList>();
  [NonSerialized]
  public List<System.Type> _types = new List<System.Type>();
  [NonSerialized]
  public List<Dictionary<string, int>> _cache = new List<Dictionary<string, int>>();
  public bool _cache_created;
  public static string current_tab_name = "";

  public GameBalanceBase() => this.InitBalance();

  public void InitBalance()
  {
    this._datas.Clear();
    this._types.Clear();
    this._cache.Clear();
    foreach (KeyValuePair<string, IList> listsAndGoogleTab in this.GetAllDataListsAndGoogleTabs())
    {
      this._datas.Add(listsAndGoogleTab.Value);
      this._types.Add(listsAndGoogleTab.Value.GetType().GetGenericArguments()[0]);
      this._cache.Add(new Dictionary<string, int>());
    }
  }

  public void CreateIDsCache()
  {
    for (int index1 = 0; index1 < this._types.Count; ++index1)
    {
      this._cache[index1].Clear();
      for (int index2 = 0; index2 < this._datas[index1].Count; ++index2)
      {
        BalanceBaseObject balanceBaseObject = this._datas[index1][index2] as BalanceBaseObject;
        this._cache[index1].Add(balanceBaseObject.id, index2);
      }
    }
    this._cache_created = true;
  }

  public void ClearBalance()
  {
    this.InitBalance();
    foreach (IList data in this._datas)
      data.Clear();
  }

  public static T DataNotFound<T>(string identifier) where T : BalanceBaseObject
  {
    Debug.LogWarning((object) $"No data for object [{typeof (T)?.ToString()}] with id = \"{identifier}\"");
    return default (T);
  }

  public static bool HaveCollectionSameID<T>(List<T> list, T data) where T : BalanceBaseObject
  {
    return list.FindIndex((Predicate<T>) (p => p.id == ((T) data).id)) != -1;
  }

  public static T GetElementByID<T>(List<T> list, string id) where T : BalanceBaseObject
  {
    return list.Find((Predicate<T>) (p => p.id == id));
  }

  public static T GetElementByID<T>(List<T> list, Dictionary<string, int> cache, string id) where T : BalanceBaseObject
  {
    if (id == null)
      return default (T);
    try
    {
      if (cache != null)
        return list[cache[id]];
      Debug.LogError((object) $"ERROR: Trying to get a {typeof (T)?.ToString()} item with a null cache");
      return default (T);
    }
    catch (Exception ex)
    {
      return default (T);
    }
  }

  public string AddToDataCollections<T>(List<T> list, T data_to_add) where T : BalanceBaseObject
  {
    if (GameBalanceBase.HaveCollectionSameID<T>(list, data_to_add))
      return "Can't add: same id already exist: " + data_to_add.id;
    list.Add(data_to_add);
    return "";
  }

  public string AddData<T>(T data_to_add) where T : BalanceBaseObject
  {
    int index = this._types.IndexOf(typeof (T));
    if (index != -1)
      return this.AddToDataCollections<T>(this._datas[index] as List<T>, data_to_add);
    Debug.LogError((object) ("Unknown type at AddData: " + typeof (T)?.ToString()));
    return (string) null;
  }

  public string AddDataUniversal(object data_to_add)
  {
    BalanceBaseObject balanceBaseObject = data_to_add as BalanceBaseObject;
    System.Type type = data_to_add.GetType();
    if (balanceBaseObject == null)
    {
      Debug.LogError((object) $"Type {type.Name} couldn't be converted to BalanceBaseObject");
      return (string) null;
    }
    int index = this._types.IndexOf(type);
    if (index == -1)
    {
      Debug.LogError((object) ("Unknown type at AddData: " + type.Name));
      return (string) null;
    }
    foreach (object obj in (IEnumerable) this._datas[index])
    {
      if ((obj as BalanceBaseObject).id == balanceBaseObject.id)
        return "Can't add: same id already exist: " + balanceBaseObject.id;
    }
    this._datas[index].Add(data_to_add);
    return "";
  }

  public T GetData<T>(string id) where T : BalanceBaseObject
  {
    return this.GetDataOrNull<T>(id) ?? GameBalanceBase.DataNotFound<T>(id);
  }

  public T GetDataOrNull<T>(string id) where T : BalanceBaseObject
  {
    if (id == null)
      return default (T);
    int index = this._types.IndexOf(typeof (T));
    if (index == -1)
    {
      Debug.LogError((object) ("Unknown type at GetData: " + typeof (T)?.ToString()));
      return default (T);
    }
    return !this._cache_created ? GameBalanceBase.GetElementByID<T>(this._datas[index] as List<T>, id) : GameBalanceBase.GetElementByID<T>(this._datas[index] as List<T>, this._cache[index], id);
  }

  public virtual Dictionary<string, IList> GetAllDataListsAndGoogleTabs()
  {
    throw new Exception("GameBalanceBase.GetAllDataListsAndGoogleTabs() should be overriden!");
  }
}
