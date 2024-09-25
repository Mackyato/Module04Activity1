using System.Runtime.CompilerServices;

namespace Module04Activity1
{
    public partial class MainPage : ContentPage
    {


        public MainPage()
        {
            InitializeComponent();
        }


        //<<---cs for longitude and latitude locations
        private async void OnGetLocationClicked(object sender, EventArgs e)
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.High
                    });
                }

                if (location != null)
                {
                    LocationLabel.Text = $"Latitude: {location.Latitude}, Longtitude: {location.Longitude}";

                    // Get Geocoding - Get address from Longtitude and Latitude
                    //GECODING STARTS HERE

                    var placemarks = await Geocoding.Default.GetPlacemarksAsync(location.Latitude, location.Longitude);

                    var placemark = placemarks?.FirstOrDefault();

                    if (placemark != null)
                    {
                        AddressLabel.Text = $"Address: {placemark.Thoroughfare}," +
                            $"{placemark.Locality}, " +
                            $"{placemark.AdminArea}," +
                            $"{placemark.PostalCode}," +
                            $"{placemark.CountryName}";
                    }
                    else
                    {
                        AddressLabel.Text = "Unable to determie the address";
                    }
                    //GEOCODING ENDS HERE
                }
                else
                {
                    LocationLabel.Text = "Unable to get Location";
                }
            }
            catch (Exception ex)
            {
                LocationLabel.Text = $"ERROR: {ex.Message}";
            }
        }
        //END HERE-->>


        //CAMERA
        //<<-- EVENT CAPTURE IMAGE
        private async void OnCapturePhotoClicked(object sender, EventArgs e)
        {
            try
            {
                if(MediaPicker.Default.IsCaptureSupported)
                {
                    //capture photo using Media Picker
                    FileResult photo = await MediaPicker.Default.CapturePhotoAsync();
                    if(photo != null)
                    {
                        await LoadPhotoAsync(photo);
                    }
                    
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occured: {ex.Message}", "OK");
            }
        }

        //LOAD PHOTO AND DISPLAY IT IN THE IMAGE CONTROL
        private async Task LoadPhotoAsync(FileResult photo)
        {
            if (photo == null)
                return;

                Stream stream = await photo.OpenReadAsync();

                CaptureImage.Source = ImageSource.FromStream(() => stream);
        }
        //END HERE -->>
    }
}
