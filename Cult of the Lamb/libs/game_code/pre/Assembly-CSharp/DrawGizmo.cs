// Decompiled with JetBrains decompiler
// Type: DrawGizmo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DrawGizmo : BaseMonoBehaviour
{
  public GameObject[] gameObjects;
  private GameObject pastObject;
  public Color color = Color.yellow;
  public float sphereSize = 0.1f;

  private void Start()
  {
  }

  private void OnDrawGizmosSelected()
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

  private void Update()
  {
  }
}
