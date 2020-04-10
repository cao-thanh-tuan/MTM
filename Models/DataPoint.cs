using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace MTM.Models
{
    [DataContract]
    public class DataPoint
    {
		public DataPoint(string label, int y)
		{
			this.Label = label;
			this.Y = y;
		}

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		//Explicitly setting the name to be used while serializing to JSON.
		[DataMember(Name = "label")]
		public string Label { get; set; }

		//Explicitly setting the name to be used while serializing to JSON.
		[DataMember(Name = "y")]
		public int Y { get; set; }
	}
}
