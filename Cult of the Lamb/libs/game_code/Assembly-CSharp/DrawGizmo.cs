// Decompiled with JetBrains decompiler
// Type: DrawGizmo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DrawGizmo : BaseMonoBehaviour
{
  public GameObject[] gameObjects;
  public GameObject pastObject;
  public Color color = Color.yellow;
  public float sphereSize = 0.1f;

  public void Start()
  {
  }

  public void OnDrawGizmosSelected()
  {
    if (this.gameObjects == null)
      return;
    foreach (GameObject gameObject in this.gameObjects)
    {
      Gizmos.color = this.color;
      Gizmos.DrawSphere(gameObject.transform.position, this.sphereSize);
      if ((Object) this.pastObject != (Object) null)
        Gizmos.DrawLine(gameObject.transform.position, this.pastObject.transform.position);
      this.pastObject = gameObject;
    }
  }

  public void Update()
  {
  }
}
