// Decompiled with JetBrains decompiler
// Type: I2.Loc.Example_LocalizedString
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
