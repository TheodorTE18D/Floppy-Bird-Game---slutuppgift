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
using System.Windows.Threading;

namespace Floppy_Bird_Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer gameTimer = new DispatcherTimer();
        // Håller koll på spelarens poäng.
        double score;
        // En integer som håller värdet på gravitationen.
        int gravity = 8;
        // En boolean som kollar ifall spelet är över eller inte.
        bool gameOver;
        // En Rect som hjälper med att hålla koll på när det blir en kollosion.
        Rect flappyBirdHitBox;

        public MainWindow()
        {
            InitializeComponent();

            gameTimer.Tick += MainEventTimer;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            // Kör start funktionen för spelet.
            StartGame();
        }

        private void MainEventTimer(object sender, EventArgs e)
        {
            // Uppdaterar poängvisarn till den aktuella poängen i double "score".
            txtScore.Content = "Score: " + score;
            // Kopplar ihop flappyBird bilden till flappy rect classen så att de sitter ihop.
            flappyBirdHitBox = new Rect(Canvas.GetLeft(flappyBird), Canvas.GetTop(flappyBird), flappyBird.Width - 8, flappyBird.Height);
            // Kopplar ihop flappyBird bilden till gravitation integern.
            Canvas.SetTop(flappyBird, Canvas.GetTop(flappyBird) + gravity);

            // Denna loop kollar ifall flappyBird bilden har gått over eller under fönstrets gränser.
            if (Canvas.GetTop(flappyBird) < -10 || Canvas.GetTop(flappyBird) > 458)
            {
                // Kör funktionen för att avsluta spelet.
                EndGame();
            }

            // Denna loop kollar igenom alla bilder som finns med i spelet.
            foreach (var x in MyCanvas.Children.OfType<Image>())
            {
                
                if ((string)x.Tag == "obs1" || (string)x.Tag == "obs2" || (string)x.Tag == "obs3")
                {
                    // Om någon av bilderna med namnen "obs1", "obs2" eller "obs3" kommer de att flyttas från vänster till höger på skärmen.
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 5);

                    // Denna loop kollar ifall någon av bilderna har rört sig förbi skärmen och isåfall flyttar tillbaka de till startpositionen och lägger till en .
                    if (Canvas.GetLeft(x) < -100)
                    {
                        Canvas.SetLeft(x, 800);

                        score += .5;
                    }

                    // Skapar en Rect för rören som kommer agera som en hitbox.
                    Rect pipeHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    
                    // Denna loop kollar ifall flappyBirds hitbox träffar rörens hitbox och isånnafall kör "Endgame" funktionen.
                    if (flappyBirdHitBox.IntersectsWith(pipeHitBox))
                    {
                        EndGame();
                    }

                }
                // Denna loop kollar ifall det är några bilder som har namnet "cloud".
                if ((string)x.Tag == "cloud")
                {
                    // Flyttar bilderna "cloud" långsamt till vänster på skärmen.
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 2);

                    // Denna loop kollar ifall bilderna har kommit av skärmen och isånnafall flyttar tillbaka det till startpositionen.
                    if (Canvas.GetLeft(x) < -250)
                    {
                        Canvas.SetLeft(x, 550);
                    }

                }
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            // Kollar ifall knappen "pilen upp" är nertryckt. 
            if(e.Key == Key.Up)
            {   
                // Roterar flappybird bilden uppåt.
                flappyBird.RenderTransform = new RotateTransform(-20, flappyBird.Width /2, flappyBird.Height/ 2);
                // Ändrar gravitaion integern till - 8.
                gravity = -8;
            }
            // Kollar ifall knappen "R" är nertryckt och ifall boolean "gameOver" är sann.
            if(e.Key == Key.R && gameOver == true)
            {   
                // Kör start funktion och börjar om spelet.
                StartGame();
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            // Roterar flappybird bilden nedåt ifall "pilen upp" kanppen inte är nedtryckt.
            flappyBird.RenderTransform = new RotateTransform(5, flappyBird.Width / 2, flappyBird.Height / 2);
            // Ändrar gravitaion integern till 8.
            gravity = 8;
        }

        private void StartGame()
        {
            // Den här funktionen är startfunktionen och kommer ladda in alla startvärden i början av spelet och när spelet startas om.

            
            int temp = 300;

            // Sätter värdet på score till 0.
            score = 0;

            // Sätter gameOver till false eftersom spelet ska köras.
            gameOver = false;
            // Sätter flappyBird bilden till toppositionen till 190 pixlar som startposition.
            Canvas.SetTop(flappyBird, 190);

            // Denna foreach loop kommer gå igenom alla bilder i spelet och sätta deras startpositioner.
            foreach (var x in MyCanvas.Children.OfType<Image>())
            {
                // Sätter obs1 vilket är de första rören till sin startposition
                if ((string)x.Tag == "obs1")
                {
                    Canvas.SetLeft(x, 500);
                }
                // Sätter obs2 vilket är de andra rören till sin startposition
                if ((string)x.Tag == "obs2")
                {
                    Canvas.SetLeft(x, 800);
                }
                // Sätter obs3 vilket är de tredje rören till sin startposition
                if ((string)x.Tag == "obs3")
                {
                    Canvas.SetLeft(x, 1100);
                }
                // Sätter molnen till sina startpositioner
                if ((string)x.Tag == "cloud")
                {
                    Canvas.SetLeft(x, 300 + temp);
                    temp = 800;
                }
            }

            gameTimer.Start();
        }

        private void EndGame()
        {
            // När denna funktionen körs stannar spelet och startar om och skriver ut texten "Game Over!! Press R to try again" 
            gameTimer.Stop();
            gameOver = true;
            txtScore.Content += " Game Over!! Press R to try again";
        }
    }
}
