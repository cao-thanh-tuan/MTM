using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MTM.Models
{
    public class DoWDataPoint
	{
		public List<DataPoint> MidNight = new List<DataPoint>();
		public List<DataPoint> Morning = new List<DataPoint>();
		public List<DataPoint> AfterNoon = new List<DataPoint>();
		public List<DataPoint> Night = new List<DataPoint>();
	}

	public class DoWDataPointClient
	{
		public string MidNight = "";
		public string Morning = "";
		public string AfterNoon = "";
		public string Night = "";
	}
}
