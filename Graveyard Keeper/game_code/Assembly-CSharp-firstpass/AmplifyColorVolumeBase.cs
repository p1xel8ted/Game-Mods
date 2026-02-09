// Decompiled with JetBrains decompiler
// Type: AmplifyColorVolumeBase
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using AmplifyColor;
using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[AddComponentMenu("")]
public class AmplifyColorVolumeBase : MonoBehaviour
{
  public Texture2D LutTexture;
  public float Exposure = 1f;
  public float EnterBlendTime = 1f;
  public int Priority;
  public bool ShowInSceneView = true;
  [HideInInspector]
  public VolumeEffectContainer EffectContainer = new VolumeEffectContainer();

  public void OnDrawGizmos()
  {
    if (!this.ShowInSceneView)
      return;
    BoxCollider component1 = this.GetComponent<BoxCollider>();
    BoxCollider2D component2 = this.GetComponent<BoxCollider2D>();
    if (!((Object) component1 != (Object) null) && !((Object) component2 != (Object) null))
      return;
    Vector3 center;
    Vector3 size;
    if ((Object) component1 != (Object) null)
    {
      center = component1.center;
      size = component1.size;
    }
    else
    {
      center = (Vector3) component2.offset;
      size = (Vector3) component2.size;
    }
    Gizmos.color = Color.green;
    Gizmos.matrix = this.transform.localToWorldMatrix;
    Gizmos.DrawWireCube(center, size);
  }

  public void OnDrawGizmosSelected()
  {
    BoxCollider component1 = this.GetComponent<BoxCollider>();
    BoxCollider2D component2 = this.GetComponent<BoxCollider2D>();
    if (!((Object) component1 != (Object) null) && !((Object) component2 != (Object) null))
      return;
    Gizmos.color = Color.green with { a = 0.2f };
    Gizmos.matrix = this.transform.localToWorldMatrix;
    Vector3 center;
    Vector3 size;
    if ((Object) component1 != (Object) null)
    {
      center = component1.center;
      size = component1.size;
    }
    else
    {
      center = (Vector3) component2.offset;
      size = (Vector3) component2.size;
    }
    Gizmos.DrawCube(center, size);
  }
}
