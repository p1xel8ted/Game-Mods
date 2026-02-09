// Decompiled with JetBrains decompiler
// Type: AnimatorStateOverriderAtom
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class AnimatorStateOverriderAtom
{
  public AnimatorStateOverriderAtom.AnimatorStates animator_source_state_type;
  public string source_state_name = "";
  public string destination_state_name = "";

  public enum AnimatorStates
  {
    FLOAT,
    INT,
    BOOL,
  }
}
