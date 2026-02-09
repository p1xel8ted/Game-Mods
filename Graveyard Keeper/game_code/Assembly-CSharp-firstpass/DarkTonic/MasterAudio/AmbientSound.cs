// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.AmbientSound
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace DarkTonic.MasterAudio;

[AddComponentMenu("Dark Tonic/Master Audio/Ambient Sound")]
[AudioScriptOrder(-20)]
public class AmbientSound : MonoBehaviour
{
  [SoundGroup]
  public string AmbientSoundGroup = "[None]";
  [Tooltip("This option is useful if your caller ever moves, as it will make the Audio Source follow to the location of the caller every frame.")]
  public bool FollowCaller;
  [Tooltip("Using this option, the Audio Source will be updated every frame to the closest position on the caller's collider, if any. This will override the Follow Caller option above and happen instead.")]
  public bool UseClosestColliderPosition;
  public bool UseTopCollider = true;
  public bool IncludeChildColliders;
  [Tooltip("This is for diagnostic purposes only. Do not change or assign this field.")]
  public Transform RuntimeFollower;
  public Transform _trans;

  public void OnEnable() => this.StartTrackers();

  public void OnDisable()
  {
    if (DarkTonic.MasterAudio.MasterAudio.AppIsShuttingDown || !this.IsValidSoundGroup || (Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance == (Object) null)
      return;
    DarkTonic.MasterAudio.MasterAudio.StopSoundGroupOfTransform(this.Trans, this.AmbientSoundGroup);
    this.RuntimeFollower = (Transform) null;
  }

  public void StartTrackers()
  {
    if (!this.IsValidSoundGroup || !AmbientUtil.InitListenerFollower())
      return;
    if (!AmbientUtil.HasListenerFolowerRigidBody)
      DarkTonic.MasterAudio.MasterAudio.LogWarning($"Your Ambient Sound script on Game Object '{this.name}' will not function because you have turned off the Listener Follower RigidBody in Advanced Settings.");
    this.RuntimeFollower = AmbientUtil.InitAudioSourceFollower(this.Trans, $"{this.name}_{this.AmbientSoundGroup}_{Random.Range(0, 9).ToString()}_Follower", this.AmbientSoundGroup, this.FollowCaller, this.UseClosestColliderPosition, this.UseTopCollider, this.IncludeChildColliders);
  }

  public bool IsValidSoundGroup
  {
    get => !DarkTonic.MasterAudio.MasterAudio.SoundGroupHardCodedNames.Contains(this.AmbientSoundGroup);
  }

  public Transform Trans
  {
    get
    {
      if ((Object) this._trans == (Object) null)
        this._trans = this.transform;
      return this._trans;
    }
  }
}
