// Decompiled with JetBrains decompiler
// Type: TechPointsSpawner
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class TechPointsSpawner : MonoBehaviour
{
  public TechPointDrop prefab;
  public float period = 0.1f;
  public Vector2 force;
  [Range(0.0f, 359f)]
  public float delta_angle;
  public int _test_r = 1;
  public int _test_g = 1;
  public int _test_b = 1;
  public int[] _need_spawn = new int[3];
  public bool _spawning;
  public float _dt;
  public bool destroy_after_spawn = true;

  public void TestSpawn() => this.Spawn(this._test_r, this._test_g, this._test_b);

  public void Spawn(int r, int g, int b)
  {
    this._need_spawn[0] = r;
    this._need_spawn[1] = g;
    this._need_spawn[2] = b;
    this._dt = 0.0f;
    this._spawning = true;
  }

  public void Update()
  {
    if (!this._spawning)
      return;
    this._dt += Time.deltaTime;
    if ((double) this._dt <= (double) this.period)
      return;
    this._dt -= this.period;
    int index = NGUITools.RandomRange(0, 2);
    if (this._need_spawn[index] == 0)
    {
      if (++index == 3)
        index = 0;
      if (this._need_spawn[index] == 0 && ++index == 3)
        index = 0;
    }
    if (this._need_spawn[0] + this._need_spawn[1] + this._need_spawn[2] == 0)
      this.OnDoneSpawning();
    else if (this._need_spawn[index] == 0)
    {
      Debug.LogWarning((object) "Something is wrong");
    }
    else
    {
      --this._need_spawn[index];
      this.SpawnTechPoint((TechPointsSpawner.Type) index);
    }
  }

  public void SpawnTechPoint(TechPointsSpawner.Type type)
  {
    TechPointDrop techPointDrop = TechPointDrop.Spawn(this.prefab, type);
    techPointDrop.transform.position = this.transform.position;
    float num1 = ((Vector2) MainGame.me.player.tf.position - (Vector2) this.transform.position).Atan2() * 57.29578f;
    float num2 = UnityEngine.Random.Range(this.force.x, this.force.y);
    float num3 = UnityEngine.Random.Range(num1 - this.delta_angle, num1 + this.delta_angle);
    techPointDrop.rigid_body.AddForce(new Vector2(Mathf.Cos(num3 * ((float) Math.PI / 180f)), Mathf.Sin(num3 * ((float) Math.PI / 180f))) * num2, ForceMode2D.Impulse);
  }

  public void OnDoneSpawning()
  {
    this._spawning = false;
    if (!this.destroy_after_spawn)
      return;
    NGUITools.Destroy((UnityEngine.Object) this.gameObject);
  }

  public enum Type
  {
    R,
    G,
    B,
  }
}
