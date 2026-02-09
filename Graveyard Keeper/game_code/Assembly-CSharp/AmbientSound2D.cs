// Decompiled with JetBrains decompiler
// Type: AmbientSound2D
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DarkTonic.MasterAudio;

#nullable disable
public class AmbientSound2D : AmbientSound
{
  public new void OnEnable()
  {
    this.transform.position = this.transform.position with
    {
      z = MainGame.camera_z
    };
    base.OnEnable();
  }
}
