// Decompiled with JetBrains decompiler
// Type: MA_EnemySpawner
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using DarkTonic.MasterAudio;
using UnityEngine;

#nullable disable
public class MA_EnemySpawner : MonoBehaviour
{
  public GameObject Enemy;
  public bool spawnerEnabled;
  public Transform trans;
  public float nextSpawnTime;

  public void Awake()
  {
    this.useGUILayout = false;
    this.trans = this.transform;
    this.nextSpawnTime = AudioUtil.Time + Random.Range(0.3f, 0.7f);
  }

  public void Update()
  {
    if (!this.spawnerEnabled || (double) Time.time < (double) this.nextSpawnTime)
      return;
    Vector3 position = this.trans.position;
    int num = Random.Range(1, 3);
    for (int index = 0; index < num; ++index)
    {
      position.x = Random.Range(position.x - 6f, position.x + 6f);
      Object.Instantiate<GameObject>(this.Enemy, position, this.Enemy.transform.rotation);
    }
    this.nextSpawnTime = AudioUtil.Time + Random.Range(0.3f, 0.7f);
  }
}
