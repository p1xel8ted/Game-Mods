// Decompiled with JetBrains decompiler
// Type: Unify.UserProfile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Unify;

public class UserProfile : MonoBehaviour
{
  public GameObject overlay;
  public Text profileName;

  public void Awake() => this.overlay.SetActive(false);

  public void Start()
  {
    SessionManager.OnSessionStart += new SessionManager.SessionEventDelegate(this.OnSessionStart);
    SessionManager.OnSessionEnd += new SessionManager.SessionEventDelegate(this.OnSessionEnd);
    if (!SessionHandler.HasSessionStarted)
      return;
    this.ShowUserProfile(true);
  }

  public void OnDestroy()
  {
    SessionManager.OnSessionStart -= new SessionManager.SessionEventDelegate(this.OnSessionStart);
    SessionManager.OnSessionEnd -= new SessionManager.SessionEventDelegate(this.OnSessionEnd);
  }

  public void OnSessionStart(Guid sessionGuid, User sessionUser) => this.ShowUserProfile(true);

  public void OnSessionEnd(Guid sessionGuid, User sessionUser) => this.ShowUserProfile(false);

  public void ShowUserProfile(bool show)
  {
    if (show)
    {
      User sessionOwner = SessionManager.GetSessionOwner();
      if (sessionOwner != null)
      {
        this.profileName.text = sessionOwner.nickName;
        this.overlay.SetActive(true);
        UserHelper.Instance.GetUserPicture(sessionOwner, 512 /*0x0200*/, (UserHelper.userPictureCallbackDelegate) (texture =>
        {
          if ((UnityEngine.Object) texture != (UnityEngine.Object) null)
          {
            int num = texture.width;
            string str1 = num.ToString();
            num = texture.height;
            string str2 = num.ToString();
            Logger.Log((object) $"USERPROFILE: profile texture: {str1}x{str2}");
          }
          else
            Logger.Log((object) "USERPROFILE: on profile picture available");
        }));
      }
      else
        this.overlay.SetActive(false);
    }
    else
      this.overlay.SetActive(false);
  }
}
