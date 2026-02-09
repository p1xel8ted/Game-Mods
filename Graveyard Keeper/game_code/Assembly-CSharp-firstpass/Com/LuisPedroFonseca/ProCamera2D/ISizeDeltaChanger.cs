// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.ISizeDeltaChanger
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

public interface ISizeDeltaChanger
{
  float AdjustSize(float deltaTime, float originalDelta);

  int SDCOrder { get; set; }
}
