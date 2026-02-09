// Decompiled with JetBrains decompiler
// Type: ParticlesIntensity
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ParticlesIntensity : MonoBehaviour
{
  public WorldGameObject player;
  [SerializeField]
  public float maxDistanceToObject = 900f;
  [SerializeField]
  public List<ParticlesIntensity.EditableParticleEntity> entities;

  public void Start()
  {
    this.player = MainGame.me.player;
    foreach (ParticlesIntensity.EditableParticleEntity entity in this.entities)
    {
      entity.initRateOverTime = entity.particleSystem.emission.rateOverTime.constant;
      entity.emission = entity.particleSystem.emission;
    }
  }

  public void Update()
  {
    for (int index = 0; index < this.entities.Count; ++index)
    {
      float num1 = Vector2.Distance((Vector2) this.transform.position, (Vector2) this.player.transform.position);
      if ((double) num1 > (double) this.maxDistanceToObject)
        this.entities[index].emission.rateOverTime = (ParticleSystem.MinMaxCurve) 0.0f;
      else if ((double) num1 <= (double) this.entities[index].minDistance)
      {
        this.entities[index].emission.rateOverTime = (ParticleSystem.MinMaxCurve) this.entities[index].initRateOverTime;
      }
      else
      {
        float num2 = (this.maxDistanceToObject - num1) / this.maxDistanceToObject;
        this.entities[index].emission.rateOverTime = (ParticleSystem.MinMaxCurve) (this.entities[index].initRateOverTime * num2);
      }
    }
  }

  [Serializable]
  public class EditableParticleEntity
  {
    [HideInInspector]
    public ParticleSystem.EmissionModule emission;
    [HideInInspector]
    public float initRateOverTime;
    public float minDistance = 100f;
    public ParticleSystem particleSystem;
  }
}
