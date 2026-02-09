// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.AmbientUtil
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace DarkTonic.MasterAudio;

public static class AmbientUtil
{
  public const string FollowerHolderName = "_Followers";
  public const string ListenerFollowerName = "~ListenerFollower~";
  public const float ListenerFollowerTrigRadius = 0.01f;
  public static Transform _followerHolder;
  public static ListenerFollower _listenerFollower;
  public static Rigidbody _listenerFollowerRB;

  public static void InitFollowerHolder()
  {
    Transform followerHolder = AmbientUtil.FollowerHolder;
    if (!((Object) followerHolder != (Object) null))
      return;
    followerHolder.DestroyAllChildren();
  }

  public static bool InitListenerFollower()
  {
    Transform listenerTrans = DarkTonic.MasterAudio.MasterAudio.ListenerTrans;
    if ((Object) listenerTrans == (Object) null)
      return false;
    ListenerFollower listenerFollower = AmbientUtil.ListenerFollower;
    if ((Object) listenerFollower == (Object) null)
      return false;
    listenerFollower.StartFollowing(listenerTrans, "[None]", 0.01f);
    return true;
  }

  public static Transform InitAudioSourceFollower(
    Transform transToFollow,
    string followerName,
    string soundGroupName,
    bool willFollowSource,
    bool willPositionOnClosestColliderPoint,
    bool useTopCollider,
    bool useChildColliders)
  {
    if ((Object) AmbientUtil.ListenerFollower == (Object) null || (Object) AmbientUtil.FollowerHolder == (Object) null)
      return (Transform) null;
    MasterAudioGroup masterAudioGroup = DarkTonic.MasterAudio.MasterAudio.GrabGroup(soundGroupName);
    if ((Object) masterAudioGroup == (Object) null)
      return (Transform) null;
    if (masterAudioGroup.groupVariations.Count == 0)
      return (Transform) null;
    float maxDistance = masterAudioGroup.groupVariations[0].VarAudio.maxDistance;
    GameObject gameObject = new GameObject(followerName);
    Transform childTransform = AmbientUtil.FollowerHolder.GetChildTransform(followerName);
    if ((Object) childTransform != (Object) null)
      Object.Destroy((Object) childTransform.gameObject);
    gameObject.transform.parent = AmbientUtil.FollowerHolder;
    gameObject.gameObject.layer = AmbientUtil.FollowerHolder.gameObject.layer;
    gameObject.gameObject.AddComponent<TransformFollower>().StartFollowing(transToFollow, soundGroupName, maxDistance, willFollowSource, willPositionOnClosestColliderPoint, useTopCollider, useChildColliders);
    return gameObject.transform;
  }

  public static ListenerFollower ListenerFollower
  {
    get
    {
      if ((Object) AmbientUtil._listenerFollower != (Object) null)
        return AmbientUtil._listenerFollower;
      if ((Object) AmbientUtil.FollowerHolder == (Object) null)
        return (ListenerFollower) null;
      Transform transform = AmbientUtil.FollowerHolder.GetChildTransform("~ListenerFollower~");
      if ((Object) transform == (Object) null)
      {
        transform = new GameObject("~ListenerFollower~").transform;
        transform.parent = AmbientUtil.FollowerHolder;
        transform.gameObject.layer = AmbientUtil.FollowerHolder.gameObject.layer;
      }
      AmbientUtil._listenerFollower = transform.GetComponent<ListenerFollower>();
      if ((Object) AmbientUtil._listenerFollower == (Object) null)
        AmbientUtil._listenerFollower = transform.gameObject.AddComponent<ListenerFollower>();
      if (DarkTonic.MasterAudio.MasterAudio.Instance.listenerFollowerHasRigidBody)
      {
        Rigidbody rigidbody = transform.gameObject.GetComponent<Rigidbody>();
        if ((Object) rigidbody == (Object) null)
          rigidbody = transform.gameObject.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;
        AmbientUtil._listenerFollowerRB = rigidbody;
      }
      return AmbientUtil._listenerFollower;
    }
  }

  public static Transform FollowerHolder
  {
    get
    {
      if (!Application.isPlaying || (Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance == (Object) null)
        return (Transform) null;
      if ((Object) AmbientUtil._followerHolder != (Object) null)
        return AmbientUtil._followerHolder;
      Transform trans = DarkTonic.MasterAudio.MasterAudio.SafeInstance.Trans;
      AmbientUtil._followerHolder = trans.GetChildTransform("_Followers");
      if ((Object) AmbientUtil._followerHolder != (Object) null)
        return AmbientUtil._followerHolder;
      AmbientUtil._followerHolder = new GameObject("_Followers").transform;
      AmbientUtil._followerHolder.parent = trans;
      AmbientUtil._followerHolder.gameObject.layer = trans.gameObject.layer;
      return AmbientUtil._followerHolder;
    }
  }

  public static bool HasListenerFollower => (Object) AmbientUtil._listenerFollower != (Object) null;

  public static bool HasListenerFolowerRigidBody
  {
    get => (Object) AmbientUtil._listenerFollowerRB != (Object) null;
  }
}
