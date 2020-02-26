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
			string currentDir = @"..\..\..\Dirs";
			Directory.SetCurrentDirectory(currentDir);

			FileSystemWatcher fileWatcher = new FileSystemWatcher(@".\dir1");

			fileWatcher.IncludeSubdirectories = true;

			fileWatcher.NotifyFilter = NotifyFilters.Size | NotifyFilters.FileName;

			fileWatcher.Changed += OnFileChange;
			fileWatcher.Created += OnFileCreate;
			fileWatcher.Deleted += OnFileDelete;
			fileWatcher.Renamed += OnFileRename;

			fileWatcher.EnableRaisingEvents = true;


			FileSystemWatcher dirWatcher = new FileSystemWatcher(@".\dir1");

			dirWatcher.IncludeSubdirectories = true;

			dirWatcher.NotifyFilter = NotifyFilters.DirectoryName;

			dirWatcher.Created += OnDirCreate;
			dirWatcher.Deleted += OnDirDelete;
			dirWatcher.Renamed += OnDirRename;

			dirWatcher.EnableRaisingEvents = true;

			while (Console.Read() != 'q') { }
		}

		private static void OnDirRename(object sender, RenamedEventArgs e)
		{
			Console.WriteLine($"The name of { e.OldFullPath } was changed to { e.Name } in dir1 and dir2");

			string pathInDir2 = e.OldFullPath.Replace("dir1", "dir2");

			Directory.Move(pathInDir2, e.FullPath.Replace("dir1", "dir2"));
		}

		private static void OnDirDelete(object sender, FileSystemEventArgs e)
		{
			Console.WriteLine($@"The directory { e.FullPath } was deleted in both dir1 and dir2");

			Directory.Delete(e.FullPath.Replace("dir1", "dir2"));
		}

		private static void OnDirCreate(object sender, FileSystemEventArgs e)
		{
			Console.WriteLine($"{ e.FullPath } was created in dir1 and dir2");

			Directory.CreateDirectory(e.FullPath.Replace("dir1", "dir2"));
		}


		private static void OnFileRename(object sender, RenamedEventArgs e)
		{
			Console.WriteLine($"The name of { e.OldFullPath } was changed to { e.Name } in dir1 and dir2");

			string pathInDir2 = e.OldFullPath.Replace("dir1", "dir2");

			File.Move(pathInDir2, e.FullPath.Replace("dir1", "dir2"), true);
		}

		private static void OnFileCreate(object sender, FileSystemEventArgs e)
		{
			Console.WriteLine($"{ e.FullPath } was created in dir1 and dir2");

			File.Create(e.FullPath.Replace("dir1", "dir2")).Close();
		}

		private static void OnFileChange(object sender, FileSystemEventArgs e)
		{

			Console.WriteLine($@"The content of { e.FullPath } was changed in dir1 and dir2");

			FileInfo fi = new FileInfo(e.FullPath);

			fi.CopyTo(e.FullPath.Replace("dir1", "dir2"), true);

		}

		private static void OnFileDelete(object sender, FileSystemEventArgs e)
		{
			Console.WriteLine($@"The file { e.FullPath } was deleted in both dir1 and dir2");

			File.Delete(e.FullPath.Replace("dir1", "dir2"));
		}


	}
}
