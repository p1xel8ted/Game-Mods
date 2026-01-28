// Decompiled with JetBrains decompiler
// Type: WaterInteraction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WaterInteraction : BaseMonoBehaviour
{
  public bool inTrigger;
  public List<Collider2D> playerColliders;
  public float timer;
  public Vector3[] PlayerOldPosition = new Vector3[2];
  public float TimeToSpawn = 1f;
  public GameObject waterVFX;
  public Vector3 prefabOffset;
  public string footstepSound = "event:/player/footstep_water";

  public void Start()
  {
  }

  public void OnTriggerEnter2D(Collider2D other)
  {
    if (!other.gameObject.CompareTag("Player"))
      return;
    if (!this.playerColliders.Contains(other))
      this.playerColliders.Add(other);
    this.inTrigger = true;
    AudioManager.Instance.playerFootstepOverride = this.footstepSound;
    PlayerFarming farmingComponent = PlayerFarming.GetPlayerFarmingComponent(other.gameObject);
    if (!((Object) farmingComponent != (Object) null))
      return;
    farmingComponent.SetActiveDustEffect(false);
  }

  public void OnTriggerExit2D(Collider2D other)
  {
    if (!other.gameObject.CompareTag("Player"))
      return;
    this.playerColliders.Remove(other);
    this.inTrigger = false;
    AudioManager.Instance.playerFootstepOverride = string.Empty;
    PlayerFarming farmingComponent = PlayerFarming.GetPlayerFarmingComponent(other.gameObject);
    if (!((Object) farmingComponent != (Object) null))
      return;
    farmingComponent.SetActiveDustEffect(true);
  }

  public void OnDisable() => AudioManager.Instance.playerFootstepOverride = string.Empty;

  public void OnDestroy() => AudioManager.Instance.playerFootstepOverride = string.Empty;

  public void Update()
  {
    for (int index = 0; index < this.playerColliders.Count; ++index)
    {
      if (this.inTrigger && (Object) this.playerColliders[index] != (Object) null && (double) (this.timer += Time.deltaTime) >= (double) this.TimeToSpawn && this.PlayerOldPosition[index] != this.playerColliders[index].gameObject.transform.position)
      {
        Vector3 position = this.playerColliders[index].gameObject.transform.position;
        this.PlayerOldPosition[index] = position;
        Object.Instantiate<GameObject>(this.waterVFX, position + this.prefabOffset, Quaternion.identity);
        this.timer = 0.0f;
      }
    }
  }
}
