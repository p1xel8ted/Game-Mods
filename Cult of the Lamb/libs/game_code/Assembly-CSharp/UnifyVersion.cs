// Decompiled with JetBrains decompiler
// Type: UnifyVersion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class UnifyVersion : MonoBehaviour
{
  public static Version version;

  public static Version Version => UnifyVersion.version;

  public static string GetVersionString() => UnifyVersion.version.ToString();

  static UnifyVersion()
  {
    TextAsset textAsset = (TextAsset) Resources.Load(nameof (version));
    if (!((UnityEngine.Object) textAsset != (UnityEngine.Object) null))
      return;
    UnifyVersion.version = new Version(textAsset.text);
  }
}
