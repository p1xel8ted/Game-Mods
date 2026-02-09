// Decompiled with JetBrains decompiler
// Type: GameResAtom
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class GameResAtom
{
  public string type;
  public float value;

  public static GameResAtom empty => new GameResAtom(nameof (empty), -1);

  public GameResAtom()
  {
  }

  public GameResAtom(GameResAtom source)
  {
    this.type = source.type;
    this.value = source.value;
  }

  public GameResAtom(string t, int v)
  {
    this.type = t;
    this.value = (float) v;
  }

  public GameResAtom(string t, float v)
  {
    this.type = t;
    this.value = v;
  }

  public bool IsEmpty() => this.type == "empty" || this.value.EqualsTo(0.0f);

  public bool IsNotEmpty() => !this.IsEmpty();

  public override string ToString() => $"[t={this.type}, v={this.value}]";
}
