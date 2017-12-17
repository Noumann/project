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
                    Debug.WriteLine(btn.Name);
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
            scoreSP.Width = 300;
            mainSP.Children.Add(scoreSP);
            gridForScoreSP();
        }

        private void buttonsForGrid(String btnName, int row, int column)
        {
            btn = new Button();
            btn.Name = "Button" + row+"_"+column;
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
            gridForSP.Width = 300;
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
            textFields(0,0,"Clicks : ","clicks");
            textFields(0, 1, ""+numOfClicks,"score");
        }

        private void textFields(int row,int column,String txt,String name)
        {
            TextBlock text = new TextBlock();
            text.Text = txt;
            text.Name = name;
            text.Margin = new Thickness(10);
            text.SetValue(Grid.RowProperty,row);
            text.SetValue(Grid.ColumnProperty, column);
            text.HorizontalAlignment = HorizontalAlignment.Center;
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

        private void solvedOrNot()
        {
            Button but = FindName("Button" + 0 +"_" + 0) as Button;
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
                            TextBlock txt = FindName("score") as TextBlock;
                            txt.Text = "Done";
                        }
                    }
                }
            }
        }
    }
}