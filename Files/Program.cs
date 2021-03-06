﻿using System;
using System.Collections;
using System.IO;

namespace Files
{
	class Program
	{

        static void Main(string[] args)
        {
            string sourceDirectory = @"C:\temp\source";
            string targetDirectory = @"C:\temp\destination";

            Copy(sourceDirectory, targetDirectory);

            

            Console.WriteLine("\r\nEnd of program");
        }

        public static void Copy(string sourceDirectory, string targetDirectory)
        {
            var diSource = new DirectoryInfo(sourceDirectory);
            var diTarget = new DirectoryInfo(targetDirectory);

            MyCopyAll(diSource, diTarget);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        static void MyCopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            if (!target.Exists)
            {
                Directory.CreateDirectory(target.FullName);
            }

            var filesInSource = source.GetFiles();

            foreach (var file in filesInSource)
            {
                file.CopyTo($@"{ target.FullName }\{ file.Name }", true);
            }

            var subdirsInSource = source.GetDirectories();

            foreach (var subdir in subdirsInSource)
            {
                var subdirInTarger = target.CreateSubdirectory(subdir.Name);
                MyCopyAll(subdir, subdirInTarger);
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
}
