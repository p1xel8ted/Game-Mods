// Decompiled with JetBrains decompiler
// Type: Steamworks.EHTTPStatusCode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace Steamworks;

public enum EHTTPStatusCode
{
  k_EHTTPStatusCodeInvalid = 0,
  k_EHTTPStatusCode100Continue = 100, // 0x00000064
  k_EHTTPStatusCode101SwitchingProtocols = 101, // 0x00000065
  k_EHTTPStatusCode200OK = 200, // 0x000000C8
  k_EHTTPStatusCode201Created = 201, // 0x000000C9
  k_EHTTPStatusCode202Accepted = 202, // 0x000000CA
  k_EHTTPStatusCode203NonAuthoritative = 203, // 0x000000CB
  k_EHTTPStatusCode204NoContent = 204, // 0x000000CC
  k_EHTTPStatusCode205ResetContent = 205, // 0x000000CD
  k_EHTTPStatusCode206PartialContent = 206, // 0x000000CE
  k_EHTTPStatusCode300MultipleChoices = 300, // 0x0000012C
  k_EHTTPStatusCode301MovedPermanently = 301, // 0x0000012D
  k_EHTTPStatusCode302Found = 302, // 0x0000012E
  k_EHTTPStatusCode303SeeOther = 303, // 0x0000012F
  k_EHTTPStatusCode304NotModified = 304, // 0x00000130
  k_EHTTPStatusCode305UseProxy = 305, // 0x00000131
  k_EHTTPStatusCode307TemporaryRedirect = 307, // 0x00000133
  k_EHTTPStatusCode400BadRequest = 400, // 0x00000190
  k_EHTTPStatusCode401Unauthorized = 401, // 0x00000191
  k_EHTTPStatusCode402PaymentRequired = 402, // 0x00000192
  k_EHTTPStatusCode403Forbidden = 403, // 0x00000193
  k_EHTTPStatusCode404NotFound = 404, // 0x00000194
  k_EHTTPStatusCode405MethodNotAllowed = 405, // 0x00000195
  k_EHTTPStatusCode406NotAcceptable = 406, // 0x00000196
  k_EHTTPStatusCode407ProxyAuthRequired = 407, // 0x00000197
  k_EHTTPStatusCode408RequestTimeout = 408, // 0x00000198
  k_EHTTPStatusCode409Conflict = 409, // 0x00000199
  k_EHTTPStatusCode410Gone = 410, // 0x0000019A
  k_EHTTPStatusCode411LengthRequired = 411, // 0x0000019B
  k_EHTTPStatusCode412PreconditionFailed = 412, // 0x0000019C
  k_EHTTPStatusCode413RequestEntityTooLarge = 413, // 0x0000019D
  k_EHTTPStatusCode414RequestURITooLong = 414, // 0x0000019E
  k_EHTTPStatusCode415UnsupportedMediaType = 415, // 0x0000019F
  k_EHTTPStatusCode416RequestedRangeNotSatisfiable = 416, // 0x000001A0
  k_EHTTPStatusCode417ExpectationFailed = 417, // 0x000001A1
  k_EHTTPStatusCode4xxUnknown = 418, // 0x000001A2
  k_EHTTPStatusCode429TooManyRequests = 429, // 0x000001AD
  k_EHTTPStatusCode500InternalServerError = 500, // 0x000001F4
  k_EHTTPStatusCode501NotImplemented = 501, // 0x000001F5
  k_EHTTPStatusCode502BadGateway = 502, // 0x000001F6
  k_EHTTPStatusCode503ServiceUnavailable = 503, // 0x000001F7
  k_EHTTPStatusCode504GatewayTimeout = 504, // 0x000001F8
  k_EHTTPStatusCode505HTTPVersionNotSupported = 505, // 0x000001F9
  k_EHTTPStatusCode5xxUnknown = 599, // 0x00000257
}
