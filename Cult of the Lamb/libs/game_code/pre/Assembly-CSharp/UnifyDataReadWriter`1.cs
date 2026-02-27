// Decompiled with JetBrains decompiler
// Type: UnifyDataReadWriter`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Data.Serialization;
using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using Unify;
using UnityEngine;

#nullable disable
public class UnifyDataReadWriter<T> : MMDataReadWriterBase<T>
{
  private Thread _thread;
  private static int count;

  static UnifyDataReadWriter() => Debug.Log((object) "UnifyDataReadWrtier: Static constructor");

  public UnifyDataReadWriter()
  {
    ++UnifyDataReadWriter<T>.count;
    Debug.Log((object) ("UnifyDataReadWriter: Constructor, count: " + (object) UnifyDataReadWriter<T>.count));
  }

  ~UnifyDataReadWriter()
  {
    --UnifyDataReadWriter<T>.count;
    Debug.Log((object) ("UnifyDataReadWriter: Destructor, count: " + (object) UnifyDataReadWriter<T>.count));
  }

  public override void Write(T data, string filename, bool encrypt, bool backup)
  {
    if (this._thread != null && this._thread.IsAlive)
    {
      Debug.Log((object) "Write thread is already running!".Colour(Color.red));
    }
    else
    {
      try
      {
        Debug.Log((object) ("UnifyDataReadWriter - Write File " + filename).Colour(Color.yellow));
        lock (InitUnifyGlobal._serializedMemoryStream)
        {
          if (InitUnifyGlobal._serializedMemoryStream.Length != 0L)
          {
            InitUnifyGlobal._serializedMemoryStream.SetLength(0L);
            InitUnifyGlobal._serializedMemoryStream.Seek(0L, SeekOrigin.Begin);
          }
          using (StreamWriter streamWriter = new StreamWriter((Stream) InitUnifyGlobal._serializedMemoryStream, Encoding.Default, 1024 /*0x0400*/, true))
          {
            using (JsonTextWriter jsonTextWriter = new JsonTextWriter((TextWriter) streamWriter))
              MMSerialization.JsonSerializer.Serialize((JsonWriter) jsonTextWriter, (object) data);
          }
          this._thread = new Thread((ThreadStart) (() =>
          {
            lock (InitUnifyGlobal._serializedMemoryStream)
            {
              UnifyDataReadWriter<T>.Zip(InitUnifyGlobal._outputMemoryStream, InitUnifyGlobal._serializedMemoryStream);
              SaveData.PutBytes(filename, InitUnifyGlobal._outputMemoryStream.GetBuffer(), (int) InitUnifyGlobal._outputMemoryStream.Length);
            }
          }));
          this._thread.Start();
          System.Action onWriteCompleted = this.OnWriteCompleted;
          if (onWriteCompleted == null)
            return;
          onWriteCompleted();
        }
      }
      catch (Exception ex)
      {
        Debug.Log((object) ex.Message.Colour(Color.red));
        Action<MMReadWriteError> onWriteError = this.OnWriteError;
        if (onWriteError != null)
          onWriteError(new MMReadWriteError(ex.Message));
        throw;
      }
    }
  }

  public override void Read(string filename)
  {
    try
    {
      Debug.Log((object) ("UnifyDataReadWriter - Read File " + filename).Colour(Color.yellow));
      if (this.FileExists(filename))
      {
        Debug.Log((object) "UnifyDataReadWriter - Read File - Successfully read file!".Colour(Color.yellow));
        this.DoLoad(filename);
      }
      else
      {
        Debug.Log((object) "UnifyDataReadWriter - Read File - File did not exist, creating new".Colour(Color.yellow));
        System.Action onCreateDefault = this.OnCreateDefault;
        if (onCreateDefault == null)
          return;
        onCreateDefault();
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex.Message.Colour(Color.red));
      Action<MMReadWriteError> onReadError = this.OnReadError;
      if (onReadError != null)
        onReadError(new MMReadWriteError(ex.Message));
      throw;
    }
  }

  private void DoLoad(string filename)
  {
    byte[] bytes = SaveData.GetBytes(filename);
    char ch1 = Convert.ToChar(bytes[0]);
    char ch2 = Convert.ToChar(bytes[1]);
    T obj;
    if (ch1 == 'Z' && ch2 == 'P' || ch1 == 'Z' && ch2 == 'B')
    {
      using (MemoryStream memoryStream = new MemoryStream(bytes))
      {
        memoryStream.Seek(2L, SeekOrigin.Begin);
        using (GZipStream gzipStream = new GZipStream((Stream) memoryStream, CompressionMode.Decompress, true))
        {
          using (StreamReader reader1 = new StreamReader((Stream) gzipStream))
          {
            using (JsonTextReader reader2 = new JsonTextReader((TextReader) reader1))
              obj = MMSerialization.JsonSerializer.Deserialize<T>((JsonReader) reader2);
          }
        }
      }
    }
    else
    {
      using (MemoryStream memoryStream = new MemoryStream(bytes))
        obj = (T) new XmlSerializer(typeof (T)).Deserialize((Stream) memoryStream);
    }
    Action<T> onReadCompleted = this.OnReadCompleted;
    if (onReadCompleted == null)
      return;
    onReadCompleted(obj);
  }

  public override void Delete(string filename)
  {
    Debug.Log((object) ("UnifyDataReadWriter - Delete File " + filename).Colour(Color.yellow));
    if (!this.FileExists(filename))
      return;
    Debug.Log((object) ("UnifyDataReadWriter - Deletion Successful " + filename).Colour(Color.yellow));
    SaveData.Delete(filename);
    System.Action deletionComplete = this.OnDeletionComplete;
    if (deletionComplete == null)
      return;
    deletionComplete();
  }

  public override bool FileExists(string filename)
  {
    Debug.Log((object) ("UnifyDataReadWriter - File Exists? " + filename).Colour(Color.yellow));
    return SaveData.Exists(filename);
  }

  private static void Zip(MemoryStream mso, MemoryStream data)
  {
    if (mso.Length != 0L)
    {
      mso.SetLength(0L);
      mso.Seek(0L, SeekOrigin.Begin);
    }
    mso.WriteByte((byte) 90);
    mso.WriteByte((byte) 66);
    using (GZipStream destination = new GZipStream((Stream) mso, CompressionMode.Compress))
    {
      data.Seek(0L, SeekOrigin.Begin);
      data.CopyTo((Stream) destination);
    }
  }
}
