using Media.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blindist1;
using Xamarin.Forms;
using Refractored.Xam.TTS;

namespace Blindist
{
    public partial class ScanPage : ContentPage
    {
        public ScanPage()
        {
            InitializeComponent();
          //  CrossTextToSpeech.Current.Speak("HI there");
           CrossTextToSpeech.Current.Speak("Hello there, I am here to help you, Please click the screen two times with 5 second gap for knowing what is in front of you. Point your Smartphone camera to the direction required.");
        }
        
        async void ScanButtonClicked(object sender, EventArgs args)
        {
            ScanButton.IsEnabled = false;
            ImageProcess img = new ImageProcess();
            img =  await img.GetImage();

            if(img !=null)
            {
                if(img.img!=null)
                {
                    image.Source = img.img;
                  
                    string temp = await img.ProcessImage(img.imgraw);
                    if (temp == null)
                    {
                        CrossTextToSpeech.Current.Speak("I can't connect at the moment, try again!");
                        return;
                    }

                    var labels =  Blindist1.Operations.GetMatch(temp);
                    labels = labels.OrderByDescending(x => x.label_score).ToList();

                    CrossTextToSpeech.Current.Speak("I guess there is " + labels[0].label_name + " in front of you" + "Also there might be " + labels[1].label_name + " in front of you");
                    ScanButton.IsEnabled = true;

                }
                else
                {
                    await DisplayAlert("Error", img.error, "Ok");
                    ScanButton.IsEnabled = true;
                }
            }

        }

    }
}
