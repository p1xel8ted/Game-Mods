// Decompiled with JetBrains decompiler
// Type: AnimatedWidget
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class AnimatedWidget : MonoBehaviour
{
  public float width = 1f;
  public float height = 1f;
  public UIWidget mWidget;

  public void OnEnable()
  {
    this.mWidget = this.GetComponent<UIWidget>();
    this.LateUpdate();
  }

  public void LateUpdate()
  {
    if (!((Object) this.mWidget != (Object) null))
      return;
    this.mWidget.width = Mathf.RoundToInt(this.width);
    this.mWidget.height = Mathf.RoundToInt(this.height);
  }
}
