// Decompiled with JetBrains decompiler
// Type: AnimatedColor
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (UIWidget))]
[ExecuteInEditMode]
public class AnimatedColor : MonoBehaviour
{
  public Color color = Color.white;
  public UIWidget mWidget;

  public void OnEnable()
  {
    this.mWidget = this.GetComponent<UIWidget>();
    this.LateUpdate();
  }

  public void LateUpdate() => this.mWidget.color = this.color;
}
