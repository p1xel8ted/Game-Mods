// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.SerializableException
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Unity.Cloud;

public class SerializableException
{
  public SerializableException()
  {
  }

  public SerializableException(Exception exception)
  {
    this.Message = exception.Message;
    this.FullText = exception.ToString();
    this.Type = exception.GetType().FullName;
    this.StackTrace = new List<SerializableStackFrame>();
    foreach (StackFrame frame in new System.Diagnostics.StackTrace(exception, true).GetFrames())
      this.StackTrace.Add(new SerializableStackFrame(frame));
    if (this.StackTrace.Count > 0)
    {
      SerializableStackFrame serializableStackFrame = this.StackTrace[0];
      this.ProblemIdentifier = $"{this.Type} at {serializableStackFrame.DeclaringType}.{serializableStackFrame.MethodName}";
    }
    else
      this.ProblemIdentifier = this.Type;
    if (this.StackTrace.Count > 1)
    {
      SerializableStackFrame serializableStackFrame1 = this.StackTrace[0];
      SerializableStackFrame serializableStackFrame2 = this.StackTrace[1];
      this.DetailedProblemIdentifier = $"{this.Type} at {serializableStackFrame1.DeclaringType}.{serializableStackFrame1.MethodName} from {serializableStackFrame2.DeclaringType}.{serializableStackFrame2.MethodName}";
    }
    if (exception.InnerException == null)
      return;
    this.InnerException = new SerializableException(exception.InnerException);
  }

  public string DetailedProblemIdentifier { get; set; }

  public string FullText { get; set; }

  public SerializableException InnerException { get; set; }

  public string Message { get; set; }

  public string ProblemIdentifier { get; set; }

  public List<SerializableStackFrame> StackTrace { get; set; }

  public string Type { get; set; }
}
