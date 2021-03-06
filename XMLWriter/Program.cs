using System;
using System.IO;
using System.Xml.Serialization;

namespace XMLWriter
{
	static class Program
	{
		private static string _workingDirectory = @"C:\Temp\";
		private static string _fileName = "xmlfile.xml";
		private static Settings mySettings = new Settings();

		static void Main()
		{
			SaveSettings();
		}

		private static void BuildSettings()
		{
			mySettings.Id = 1234;
			mySettings.Name = "John";
		}

		private static void SaveSettings()
		{
			//Create settings.xml folder in AppData/RTFEditor
			if (!Directory.Exists(_workingDirectory))
				try
				{
					Directory.CreateDirectory(_workingDirectory);
				}
				catch (IOException myIoException)
				{
					Console.WriteLine(@"Exception {0} could not access directory", myIoException.GetType().Name);
				}

			try
			{
				File.Delete(_workingDirectory + _fileName);
			}
			catch (UnauthorizedAccessException myUnauthorizedAccessException)
			{
				Console.WriteLine(@"Exception {0} could not lock file!",
					myUnauthorizedAccessException.GetType().Name);
			}

			BuildSettings();
			try
			{
				using var myWriter = new StreamWriter(_workingDirectory + _fileName);
				XmlSerializer x = new XmlSerializer(typeof(Settings));
				x.Serialize(myWriter, mySettings);
			}
			catch (DirectoryNotFoundException myDirectoryNotFoundException)
			{
				Console.WriteLine(@"Exception {0} Directory not found!", myDirectoryNotFoundException.GetType().Name);
			}

			LoadSettings();
		}

		private static void LoadSettings()
		{
			try
			{
				var mySerializer = new XmlSerializer(typeof(Settings));
				using var myFileStream = new FileStream(_workingDirectory + _fileName, FileMode.Open);
				var myObject = (Settings)mySerializer.Deserialize(myFileStream);

				Console.WriteLine(myObject.Id);
				Console.WriteLine(myObject.Name);
			}
			catch (DirectoryNotFoundException e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}

	public class Settings
	{
		public int Id { get; set; }
		public string Name {get; set; }
	}
}
