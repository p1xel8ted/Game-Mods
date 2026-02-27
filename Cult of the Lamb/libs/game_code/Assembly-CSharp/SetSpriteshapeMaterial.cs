// Decompiled with JetBrains decompiler
// Type: SetSpriteshapeMaterial
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
      if ((Object) spriteShape1.ss == (Object) spriteShape)
        return spriteShape1.GroundTag;
    }
    return GroundType.Grass;
  }

  public void SetSpriteShapeMaterials()
  {
    SpriteShapeController[] objectsOfType = Object.FindObjectsOfType(typeof (SpriteShapeController)) as SpriteShapeController[];
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
      if ((Object) this.SpriteShapes[index].ss == (Object) ss)
      {
        if ((Object) this.SpriteShapes[index].m == (Object) null)
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
      if ((Object) this.SpriteShapes[index].ss == (Object) ss)
      {
        Debug.Log((object) this.SpriteShapes[index].m);
        return this.SpriteShapes[index].ssr;
      }
    }
    Debug.Log((object) "Couldn't Find Spriteshape");
    return (SpriteShapeRenderer) null;
  }
}
