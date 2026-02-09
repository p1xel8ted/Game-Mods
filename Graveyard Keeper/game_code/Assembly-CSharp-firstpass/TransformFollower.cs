// Decompiled with JetBrains decompiler
// Type: TransformFollower
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using DarkTonic.MasterAudio;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TransformFollower : MonoBehaviour
{
  [Tooltip("This is for diagnostic purposes only. Do not change or assign this field.")]
  public Transform RuntimeFollowingTransform;
  public GameObject _goToFollow;
  public Transform _trans;
  public GameObject _go;
  public SphereCollider _collider;
  public string _soundType;
  public bool _willFollowSource;
  public bool _isInsideTrigger;
  public bool _hasPlayedSound;
  public bool _groupLoadFailed;
  public MasterAudioGroup _groupToPlay;
  public bool _positionAtClosestColliderPoint;
  public List<Collider> _actorColliders = new List<Collider>();
  public List<Collider2D> _actorColliders2D = new List<Collider2D>();
  public Vector3 _lastListenerPos = new Vector3(float.MinValue, float.MinValue, float.MinValue);
  public Dictionary<Collider, Vector3> _lastPositionByCollider = new Dictionary<Collider, Vector3>();
  public Dictionary<Collider2D, Vector3> _lastPositionByCollider2D = new Dictionary<Collider2D, Vector3>();

  public void Awake()
  {
    if ((Object) this.Trigger == (Object) null || this._actorColliders.Count == 0 || this._actorColliders2D.Count == 0 || this._positionAtClosestColliderPoint)
      return;
    int num = this._lastListenerPos == Vector3.zero ? 1 : 0;
  }

  public void Start() => this._groupToPlay = DarkTonic.MasterAudio.MasterAudio.GrabGroup(this._soundType, false);

  public void StartFollowing(
    Transform transToFollow,
    string soundType,
    float trigRadius,
    bool willFollowSource,
    bool positionAtClosestColliderPoint,
    bool useTopCollider,
    bool useChildColliders)
  {
    this.RuntimeFollowingTransform = transToFollow;
    this._goToFollow = transToFollow.gameObject;
    this.Trigger.radius = trigRadius;
    this._soundType = soundType;
    this._willFollowSource = willFollowSource;
    this._lastPositionByCollider.Clear();
    this._lastPositionByCollider2D.Clear();
    if (useTopCollider)
    {
      Collider component1 = transToFollow.GetComponent<Collider>();
      if ((Object) component1 != (Object) null)
      {
        this._actorColliders.Add(component1);
        this._lastPositionByCollider.Add(component1, transToFollow.position);
      }
      else
      {
        Collider2D component2 = transToFollow.GetComponent<Collider2D>();
        if ((Object) component2 != (Object) null)
        {
          this._actorColliders2D.Add(component2);
          this._lastPositionByCollider2D.Add(component2, transToFollow.position);
        }
      }
    }
    if (useChildColliders)
    {
      for (int index = 0; index < this.Trans.childCount; ++index)
      {
        Transform child = this.Trans.GetChild(index);
        Collider component3 = child.GetComponent<Collider>();
        if ((Object) component3 != (Object) null)
        {
          this._actorColliders.Add(component3);
          this._lastPositionByCollider.Add(component3, transToFollow.position);
        }
        else
        {
          Collider2D component4 = child.GetComponent<Collider2D>();
          if ((Object) component4 != (Object) null)
          {
            this._actorColliders2D.Add(component4);
            this._lastPositionByCollider2D.Add(component4, transToFollow.position);
          }
        }
      }
    }
    this._lastListenerPos = DarkTonic.MasterAudio.MasterAudio.ListenerTrans.position;
    if (((this._actorColliders.Count != 0 ? 0 : (this._actorColliders2D.Count == 0 ? 1 : 0)) & (positionAtClosestColliderPoint ? 1 : 0)) != 0)
    {
      Debug.Log((object) $"Can't follow collider of '{transToFollow.name}' because it doesn't have any colliders.");
    }
    else
    {
      this._positionAtClosestColliderPoint = positionAtClosestColliderPoint;
      if (!this._positionAtClosestColliderPoint)
        return;
      DarkTonic.MasterAudio.MasterAudio.QueueTransformFollowerForColliderPositionRecalc(this);
    }
  }

  public void StopFollowing()
  {
    this.RuntimeFollowingTransform = (Transform) null;
    Object.Destroy((Object) this.GameObj);
  }

  public void OnTriggerEnter(Collider other)
  {
    if ((Object) this.RuntimeFollowingTransform == (Object) null || (Object) other == (Object) null || this.name == "~ListenerFollower~" || other.name != "~ListenerFollower~")
      return;
    this._isInsideTrigger = true;
    if ((Object) this._groupToPlay != (Object) null)
    {
      switch (this._groupToPlay.GroupLoadStatus)
      {
        case DarkTonic.MasterAudio.MasterAudio.InternetFileLoadStatus.Loading:
          return;
        case DarkTonic.MasterAudio.MasterAudio.InternetFileLoadStatus.Failed:
          if (DarkTonic.MasterAudio.MasterAudio.LogSoundsEnabled)
            DarkTonic.MasterAudio.MasterAudio.LogWarning($"TransformFollower: '{this.name}' not attempting to play Sound Group '{this._soundType}' because the Sound Group failed to load.");
          this._groupLoadFailed = true;
          return;
      }
    }
    this.PlaySound();
  }

  public void PlaySound()
  {
    if (this._willFollowSource)
      DarkTonic.MasterAudio.MasterAudio.PlaySound3DFollowTransformAndForget(this._soundType, this.RuntimeFollowingTransform);
    else
      DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransformAndForget(this._soundType, this.RuntimeFollowingTransform);
    this._hasPlayedSound = true;
  }

  public void LateUpdate()
  {
    if ((Object) this.RuntimeFollowingTransform == (Object) null || !DTMonoHelper.IsActive(this._goToFollow))
    {
      this.StopFollowing();
    }
    else
    {
      if (!this._positionAtClosestColliderPoint)
        this.Trans.position = this.RuntimeFollowingTransform.position;
      if (!this._isInsideTrigger || this._hasPlayedSound || this._groupLoadFailed)
        return;
      switch (this._groupToPlay.GroupLoadStatus)
      {
        case DarkTonic.MasterAudio.MasterAudio.InternetFileLoadStatus.Loaded:
          this.PlaySound();
          break;
        case DarkTonic.MasterAudio.MasterAudio.InternetFileLoadStatus.Failed:
          if (DarkTonic.MasterAudio.MasterAudio.LogSoundsEnabled)
            DarkTonic.MasterAudio.MasterAudio.LogWarning($"TransformFollower: '{this.name}' not attempting to play Sound Group '{this._soundType}' because the Sound Group failed to load.");
          this._groupLoadFailed = true;
          break;
      }
    }
  }

  public bool RecalcClosestColliderPosition()
  {
    Vector3 position1 = DarkTonic.MasterAudio.MasterAudio.ListenerTrans.position;
    bool flag1 = this._lastListenerPos != position1;
    Vector3 vector3_1 = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
    float num = float.MaxValue;
    bool flag2 = false;
    if (this._actorColliders.Count > 0)
    {
      if (this._actorColliders.Count == 1)
      {
        Collider actorCollider = this._actorColliders[0];
        Vector3 position2 = actorCollider.transform.position;
        if (this._lastPositionByCollider[actorCollider] == position2 && !flag1)
          return false;
        flag2 = true;
        vector3_1 = actorCollider.ClosestPoint(position1);
        this._lastPositionByCollider[actorCollider] = position2;
      }
      else
      {
        for (int index = 0; index < this._actorColliders.Count; ++index)
        {
          Collider actorCollider = this._actorColliders[index];
          Vector3 position3 = actorCollider.transform.position;
          if (!(this._lastPositionByCollider[actorCollider] == position3) || flag1)
          {
            flag2 = true;
            Vector3 vector3_2 = actorCollider.ClosestPoint(position1);
            float sqrMagnitude = (position1 - vector3_2).sqrMagnitude;
            if ((double) sqrMagnitude < (double) num)
            {
              vector3_1 = vector3_2;
              num = sqrMagnitude;
            }
            this._lastPositionByCollider[actorCollider] = position3;
          }
        }
      }
    }
    else
    {
      if (this._actorColliders2D.Count <= 0)
        return false;
      if (this._actorColliders2D.Count == 1)
      {
        Collider2D key = this._actorColliders2D[0];
        Vector3 position4 = key.transform.position;
        if (this._lastPositionByCollider2D[key] == position4 && !flag1)
          return false;
        flag2 = true;
        vector3_1 = key.bounds.ClosestPoint(position1);
        this._lastPositionByCollider2D[key] = position4;
      }
      else
      {
        for (int index = 0; index < this._actorColliders2D.Count; ++index)
        {
          Collider2D key = this._actorColliders2D[index];
          Vector3 position5 = key.transform.position;
          if (!(this._lastPositionByCollider2D[key] == position5) || flag1)
          {
            flag2 = true;
            Vector3 vector3_3 = key.bounds.ClosestPoint(position1);
            float sqrMagnitude = (position1 - vector3_3).sqrMagnitude;
            if ((double) sqrMagnitude < (double) num)
            {
              vector3_1 = vector3_3;
              num = sqrMagnitude;
            }
            this._lastPositionByCollider2D[key] = position5;
          }
        }
      }
    }
    if (!flag2)
      return false;
    this.Trans.position = vector3_1;
    this.Trans.LookAt(DarkTonic.MasterAudio.MasterAudio.ListenerTrans);
    this._lastListenerPos = position1;
    return true;
  }

  public void OnTriggerExit(Collider other)
  {
    if ((Object) this.RuntimeFollowingTransform == (Object) null || (Object) other == (Object) null || other.name != "~ListenerFollower~")
      return;
    this._isInsideTrigger = false;
    this._hasPlayedSound = false;
    DarkTonic.MasterAudio.MasterAudio.StopSoundGroupOfTransform(this.RuntimeFollowingTransform, this._soundType);
  }

  public SphereCollider Trigger
  {
    get
    {
      if ((Object) this._collider != (Object) null)
        return this._collider;
      this._collider = this.GameObj.AddComponent<SphereCollider>();
      this._collider.isTrigger = true;
      return this._collider;
    }
  }

  public GameObject GameObj
  {
    get
    {
      if ((Object) this._go != (Object) null)
        return this._go;
      this._go = this.gameObject;
      return this._go;
    }
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
