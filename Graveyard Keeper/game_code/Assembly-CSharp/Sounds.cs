// Decompiled with JetBrains decompiler
// Type: Sounds
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DarkTonic.MasterAudio;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[DefaultExecutionOrder(-3000)]
public class Sounds : MonoBehaviour
{
  public static HashSet<string> _played_sounds = new HashSet<string>();
  public static bool ignore_window_sounds = false;

  public static void Init() => SingletonGameObjects.FindOrCreate<Sounds>();

  public void Update() => Sounds._played_sounds.Clear();

  public static bool WasAnySoundPlayedThisFrame() => Sounds._played_sounds.Count > 0;

  public static float CalcSoundVolume(Vector2? pos, float custom_distance = 0.0f)
  {
    float num1 = 1f;
    Vector2 position = (Vector2) MainGame.me.world_cam.transform.position;
    float num2 = (!pos.HasValue ? 0.0f : (position - pos.Value).magnitude / 96f) - 3.5f;
    if ((double) num2 > 0.0)
      num1 = (float) (1.0 - (double) num2 / (6.0 + (double) custom_distance));
    return Mathf.Clamp01(num1);
  }

  public static PlaySoundResult PlaySound(
    string snd,
    Vector2? pos = null,
    bool force_play = false,
    float custom_distance = 0.0f)
  {
    if (Sounds._played_sounds.Contains(snd) || GUIElements.gui_is_initializing)
      return (PlaySoundResult) null;
    float volumePercentage = Sounds.CalcSoundVolume(pos, custom_distance);
    PlaySoundResult playSoundResult = (PlaySoundResult) null;
    if ((double) volumePercentage > 0.01 | force_play)
    {
      Sounds._played_sounds.Add(snd);
      playSoundResult = DarkTonic.MasterAudio.MasterAudio.PlaySound(snd, volumePercentage);
    }
    return playSoundResult;
  }

  public static void OnGUIClick() => Sounds.PlaySound("gui_click");

  public static void OnGUITabClick() => Sounds.PlaySound("tab_click");

  public static void OnGUIHover(Sounds.ElementType element_type = Sounds.ElementType.Unknown)
  {
    if (element_type == Sounds.ElementType.ItemCell)
      Sounds.PlaySound("gui_hover_light");
    else
      Sounds.PlaySound("gui_hover");
  }

  public static void OnClosePressed()
  {
    if (Sounds.ignore_window_sounds)
      return;
    Sounds.PlaySound("win_close");
  }

  public static void OnWindowOpened()
  {
    if (Sounds.ignore_window_sounds)
      return;
    Sounds.PlaySound("win_open");
  }

  public static void OnToolEquip(bool equip)
  {
    Sounds.PlaySound(equip ? "equip_tool" : "unequip_tool");
  }

  public enum ElementType
  {
    Unknown,
    Button,
    ItemCell,
  }
}
