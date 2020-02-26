using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics.CodeAnalysis;

namespace Files_SyncFolders_Realtime
{
	class Program
	{
		static void Main(string[] args)
		{
			string rootPath = @"..\..\..\Dirs";
			Directory.SetCurrentDirectory(rootPath);

			var startingDir1 = new DirectoryInfo(@".\dir1");
			var startingDir2 = new DirectoryInfo(@".\dir2");

			MakeDirsIdentical(startingDir1, startingDir2);
		}

		static void MakeDirsIdentical(DirectoryInfo dir1, DirectoryInfo dir2)
		{

			#region Copy files

			var filesDir1 = dir1.GetFiles("*", SearchOption.TopDirectoryOnly);
			var filesDir2 = dir2.GetFiles("*", SearchOption.TopDirectoryOnly);

			// Get files from dir1 that don't exist in dir2 and copy them to dir2
			var filesToCopy = filesDir1.Except(filesDir2, new FileInfoEqualityComparer());

			foreach (var file in filesToCopy)
			{
				Console.WriteLine($@"Copying { file.Name } from { dir1.FullName } to { dir2.FullName }");
				Console.WriteLine();

				file.CopyTo(@$"{ dir2.FullName }\{ file.Name }", true);
			}
			#endregion

			#region Delete files
			// Get files from dir2 that don't exist in dir1 and delete them
			var filesToDelete = filesDir2.Except(filesDir1, new FileInfoEqualityComparer());

			foreach (var file in filesToDelete)
			{
				Console.WriteLine($"Deleting { file.FullName }");
				Console.WriteLine();

				file.Delete();
			}
			#endregion

			#region Copy subdirs

			var subdirsDir1 = dir1.GetDirectories();
			var subdirsDir2 = dir2.GetDirectories();

			// Get subdirectories in dir1 that don't exist in dir2
			var subdirsToCopy = subdirsDir1.Except(subdirsDir2, new DirectoryInfoEqualityComparer());

			// and copy them to dir2
			foreach (var subdir in subdirsToCopy)
			{
				Console.WriteLine($"Copying { subdir.Name } from { subdir.FullName } to { dir2.FullName }");
				Console.WriteLine();

				var subdirTarget = dir2.CreateSubdirectory(subdir.Name);

				CopyAll(subdir, subdirTarget);
			}
			#endregion

			#region Delete subdirs

			// Get subdirs in dir2 that don't exist in dir1
			var dirsToDelete = subdirsDir2.Except(subdirsDir1, new DirectoryInfoEqualityComparer());

			// Delete them
			foreach (var dir in dirsToDelete)
			{
				Console.WriteLine($"Deleting { dir.FullName }");
				Console.WriteLine();

				dir.Delete(true);
			}
			#endregion

			#region Run MakeDirsIdentical on the subdirs with the same name

			// Get names of identical subdirs
			var identicalSubdirsNames = subdirsDir1.Intersect(subdirsDir2, new DirectoryInfoEqualityComparer()).Select(s => s.Name);

			// Run MakeDirsIdentical recursively on each pair of subdirs with the same name
			foreach (var subdirName in identicalSubdirsNames)
			{
				MakeDirsIdentical(new DirectoryInfo(@$"{ dir1.FullName }\{ subdirName }"), new DirectoryInfo(@$"{ dir2.FullName }\{ subdirName }"));
			}
			#endregion
		}

		static void CopyAll(DirectoryInfo source, DirectoryInfo target)
		{
			if (!target.Exists)
			{
				Directory.CreateDirectory(target.FullName);
			}

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

	}
}
