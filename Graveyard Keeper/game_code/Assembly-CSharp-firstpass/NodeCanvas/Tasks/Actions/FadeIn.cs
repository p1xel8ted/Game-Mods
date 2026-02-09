// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.FadeIn
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("Camera")]
public class FadeIn : ActionTask
{
  public float fadeTime = 1f;

  public override void OnExecute() => CameraFader.current.FadeIn(this.fadeTime);

  public override void OnUpdate()
  {
    if ((double) this.elapsedTime < (double) this.fadeTime)
      return;
    this.EndAction();
  }
}
