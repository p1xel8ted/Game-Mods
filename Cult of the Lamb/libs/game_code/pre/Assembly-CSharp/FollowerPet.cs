// Decompiled with JetBrains decompiler
// Type: FollowerPet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class FollowerPet : MonoBehaviour
{
  public static List<FollowerPet> FollowerPets = new List<FollowerPet>();
  private Follower follower;

  public Follower Follower => this.follower;

  public static void Create(FollowerPet.FollowerPetType petType, Follower target)
  {
    string key = "";
    if (petType == FollowerPet.FollowerPetType.Spider)
      key = "Assets/Prefabs/Spider Pet.prefab";
    Addressables.InstantiateAsync((object) key, target.transform.parent).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      obj.Result.transform.position = target.transform.position;
      obj.Result.GetComponent<FollowerPet>().follower = target;
      if (!((UnityEngine.Object) obj.Result.GetComponent<CritterSpider>() != (UnityEngine.Object) null))
        return;
      obj.Result.GetComponent<CritterSpider>().TargetHost = target.gameObject;
    });
  }

  private void Awake() => FollowerPet.FollowerPets.Add(this);

  private void OnDestroy() => FollowerPet.FollowerPets.Remove(this);

  public enum FollowerPetType
  {
    Spider,
  }
}
