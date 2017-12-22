using System;
using System.IO;
using System.Linq;

namespace ConsoleApplication1
{
	class Program
	{
		private static string _keyStart = " (2015";
		private static string _keyEnd = " UTC)";

		static void Main(string[] args)
		{
			RecurseFolders(@"c:\dev\temp");
		}

		private static void RecurseFolders(string startPath)
		{
			try
			{
				RenameFilesInFolder(startPath);

				var dir = new DirectoryInfo(startPath);
				var folders = dir.EnumerateDirectories();

				foreach (var folder in folders)
					RecurseFolders(folder.FullName);
			}
			catch (Exception e)
			{
				Console.WriteLine("Failure: " + e);
			}
		}

		private static void RenameFilesInFolder(string folder)
		{
			try
			{
				var dir = new DirectoryInfo(folder);
				var files = dir.EnumerateFiles().Where(file => file.Name.Contains(_keyStart));

				Console.WriteLine("Found {0} matching files in {1}", files.Count(), dir);

				foreach (var file in files)
				{
					int start = file.Name.IndexOf(_keyStart);
					int end = file.Name.LastIndexOf(_keyEnd);
					if (start == -1 || end == -1)
						continue;

					end += _keyEnd.Length;

					var newFile = file.Name.Remove(start, end - start);
					var newPath = Path.Combine(file.Directory.FullName, newFile);
					File.Move(file.FullName, newPath);
					Console.WriteLine("Old: '{0}', New: '{1}'", file.Name, newFile);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("Failure: " + e);
			}
		}
	}
}
