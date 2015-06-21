using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;

namespace TypedList {
	using System.ComponentModel;
	using System.Xml.Linq;
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
			var xe = XElement.Parse(@"<r><e a='av' b='bv'/><e a='av' b='bv' c='vvvvvvv v ' /></r>");
			this.DataContext = new { Items = xe.ToTypedElements() };
		}
	}
}
