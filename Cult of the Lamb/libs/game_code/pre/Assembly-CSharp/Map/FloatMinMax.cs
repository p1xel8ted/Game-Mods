// Decompiled with JetBrains decompiler
// Type: Map.FloatMinMax
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Map;

[Serializable]
public class FloatMinMax
{
  public float min;
  public float max;

  public float GetValue() => UnityEngine.Random.Range(this.min, this.max);
}
