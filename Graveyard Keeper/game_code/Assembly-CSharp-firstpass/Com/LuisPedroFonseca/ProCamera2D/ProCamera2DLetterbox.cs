// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.ProCamera2DLetterbox
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections;
using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[ExecuteInEditMode]
public class ProCamera2DLetterbox : MonoBehaviour
{
  [Range(0.0f, 0.5f)]
  public float Amount;
  public Color Color;
  public Material _material;

  public Material material
  {
    get
    {
      if ((Object) this._material == (Object) null)
      {
        this._material = new Material(Shader.Find("Hidden/ProCamera2D/Letterbox"));
        this._material.hideFlags = HideFlags.HideAndDontSave;
      }
      return this._material;
    }
  }

  public void Start()
  {
    if (SystemInfo.supportsImageEffects)
      return;
    this.enabled = false;
  }

  public void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
  {
    if (Mathf.Approximately(this.Amount, 0.0f) || (Object) this.material == (Object) null)
    {
      Graphics.Blit((Texture) sourceTexture, destTexture);
    }
    else
    {
      this.Amount = Mathf.Clamp01(this.Amount);
      this.material.SetFloat("_Top", 1f - this.Amount);
      this.material.SetFloat("_Bottom", this.Amount);
      this.material.SetColor("_Color", this.Color);
      Graphics.Blit((Texture) sourceTexture, destTexture, this.material);
    }
  }

  public void OnDisable()
  {
    if (!(bool) (Object) this._material)
      return;
    Object.DestroyImmediate((Object) this._material);
  }

  public void TweenTo(float targetAmount, float duration)
  {
    this.StopAllCoroutines();
    this.StartCoroutine(this.TweenToRoutine(targetAmount, duration));
  }

  public IEnumerator TweenToRoutine(float targetAmount, float duration)
  {
    float initialAmount = this.Amount;
    float t = 0.0f;
    while ((double) t <= 1.0)
    {
      t += Time.deltaTime / duration;
      this.Amount = Utils.EaseFromTo(initialAmount, targetAmount, t, EaseType.EaseOut);
      yield return (object) null;
    }
    this.Amount = targetAmount;
    yield return (object) null;
  }
}
