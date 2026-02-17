// Decompiled with JetBrains decompiler
// Type: RedBlueGames.Tools.TextTyper.TextTagParser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

#nullable disable
namespace RedBlueGames.Tools.TextTyper;

public sealed class TextTagParser
{
  public static string[] UnityTagTypes = new string[5]
  {
    "b",
    "i",
    "size",
    "color",
    "style"
  };
  public static string[] CustomTagTypes = new string[3]
  {
    "delay",
    "anim",
    "animation"
  };

  public static List<TextTagParser.TextSymbol> CreateSymbolListFromText(string text)
  {
    List<TextTagParser.TextSymbol> symbolListFromText = new List<TextTagParser.TextSymbol>();
    int startIndex = 0;
    while (startIndex < text.Length)
    {
      string text1 = text.Substring(startIndex, text.Length - startIndex);
      TextTagParser.TextSymbol textSymbol = !RichTextTag.StringStartsWithTag(text1) ? new TextTagParser.TextSymbol(text1.Substring(0, 1)) : new TextTagParser.TextSymbol(RichTextTag.ParseNext(text1));
      startIndex += textSymbol.Length;
      symbolListFromText.Add(textSymbol);
    }
    return symbolListFromText;
  }

  public static string RemoveAllTags(string textWithTags)
  {
    return TextTagParser.RemoveCustomTags(TextTagParser.RemoveUnityTags(textWithTags));
  }

  public static string RemoveCustomTags(string textWithTags)
  {
    return TextTagParser.RemoveTags(textWithTags, TextTagParser.CustomTagTypes);
  }

  public static string RemoveUnityTags(string textWithTags)
  {
    return TextTagParser.RemoveTags(textWithTags, TextTagParser.UnityTagTypes);
  }

  public static string RemoveTags(string textWithTags, params string[] tags)
  {
    string text = textWithTags;
    foreach (string tag in tags)
      text = RichTextTag.RemoveTagsFromString(text, tag);
    return text;
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct CustomTags
  {
    public const string Delay = "delay";
    public const string Anim = "anim";
    public const string Animation = "animation";
  }

  public class TextSymbol
  {
    [CompilerGenerated]
    public char \u003CCharacter\u003Ek__BackingField;
    [CompilerGenerated]
    public RichTextTag \u003CTag\u003Ek__BackingField;

    public TextSymbol(string character) => this.Character = character[0];

    public TextSymbol(RichTextTag tag) => this.Tag = tag;

    public char Character
    {
      get => this.\u003CCharacter\u003Ek__BackingField;
      set => this.\u003CCharacter\u003Ek__BackingField = value;
    }

    public RichTextTag Tag
    {
      get => this.\u003CTag\u003Ek__BackingField;
      set => this.\u003CTag\u003Ek__BackingField = value;
    }

    public int Length => this.Text.Length;

    public string Text => this.IsTag ? this.Tag.TagText : this.Character.ToString();

    public bool IsTag => this.Tag != null;

    public float GetFloatParameter(float defaultValue = 0.0f)
    {
      if (!this.IsTag)
      {
        Debug.LogWarning((object) "Attempted to retrieve parameter from symbol that is not a tag.");
        return defaultValue;
      }
      float result;
      if (!float.TryParse(this.Tag.Parameter, out result))
      {
        Debug.LogWarning((object) $"Found Invalid parameter format in tag [{this.Tag}]. Parameter [{this.Tag.Parameter}] does not parse to a float.");
        result = defaultValue;
      }
      return result;
    }
  }
}
