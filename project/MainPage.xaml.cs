using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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


        StackPanel mainSP;
        StackPanel boardSP;
        StackPanel scoreSP;
        Grid board;
        Border border;
        Button btn;
        Grid gridForSP;
        int numOfClicks = 0;
        int rows = 5;
        int buttonName = 1;

        public MainPage()
        {
            this.InitializeComponent();
            mainStackPanel();
            shuffle();
        }


        private void mainStackPanel()
        {
            mainSP = new StackPanel();
            mainSP.Name = "mainSP";
            mainSP.Background = new SolidColorBrush(Colors.Gray);
            mainSP.VerticalAlignment = VerticalAlignment.Center;
            mainSP.HorizontalAlignment = HorizontalAlignment.Center;
            mainSP.Orientation = Orientation.Horizontal;
            rootGrid.Children.Add(mainSP);
            boardStackPanel();
            scoreStackPanel();
        }

        private void boardStackPanel()
        {
            boardSP = new StackPanel();
            boardSP.Name = "boardSP";
            boardSP.Background = new SolidColorBrush(Colors.Gray);
            boardSP.VerticalAlignment = VerticalAlignment.Center;
            boardSP.HorizontalAlignment = HorizontalAlignment.Center;
            mainSP.Children.Add(boardSP);
            creatBoard();
        }

        private void creatBoard()
        {
            board = new Grid();
            board.Name = "board";
            board.HorizontalAlignment = HorizontalAlignment.Center;
            board.VerticalAlignment = VerticalAlignment.Top;
            board.Height = 500;
            board.Width = 500;
            board.Background = new SolidColorBrush(Colors.Cornsilk);
            board.Margin = new Thickness(5);
            for (int i = 0; i < rows; i++)
            {
                board.RowDefinitions.Add(new RowDefinition());
                board.ColumnDefinitions.Add(new ColumnDefinition());
            }
            boardSP.Children.Add(board);
            borderForBoard();

            Random rand = new Random();
            int row = rand.Next(0, 5);
            int column = rand.Next(0, 5);

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < rows; c++)
                {
                    if (r == row && c == column)
                    {
                        btn.Content = "";
                        buttonName--;
                    }
                    buttonsForGrid("" + buttonName, r, c);
                    buttonName++;
                }
            }
        }

        private void borderForBoard()
        {
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
                    border.Background = new SolidColorBrush(Colors.MediumSeaGreen);
                    board.Children.Add(border);
                }
            }
        }

        private void scoreStackPanel()
        {
            scoreSP = new StackPanel();
            scoreSP.Name = "scoreSP";
            scoreSP.Background = new SolidColorBrush(Colors.Goldenrod);
            scoreSP.VerticalAlignment = VerticalAlignment.Top;
            scoreSP.Margin = new Thickness(5);
            scoreSP.Orientation = Orientation.Vertical;
            scoreSP.Height = 500;
            scoreSP.Width = 200;
            mainSP.Children.Add(scoreSP);
            gridForScoreSP();
        }

        private void buttonsForGrid(String btnName, int row, int column)
        {
            btn = new Button();
            btn.Name = "Button" + row + "_" + column;
            btn.Width = 95;
            btn.Height = 95;
            btn.Margin = new Thickness(3);
            btn.Content = btnName;
            btn.SetValue(Grid.RowProperty, row);
            btn.SetValue(Grid.ColumnProperty, column);
            btn.Background = new SolidColorBrush(Colors.Khaki);
            board.Children.Add(btn);
            btn.Click += Btn_Click;
        }

        private void gridForScoreSP()
        {
            gridForSP = new Grid();
            gridForSP.Name = "board";
            gridForSP.HorizontalAlignment = HorizontalAlignment.Center;
            gridForSP.VerticalAlignment = VerticalAlignment.Top;
            gridForSP.Height = 490;
            gridForSP.Width = 200;
            gridForSP.Background = new SolidColorBrush(Colors.LightPink);
            gridForSP.Margin = new Thickness(5);
            for (int i = 0; i < 4; i++)
            {
                gridForSP.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < 2; i++)
            {
                gridForSP.ColumnDefinitions.Add(new ColumnDefinition());
            }
            scoreSP.Children.Add(gridForSP);
            textFields(0, 0, "Clicks  :    ", "clicks");
            textFields(0, 1, "" + numOfClicks, "score");
            textFields(1, 0, "", "gameOver");
        }

        private void textFields(int row, int column, String txt, String name)
        {
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

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button buttonClicked = (Button)sender;
            int xPos = (int)buttonClicked.GetValue(Grid.RowProperty);
            int yPos = (int)buttonClicked.GetValue(Grid.ColumnProperty);

            foreach (object obj in board.Children)
            {
                if (typeof(Button) == obj.GetType())
                {
                    Button butn;
                    butn = (Button)obj;
                    if (butn.Content.Equals(""))
                    {
                        if (((int)butn.GetValue(Grid.RowProperty) == xPos - 1) && ((int)butn.GetValue(Grid.ColumnProperty) == yPos) ||
                            ((int)butn.GetValue(Grid.RowProperty) == xPos) && ((int)butn.GetValue(Grid.ColumnProperty) == yPos + 1) ||
                            ((int)butn.GetValue(Grid.RowProperty) == xPos + 1) && ((int)butn.GetValue(Grid.ColumnProperty) == yPos) ||
                            ((int)butn.GetValue(Grid.RowProperty) == xPos) && ((int)butn.GetValue(Grid.ColumnProperty) == yPos - 1))
                        {
                            butn.Content = buttonClicked.Content;
                            buttonClicked.Content = "";
                            TextBlock txt = FindName("score") as TextBlock;
                            numOfClicks++;
                            txt.Text = "" + numOfClicks;
                            solvedOrNot();
                        }
                    }
                }
            }
        }

        private async void solvedOrNot()
        {
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
                                                                                                            txt.Text = "Well Done...!!!";
                                                                                                            var dialog = new Windows.UI.Popups.MessageDialog("Well Done.....!!!\nCLOSE TO START NEW GAME");
                                                                                                            await dialog.ShowAsync();
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

        private void shuffle()
        {
            int i, j;
            int[] a = new int[25];
            Boolean flag = false;
            i = 1;
            do
            {
                Random rand = new Random();
                int Rn = Convert.ToInt32((rand.Next(0, 24))+1);
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
                    Button but = FindName("Button" + row + "_" + col) as Button;
                    if (!(but.Content.Equals("")))
                    {                       
                        but.Content = a[l];
                        l++;
                    }
                }
            }
            numOfClicks = 0;
        }
    }
}