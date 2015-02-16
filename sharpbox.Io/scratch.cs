  public class Client
  {
    /// <summary>
    /// Used to read the contents of a file using the IFilters present on the machine.
    /// </summary>
    private TextReader _reader;

    public Client() { }

    #region CRUD

    public Byte[] GetByFilename(string path)
    {
      return System.IO.File.ReadAllBytes(path);
    }

    public void Save(string path, Stream input)
    {
      using (Stream file = System.IO.File.Create(path))
      {
        CopyStream(input, file);
      }

    }

    public void Delete(string path)
    {
      System.IO.File.Delete(path);
    }

    #endregion

    #region Helper(s)

    public String ReadFileContents(string path)
    {
      try
      {
        TextReader reader = new FilterReader(path);
        using (reader)
        {
          var text = reader.ReadToEnd();
          return text;
        }
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public string[] ReadFileLines(string path)
    {
      return System.IO.File.ReadAllLines(path);
    } 

    /// <summary>
    /// Copies the contents of input to output. Doesn't close either stream.
    /// </summary>
    private void CopyStream(Stream input, Stream output)
    {
      var buffer = new byte[8 * 1024];
      int len;
      while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
      {
        output.Write(buffer, 0, len);
      }
    }

    /// <summary>
    /// Not a perfect solution to the "Is File locked?" aka "Don't return me until the write operatin is finished"...but there is no perfect solution @SEE: http://stackoverflow.com/questions/876473/is-there-a-way-to-check-if-a-file-is-in-use/937558#937558
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public virtual bool IsFileLocked(FileInfo file)
    {
      FileStream stream = null;

      try
      {
        stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
      }
      catch (IOException)
      {
        //the file is unavailable because it is:
        //still being written to
        //or being processed by another thread
        //or does not exist (has already been processed)
        return true;
      }
      finally
      {
        if (stream != null)
          stream.Close();
      }

      //file is not locked
      return false;
    }
    #endregion
  }
