// Decompiled with JetBrains decompiler
// Type: Barricade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class Barricade : BaseMonoBehaviour
{
  public static List<Barricade> barricades = new List<Barricade>();
  public bool occupied;

  public void Start() => Barricade.barricades.Add(this);

  public void OnDestroy() => Barricade.barricades.Remove(this);
}
