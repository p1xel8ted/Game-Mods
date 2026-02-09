// Decompiled with JetBrains decompiler
// Type: TreeDisappearAnimation
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class TreeDisappearAnimation : DisappearAnimation
{
  public GJCommons.VoidDelegate _on_complete;
  public List<FallingPart> _parts = new List<FallingPart>();

  public override void StartAnimation(GJCommons.VoidDelegate on_complete)
  {
    this._parts.Clear();
    this._on_complete = on_complete;
    foreach (FallingPart componentsInChild in this.GetComponentsInChildren<FallingPart>())
    {
      FallingPart p1 = componentsInChild;
      this._parts.Add(p1);
      componentsInChild.StartAnimation((GJCommons.VoidDelegate) (() => this._parts.Remove(p1)));
    }
    DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransform("tree_fall", this.transform);
  }

  public void Update()
  {
    if (this._on_complete == null || this._parts.Count != 0)
      return;
    GJCommons.VoidDelegate onComplete = this._on_complete;
    this._on_complete = (GJCommons.VoidDelegate) null;
    if (onComplete == null)
      return;
    onComplete();
  }
}
