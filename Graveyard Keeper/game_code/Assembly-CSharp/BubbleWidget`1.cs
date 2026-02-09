// Decompiled with JetBrains decompiler
// Type: BubbleWidget`1
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (PixelPerfectGUI))]
public abstract class BubbleWidget<T> : BubbleWidgetBase where T : BubbleWidgetData
{
  public T data;
  [HideInInspector]
  [SerializeField]
  public UIWidget ui_widget;
  [HideInInspector]
  [SerializeField]
  public bool initialized;

  public abstract void Draw(T data);

  public override void Init()
  {
    this.ui_widget = this.GetComponent<UIWidget>();
    this.initialized = true;
  }

  public override System.Type GetWidgetType() => typeof (T);

  public override void BaseDraw(BubbleWidgetData data) => this.Draw(data as T);

  public override Vector2 GetSize()
  {
    if (!this.initialized || (UnityEngine.Object) this.ui_widget == (UnityEngine.Object) null)
      this.Init();
    return this.ui_widget.localSize;
  }
}
