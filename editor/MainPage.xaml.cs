using System.Collections.ObjectModel;

namespace Argon.Editor; 
public partial class MainPage : ContentPage {
	ObservableCollection<string> maps = new ObservableCollection<string>();
	ObservableCollection<string> assets = new ObservableCollection<string>();

	public MainPage() {
		InitializeComponent();

		maps.Add("Zolder");
		maps.Add("Heusden");
		maps.Add("Bolderberg");
		maps.Add("Boekt");
		maps.Add("Eversel");
		maps.Add("Viversel");
		maps.Add("Berkenbos");
		maps.Add("Voort");

		mapList.ItemsSource = maps;

		assets.Add("brood");
		assets.Add("kaas");
		assets.Add("eieren");
		assets.Add("boter");
		assets.Add("chocolade");
		assets.Add("appel");
		assets.Add("peer");
		assets.Add("salami");
		assets.Add("taart");
		assets.Add("pasta");
		assets.Add("pizza");
		assets.Add("bloem");
		assets.Add("suiker");
		assets.Add("zout");

		assetList.ItemsSource = assets;
	}
}