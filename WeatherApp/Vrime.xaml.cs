using WeatherApp.Service;

namespace WeatherApp;

public partial class Vrime : ContentPage
{

    public List<Models.List> WeatherList;
    private double latitude;
    private double longitude;


    public Vrime()
	{
		InitializeComponent();
	}

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await SearchLocation();
        await GetWeatherByLocation(latitude, longitude);

    }

    public async Task SearchLocation ()
    {
        var location = await Geolocation.GetLocationAsync();
        latitude = location.Latitude;
        longitude = location.Longitude;
    }

    private async void TapLoc_Tapped(object sender, EventArgs e)
    {
        await SearchLocation();
        await GetWeatherByLocation(latitude, longitude);
    }

    public async Task GetWeatherByLocation(double latitude, double longitude)
    {

        var result = await ApiService.GetWeather(latitude, longitude);
        UpdateResult(result);

    }

    private async void SearchButton_Clicked(object sender, EventArgs e)
    {
        string response = await DisplayPromptAsync(title: "", message: "", placeholder: "Choose Location", accept: "Search", cancel: "Cancel");
        if (response != null)
        {
            await GetWeatherByCityName(response);
        }
    }

    public async Task GetWeatherByCityName(string city)
    {

        var result = await ApiService.GetWeatherByCity(city);
        UpdateResult(result);

    }


    public void UpdateResult(dynamic result)
    {
        WeatherList = new List<Models.List>(result.list);
       
        CollectionWeather.ItemsSource = WeatherList;

        LabelCity.Text = result.city.name;
        LabelWeatherDesc.Text = result.list[0].weather[0].description;
        LabelTemperature.Text = result.list[0].main.temperature + "°C";
        LabelHumidity.Text = result.list[0].main.humidity + "%";
        LabelWind.Text = result.list[0].wind.speed + "km/h";
        WeatherIcon.Source = result.list[0].weather[0].iconUrl;

    }
}