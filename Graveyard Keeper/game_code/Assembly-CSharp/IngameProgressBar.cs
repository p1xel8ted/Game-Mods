// Decompiled with JetBrains decompiler
// Type: IngameProgressBar
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class IngameProgressBar : MonoBehaviour
{
  public GameObject bar_object;
  public IngameProgressBar.ScaleType scale_type;
  public float min_scale;
  public float max_scale;
  public string game_res;

  public void UpdateBar()
  {
    WorldObjectPart component = this.GetComponent<WorldObjectPart>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    WorldGameObject parent = component.parent;
    if ((UnityEngine.Object) parent == (UnityEngine.Object) null)
      return;
    float num = this.min_scale + (this.max_scale - this.min_scale) * parent.GetParam(this.game_res);
    Vector3 localScale = this.bar_object.transform.localScale;
    switch (this.scale_type)
    {
      case IngameProgressBar.ScaleType.Vertical:
        this.bar_object.transform.localScale = new Vector3(localScale.x, num, localScale.z);
        break;
      case IngameProgressBar.ScaleType.Horizontal:
        this.bar_object.transform.localScale = new Vector3(num, localScale.y, localScale.z);
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  public enum ScaleType
  {
    Vertical,
    Horizontal,
  }
}
