// Decompiled with JetBrains decompiler
// Type: BubbleWidgetOptionsItem
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BubbleWidgetOptionsItem : MonoBehaviour
{
  [HideInInspector]
  [SerializeField]
  public UIWidget ui_widget;
  [SerializeField]
  [HideInInspector]
  public UIButton button;
  [SerializeField]
  [HideInInspector]
  public UILabel label;
  public Color disable_outline_color;
  public System.Action callback;

  public void Init()
  {
    this.ui_widget = this.GetComponent<UIWidget>();
    this.label = this.GetComponent<UILabel>();
    this.button = this.GetComponent<UIButton>();
  }

  public void Draw(string name, System.Action callback, bool enabled = true)
  {
    this.name = name;
    this.label.text = GJL.L(name);
    this.callback = callback;
    if (!enabled)
      this.label.color = this.button.disabledColor;
    this.button.isEnabled = enabled;
    this.label.effectColor = enabled ? Color.black : this.disable_outline_color;
  }

  public void OnItemSelect() => this.callback.TryInvoke();

  public void OnItemOver()
  {
    if (!this.button.isEnabled)
      return;
    Sounds.OnGUIHover(Sounds.ElementType.ItemCell);
  }

  public Vector2 GetSize()
  {
    if ((UnityEngine.Object) this.ui_widget == (UnityEngine.Object) null || (UnityEngine.Object) this.label == (UnityEngine.Object) null)
      this.Init();
    return this.ui_widget.localSize;
  }
}
