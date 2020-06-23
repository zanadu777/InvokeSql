using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;

namespace SqlAutomation
{
    public class FilePath
    {
        private string filePath;
        public FilePath(FileInfo fileinfo)
        {
            filePath = fileinfo.FullName;
        }

        public FilePath(DirectoryInfo directory, FileInfo fileInfo)
        {
            filePath = Path.Combine(directory.FullName, fileInfo.Name);
        }


        public FilePath(DirectoryInfo directory, string fileName)
        {
            filePath = Path.Combine(directory.FullName, fileName);
        }

        public void SetExtension(string extension)
        {
            filePath = Path.Combine(Path.GetDirectoryName(filePath),
                Path.GetFileNameWithoutExtension(filePath) + extension);
        }

        public void Append(string text)
        {
            filePath = Path.Combine(Path.GetDirectoryName(filePath),
                Path.GetFileNameWithoutExtension(filePath) + text + Path.GetExtension(filePath));
        }

        public void Append(DateTime dateTime, string format)
        {
            Append(dateTime.ToString(format));
        }

        public override string ToString()
        {
            return filePath;
        }

        public FileInfo ToFileInfo()
        {
            return new FileInfo(filePath);
        }


        public static bool TestDirectoryPath(string path, List<string> errors = null)
        {
            return TestFileSystemPaths(path, errors);
        }

        public static bool TestFileSystemPaths(string path, List<string> errors = null)
        {
            bool isOk = true;
            if (!Path.IsPathRooted(path))
            {
                isOk = false;
                errors?.Add($"Path [{path}] does not have a root in the file system");
            }
            var invalidChars = System.IO.Path.GetInvalidFileNameChars();
            var invalidInPath = (from char c in Path.GetFileName(path)
                                 where invalidChars.Contains(c)
                                 select c.ToString()).ToArray();

            if (invalidInPath.Length > 0)
            {
                isOk = false;
                errors?.Add($"Path [{path}] has the following invalid characters [{string.Join(",", invalidInPath)}]");
            }

            return isOk;
        }

        public static bool TestFilePath(string path, List<string> errors = null)
        {
            return TestFileSystemPaths(path, errors);
        }



    }
}
