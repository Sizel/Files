using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Files_SyncFolders
{
	class Program
	{
		static void Main(string[] args)
		{
			string rootPath = @"..\..\..\Dirs";
			Directory.SetCurrentDirectory(rootPath);

			var filesOfDir1 = Directory.GetFiles(@".\dir1");
			var filesOFDir2 = Directory.GetFiles(@".\dir2");

			PrintCollection(filesOfDir1);
			PrintCollection(filesOFDir2);


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
}
