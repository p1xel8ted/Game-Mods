// Decompiled with JetBrains decompiler
// Type: VirtualCursor
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class VirtualCursor : MonoBehaviour
{
  public Collider2D collider2D;
  public float speed = 7f;
  public const float Z_POSITION = 0.0f;
  public Transform left;
  public Transform right;
  public Transform up;
  public Transform down;

  public void Awake() => this.gameObject.SetActive(false);

  public void Update()
  {
    Vector2 vector2 = (Vector2) this.transform.position + this.speed * LazyInput.GetDirection2() * (float) GameSettings.current_resolution.pixel_size;
    if ((double) vector2.x < (double) this.left.position.x)
      vector2.x = this.left.position.x;
    if ((double) vector2.x > (double) this.right.position.x)
      vector2.x = this.right.position.x;
    if ((double) vector2.y < (double) this.down.position.y)
      vector2.y = this.down.position.y;
    if ((double) vector2.y > (double) this.up.position.y)
      vector2.y = this.up.position.y;
    this.transform.position = new Vector3(vector2.x, vector2.y, 0.0f);
  }

  public void EnableAsMapCursor(Transform left, Transform right, Transform up, Transform down)
  {
    this.left = left;
    this.right = right;
    this.up = up;
    this.down = down;
    this.gameObject.SetActive(true);
  }
}
