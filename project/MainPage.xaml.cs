using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace project
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        #region Global Variables
        StackPanel mainSP;
        StackPanel boardSP;
        StackPanel scoreSP;
        Grid board;
        Border border;
        Button btn;
        TextBlock t;
        Grid gridForSP;
        int numOfClicks = 0;
        int record = 0;
        int rows = 5;
        int buttonName = 1;
        #endregion

        #region Main Page
        public MainPage()
        {
            this.InitializeComponent();
            this.Loading += MainPage_Loading;
            mainStackPanel();
            shuffle();
        }

        private void MainPage_Loading(FrameworkElement sender, object args)
        {
            //saving the local machine settings
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            string strSetting;
            try
            {
                //saving the best score
                strSetting = localSettings.Values["recordScore"].ToString();
                t = FindName("recordScore") as TextBlock;
                t.Text = strSetting;
                record = Convert.ToInt32(strSetting);
            }
            //throw an error exception if there is any
            catch (Exception ex)
            {
                t.Text = "Error: " + ex.HResult + ", " + ex.Message;
            }
        }
        #endregion

        #region Main Stack Panel
        private void mainStackPanel()
        {
            //main stack panel whil will have all the other stack panels or grids
            mainSP = new StackPanel();
            mainSP.Name = "mainSP";
            mainSP.VerticalAlignment = VerticalAlignment.Center;
            mainSP.HorizontalAlignment = HorizontalAlignment.Center;
            mainSP.Orientation = Orientation.Horizontal;
            rootGrid.Children.Add(mainSP);
            //projection on the main stack panel 
            PlaneProjection m = new PlaneProjection();
            m.RotationX = -10;
            m.RotationZ = -5;
            mainSP.Projection = m;
            boardStackPanel();
            scoreStackPanel();
        }
        #endregion

        #region Stack Panel for creating the board
        private void boardStackPanel()
        {
            //stack panel for creating the board
            boardSP = new StackPanel();
            boardSP.Name = "boardSP";
            boardSP.VerticalAlignment = VerticalAlignment.Center;
            boardSP.HorizontalAlignment = HorizontalAlignment.Center;
            mainSP.Children.Add(boardSP);
            creatBoard();
        }
        #endregion

        #region Creating the Board and Populating it with the Buttons
        private void creatBoard()
        {
            //creating the actual board 
            //which has 5 rows and 5 columns
            board = new Grid();
            board.Name = "board";
            board.HorizontalAlignment = HorizontalAlignment.Center;
            board.VerticalAlignment = VerticalAlignment.Top;
            board.Height = 500;
            board.Width = 500;
            board.Background = new SolidColorBrush(Colors.Cornsilk);
            for (int i = 0; i < rows; i++)
            {
                board.RowDefinitions.Add(new RowDefinition());
                board.ColumnDefinitions.Add(new ColumnDefinition());
            }
            boardSP.Children.Add(board);
            borderForBoard();
            //create a random spot for the empty button
            //in order to slide the other buttons and solve the puzzle
            Random rand = new Random();
            int row = rand.Next(0, 5);
            int column = rand.Next(0, 5);

            //if row number and column number mathes the ramdom number generated
            //then creat a button but without the content
            //and the name of the button decreases by 1
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < rows; c++)
                {
                    if (r == row && c == column)
                    {
                        btn.Content = " ";
                        buttonName--;
                    }
                    buttonsForGrid("" + buttonName, r, c);
                    buttonName++;
                }
            }
        }
        #endregion

        #region Border for the Board
        private void borderForBoard()
        {
            //creating the boarder on the grid
            //which has equal number of rows and columns
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < rows; col++)
                {
                    border = new Border();
                    border.Name = "border";
                    border.Height = 98;
                    border.Width = 98;
                    border.HorizontalAlignment = HorizontalAlignment.Center;
                    border.VerticalAlignment = VerticalAlignment.Center;
                    border.SetValue(Grid.RowProperty, row);
                    border.SetValue(Grid.ColumnProperty, col);
                    border.Background = new SolidColorBrush(Colors.Salmon);
                    board.Children.Add(border);
                }
            }
        }
        #endregion

        #region Stack panel for score and record score and creating new game and Exit the game
        private void scoreStackPanel()
        {
            //another stack panel for displaying the scores 
            //Least moves and 
            //tell the player whether they have solved the puzzel or not
            scoreSP = new StackPanel();
            scoreSP.Name = "scoreSP";
            scoreSP.Background = new SolidColorBrush(Colors.SandyBrown);
            scoreSP.VerticalAlignment = VerticalAlignment.Top;
            scoreSP.Margin = new Thickness(3);
            scoreSP.Orientation = Orientation.Vertical;
            scoreSP.Height = 500;
            scoreSP.Width = 200;
            mainSP.Children.Add(scoreSP);
            gridForScoreSP();
        }
        #endregion

        #region Creating the buttons
        private void buttonsForGrid(String btnName, int row, int column)
        {
            //creating the actual buttons
            //and creat a click event on the buttons 
            btn = new Button();
            btn.Name = "Button" + row + "_" + column;
            btn.Width = 95;
            btn.Height = 95;
            btn.Margin = new Thickness(3);
            btn.Content = btnName;
            btn.SetValue(Grid.RowProperty, row);
            btn.SetValue(Grid.ColumnProperty, column);
            btn.Background = new SolidColorBrush(Colors.MediumTurquoise);
            board.Children.Add(btn);
            btn.Click += Btn_Click;
        }
        #endregion

        #region Creating a grid to give sections to each component e.g New Game or Exit
        private void gridForScoreSP()
        {
            //Creating a grid to give sections to each component e.g New Game or Exit
            gridForSP = new Grid();
            gridForSP.Name = "board";
            gridForSP.HorizontalAlignment = HorizontalAlignment.Center;
            gridForSP.VerticalAlignment = VerticalAlignment.Top;
            gridForSP.Height = 490;
            gridForSP.Width = 200;
            gridForSP.Background = new SolidColorBrush(Colors.PeachPuff);
            gridForSP.Margin = new Thickness(5);
            //grid has 4 rows
            for (int i = 0; i < 4; i++)
            {
                gridForSP.RowDefinitions.Add(new RowDefinition());
            }
            //and 2 columns
            for (int i = 0; i < 2; i++)
            {
                gridForSP.ColumnDefinitions.Add(new ColumnDefinition());
            }
            scoreSP.Children.Add(gridForSP);
            //To display the Least Moves
            textFields(0, 0, "Least Moves", "record");
            textFields(0, 1, "" + record, "recordScore");
            //To tell the user how many moves they have made already
            textFields(1, 0, "Total Moves", "clicks");
            textFields(1, 1, "" + numOfClicks, "score");
            //tell the user whether the game is over or not
            textFields(2, 0, "", "gameOver");
            //creat a button to creat a new game or exit the game
            exitAndNewGame("newGame", "New Game", HorizontalAlignment.Center, VerticalAlignment.Top);
            exitAndNewGame("exit", "Exit", HorizontalAlignment.Center, VerticalAlignment.Center);
        }
        #endregion

        #region Creating the Text Fields or Lables to display the required information
        private void textFields(int row, int column, String txt, String name)
        {
            //Creating the Text Fields or Lables to display the required information
            TextBlock text = new TextBlock();
            text.Text = txt;
            text.Name = name;
            text.Margin = new Thickness(10);
            text.SetValue(Grid.RowProperty, row);
            text.SetValue(Grid.ColumnProperty, column);
            text.HorizontalAlignment = HorizontalAlignment.Left;
            text.VerticalAlignment = VerticalAlignment.Center;
            gridForSP.Children.Add(text);
        }
        #endregion

        #region Button click event on the main Grid
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            //clicked event on buttons in order to
            //solve the puzzle
            Button buttonClicked = (Button)sender;
            //geting the position of the buton on the grid
            int xPos = (int)buttonClicked.GetValue(Grid.RowProperty);
            int yPos = (int)buttonClicked.GetValue(Grid.ColumnProperty);

            //for each loop to validate thet the valid move
            //setting the rules for the game
            //that the player can not slide any piece
            //any empty piece has to be around the piece is being played 
            foreach (object obj in board.Children)
            {
                if (typeof(Button) == obj.GetType())
                {
                    Button butn;
                    butn = (Button)obj;
                    if (butn.Content.Equals(" "))
                    {
                        if (((int)butn.GetValue(Grid.RowProperty) == xPos - 1) && ((int)butn.GetValue(Grid.ColumnProperty) == yPos) ||
                            ((int)butn.GetValue(Grid.RowProperty) == xPos) && ((int)butn.GetValue(Grid.ColumnProperty) == yPos + 1) ||
                            ((int)butn.GetValue(Grid.RowProperty) == xPos + 1) && ((int)butn.GetValue(Grid.ColumnProperty) == yPos) ||
                            ((int)butn.GetValue(Grid.RowProperty) == xPos) && ((int)butn.GetValue(Grid.ColumnProperty) == yPos - 1))
                        {
                            //if all the conditions are true
                            //change the properties of the components
                            //and check whether the puzzle
                            //has been solved or not if not continue
                            //otherwise call the method
                            //solved or not
                            butn.Content = buttonClicked.Content;
                            buttonClicked.Content = " ";
                            TextBlock txt = FindName("score") as TextBlock;
                            numOfClicks++;
                            txt.Text = "" + numOfClicks;
                            solvedOrNot();
                        }
                    }
                }
            }
        }
        #endregion

        #region Ckecking whether the Puzzle is Solved or not
        private async void solvedOrNot()
        {
            //multiple condition 
            //condition inside a conditon
            //all the pieces has to be in a sequence
            //if all the components are in scequence 
            //in every condition first thing is to find the piece
            //then check the sequence
            //if the sequence is right
            //display the user that tehy have solved the game
            //otherwisw continue
            Button but = FindName("Button" + 0 + "_" + 0) as Button;
            if (but.Content.Equals("1"))
            {
                but = FindName("Button" + 0 + "_" + 1) as Button;
                if (but.Content.Equals("2"))
                {
                    but = FindName("Button" + 0 + "_" + 2) as Button;
                    if (but.Content.Equals("3"))
                    {
                        but = FindName("Button" + 0 + "_" + 3) as Button;
                        if (but.Content.Equals("4"))
                        {
                            but = FindName("Button" + 0 + "_" + 4) as Button;
                            if (but.Content.Equals("5"))
                            {
                                but = FindName("Button" + 1 + "_" + 0) as Button;
                                if (but.Content.Equals("6"))
                                {
                                    but = FindName("Button" + 1 + "_" + 1) as Button;
                                    if (but.Content.Equals("7"))
                                    {
                                        but = FindName("Button" + 1 + "_" + 2) as Button;
                                        if (but.Content.Equals("8"))
                                        {
                                            but = FindName("Button" + 1 + "_" + 3) as Button;
                                            if (but.Content.Equals("9"))
                                            {
                                                but = FindName("Button" + 1 + "_" + 4) as Button;
                                                if (but.Content.Equals("10"))
                                                {
                                                    but = FindName("Button" + 2 + "_" + 0) as Button;
                                                    if (but.Content.Equals("11"))
                                                    {
                                                        but = FindName("Button" + 2 + "_" + 1) as Button;
                                                        if (but.Content.Equals("12"))
                                                        {
                                                            but = FindName("Button" + 2 + "_" + 2) as Button;
                                                            if (but.Content.Equals("13"))
                                                            {
                                                                but = FindName("Button" + 2 + "_" + 3) as Button;
                                                                if (but.Content.Equals("14"))
                                                                {
                                                                    but = FindName("Button" + 2 + "_" + 4) as Button;
                                                                    if (but.Content.Equals("15"))
                                                                    {
                                                                        but = FindName("Button" + 3 + "_" + 0) as Button;
                                                                        if (but.Content.Equals("16"))
                                                                        {
                                                                            but = FindName("Button" + 3 + "_" + 1) as Button;
                                                                            if (but.Content.Equals("17"))
                                                                            {
                                                                                but = FindName("Button" + 3 + "_" + 2) as Button;
                                                                                if (but.Content.Equals("18"))
                                                                                {
                                                                                    but = FindName("Button" + 3 + "_" + 3) as Button;
                                                                                    if (but.Content.Equals("19"))
                                                                                    {
                                                                                        but = FindName("Button" + 3 + "_" + 4) as Button;
                                                                                        if (but.Content.Equals("20"))
                                                                                        {
                                                                                            but = FindName("Button" + 4 + "_" + 0) as Button;
                                                                                            if (but.Content.Equals("21"))
                                                                                            {
                                                                                                but = FindName("Button" + 4 + "_" + 1) as Button;
                                                                                                if (but.Content.Equals("22"))
                                                                                                {
                                                                                                    but = FindName("Button" + 4 + "_" + 2) as Button;
                                                                                                    if (but.Content.Equals("23"))
                                                                                                    {
                                                                                                        but = FindName("Button" + 4 + "_" + 3) as Button;
                                                                                                        if (but.Content.Equals("24"))
                                                                                                        {
                                                                                                            TextBlock txt = FindName("gameOver") as TextBlock;
                                                                                                            txt.Text = "Well Done.!!!";

                                                                                                            if (record > numOfClicks)
                                                                                                            {
                                                                                                                //saving the local settings
                                                                                                                record = numOfClicks;
                                                                                                                TextBlock recordScore = FindName("recordScore") as TextBlock;
                                                                                                                recordScore.Text = "" + record;
                                                                                                                // access the data container called LocalSettings
                                                                                                                //reading
                                                                                                                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                                                                                                                try
                                                                                                                {
                                                                                                                    int temp = Convert.ToInt32(localSettings.Values["recordScore"]);
                                                                                                                    if (temp > record)
                                                                                                                    {
                                                                                                                        localSettings.Values["recordScore"] = record.ToString();
                                                                                                                    }
                                                                                                                }
                                                                                                                catch
                                                                                                                {
                                                                                                                    // doesn't exist, just set the value
                                                                                                                    localSettings.Values["recordScore"] = record.ToString();
                                                                                                                }
                                                                                                            }
                                                                                                            //ask the user if they want to start the new game
                                                                                                            var dialog = new Windows.UI.Popups.MessageDialog("Well Done.....!!!\nCLOSE TO START NEW GAME");
                                                                                                            await dialog.ShowAsync();
                                                                                                            rootGrid.Children.Remove(mainSP);
                                                                                                            mainStackPanel();
                                                                                                            //at the end of the the solution shuffle the pieces 
                                                                                                            //agian in order to create a new puzzle again
                                                                                                            shuffle();
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Shuffle the Buttons to create a new game or creat a Puzzle
        private void shuffle()
        {
            //shuffle teh components
            //whenever this method is called
            //a new puzzle is created
            int i, j;
            int[] a = new int[25];
            Boolean flag = false;
            i = 1;
            do
            {
                //generating the ramond numbers
                //and save them in a array
                Random rand = new Random();
                int Rn = Convert.ToInt32((rand.Next(0, 24)) + 1);
                for (j = 1; j <= i; j++)
                {
                    if (a[j] == Rn)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag == true)
                {
                    flag = false;
                }
                else
                {
                    a[i] = Rn;
                    i = i + 1;
                }
            } while (i <= 24);

            int l = 1;
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < rows; col++)
                {
                    //assigning the values from the array list 
                    //first find the component and then give it a value
                    //from array list
                    Button but = FindName("Button" + row + "_" + col) as Button;
                    if (!(but.Content.Equals(" ")))
                    {
                        but.Content = a[l];
                        l++;
                    }
                }
            }
            numOfClicks = 0;
            TextBlock txt = FindName("score") as TextBlock;
            txt.Text = "" + numOfClicks;
            TextBlock gameOver = FindName("gameOver") as TextBlock;
            gameOver.Text = "";
        }
        #endregion

        #region Create Button to either create a new Game or exit the current game
        private void exitAndNewGame(String butnName, String name, HorizontalAlignment allign, VerticalAlignment vAllign)
        {
            //Create Button to either create a new Game or exit the current game
            Button butn = new Button();
            butn.Width = 100;
            butn.Height = 38;
            butn.Name = butnName;
            butn.Margin = new Thickness(3);
            butn.HorizontalAlignment = allign;
            butn.VerticalAlignment = vAllign;
            butn.Content = name;
            butn.SetValue(Grid.RowProperty, 3);
            butn.SetValue(Grid.ColumnProperty, 0);
            butn.SetValue(Grid.ColumnSpanProperty, 2);
            butn.Background = new SolidColorBrush(Colors.GhostWhite);
            gridForSP.Children.Add(butn);
            butn.Click += Butn_Click;
        }
        #endregion

        #region Click event on Button to create a new game or Exit the Game
        private void Butn_Click(object sender, RoutedEventArgs e)
        {
            // Click event on Button to create a new game or Exit the Game
            Button button = (Button)sender;
            if (button.Content.Equals("New Game"))
            {
                rootGrid.Children.Remove(mainSP);
                mainStackPanel();
                shuffle();
            }
            else if (button.Content.Equals("Exit"))
            {
                Application.Current.Exit();
            }
        }
        #endregion


    }
}