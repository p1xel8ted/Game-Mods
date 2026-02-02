// Decompiled with JetBrains decompiler
// Type: MMDebugText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Text;

#nullable disable
public class MMDebugText : MonoSingleton<MMDebugText>
{
  public static List<string> s_DebugLines = new List<string>();
  public static StringBuilder s_StringBuilder = new StringBuilder();
}
