// Decompiled with JetBrains decompiler
// Type: RewiredConsts.PhotoModeInputSource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
namespace RewiredConsts;

public class PhotoModeInputSource : CategoryInputSource
{
  public override int Category => 2;

  public static int[] AllBindings
  {
    get
    {
      return new int[13]
      {
        83,
        77,
        84,
        76,
        75,
        90,
        80 /*0x50*/,
        85,
        89,
        91,
        88,
        86,
        87
      };
    }
  }

  public bool GetPlaceStickerButtonDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonDown(85, playerFarming);
  }

  public bool GetPlaceStickerButtonUp(PlayerFarming playerFarming = null)
  {
    return this.GetButtonUp(85, playerFarming);
  }

  public float GetStickerScaleAxis(PlayerFarming playerFarming = null)
  {
    return this.GetAxis(87, playerFarming);
  }

  public float GetStickerRotateAxis(PlayerFarming playerFarming = null)
  {
    return this.GetAxis(86, playerFarming);
  }

  public bool GetFlipStickerButtonDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonDown(88, playerFarming);
  }

  public bool GetUndoButtonDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonDown(89, playerFarming);
  }

  public bool GetSaveButtonDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonDown(90, playerFarming);
  }

  public bool GetClearStickersButtonDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonDown(91, playerFarming);
  }

  public bool GetTakePhotoButtonDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonDown(76, playerFarming);
  }

  public bool GetDeletePhotoButtonDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonDown(80 /*0x50*/, playerFarming);
  }

  public bool GetGalleryFolderButtonDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonDown(75, playerFarming);
  }

  public float GetCameraHeightAxis(PlayerFarming playerFarming = null)
  {
    return this.GetAxis(77, playerFarming);
  }

  public float GetFocusAxis(PlayerFarming playerFarming = null) => this.GetAxis(83, playerFarming);

  public float GetCameraTiltAxis(PlayerFarming playerFarming = null)
  {
    return this.GetAxis(84, playerFarming);
  }
}
