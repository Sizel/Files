using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics.CodeAnalysis;

namespace Files_SyncFolders
{
	class Program
	{
		static void Main(string[] args)
		{
			string rootPath = @"..\..\..\Dirs";
			Directory.SetCurrentDirectory(rootPath);

			var dir1 = new DirectoryInfo(@".\dir1");
			var filesDir1 = dir1.GetFiles("*", SearchOption.TopDirectoryOnly);

			Directory.CreateDirectory()

			var dir2 = new DirectoryInfo(@".\dir2");
			var filesDir2 = dir2.GetFiles("*", SearchOption.TopDirectoryOnly);

			#region Copy
			var filesToCopy = filesDir1.Except(filesDir2, new FileInfoEqualityComparer());

			foreach (var file in filesToCopy)
			{
				file.CopyTo(@$"{ dir2.Name }\{ file.Name }", true);
			}
			#endregion

			#region Delete
			var filesToDelete = filesDir2.Except(filesDir1, new FileInfoEqualityComparer());

			foreach (var file in filesToDelete)
			{
				file.Delete();
			}
			#endregion
		}


		static void PrintCollection(IEnumerable c)
		{
			foreach (var item in c)
			{
				Console.WriteLine($"{ item } ");
			}
			Console.WriteLine();
		}
	}

	class FsInfoEqualityComparer : IEqualityComparer<FileSystemInfo>
	{
		public bool Equals([AllowNull] FileSystemInfo x, [AllowNull] FileSystemInfo y)
		{
			return x.GetHashCode() == y.GetHashCode();
		}

		public int GetHashCode([DisallowNull] FileSystemInfo obj)
		{
			return obj.GetHashCode();
		}
	}

	class FileInfoEqualityComparer : IEqualityComparer<FileInfo>
	{
		public bool Equals([AllowNull] FileInfo x, [AllowNull] FileInfo y)
		{
			return GetHashCode(x) == GetHashCode(y);
		}

		public int GetHashCode([DisallowNull] FileInfo file)
		{
			SHA256 sha256 = SHA256.Create();

			FileStream fileStream = file.OpenRead(); 
				
			byte[] hash = sha256.ComputeHash(fileStream);

			fileStream.Close(); // why is the stream not closed automaticly after return?

			return BitConverter.ToInt32(hash, 0);
		}
	}
}
