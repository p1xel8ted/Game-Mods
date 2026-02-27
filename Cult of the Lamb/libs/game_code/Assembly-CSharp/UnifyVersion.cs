// Decompiled with JetBrains decompiler
// Type: UnifyVersion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class UnifyVersion : MonoBehaviour
{
  public static Version version;
  public static string Timestamp;
  public static string Branch;
  public static string Githash;

  public static Version Version => UnifyVersion.version;

  public static string GetVersionString() => UnifyVersion.version.ToString();

  static UnifyVersion()
  {
    TextAsset textAsset1 = (TextAsset) Resources.Load(nameof (version));
    if ((UnityEngine.Object) textAsset1 != (UnityEngine.Object) null)
      UnifyVersion.version = new Version(textAsset1.text);
    TextAsset textAsset2 = (TextAsset) Resources.Load("build");
    if (!((UnityEngine.Object) textAsset2 != (UnityEngine.Object) null))
      return;
    try
    {
      string[] strArray = textAsset2.text.Split(':', StringSplitOptions.None);
      UnifyVersion.Timestamp = strArray[0];
      UnifyVersion.Branch = strArray[1];
      UnifyVersion.Githash = strArray[2];
    }
    catch (Exception ex)
    {
    }
  }
}
