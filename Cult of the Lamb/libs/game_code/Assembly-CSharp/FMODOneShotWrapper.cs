// Decompiled with JetBrains decompiler
// Type: FMODOneShotWrapper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
