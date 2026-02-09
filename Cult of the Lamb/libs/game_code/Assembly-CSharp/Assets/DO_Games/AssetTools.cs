// Decompiled with JetBrains decompiler
// Type: Assets.DO_Games.AssetTools
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Assets.DO_Games;

public class AssetTools
{
  public static List<string> baseTextures = new List<string>();

  public static void ListLoadedTextures()
  {
    List<Texture2D> allObjects = AssetTools.GetAllObjects<Texture2D>();
    AssetTools.baseTextures.Clear();
    string str = "name, width, height, mipCount, Readable, format\n";
    foreach (Texture2D texture2D in allObjects)
    {
      AssetTools.baseTextures.Add(texture2D.name);
      str += $"{texture2D.name} {texture2D.width} {texture2D.height} {texture2D.mipmapCount} {texture2D.isReadable} {texture2D.format.ToString()}\n";
    }
  }

  public static void DiffTextures()
  {
    List<Texture2D> allObjects = AssetTools.GetAllObjects<Texture2D>();
    string message = "Diff textures: \n";
    foreach (Texture2D texture2D in allObjects)
    {
      if (!AssetTools.baseTextures.Contains(texture2D.name))
        message = $"{message}{texture2D.name}\n";
    }
    Debug.Log((object) message);
  }

  public static List<T> GetAllObjects<T>()
  {
    List<T> allObjects = new List<T>();
    foreach (T obj in Resources.FindObjectsOfTypeAll((System.Type) typeof (T)) as T[])
      allObjects.Add(obj);
    return allObjects;
  }
}
