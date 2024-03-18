using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspirusApp
{
	public class ProcessIncomingFiles
	{
		Logger logger = new Logger();

	
		public void SplitIncomingFile()
		{

			string incomingfilePath = ConfigurationManager.AppSettings["localIncomingPath"];
			string ArchiveincomingfilePath = ConfigurationManager.AppSettings["localIncomingArchivePath"];
			string [] files= Directory.GetFiles(incomingfilePath);
			foreach (string file in files)
			{
				Dictionary<string, List<string>> groupedData = GroupData(file);
				foreach (var kvp in groupedData)
				{
					string customerId = kvp.Key.Split('|')[0];
					string vendorItemId = kvp.Key.Split('|')[1];
					string fileName = $"{customerId}_{vendorItemId}.csv";
					string filePath = Path.Combine(incomingfilePath, fileName);
					File.WriteAllLines(filePath, kvp.Value);
				}

				logger.InfoMessage("Data files created successfully for " + file);
				FileManipulation.FileManip.ArchiveFile(file, ArchiveincomingfilePath, false);
			}
		}

			static Dictionary<string, List<string>> GroupData(string filePath)
			{
				Dictionary<string, List<string>> groupedData = new Dictionary<string, List<string>>();

				using (StreamReader reader = new StreamReader(filePath))
				{
				string header = reader.ReadLine(); // Read header

				// Skip header if exists
				//reader.ReadLine();

					while (!reader.EndOfStream)
					{
						string line = reader.ReadLine();
						string[] values = line.Split(',');

						string vendorItemId = values[2].Trim();
						string customerId = values[4].Trim();
						string data = string.Join(",", values.ToArray());

						string key = $"{customerId}|{vendorItemId}";

						if (!groupedData.ContainsKey(key))
						{
							groupedData[key] = new List<string>();
						   groupedData[key].Add(header); // Add header to grouped data

					}

					groupedData[key].Add(data);
					}
				}

				return groupedData;
			}
		}
	}
