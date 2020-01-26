using System.Collections.Generic;
using Utils;

namespace Restaurant.Core
{
	public class Order
	{
		private const string _pathToArticles = "Articles.csv";

		private Guest _guest;
		private static List<Guest> _guestList = new List<Guest>();
		public Article _article;
		public static string[][] _articles;
		public Order(string delay, string name, string orderTypeParameter, string articleParameter)
		{
			Delay = delay;
			Name = name;
			OrderTypeParameter = orderTypeParameter;
			ArticleParameter = articleParameter;
			if (_articles == null)
			{
				_articles = MyFile.ReadStringMatrixFromCsv(_pathToArticles, true);
			}
			int orderPlace = 0;	//Search for ordered article position in the table
			for (int i = 0; i < _articles.Length; i++)
			{
				bool check = false;
				for (int j = 0; j < _articles[i].Length; j++)
				{
					if (_articles[i][j].Equals(ArticleParameter))
					{
						orderPlace = i;
						check = true;
						break;
					}
				}	// if article found
				if (check) break;
			}
			_article = new Article(ArticleParameter, double.Parse(_articles[orderPlace][(int)OrderType.ToPay]), int.Parse(_articles[orderPlace][(int)OrderType.Ready]));
			_guest = new Guest(Name, _article.ArticlePreis);
			if(orderTypeParameter == "Order")
			{	// Bill
				_guestList.Add(_guest);
			}
		}

		public string Delay { get; set; }
		public string Name { get; set; }
		public string OrderTypeParameter { get; set; }
		public string ArticleParameter { get; set; }

		public override string ToString()
		{
			if (OrderTypeParameter.Equals(OrderType.Order.ToString()))
			{
				return $"{Delay} {_article.ArticleName} für {_guest.Name} ist bestellt.";
			}
			else if (OrderTypeParameter.Equals(OrderType.Ready.ToString()))
			{
				return $"{Delay} {_article.ArticleName} für {_guest.Name} wird serviert.";
			}
			else if (OrderTypeParameter.Equals(OrderType.ToPay.ToString()))
			{
				double d = 0;
				foreach (var item in _guestList)
				{
					if (item.Name.Equals(_guest.Name))
					{
						d += item.Sum;
					}
				}
				return $"{Delay} {_guest.Name} bezahlt {d} Euro.";
			}
			else return "Keine Gäste mehr...";
		}
	}
}
