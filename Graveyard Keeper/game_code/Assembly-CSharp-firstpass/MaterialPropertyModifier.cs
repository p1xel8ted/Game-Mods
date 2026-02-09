// Decompiled with JetBrains decompiler
// Type: MaterialPropertyModifier
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class MaterialPropertyModifier : MonoBehaviour
{
  public MaterialPropertyBlock _properties;
  public MaterialPropertyBlock _tmp_properties;

  public void DoUpdateRenderer(
    Renderer r,
    MaterialPropertyModifier.PropertyModifier modify_delegate)
  {
    if (this._properties == null)
    {
      this._properties = new MaterialPropertyBlock();
      this._tmp_properties = new MaterialPropertyBlock();
    }
    if ((Object) r == (Object) null)
      return;
    this._properties.Clear();
    r.GetPropertyBlock(this._tmp_properties);
    Texture texture = this._tmp_properties.GetTexture("_MainTex");
    if ((Object) texture != (Object) null)
      this._properties.SetTexture("_MainTex", texture);
    else
      Debug.Log((object) "Main tex is null");
    if (modify_delegate != null)
      modify_delegate(this._properties);
    r.SetPropertyBlock(this._properties);
  }

  public virtual void UpdateRenderer()
  {
  }

  public virtual void OnBecameVisible() => this.UpdateRenderer();

  public delegate void PropertyModifier(MaterialPropertyBlock mat);
}
