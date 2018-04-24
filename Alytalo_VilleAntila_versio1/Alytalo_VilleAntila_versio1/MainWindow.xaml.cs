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
using System.Windows.Threading; //tarvitaan timeriin!

namespace Alytalo_VilleAntila_versio1
{
    
    public partial class MainWindow : Window
    {
        public Lights LamppuOlohuone = new Lights();
        public Lights LamppuKeittio = new Lights();
        public Sauna Sauna1 = new Sauna();
        public Thermostat Termostaatti = new Thermostat();

        // tarvitaan kaksi timeria, lämmitykseen ja jäähtymiseen
        public DispatcherTimer SaunaTimer = new DispatcherTimer();
        public DispatcherTimer SaunaTimer2 = new DispatcherTimer();

        // käytetään vakioita selkeyden ja muokattavuuden vuoksi
        public int TALON_MAX_LAMPOTILA = 28;
        public int TALON_MIN_LAMPOTILA = 14;
        public int SAUNAN_TAVOITELAMPOTILA = 85;
        public int ALOITUSLAMPOTILA = 20;
        public int VALOT_HAMARA = 33;
        public int VALOT_PUOLIVALOT = 66;
        public int VALOT_KIRKAS = 100;

        public MainWindow()
        {
            InitializeComponent();

            // kaikki lamput ovat aluksi pois päältä sekä talon ja saunan lämpö aloituslämpötilassa
          
            LamppuOlohuoneTila.Text = "Olohuoneen" + AsetaKirkkaus(LamppuOlohuone, 0);
            LamppuKeittioTila.Text = "Keittiön" + AsetaKirkkaus(LamppuKeittio, 0);

            Termostaatti.Temperature = ALOITUSLAMPOTILA;
            Lampotila.Text = "Lämpötila on " + Termostaatti.Temperature + " astetta.";

            Sauna1.SaunaTemperature = ALOITUSLAMPOTILA;

            // saunan ajastimet
            SaunaTimer.Tick += SaunaLampenee;
            SaunaTimer2.Tick += SaunaJaahtyy;
            SaunaTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            SaunaTimer2.Interval = new TimeSpan(0, 0, 0, 0, 50);

        }

        // metodi lamppujen ohjaukseen, palauttaa lampun tilan stringinä
        private string AsetaKirkkaus(Lights lamppu, int kirkkaus)
        {
            if (kirkkaus > 0)
            {
                lamppu.Switched = true;
                lamppu.Dimmer = kirkkaus.ToString();
                return (" lamppu on päällä, kirkkaus " + lamppu.Dimmer + " %.");
            }
            else
            {
                lamppu.Switched = false;
                lamppu.Dimmer = "0";
                return (" lamppu ei ole päällä.");
            }
          
        }

        // metodit saunan lämpötilan muutokseen
        private void SaunaLampenee(object sender, EventArgs e)
        {
            Sauna1.SaunaTemperature++;
            Sauna1Lampotila.Text = "Saunan lämpötila on " + Sauna1.SaunaTemperature.ToString() + " astetta.";
            if (Sauna1.SaunaTemperature >= SAUNAN_TAVOITELAMPOTILA)
            {
                SaunaTimer.Stop();
            }
        }

        private void SaunaJaahtyy(object sender, EventArgs e)
        {
            Sauna1.SaunaTemperature--;
            Sauna1Lampotila.Text = "Saunan lämpötila on " + Sauna1.SaunaTemperature.ToString() + " astetta.";
            if (Sauna1.SaunaTemperature <= ALOITUSLAMPOTILA)
            {
                SaunaTimer2.Stop();
                Sauna1Lampotila.Text = "";
            }
        }

        // Olohuoneen lampun ohjaus napeilla
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LamppuOlohuoneTila.Text = "Olohuoneen" + AsetaKirkkaus(LamppuOlohuone, 0);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            LamppuOlohuoneTila.Text = "Olohuoneen" + AsetaKirkkaus(LamppuOlohuone, VALOT_HAMARA);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            LamppuOlohuoneTila.Text = "Olohuoneen" + AsetaKirkkaus(LamppuOlohuone, VALOT_PUOLIVALOT);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            LamppuOlohuoneTila.Text = "Olohuoneen" + AsetaKirkkaus(LamppuOlohuone, VALOT_KIRKAS);
        }

        // Keittiön lampun ohjaus sliderilla
        private void Slider_KeittioMuuttuu(object sender, RoutedEventArgs e)
        {
            LamppuKeittioTila.Text = "Keittiön" + AsetaKirkkaus(LamppuKeittio, (int)sliderKeittio.Value);
        }
        
        // Saunan virtakytkin, vain yksi timer saa kerrallaan olla päällä
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Sauna1.Kytke();
            if (Sauna1.Switched)
            {
                SaunaTimer2.Stop();
                SaunaTimer.Start();
                Sauna1Tila.Text = "SAUNA PÄÄLLÄ";
            }
            else
            {
                SaunaTimer.Stop();
                SaunaTimer2.Start();
                Sauna1Tila.Text = "";
            }
        }

        // Talon lämpötilan säätö
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            { 
                int apu = Int32.Parse(TavoiteLampotila.Text);
                if (apu > TALON_MAX_LAMPOTILA)
                {
                    apu = TALON_MAX_LAMPOTILA;
                }
                if (apu < TALON_MIN_LAMPOTILA)
                {
                    apu = TALON_MIN_LAMPOTILA;
                }
                Termostaatti.SetTemperature(apu);
                Lampotila.Text = "Lämpötila on " + Termostaatti.Temperature.ToString() + " astetta.";
            }
            catch
            {
                MessageBox.Show("Kokonaisluku, kiitos!");
            }
            TavoiteLampotila.Text = "";
        }
    }
}
