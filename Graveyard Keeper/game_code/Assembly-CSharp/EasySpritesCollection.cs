// Decompiled with JetBrains decompiler
// Type: EasySpritesCollection
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

#nullable disable
[CreateAssetMenu(fileName = "sprite_collection", menuName = "Sprite collection")]
public class EasySpritesCollection : ScriptableObject
{
  [HideInInspector]
  public List<UnityEngine.Sprite> sprites = new List<UnityEngine.Sprite>();
  [HideInInspector]
  public List<string> sprite_names = new List<string>();
  public ESCFolderSplitter folder_splitter;
  public static Dictionary<string, UnityEngine.Sprite> hash = new Dictionary<string, UnityEngine.Sprite>();
  public static Dictionary<string, int> hash_sub = new Dictionary<string, int>();
  public static Dictionary<string, string> hash_lower_case = new Dictionary<string, string>();
  public static bool hash_created = false;
  [Space(10f)]
  public List<string> folders = new List<string>();
  public static List<EasySpritesCollection> _all_collections = new List<EasySpritesCollection>();
  public static Dictionary<int, EasySpriteCollectionSub> _loaded_sub_collections = new Dictionary<int, EasySpriteCollectionSub>();
  [Space(10f)]
  public List<string> sub_sprite_names = new List<string>();
  [Space(10f)]
  public List<int> sub_group_ids = new List<int>();
  public int _total_sprites_for_inspector;
  public static bool _all_atlases_started_loading = false;

  public static void Load(string filename = "sprite_collection")
  {
    EasySpritesCollection spritesCollection = Resources.Load<EasySpritesCollection>(filename);
    if ((UnityEngine.Object) spritesCollection == (UnityEngine.Object) null)
    {
      Debug.LogError((object) ("Error loading sprite collection: " + filename));
    }
    else
    {
      if (!EasySpritesCollection._all_collections.Contains(spritesCollection))
        EasySpritesCollection._all_collections.Add(spritesCollection);
      EasySpritesCollection.hash_created = false;
      EasySpritesCollection.CreateHash();
    }
  }

  public static void CreateHash()
  {
    if (EasySpritesCollection.hash_created)
      return;
    Debug.Log((object) nameof (CreateHash));
    EasySpritesCollection.hash.Clear();
    foreach (EasySpritesCollection allCollection in EasySpritesCollection._all_collections)
    {
      for (int index = 0; index < allCollection.sprite_names.Count; ++index)
      {
        if (!EasySpritesCollection.hash.ContainsKey(allCollection.sprite_names[index]))
          EasySpritesCollection.hash.Add(allCollection.sprite_names[index], allCollection.sprites[index]);
      }
      for (int index = 0; index < allCollection.sub_sprite_names.Count; ++index)
      {
        if (!EasySpritesCollection.hash_sub.ContainsKey(allCollection.sub_sprite_names[index]))
          EasySpritesCollection.hash_sub.Add(allCollection.sub_sprite_names[index], allCollection.sub_group_ids[index]);
      }
    }
    EasySpritesCollection.hash_created = true;
  }

  public static UnityEngine.Sprite GetSprite(
    string sprite_name,
    bool not_found_is_valid = false,
    string sprite_if_not_found = "")
  {
    if (!EasySpritesCollection.hash_created)
      EasySpritesCollection.CreateHash();
    string str;
    if (string.IsNullOrEmpty(sprite_name))
      str = string.Empty;
    else if (!EasySpritesCollection.hash_lower_case.TryGetValue(sprite_name, out str))
    {
      str = sprite_name.ToLower();
      EasySpritesCollection.hash_lower_case.Add(sprite_name, str);
    }
    if (EasySpritesCollection.hash.ContainsKey(str))
      return EasySpritesCollection.hash[str];
    if (!EasySpritesCollection.hash_sub.ContainsKey(str))
    {
      if (not_found_is_valid)
        return !string.IsNullOrEmpty(sprite_if_not_found) ? EasySpritesCollection.GetSprite(sprite_if_not_found) : (UnityEngine.Sprite) null;
      Debug.LogError((object) $"Sprite '{str}({sprite_name})' is not found in collections! (collections: {EasySpritesCollection._all_collections.Count.ToString()}, total size: {EasySpritesCollection.hash.Count.ToString()})");
      return (UnityEngine.Sprite) null;
    }
    int key = EasySpritesCollection.hash_sub[str];
    if (EasySpritesCollection._loaded_sub_collections.ContainsKey(key))
      return EasySpritesCollection._loaded_sub_collections[key].GetSprite(str);
    Debug.Log((object) ("Loading sprite sub-collection id = " + key.ToString()));
    EasySpriteCollectionSub spriteCollectionSub = Resources.Load<EasySpriteCollectionSub>("SpriteSubCollections/esc_subc_" + key.ToString());
    if ((UnityEngine.Object) spriteCollectionSub == (UnityEngine.Object) null)
    {
      Debug.LogError((object) ("Couldn't load sprite sub-collection, id = " + key.ToString()));
      return (UnityEngine.Sprite) null;
    }
    EasySpritesCollection._loaded_sub_collections.Add(key, spriteCollectionSub);
    return spriteCollectionSub.GetSprite(str);
  }

  public static int total_collections => EasySpritesCollection._all_collections.Count;

  public static bool SetSpriteOrDisableGameObject(UI2DSprite ngui_spr, string sprite_name)
  {
    if (string.IsNullOrEmpty(sprite_name))
    {
      ngui_spr.gameObject.SetActive(false);
      return false;
    }
    ngui_spr.gameObject.SetActive(true);
    ngui_spr.sprite2D = EasySpritesCollection.GetSprite(sprite_name);
    return true;
  }

  public static void LoadAllAtlasesAsync(string atlases_list_name = "sprite_collection_atlases")
  {
    if (EasySpritesCollection._all_atlases_started_loading)
      return;
    EasySpritesCollection._all_atlases_started_loading = true;
    if (EasySpritesCollection._all_collections.Count == 0)
    {
      Debug.LogError((object) "_all_collections is zero length");
    }
    else
    {
      Debug.Log((object) ("LoadAllAtlasesAsync " + atlases_list_name));
      EasySpriteCollectionAtlases collectionAtlases = Resources.Load<EasySpriteCollectionAtlases>(atlases_list_name);
      if ((UnityEngine.Object) collectionAtlases == (UnityEngine.Object) null)
      {
        Debug.LogError((object) ("Couldn't load atlases: " + atlases_list_name));
      }
      else
      {
        foreach (string atlasesName in collectionAtlases.atlases_names)
          EasySpriteCollectionManager.StartTrackingResourceRequest(atlasesName, Resources.LoadAsync<SpriteAtlas>(atlasesName), new Action<UnityEngine.Object>(EasySpritesCollection._all_collections[0].OnAtlasLoaded));
      }
    }
  }

  public void OnAtlasLoaded(UnityEngine.Object atlas)
  {
    SpriteAtlas context = atlas as SpriteAtlas;
    if ((UnityEngine.Object) context == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "OnAtlasLoaded: atlas is null");
    }
    else
    {
      Debug.Log((object) $"OnAtlasLoaded {context.name}, sprites: {context.spriteCount.ToString()}", (UnityEngine.Object) context);
      UnityEngine.Sprite[] sprites = new UnityEngine.Sprite[context.spriteCount];
      context.GetSprites(sprites);
      foreach (UnityEngine.Sprite sprite in sprites)
      {
        string lower = sprite.name.Replace("(Clone)", "").ToLower();
        if (EasySpritesCollection.hash.ContainsKey(lower))
          Debug.LogWarning((object) ("Error adding sprite to a library - duplicate name: " + lower));
        else
          EasySpritesCollection.hash.Add(lower, sprite);
      }
    }
  }
}
