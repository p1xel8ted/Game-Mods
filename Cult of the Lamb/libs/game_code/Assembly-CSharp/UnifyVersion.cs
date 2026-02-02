// Decompiled with JetBrains decompiler
// Type: UnifyVersion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
