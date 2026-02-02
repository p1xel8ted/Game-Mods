// Decompiled with JetBrains decompiler
// Type: Barricade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
