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
        public MainPage()
        {
            this.InitializeComponent();
            mainStackPanel();
        }

        StackPanel mainSP;
        StackPanel boardSP;
        StackPanel scoreSP;
        Grid board;
        Border border;
        Button btn;
        int rows = 5;
        int buttonName = 1;


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
            scoreSP.Width = 300;
            mainSP.Children.Add(scoreSP);
        }

        private void buttonsForGrid(String btnName, int row, int column)
        {
            btn = new Button();
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
                        }
                    }
                }
            }
        }
    }
}