// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeAutoSortingCanvas
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Flockade;

[RequireComponent(typeof (Canvas))]
public class FlockadeAutoSortingCanvas : MonoBehaviour
{
  public virtual void Awake()
  {
    Canvas component = this.GetComponent<Canvas>();
    Canvas componentInParent = (bool) (Object) this.transform.parent ? this.transform.parent.GetComponentInParent<Canvas>() : (Canvas) null;
    if (!(bool) (Object) componentInParent)
      return;
    component.overrideSorting = true;
    component.sortingOrder = componentInParent.sortingOrder + 1;
  }
}
