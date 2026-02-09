// Decompiled with JetBrains decompiler
// Type: EasySpriteCollectionSub
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EasySpriteCollectionSub : ScriptableObject
{
  public int id = -1;
  public List<UnityEngine.Sprite> sprites = new List<UnityEngine.Sprite>();
  public List<string> sprite_names = new List<string>();
  public bool hash_created;
  public Dictionary<string, UnityEngine.Sprite> hash = new Dictionary<string, UnityEngine.Sprite>();

  public void CreateHash()
  {
    if (this.hash_created)
      return;
    this.hash.Clear();
    for (int index = 0; index < this.sprite_names.Count; ++index)
    {
      if (!this.hash.ContainsKey(this.sprite_names[index]))
        this.hash.Add(this.sprite_names[index], this.sprites[index]);
    }
    this.hash_created = true;
  }

  public UnityEngine.Sprite GetSprite(string spr_name)
  {
    if (!this.hash_created)
      this.CreateHash();
    if (this.hash.ContainsKey(spr_name))
      return this.hash[spr_name];
    Debug.LogError((object) $"Sprite '{spr_name}' is not found in sub-collection!");
    return (UnityEngine.Sprite) null;
  }
}
