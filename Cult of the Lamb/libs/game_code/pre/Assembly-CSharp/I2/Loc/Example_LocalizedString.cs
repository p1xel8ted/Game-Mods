// Decompiled with JetBrains decompiler
// Type: I2.Loc.Example_LocalizedString
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace I2.Loc;

public class Example_LocalizedString : MonoBehaviour
{
  public LocalizedString _MyLocalizedString;
  public string _NormalString;
  [TermsPopup("")]
  public string _StringWithTermPopup;

  public void Start()
  {
    Debug.Log((object) this._MyLocalizedString);
    Debug.Log((object) LocalizationManager.GetTranslation(this._NormalString));
    Debug.Log((object) LocalizationManager.GetTranslation(this._StringWithTermPopup));
    Debug.Log((object) (string) (LocalizedString) "Term2");
    Debug.Log((object) this._MyLocalizedString);
    Debug.Log((object) (LocalizedString) "Term3");
    Debug.Log((object) ((LocalizedString) "Term3" with
    {
      mRTL_IgnoreArabicFix = true
    }));
    LocalizedString message = (LocalizedString) "Term3" with
    {
      mRTL_ConvertNumbers = true,
      mRTL_MaxLineLength = 20
    };
    Debug.Log((object) message);
    Debug.Log((object) message);
  }
}
