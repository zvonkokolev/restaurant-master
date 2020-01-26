using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Utils;

namespace Restaurant.Core
{
	public class Waiter
	{
		private const string _pfadTasks = "Tasks.csv";
		private static Waiter _instance;
		private Queue<Order> _ordersOrdered = new Queue<Order>();
		private Queue<Order> _ordersServed = new Queue<Order>();
		private Queue<Order> _toPay = new Queue<Order>();
		public static Waiter Instance
		{
			get
			{
				if(_instance == null)
				{
					_instance = new Waiter();
				}
				return _instance;
			}
		}

		public event EventHandler<Order> NewOrder;

		private Waiter()
		{  //Hier meldet sich der Kelner als Beobachter der auf den Schnellen Uhr schaut
			FastClock.Instance.OneMinuteIsOver += OnOneMinuteIsOverFromFastClock;
		}

		private void OnOneMinuteIsOverFromFastClock(object sender, DateTime fastClockTime)
		{
			for (int i = 0; i < _ordersOrdered.Count; i++)
			{
				if (fastClockTime.ToShortTimeString().Equals(_ordersOrdered.ElementAt(i).Delay))
				{
					if(_ordersOrdered.Count > 0)
					{
						Waiter_NewOrder(_ordersOrdered.ElementAt(i));
					}
				}
			}
			for (int i = 0; i < _ordersServed.Count; i++)
			{
				if (fastClockTime.ToShortTimeString().Equals(_ordersServed.ElementAt(i).Delay))
				{
					if (_ordersServed.Count > 0)
					{
						Waiter_NewOrder(_ordersServed.ElementAt(i));
					}
				}
			}
		}

		public void GetOrder()
		{
			string pfad = MyFile.GetFullNameInApplicationTree(_pfadTasks);
			string[] lines = File.ReadAllLines(pfad, Encoding.Default);
			for (int i = 1; i < lines.Length; i++)
			{
				string[] line = lines[i].Split(';');
				double delayMinutes = double.Parse(line[0]);
				string displayDelayedMinutes = FastClock.Instance.Time.AddMinutes(delayMinutes).ToShortTimeString();
				Order order = new Order(displayDelayedMinutes, line[1], line[2], line[3]);
				_ordersOrdered.Enqueue(order);
			}
			ManageOrder();
		}

		protected virtual void Waiter_NewOrder(Order order)
		{
			NewOrder?.Invoke(this, order);
		}

		public void ManageOrder()
		{
			foreach (Order item in _ordersOrdered)
			{
				if (!item.OrderTypeParameter.Equals("ToPay"))
				{
					double delayMinutes = double.Parse(item.Delay.Remove(0, 3)) + (double)item._article.TimeToBuild;
					string displayDelayedMinutes = FastClock.Instance.Time.AddMinutes(delayMinutes).ToShortTimeString();
					Order order = new Order(displayDelayedMinutes, item.Name, "Ready", item.ArticleParameter);
					_ordersServed.Enqueue(order);
				}
			}
			ServeOrder();
		}

		private void ServeOrder()
		{
			foreach (Order item in _ordersOrdered)
			{
				if (item.OrderTypeParameter.Equals("ToPay"))
				{
					_toPay.Enqueue(item);
				}
			}
		}
	}
}
