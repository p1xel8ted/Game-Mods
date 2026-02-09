// Decompiled with JetBrains decompiler
// Type: SetSpriteshapeMaterial
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

#nullable disable
public class SetSpriteshapeMaterial : BaseMonoBehaviour
{
  public List<SpriteShapeMaterial> SpriteShapes = new List<SpriteShapeMaterial>();
  public Material fallBackMaterial;
  public Material tmpMaterial;
  public static SetSpriteshapeMaterial Instance;

  public void OnEnable() => SetSpriteshapeMaterial.Instance = this;

  public void OnDestroy()
  {
    this.SpriteShapes.Clear();
    SetSpriteshapeMaterial.Instance = (SetSpriteshapeMaterial) null;
  }

  public GroundType GetGroundType(SpriteShape spriteShape)
  {
    foreach (SpriteShapeMaterial spriteShape1 in this.SpriteShapes)
    {
      if ((UnityEngine.Object) spriteShape1.ss == (UnityEngine.Object) spriteShape)
        return spriteShape1.GroundTag;
    }
    return GroundType.Grass;
  }

  public void SetSpriteShapeMaterials()
  {
    SpriteShapeController[] objectsOfType = UnityEngine.Object.FindObjectsOfType((System.Type) typeof (SpriteShapeController)) as SpriteShapeController[];
    Debug.Log((object) $"Found {objectsOfType.Length.ToString()} instances with this script attached");
    foreach (SpriteShapeController spriteShapeController in objectsOfType)
    {
      this.tmpMaterial = this.GetSpriteshapeMaterial(spriteShapeController.spriteShape);
      spriteShapeController.spriteShapeRenderer.sharedMaterial = this.tmpMaterial;
    }
  }

  public Material GetSpriteshapeMaterial(SpriteShape ss)
  {
    Debug.Log((object) ss);
    for (int index = 0; index < this.SpriteShapes.Count - 1; ++index)
    {
      if ((UnityEngine.Object) this.SpriteShapes[index].ss == (UnityEngine.Object) ss)
      {
        if ((UnityEngine.Object) this.SpriteShapes[index].m == (UnityEngine.Object) null)
          return this.fallBackMaterial;
        Debug.Log((object) this.SpriteShapes[index].m);
        return this.SpriteShapes[index].m;
      }
    }
    Debug.Log((object) "Couldn't Find Spriteshape");
    return this.fallBackMaterial;
  }

  public SpriteShapeRenderer GetSpriteshapeRenderer(SpriteShape ss)
  {
    Debug.Log((object) ss);
    for (int index = 0; index < this.SpriteShapes.Count - 1; ++index)
    {
      if ((UnityEngine.Object) this.SpriteShapes[index].ss == (UnityEngine.Object) ss)
      {
        Debug.Log((object) this.SpriteShapes[index].m);
        return this.SpriteShapes[index].ssr;
      }
    }
    Debug.Log((object) "Couldn't Find Spriteshape");
    return (SpriteShapeRenderer) null;
  }
}
