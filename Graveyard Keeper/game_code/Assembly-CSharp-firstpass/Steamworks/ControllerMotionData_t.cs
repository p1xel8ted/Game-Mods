// Decompiled with JetBrains decompiler
// Type: Steamworks.ControllerMotionData_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct ControllerMotionData_t
{
  public float rotQuatX;
  public float rotQuatY;
  public float rotQuatZ;
  public float rotQuatW;
  public float posAccelX;
  public float posAccelY;
  public float posAccelZ;
  public float rotVelX;
  public float rotVelY;
  public float rotVelZ;
}
