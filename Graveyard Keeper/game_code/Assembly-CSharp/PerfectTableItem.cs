// Decompiled with JetBrains decompiler
// Type: PerfectTableItem
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class PerfectTableItem : MonoBehaviour
{
  public Transform _tf;

  public Transform tf
  {
    get
    {
      if ((Object) this._tf == (Object) null)
        this._tf = this.transform;
      return this._tf;
    }
  }

  public void Update()
  {
    Vector2 localPosition = (Vector2) this.tf.localPosition;
    localPosition.x = (float) Mathf.RoundToInt(localPosition.x * 10f) / 10f;
    localPosition.y = (float) Mathf.RoundToInt(localPosition.y * 10f) / 10f;
    this.tf.localPosition = (Vector3) localPosition;
    this.tf.localScale = Vector3.one;
  }
}
