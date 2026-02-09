// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.ImageEffects.ImageEffectBase
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace UnityStandardAssets.ImageEffects;

[RequireComponent(typeof (Camera))]
[AddComponentMenu("")]
public class ImageEffectBase : MonoBehaviour
{
  public Shader shader;
  public Material m_Material;

  public virtual void Start()
  {
    if (!SystemInfo.supportsImageEffects)
    {
      this.enabled = false;
    }
    else
    {
      if ((bool) (Object) this.shader && this.shader.isSupported)
        return;
      this.enabled = false;
    }
  }

  public Material material
  {
    get
    {
      if ((Object) this.m_Material == (Object) null)
      {
        this.m_Material = new Material(this.shader);
        this.m_Material.hideFlags = HideFlags.HideAndDontSave;
      }
      return this.m_Material;
    }
  }

  public virtual void OnDisable()
  {
    if (!(bool) (Object) this.m_Material)
      return;
    Object.DestroyImmediate((Object) this.m_Material);
  }
}
