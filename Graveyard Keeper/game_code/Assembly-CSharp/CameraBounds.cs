// Decompiled with JetBrains decompiler
// Type: CameraBounds
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (BoxCollider2D))]
public class CameraBounds : MonoBehaviour
{
  public bool _initialized;
  [Range(0.0f, 2f)]
  public float offset = 1f;
  [Range(0.0f, 2f)]
  public float transition_time = 1f;
  public EaseType ease_type = EaseType.EaseOut;

  public void Awake()
  {
    BoxCollider2D component = this.GetComponent<BoxCollider2D>();
    component.offset = component.offset.Round(0.1f);
    component.size = component.size.Round(0.1f);
  }

  public void Update()
  {
    if (this._initialized || !GameLoader.camera_initialized)
      return;
    BoxCollider2D component = this.GetComponent<BoxCollider2D>();
    Transform transform = new GameObject(this.name).transform;
    transform.SetParent(this.transform, false);
    transform.position = component.bounds.center with
    {
      z = 0.0f
    };
    transform.localScale = component.bounds.size;
    ProCamera2DTriggerBoundaries dtriggerBoundaries = transform.gameObject.AddComponent<ProCamera2DTriggerBoundaries>();
    int num1;
    bool flag1 = (num1 = 1) != 0;
    dtriggerBoundaries.UseRightBoundary = num1 != 0;
    int num2;
    bool flag2 = (num2 = flag1 ? 1 : 0) != 0;
    dtriggerBoundaries.UseLeftBoundary = num2 != 0;
    int num3;
    bool flag3 = (num3 = flag2 ? 1 : 0) != 0;
    dtriggerBoundaries.UseBottomBoundary = num3 != 0;
    dtriggerBoundaries.UseTopBoundary = flag3;
    float num4 = this.offset * 96f;
    dtriggerBoundaries.TopBoundary = component.bounds.max.y + num4;
    dtriggerBoundaries.BottomBoundary = component.bounds.min.y - num4;
    dtriggerBoundaries.LeftBoundary = component.bounds.min.x - num4;
    dtriggerBoundaries.RightBoundary = component.bounds.max.x + num4;
    dtriggerBoundaries.TriggerShape = TriggerShape.RECTANGLE;
    dtriggerBoundaries.AreBoundariesRelative = false;
    dtriggerBoundaries.TransitionEaseType = this.ease_type;
    dtriggerBoundaries.TransitionDuration = this.transition_time;
    this._initialized = true;
  }
}
