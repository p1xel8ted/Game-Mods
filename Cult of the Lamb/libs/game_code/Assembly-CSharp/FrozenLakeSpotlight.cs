// Decompiled with JetBrains decompiler
// Type: FrozenLakeSpotlight
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FrozenLakeSpotlight : MonoBehaviour
{
  [SerializeField]
  public int playerID;
  [SerializeField]
  public GameObject lighthousePosition;
  [SerializeField]
  public GameObject pivot;
  public SpriteRenderer[] spriteRenderers;

  public void Awake()
  {
    this.spriteRenderers = this.GetComponentsInChildren<SpriteRenderer>();
    foreach (Renderer spriteRenderer in this.spriteRenderers)
      spriteRenderer.enabled = false;
  }

  public void Update()
  {
    if (PlayerFarming.players.Count < this.playerID)
    {
      foreach (Renderer spriteRenderer in this.spriteRenderers)
        spriteRenderer.enabled = false;
    }
    else
    {
      foreach (Renderer spriteRenderer in this.spriteRenderers)
        spriteRenderer.enabled = (double) PlayerFarming.players[this.playerID].transform.position.y < -25.0;
      this.transform.position = PlayerFarming.players[this.playerID].transform.position;
      this.pivot.transform.LookAt(this.lighthousePosition.transform, Vector3.forward);
    }
  }
}
