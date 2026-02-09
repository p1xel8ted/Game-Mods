// Decompiled with JetBrains decompiler
// Type: Steamworks.EBroadcastUploadResult
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace Steamworks;

public enum EBroadcastUploadResult
{
  k_EBroadcastUploadResultNone,
  k_EBroadcastUploadResultOK,
  k_EBroadcastUploadResultInitFailed,
  k_EBroadcastUploadResultFrameFailed,
  k_EBroadcastUploadResultTimeout,
  k_EBroadcastUploadResultBandwidthExceeded,
  k_EBroadcastUploadResultLowFPS,
  k_EBroadcastUploadResultMissingKeyFrames,
  k_EBroadcastUploadResultNoConnection,
  k_EBroadcastUploadResultRelayFailed,
  k_EBroadcastUploadResultSettingsChanged,
  k_EBroadcastUploadResultMissingAudio,
  k_EBroadcastUploadResultTooFarBehind,
  k_EBroadcastUploadResultTranscodeBehind,
}
