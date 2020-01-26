using System;

namespace Restaurant.Core
{
	public class Guest
	{
		private double _sum;
		public Guest()
		{
		}
		public Guest(string name)
		{
			Name = name;
		}
		public Guest(string name, double summe)
		{
			Name = name;
			Sum = summe;
		}
		public string Name { get; set; }
		public double Sum { get; set; }
	}
}
