// Decompiled with JetBrains decompiler
// Type: TrailTypeDefinition
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DarkTonic.MasterAudio;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[CreateAssetMenu]
public class TrailTypeDefinition : ScriptableObject
{
  public Ground.GroudType type;
  public Color color = Color.white;
  public List<UnityEngine.Sprite> hor_b;
  public List<UnityEngine.Sprite> hor_t;
  public List<UnityEngine.Sprite> vert_l;
  public List<UnityEngine.Sprite> vert_r;
  public List<UnityEngine.Sprite> diag_lt;
  public List<UnityEngine.Sprite> diag_rb;
  public bool custom_trail_dist;
  public float leave_trail_dist = 370f;
  public bool custom_trail_decrease;
  public float trail_decrease = 0.9f;
  [SoundGroup]
  public string sound = "[None]";

  public UnityEngine.Sprite GetByDirection(Vector2 dir, bool is_left_foot, out bool flip)
  {
    List<UnityEngine.Sprite> spriteList = this.hor_b;
    double num = (double) Mathf.Atan2(dir.y, dir.x) * 57.295780181884766;
    flip = false;
    switch (Mathf.RoundToInt((float) (num / 45.0)))
    {
      case -4:
      case 4:
        spriteList = !is_left_foot ? this.hor_b : this.hor_t;
        break;
      case -3:
        spriteList = is_left_foot ? this.diag_lt : this.diag_rb;
        break;
      case -2:
        spriteList = is_left_foot ? this.vert_l : this.vert_r;
        break;
      case -1:
        flip = true;
        spriteList = is_left_foot ? this.diag_rb : this.diag_lt;
        break;
      case 0:
        spriteList = is_left_foot ? this.hor_b : this.hor_t;
        break;
      case 1:
        spriteList = !is_left_foot ? this.diag_lt : this.diag_rb;
        break;
      case 2:
        spriteList = !is_left_foot ? this.vert_l : this.vert_r;
        break;
      case 3:
        flip = true;
        spriteList = !is_left_foot ? this.diag_rb : this.diag_lt;
        break;
    }
    return spriteList.Count == 0 ? (UnityEngine.Sprite) null : spriteList[NGUITools.RandomRange(0, spriteList.Count - 1)];
  }
}
