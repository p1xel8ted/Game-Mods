// Decompiled with JetBrains decompiler
// Type: Barricade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class Barricade : BaseMonoBehaviour
{
  public static List<Barricade> barricades = new List<Barricade>();
  public bool occupied;

  private void Start() => Barricade.barricades.Add(this);

  private void OnDestroy() => Barricade.barricades.Remove(this);
}
