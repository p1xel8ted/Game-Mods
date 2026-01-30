// Decompiled with JetBrains decompiler
// Type: UnboundBinding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
