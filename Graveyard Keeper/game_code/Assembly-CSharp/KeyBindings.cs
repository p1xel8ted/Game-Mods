// Decompiled with JetBrains decompiler
// Type: KeyBindings
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class KeyBindings
{
  public static Dictionary<GameKey, GamePadButton> gamepad_bindings = new Dictionary<GameKey, GamePadButton>()
  {
    {
      GameKey.Attack,
      GamePadButton.X
    },
    {
      GameKey.Dash,
      GamePadButton.LB
    },
    {
      GameKey.Dash2,
      GamePadButton.RB
    },
    {
      GameKey.Interaction,
      GamePadButton.A
    },
    {
      GameKey.Work,
      GamePadButton.Y
    },
    {
      GameKey.Action,
      GamePadButton.X
    },
    {
      GameKey.GameGUI,
      GamePadButton.Back
    },
    {
      GameKey.IngameMenu,
      GamePadButton.Start
    },
    {
      GameKey.Toolbar1,
      GamePadButton.DUp
    },
    {
      GameKey.Toolbar2,
      GamePadButton.DRight
    },
    {
      GameKey.Toolbar3,
      GamePadButton.DDown
    },
    {
      GameKey.Toolbar4,
      GamePadButton.DLeft
    },
    {
      GameKey.Left,
      GamePadButton.DLeft
    },
    {
      GameKey.Right,
      GamePadButton.DRight
    },
    {
      GameKey.Up,
      GamePadButton.DUp
    },
    {
      GameKey.Down,
      GamePadButton.DDown
    },
    {
      GameKey.Select,
      GamePadButton.A
    },
    {
      GameKey.Back,
      GamePadButton.B
    },
    {
      GameKey.Option1,
      GamePadButton.X
    },
    {
      GameKey.Option2,
      GamePadButton.Y
    },
    {
      GameKey.SliderDec,
      GamePadButton.DLeft
    },
    {
      GameKey.SliderInc,
      GamePadButton.DRight
    },
    {
      GameKey.PrevTab,
      GamePadButton.LB
    },
    {
      GameKey.NextTab,
      GamePadButton.RB
    },
    {
      GameKey.PrevSubTab,
      GamePadButton.LT
    },
    {
      GameKey.NextSubTub,
      GamePadButton.RT
    },
    {
      GameKey.RotateLeft,
      GamePadButton.LB
    },
    {
      GameKey.RotateRight,
      GamePadButton.RB
    },
    {
      GameKey.ShowWGOQualities,
      GamePadButton.LT
    },
    {
      GameKey.MiniGameAction,
      GamePadButton.A
    }
  };
  public static Dictionary<GameKey, KeyCode[]> default_bindings = (Dictionary<GameKey, KeyCode[]>) null;
  public static Dictionary<GameKey, KeyCode[]> keyboard_bindings = new Dictionary<GameKey, KeyCode[]>()
  {
    {
      GameKey.Attack,
      new KeyCode[1]{ KeyCode.Space }
    },
    {
      GameKey.Dash,
      new KeyCode[1]{ KeyCode.LeftShift }
    },
    {
      GameKey.Dash2,
      new KeyCode[1]{ KeyCode.RightShift }
    },
    {
      GameKey.Interaction,
      new KeyCode[1]{ KeyCode.E }
    },
    {
      GameKey.Work,
      new KeyCode[1]{ KeyCode.F }
    },
    {
      GameKey.Action,
      new KeyCode[1]{ KeyCode.X }
    },
    {
      GameKey.GameGUI,
      new KeyCode[1]{ KeyCode.Tab }
    },
    {
      GameKey.IngameMenu,
      new KeyCode[1]{ KeyCode.Escape }
    },
    {
      GameKey.Toolbar1,
      new KeyCode[1]{ KeyCode.Alpha1 }
    },
    {
      GameKey.Toolbar2,
      new KeyCode[1]{ KeyCode.Alpha2 }
    },
    {
      GameKey.Toolbar3,
      new KeyCode[1]{ KeyCode.Alpha3 }
    },
    {
      GameKey.Toolbar4,
      new KeyCode[1]{ KeyCode.Alpha4 }
    },
    {
      GameKey.Left,
      new KeyCode[2]{ KeyCode.A, KeyCode.LeftArrow }
    },
    {
      GameKey.Right,
      new KeyCode[2]{ KeyCode.D, KeyCode.RightArrow }
    },
    {
      GameKey.Up,
      new KeyCode[2]{ KeyCode.W, KeyCode.UpArrow }
    },
    {
      GameKey.Down,
      new KeyCode[2]{ KeyCode.S, KeyCode.DownArrow }
    },
    {
      GameKey.Back,
      new KeyCode[1]{ KeyCode.Escape }
    },
    {
      GameKey.SliderDec,
      new KeyCode[2]{ KeyCode.A, KeyCode.LeftArrow }
    },
    {
      GameKey.SliderInc,
      new KeyCode[2]{ KeyCode.D, KeyCode.RightArrow }
    },
    {
      GameKey.RotateRight,
      new KeyCode[1]{ KeyCode.R }
    },
    {
      GameKey.ShowWGOQualities,
      new KeyCode[1]{ KeyCode.LeftControl }
    },
    {
      GameKey.MiniGameAction,
      new KeyCode[1]{ KeyCode.E }
    },
    {
      GameKey.Inventory,
      new KeyCode[1]{ KeyCode.I }
    },
    {
      GameKey.Techs,
      new KeyCode[1]{ KeyCode.T }
    },
    {
      GameKey.Map,
      new KeyCode[1]{ KeyCode.M }
    },
    {
      GameKey.KnownNPCs,
      new KeyCode[1]{ KeyCode.N }
    }
  };
  public static Dictionary<GameKey, GameKey[]> mouse_bindings = new Dictionary<GameKey, GameKey[]>()
  {
    {
      GameKey.LeftClick,
      new GameKey[1]{ GameKey.MiniGameAction }
    },
    {
      GameKey.RightClick,
      new GameKey[1]{ GameKey.Interaction }
    }
  };

  static KeyBindings()
  {
    Debug.Log((object) "Save default key bindings");
    KeyBindings.default_bindings = new Dictionary<GameKey, KeyCode[]>();
    foreach (KeyValuePair<GameKey, KeyCode[]> keyboardBinding in KeyBindings.keyboard_bindings)
      KeyBindings.default_bindings.Add(keyboardBinding.Key, keyboardBinding.Value);
  }

  public static void Reset()
  {
    Debug.Log((object) "Reset key bindings");
    foreach (KeyValuePair<GameKey, KeyCode[]> defaultBinding in KeyBindings.default_bindings)
      KeyBindings.keyboard_bindings[defaultBinding.Key] = defaultBinding.Value;
  }

  public static string ToJSON()
  {
    KeyBindings.SerializableBindings serializableBindings = new KeyBindings.SerializableBindings();
    foreach (KeyValuePair<GameKey, KeyCode[]> keyboardBinding in KeyBindings.keyboard_bindings)
    {
      serializableBindings.keys.Add(keyboardBinding.Key);
      KeyBindings.SerializableBindings.KeysList keysList = new KeyBindings.SerializableBindings.KeysList()
      {
        keys = new int[keyboardBinding.Value.Length]
      };
      for (int index = 0; index < keyboardBinding.Value.Length; ++index)
        keysList.keys[index] = (int) keyboardBinding.Value[index];
      serializableBindings.values.Add(keysList);
    }
    return JsonUtility.ToJson((object) serializableBindings);
  }

  public static void FromJSON(string json)
  {
    if (string.IsNullOrEmpty(json))
      return;
    KeyBindings.SerializableBindings serializableBindings;
    try
    {
      serializableBindings = JsonUtility.FromJson<KeyBindings.SerializableBindings>(json);
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ("Error reading key binding: " + ex?.ToString()));
      return;
    }
    if (serializableBindings == null)
      return;
    for (int index1 = 0; index1 < serializableBindings.keys.Count; ++index1)
    {
      GameKey key = serializableBindings.keys[index1];
      KeyBindings.SerializableBindings.KeysList keysList = serializableBindings.values[index1];
      KeyCode[] keyCodeArray = new KeyCode[keysList.keys.Length];
      for (int index2 = 0; index2 < keysList.keys.Length; ++index2)
        keyCodeArray[index2] = (KeyCode) keysList.keys[index2];
      KeyBindings.keyboard_bindings[key] = keyCodeArray;
    }
    Debug.Log((object) $"Deserialize keyboard binding: {serializableBindings.keys.Count}");
  }

  public static void RedefineKey(GameKey gk, KeyCode key)
  {
    foreach (GameKey key1 in KeyBindings.keyboard_bindings.Keys)
    {
      if (key1 == gk)
      {
        KeyBindings.keyboard_bindings[gk] = new KeyCode[1]
        {
          key
        };
        return;
      }
    }
    KeyBindings.keyboard_bindings[GameKey.MiniGameAction] = KeyBindings.keyboard_bindings[GameKey.Interaction];
    Debug.LogError((object) ("Couldn't find a GameKey to redefine: " + gk.ToString()));
  }

  [Serializable]
  public class SerializableBindings
  {
    public List<GameKey> keys = new List<GameKey>();
    public List<KeyBindings.SerializableBindings.KeysList> values = new List<KeyBindings.SerializableBindings.KeysList>();

    [Serializable]
    public class KeysList
    {
      public int[] keys;
    }
  }
}
