using System;
using System.Collections;
using System.IO;

namespace Files
{
	class Program
	{

		static void Main(string[] args)
		{
			string rootPath = @"..\..\..\Dirs";
			Directory.SetCurrentDirectory(rootPath);


			FileStream fileStream = new FileStream(@".\a.txt", FileMode.Open, FileAccess.ReadWrite);

			StreamWriter streamWriter = new StreamWriter(fileStream); // stream writer не наследуется от стрима, но композирует его, получая доступ к его интерфейсу и возможность работать с ним и за счет этого мы получаем возможность работать со стримом через стримрайтер

			streamWriter.Write("abcde");
			streamWriter.Flush();

			//streamWriter.Close();

			fileStream.Position = 0;

			StreamReader streamReader = new StreamReader(fileStream);
			string readString = streamReader.ReadLine();

			Console.WriteLine(readString);

			//Directory.CreateDirectory(@".\dir3\dir4");
			//Console.WriteLine(Directory.Exists(@".\dir5"));

			//Directory.Delete(@".\dir5");

			//var dirs = Directory.EnumerateDirectories(".", ".", SearchOption.AllDirectories);
			//foreach (var dir in dirs)
			//{
			//	Console.WriteLine(dir);
			//}


			//var files = Directory.EnumerateFiles(".", ".", SearchOption.AllDirectories);
			//foreach (var f in files)
			//{
			//	var lastModified = Directory.GetLastWriteTime(f);

			//	var parentDir = Directory.GetParent(f);
			//	Console.WriteLine($"{ f }, last modified: { lastModified }");
			//	Console.WriteLine($"Parent dir: { parentDir.FullName }, { parentDir.Attributes }, { parentDir.LastWriteTime }");

			//	Console.WriteLine();
			//	Console.WriteLine();
			//}

			//File.WriteAllText(@"./b.txt", "rjif");

			//File.AppendAllLines(@"./b.txt", new string[] { "line1", "line2" });
			//File.Copy("./b.txt", "./dir2/b.txt", true);

			//File.Create("./b.txt", 20, FileOptions.RandomAccess);

			//byte[] bytes = File.ReadAllBytes("./b.txt");
			//PrintCollection(bytes);

			//string str = File.ReadAllText("./b.txt");
			//Console.WriteLine(str);

			//DirectoryInfo directoryInfo = new DirectoryInfo(".");
			//DirectoryInfo[] fileSystemInfos =  directoryInfo.GetDirectories();

			StreamReader streamReader = new StreamReader(@"./b.txt");
			//Console.WriteLine((char)streamReader.Read());
			//Console.WriteLine((char)streamReader.Read());
			//Console.WriteLine((char)streamReader.Read());
			//Console.WriteLine((char)streamReader.Read());
			//Console.WriteLine((char)streamReader.Read());

			var stream = new FileStream(@".\b.txt", FileMode.Open);

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
