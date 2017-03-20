using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace decision_tree_learning
{
	public class FileReader
	{
		List<Attribute> attributes = new List<Attribute> ();
		List<DataPoint> examples;

		public FileReader ()
		{
		}

		public void ReadFile(string filename, bool isTestingFile = false)
		{
			examples = new List<DataPoint> ();

			if (!File.Exists (filename)) {
				Console.WriteLine ("File \"{0}\" does not exist.", filename);
				Environment.Exit (1);
			}
				
			try {
				Regex relation = new Regex (@"^@(?i)RELATION(?-i)\s+(?<name>.*?)$");
				Regex attribute = new Regex (@"^@(?i)ATTRIBUTE(?-i)\s+(?<name>.*?)\s+(?<values>(.+))$");
				Regex data = new Regex (@"^@(?i)DATA(?-i)$");
		 
				StreamReader sr = new StreamReader (filename);
				string line = null;

				while ((line = sr.ReadLine ()) != null) {
					if (relation.IsMatch (line)) {
						continue;
					}

					if (attribute.IsMatch (line)) {
						if (isTestingFile)
							continue;
						Match match = attribute.Match (line);

						string name = match.Groups ["name"].Value;
						string values = match.Groups ["values"].Value;
						Attribute newAttribute = new Attribute(name, values);
						attributes.Add(newAttribute);
						continue;
					}

					if (data.IsMatch (line)) {
						while ((line = sr.ReadLine ()) != null) {
							if (line == "")
								continue;
							DataPoint example = new DataPoint();
							bool success = example.AddDataInfo(attributes, line);
							if (success || isTestingFile)
								examples.Add(example);
						}
						break;
					}
				}
				 
			} catch (Exception e) {
				Console.WriteLine ("File \"{0}\" could not be read.", filename);
				Console.WriteLine (e.Message);
				Environment.Exit (1);
			}
		}

		public List<Attribute> GetAttributes()
		{
			return attributes;
		}

		public List<DataPoint> GetData()
		{
			return examples;
		}
	}
}