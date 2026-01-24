// Decompiled with JetBrains decompiler
// Type: FMODOneShotWrapper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;

#nullable disable
public class FMODOneShotWrapper
{
  public EventInstance Instance;
  public bool Released;

  public FMODOneShotWrapper(EventInstance instance) => this.Instance = instance;

  public void Stop(STOP_MODE stopMode)
  {
    if (this.Released || !this.Instance.isValid())
      return;
    int num = (int) this.Instance.stop(stopMode);
    this.Cleanup();
  }

  public void Cleanup()
  {
    if (this.Released)
      return;
    if (this.Instance.isValid())
    {
      int num1 = (int) this.Instance.setCallback((EVENT_CALLBACK) null);
      int num2 = (int) this.Instance.release();
    }
    this.Released = true;
  }
}
