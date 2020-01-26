using Restaurant.Core;
using System;
using System.Diagnostics;

namespace Restaurant.Wpf
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();
		}
		private void MetroWindow_Initialized(object sender, EventArgs e)
		{
			Title = $"restaurantsimulator, uhrzeit: {(FastClock.Instance.Time = Convert.ToDateTime("12:00")).ToShortTimeString()}";
			FastClock.Instance.OneMinuteIsOver += OnOneMinuteIsOverFromFastClock;
			Waiter.Instance.NewOrder += OnNewOrderFromWaiter;		
			Waiter.Instance.GetOrder();
		}
		private void OnOneMinuteIsOverFromFastClock(object source, DateTime time)
		{   // FastClock.Instance
			Title = $"restaurantsimulator, uhrzeit: {time.ToShortTimeString()}";
		}
		private void OnNewOrderFromWaiter(object source, Order order)
		{	// Waiter.Instance
			TextBlockLog.Inlines.Add(order.ToString());
			TextBlockLog.Inlines.Add("\n");
			//Debug.WriteLine(order.ToString());
		}
	}
}
