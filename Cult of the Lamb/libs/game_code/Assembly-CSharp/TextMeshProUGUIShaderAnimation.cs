// Decompiled with JetBrains decompiler
// Type: TextMeshProUGUIShaderAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class TextMeshProUGUIShaderAnimation : BaseMonoBehaviour
{
  public TextMeshProUGUI _tmPro;
  public Material _material;
  public float _lastFaceDilate;
  public float _faceDilate;

  public void Start()
  {
    this._lastFaceDilate = -2f;
    this._tmPro = this.GetComponent<TextMeshProUGUI>();
    this._material = new Material(this._tmPro.fontSharedMaterial);
    this._tmPro.fontMaterial = this._material;
    this._material.SetFloat(ShaderUtilities.ID_FaceDilate, -1f);
  }

  public void OnEnable()
  {
    this._tmPro = this.GetComponent<TextMeshProUGUI>();
    if ((Object) this._tmPro.fontMaterial != (Object) this._material)
      this._tmPro.fontMaterial = this._material;
    this._lastFaceDilate = -1f;
    if ((Object) this._material == (Object) null)
      this._material = new Material(this._tmPro.fontSharedMaterial);
    this._material.SetFloat(ShaderUtilities.ID_FaceDilate, -1f);
    this._tmPro.ForceMeshUpdate(false, false);
  }

  public void Update()
  {
    if ((Object) this._tmPro.fontMaterial != (Object) this._material)
      this._tmPro.fontMaterial = this._material;
    if ((Object) this._material == (Object) null)
    {
      this._material = new Material(this._tmPro.fontSharedMaterial);
      this._tmPro.ForceMeshUpdate(false, false);
      this._tmPro.fontMaterial = this._material;
    }
    if ((Object) this._material == (Object) null || !this._material.HasProperty(ShaderUtilities.ID_FaceDilate) || (double) this._faceDilate == (double) this._lastFaceDilate)
      return;
    if ((Object) this._tmPro.fontMaterial != (Object) this._material)
      this._tmPro.fontMaterial = this._material;
    this._lastFaceDilate = this._faceDilate;
    this._material.SetFloat(ShaderUtilities.ID_FaceDilate, this._faceDilate);
  }
}
