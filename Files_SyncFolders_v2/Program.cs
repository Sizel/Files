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
			var dir2 = new DirectoryInfo(@".\dir2");

			var filesInDir1 = dir1.GetFiles();
			var filesInDir2 = dir2.GetFiles();

			var filesToCopy = filesInDir1.Except(filesInDir2, new FileInfoEqualityComparer());

			foreach (var file in filesToCopy)
			{
				file.CopyTo($@"{ dir2.FullName }\{ file.Name }");
			}

			var filesToDelete = filesInDir2.Except(filesInDir1, new FileInfoEqualityComparer());

			foreach (var file in filesToDelete)
			{
				file.Delete();
			}

			string s = "f";
			s.GetHashCode();
			//MakeDirsIdentical(dir1, dir2);
		}

		static int GetHashForFile(FileInfo f)
		{
			SHA256 sha256 = SHA256.Create();

			FileStream fileStream = f.OpenRead();

			byte[] hash = sha256.ComputeHash(fileStream);

			fileStream.Close(); // why is the stream not closed automaticly after return?

			return BitConverter.ToInt32(hash, 0);
		}

		static void MakeDirsIdentical(DirectoryInfo dir1, DirectoryInfo dir2)
		{
			var filesDir1 = dir1.GetFiles("*", SearchOption.TopDirectoryOnly);
			var filesDir2 = dir2.GetFiles("*", SearchOption.TopDirectoryOnly);

			#region Copy
			var filesToCopy = filesDir1.Except(filesDir2, new FileInfoEqualityComparer());

			foreach (var file in filesToCopy)
			{
				file.CopyTo(@$"{ dir2.FullName }\{ file.Name }", true);
			}
			#endregion

			#region Delete
			var filesToDelete = filesDir2.Except(filesDir1, new FileInfoEqualityComparer());

			foreach (var file in filesToDelete)
			{
				file.Delete();
			}
			#endregion

			var subdirsDir1 = dir1.GetDirectories();
			var subdirsDir2 = dir2.GetDirectories();

			var dirsToCopy = subdirsDir1.Except(subdirsDir2, new DirectoryInfoEqualityComparer());

			foreach (var dir in dirsToCopy)
			{
				var subdirToCopyTo = dir2.CreateSubdirectory(dir.Name);
				CopyAll(dir, subdirToCopyTo);
			}

			var dirsToDelete = subdirsDir2.Except(subdirsDir1, new DirectoryInfoEqualityComparer());

			foreach (var dir in dirsToDelete)
			{
				dir.Delete(true);
			}

			var identicalDirs = subdirsDir1.Intersect(subdirsDir2, new DirectoryInfoEqualityComparer());

			foreach (var dir in identicalDirs)
			{
				var identicalSubdirIndir1 = subdirsDir1.Where(s => s.Name.Equals(dir.Name));
				var identicalSubdirIndir2 = subdirsDir2.Where(s => s.Name.Equals(dir.Name));
				MakeDirsIdentical(identicalSubdirIndir1.First(), identicalSubdirIndir2.First());
			}
		}

		static void CopyAll(DirectoryInfo source, DirectoryInfo target)
		{
			Directory.CreateDirectory(target.FullName);

			// Copy each file into the new directory.
			foreach (FileInfo fi in source.GetFiles())
			{
				fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
			}

			// Copy each subdirectory using recursion.
			foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
			{
				DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
				CopyAll(diSourceSubDir, nextTargetSubDir);
			}
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

	class FileInfoEqualityComparer : IEqualityComparer<FileInfo>
	{
		public bool Equals([AllowNull] FileInfo x, [AllowNull] FileInfo y)
		{
			return GetHashCode(x) == GetHashCode(y) && x.Name.Equals(y.Name);
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

	class DirectoryInfoEqualityComparer : IEqualityComparer<DirectoryInfo>
	{
		public bool Equals([AllowNull] DirectoryInfo x, [AllowNull] DirectoryInfo y)
		{
			return x.Name.Equals(y.Name);
		}

		public int GetHashCode([DisallowNull] DirectoryInfo obj)
		{
			return 0;
		}
	}
}
