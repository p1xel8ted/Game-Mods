// Decompiled with JetBrains decompiler
// Type: MMDebugText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Text;

#nullable disable
public class MMDebugText : MonoSingleton<MMDebugText>
{
  public static List<string> s_DebugLines = new List<string>();
  public static StringBuilder s_StringBuilder = new StringBuilder();
}
