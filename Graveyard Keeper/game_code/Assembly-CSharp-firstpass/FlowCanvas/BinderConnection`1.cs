// Decompiled with JetBrains decompiler
// Type: FlowCanvas.BinderConnection`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace FlowCanvas;

public class BinderConnection<T> : BinderConnection
{
  public override void Bind()
  {
    if (!this.isActive)
      return;
    this.DoNormalBinding(this.sourcePort, this.targetPort);
  }

  public override void UnBind()
  {
    if (!(this.targetPort is ValueInput))
      return;
    (this.targetPort as ValueInput).UnBind();
  }

  public void DoNormalBinding(Port source, Port target)
  {
    (target as ValueInput<T>).BindTo((ValueOutput) source);
  }
}
