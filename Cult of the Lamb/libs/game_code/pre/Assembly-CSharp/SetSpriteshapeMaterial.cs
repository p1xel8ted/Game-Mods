// Decompiled with JetBrains decompiler
// Type: SetSpriteshapeMaterial
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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

  private void OnEnable() => SetSpriteshapeMaterial.Instance = this;

  private void OnDestroy()
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
    Debug.Log((object) $"Found {(object) objectsOfType.Length} instances with this script attached");
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
