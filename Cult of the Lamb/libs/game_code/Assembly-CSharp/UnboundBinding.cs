// Decompiled with JetBrains decompiler
// Type: UnboundBinding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired;
using System;

#nullable disable
[Serializable]
public struct UnboundBinding(int action, int category, Pole axisContribution)
{
  public int Action = action;
  public int Category = category;
  public Pole AxisContribution = axisContribution;
}
