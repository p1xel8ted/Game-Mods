// Decompiled with JetBrains decompiler
// Type: SkinChanger
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
public class SkinChanger
{
  public List<SpriteRenderer> _sprites;
  public WorldGameObject _wgo;
  public SkinPreset _skin;
  public static Dictionary<int, string> _skin_ids_cache = new Dictionary<int, string>();
  public static StringBuilder _sb = new StringBuilder();
  public static Dictionary<int, UnityEngine.Sprite> _sprite_hash = new Dictionary<int, UnityEngine.Sprite>();
  public Dictionary<int, UnityEngine.Sprite> _skinned_sprite_top_level_hash = new Dictionary<int, UnityEngine.Sprite>();
  public static Dictionary<int, bool> _valid_sprite_hash = new Dictionary<int, bool>();
  public static char[] _chars = new char[100];

  public SkinChanger(WorldGameObject wgo)
  {
    this._wgo = wgo;
    this.OnWGOChanged();
  }

  public void ApplySkin(SkinPreset skin)
  {
    this._skin = skin;
    this._skinned_sprite_top_level_hash.Clear();
  }

  public void OnWGOChanged()
  {
    this._sprites = ((IEnumerable<SpriteRenderer>) this._wgo.GetComponentsInChildren<SpriteRenderer>(true)).ToList<SpriteRenderer>();
  }

  public string ReplaceSkinId(string old_skin_id, int skin_id)
  {
    if (skin_id == -1)
      return old_skin_id;
    if (skin_id == 0)
      return "";
    string str;
    if (SkinChanger._skin_ids_cache.TryGetValue(skin_id, out str))
      return str;
    str = skin_id.ToString("0##");
    SkinChanger._skin_ids_cache.Add(skin_id, str);
    return str;
  }

  public void CustomLateUpdate()
  {
    if ((Object) this._skin == (Object) null)
      return;
    for (int index = 0; index < this._sprites.Count; ++index)
    {
      SpriteRenderer sprite1 = this._sprites[index];
      bool flag = false;
      int key = 0;
      if ((Object) sprite1.sprite != (Object) null)
      {
        key = sprite1.sprite.GetInstanceID();
        if (!SkinChanger._valid_sprite_hash.TryGetValue(key, out flag))
        {
          flag = this.IsValidSprite(sprite1);
          SkinChanger._valid_sprite_hash.Add(key, flag);
        }
      }
      if (flag)
      {
        UnityEngine.Sprite sprite2;
        if (!this._skinned_sprite_top_level_hash.TryGetValue(key, out sprite2))
        {
          string name = sprite1.sprite.name;
          char ch1 = name[4];
          char ch2 = name[5];
          char ch3 = name[6];
          int v = -1;
          if (ch1 == 'b' && ch2 == 'd' && ch3 == 'y')
            v = this._skin.body;
          else if (ch1 == 'h' && ch2 == 'e' && ch3 == 'd')
            v = this._skin.head;
          else if (ch1 == 'b' && ch2 == 'o' && ch3 == 't')
            v = this._skin.bot;
          else if (ch1 == 'm' && ch2 == 'i' && ch3 == 'd')
            v = this._skin.mid;
          else if (ch1 == 'b' && ch2 == 'k' && ch3 == 'p')
            v = this._skin.backpack;
          if (v == 0 || string.IsNullOrEmpty(name))
          {
            sprite1.enabled = false;
            sprite1.sprite = (UnityEngine.Sprite) null;
          }
          else
          {
            sprite1.enabled = true;
            if (v != -1)
            {
              GarbagelessStrings.StringToChars(ref name, ref SkinChanger._chars);
              GarbagelessStrings.IntToCharsWithLeadingZeros(v, ref SkinChanger._chars, 3, 0);
              int hashCode = GarbagelessStrings.GetHashCode(ref SkinChanger._chars);
              if (!SkinChanger._sprite_hash.TryGetValue(hashCode, out sprite2))
              {
                sprite2 = EasySpritesCollection.GetSprite(GarbagelessStrings.CharsToString(ref SkinChanger._chars));
                SkinChanger._sprite_hash.Add(hashCode, sprite2);
              }
              sprite1.sprite = sprite2;
            }
          }
          this._skinned_sprite_top_level_hash.Add(key, v == -1 ? sprite1.sprite : sprite2);
        }
        else
        {
          sprite1.sprite = sprite2;
          sprite1.enabled = (Object) sprite2 != (Object) null;
        }
      }
    }
  }

  public bool IsValidSprite(SpriteRenderer spr)
  {
    if ((Object) spr.sprite == (Object) null)
      return false;
    string name = spr.sprite.name;
    if (name.Length <= 7)
      return false;
    for (int index = 0; index < 3; ++index)
    {
      char ch = name[index];
      if (ch < '0' || ch > '9')
        return false;
    }
    return true;
  }
}
