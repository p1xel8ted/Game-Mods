// Decompiled with JetBrains decompiler
// Type: ColliderCulling2D
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ColliderCulling2D : MonoBehaviour
{
  [Header("Player Tracking")]
  public List<PlayerFarming> players;
  public float activationRadius = 7f;
  public float updateInterval = 0.2f;
  public int checksPerFrame = 500;
  public List<Collider2D> allColliders = new List<Collider2D>();
  public int currentIndex;
  public float timeSinceLastUpdate;

  public void Update()
  {
    if (this.players == null || this.players.Count == 0 || this.allColliders.Count == 0)
      return;
    this.timeSinceLastUpdate += Time.deltaTime;
    if ((double) this.timeSinceLastUpdate < (double) this.updateInterval)
      return;
    this.timeSinceLastUpdate = 0.0f;
    int num = 0;
    while (num < this.checksPerFrame)
    {
      Collider2D allCollider = this.allColliders[this.currentIndex];
      if ((Object) allCollider == (Object) null)
      {
        this.allColliders.RemoveAt(this.currentIndex);
      }
      else
      {
        foreach (PlayerFarming player in this.players)
        {
          if (!((Object) player == (Object) null))
          {
            bool flag = (double) Vector3.Distance(player.transform.position, allCollider.transform.position) <= (double) this.activationRadius;
            if (allCollider.enabled != flag)
              allCollider.enabled = flag;
            if (flag)
              break;
          }
        }
        this.currentIndex = (this.currentIndex + 1) % this.allColliders.Count;
        ++num;
      }
    }
  }
}
