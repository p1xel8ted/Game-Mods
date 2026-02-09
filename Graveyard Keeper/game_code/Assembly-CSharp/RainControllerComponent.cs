// Decompiled with JetBrains decompiler
// Type: RainControllerComponent
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

#nullable disable
public class RainControllerComponent : AbstractControllerComponent
{
  public CameraFilterPack_Atmosphere_Rain_Pro rain;

  public override void Set(float a)
  {
    this.rain.enabled = (double) a > 0.0;
    this.rain.Fade = a;
  }
}
