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
using System.Timers;
using System.Windows.Threading;

namespace Juste_Prix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        int timed = 60;
        readonly int max = 100;
        readonly int maxSeconds = 0;
        readonly int min = 0;
        int numberOfTries = 0;
        string inputTxt;
        int inputToInt;
        int randomNumber;
        bool timerIsOn;

        public MainWindow()
        {
            InitializeComponent();
            NewGame();
        }

        //Initialise le timer
        bool Timer()
        {
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(dispatcherTimer_Tick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.IsEnabled = true;
            timer.Start();
            return timerIsOn = true;
        }

        //Evénements à chaque Tick
        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            timed--;
            txtBlockSeconds.Text = "Nombre de secondes restantes : " + timed;
            TimedRunnedOut();
        }

        //Méthode permettant d'arrêter le timer en cas de délai dépassé
        void TimedRunnedOut()
        {
            if (timed <= maxSeconds)
            {
                timer.IsEnabled = false;
                timer.Stop();
                txtBlockInfo.Text = "Délai dépassé, c'est perdu";
            }
        }


        void StopTimer()
        {
            timer.IsEnabled = false;
            timer.Stop();
        }


        //Permet de savoir si le jeu est en cours
        bool GameIsOn()
        {
            if (timed == maxSeconds || numberOfTries > 5)
            {
                txtBlockInfo.Text = "il faut faire une nouvelle partie";
                return false;
            }
            else return true;
        }


        //Logique principale du programme
        void Engine()
        {
            bool isNumeric = int.TryParse(inputTxt, out inputToInt);

            if (GameIsOn() == true)
            {
                if (isNumeric == true && inputToInt == 0)
                    txtBlockInfo.Text = "La valeur doit être supérieur à 0";
                else
                    txtBlockInfo.Text = "Il faut rentrer un nombre";


                if (isNumeric == true && inputToInt != 0)
                {
                    if (inputToInt > max || inputToInt < min)
                        txtBlockInfo.Text = "La valeur doit être entre 1 et 100";
                    else
                    {
                        if (inputToInt > randomNumber)
                        {
                            txtBlockInfo.Text = "C'est moins";
                            numberOfTries++;
                            if (numberOfTries > 5)
                            {
                                txtBlockInfo.Text = "C'est perdu, trop d'essais";
                                StopTimer();
                            }
                        }
                        else
                        {
                            txtBlockInfo.Text = "C'est plus";
                            numberOfTries++;
                            if (numberOfTries > 5)
                            {
                                txtBlockInfo.Text = "C'est perdu, trop d'essais";
                                StopTimer();
                            }
                        }
                        if (inputToInt == randomNumber)
                            Victory();
                    }
                }
            }
        }

        //Méthode Nouvelle pour faire une nouvelle partie
        void NewGame()
        {
            randomNumber = RandomedNumber();
            txtBlockInfo.Text = "Rentrez un nombre";

            timed = 60;
            numberOfTries = 0;

            txtBlockTry.Text = "Nombre d'essais : " + numberOfTries;
            txtBlockSeconds.Text = "Nombre de secondes restantes : " + timed;
            txtBoxInput.Text = string.Empty;
            if (timerIsOn == true)
            {
                StopTimer();
            }
            Timer();
        }

        //Méthode pour l'attribution d'un nombre aléatoire
        int RandomedNumber()
        {
            return new Random().Next(min, max);
        }

        void UpdateTry()
        {
            txtBlockTry.Text = "Nombre d'essais : " + numberOfTries;
        }

        void Victory()
        {
            txtBlockInfo.Text = "Victoire !";
            StopTimer();
        }


        private void btnValidate_Click(object sender, RoutedEventArgs e)
        {
            inputTxt = txtBoxInput.Text;
            if (inputTxt == string.Empty)
                txtBlockInfo.Text = "Il faut rentrer un nombre";
            if (numberOfTries < 6)
                Engine();
            UpdateTry();
        }

        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
        }
    }
}