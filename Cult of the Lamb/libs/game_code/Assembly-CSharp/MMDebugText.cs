// Decompiled with JetBrains decompiler
// Type: MMDebugText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Text;

#nullable disable
public class MMDebugText : MonoSingleton<MMDebugText>
{
  public static List<string> s_DebugLines = new List<string>();
  public static StringBuilder s_StringBuilder = new StringBuilder();
}
