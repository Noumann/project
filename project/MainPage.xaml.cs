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
        Border border;
        Grid board;
        StackPanel player1SP;
        StackPanel player2SP;
        int newPieceRow;
        int newPieceColumn;
        Ellipse ellipseP1;
        Ellipse ellipseP2;
        int white = 0;
        int black = 0;
        int rowCount;
        int colCount;
        Grid grid;
        Grid grd;
        TextBlock score;
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
            mainSP.Orientation = Orientation.Horizontal;
            rootGrid.Children.Add(mainSP);
            player1StackPanel();
            boardStackPanel();
            player2StackPanel();
        }
        private void player1StackPanel()
        {
            player1SP = new StackPanel();
            player1SP.Name = "Player1SP";
            player1SP.Background = new SolidColorBrush(Colors.Tomato);
            player1SP.VerticalAlignment = VerticalAlignment.Top;
            player1SP.Margin = new Thickness(20);
            player1SP.Orientation = Orientation.Vertical;
            mainSP.Children.Add(player1SP);
            createPlayer1Ellipse();            
            gridForScore();
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
        private void player2StackPanel()
        {
            player2SP = new StackPanel();
            player2SP.Name = "player2SP";
            player2SP.Background = new SolidColorBrush(Colors.Blue);
            player2SP.VerticalAlignment = VerticalAlignment.Top;
            player2SP.Margin = new Thickness(20);
            player2SP.Orientation = Orientation.Vertical;
            mainSP.Children.Add(player2SP);
            createPlayer2Ellipse();         
            gridForScoree();
        }

        private bool SquareHasPiece(int xPos, int yPos)
        {
            foreach (object obj in board.Children)
            {
                if (typeof(Ellipse) == obj.GetType())
                {
                    Ellipse piece;
                    piece = (Ellipse)obj;
                    if (piece.Name.Contains("white")|| piece.Name.Contains("black"))
                    {
                        if (((int)piece.GetValue(Grid.ColumnProperty) == xPos) && ((int)piece.GetValue(Grid.RowProperty) == yPos))
                        {
                            return true;
                            Debug.WriteLine("has piece");
                        }
                    }
                }
            }
            return false;
        }
        private void creatBoard()
        {
            board = new Grid();
            board.Name = "board";
            board.HorizontalAlignment = HorizontalAlignment.Center;
            board.VerticalAlignment = VerticalAlignment.Top;
            board.Height = 700;
            board.Width = 700;
            board.Background = new SolidColorBrush(Colors.Beige);
            board.Margin = new Thickness(5);
            for (int i = 0; i < 8; i++)
            {
                board.RowDefinitions.Add(new RowDefinition());
                board.ColumnDefinitions.Add(new ColumnDefinition());
            }
            boardSP.Children.Add(board);
            borderForBoard();
            createWhiteEllipse(3, 3, new SolidColorBrush(Colors.White));
            createWhiteEllipse(4, 4, new SolidColorBrush(Colors.White));
            createBlackEllipse(3, 4, new SolidColorBrush(Colors.Black));
            createBlackEllipse(4, 3, new SolidColorBrush(Colors.Black));
        }
        private void gridForScore()
        {
            grd = new Grid();
            grd.Name = "scoreGridP1";
            grd.HorizontalAlignment = HorizontalAlignment.Center;
            grd.VerticalAlignment = VerticalAlignment.Top;
            grd.Height = 605;
            grd.Width = 150;
            grd.Background = new SolidColorBrush(Colors.BurlyWood);
            grd.Margin = new Thickness(5);
            for (int i = 0; i < 1; i++)
            {
                grd.RowDefinitions.Add(new RowDefinition());
                grd.ColumnDefinitions.Add(new ColumnDefinition());
            }
            player1SP.Children.Add(grd);
            playersScore("", HorizontalAlignment.Left,grd);
        }
        private void gridForScoree()
        {
            grid = new Grid();
            grid.Name = "scoreGridP2";
            grid.HorizontalAlignment = HorizontalAlignment.Center;
            grid.VerticalAlignment = VerticalAlignment.Top;
            grid.Height = 605;
            grid.Width = 150;
            grid.Background = new SolidColorBrush(Colors.BurlyWood);
            grid.Margin = new Thickness(5);
            for (int i = 0; i < 1; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            player2SP.Children.Add(grid);
            playersScore("", HorizontalAlignment.Left,grid);
        }

        private void borderForBoard()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    border = new Border();
                    border.Name = "border";
                    border.Height = 86;
                    border.Width = 86;
                    border.HorizontalAlignment = HorizontalAlignment.Center;
                    border.VerticalAlignment = VerticalAlignment.Center;
                    border.SetValue(Grid.RowProperty, row);
                    border.SetValue(Grid.ColumnProperty, col);
                    border.Background = new SolidColorBrush(Colors.Violet);
                    board.Children.Add(border);
                    border.Tapped += Border_Tapped;
                }
            }
        }

        private void Border_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Border boardValue = (Border)sender;
            newPieceRow = (int)boardValue.GetValue(Grid.RowProperty);
            newPieceColumn = (int)boardValue.GetValue(Grid.ColumnProperty);
            if (!SquareHasPiece(newPieceColumn, newPieceRow)) {                
                Debug.WriteLine(newPieceRow + " and " + newPieceColumn);
                creatNewPiece(newPieceRow, newPieceColumn, new SolidColorBrush());
                ellipseP1.Visibility = Visibility.Visible;
                ellipseP2.Visibility = Visibility.Visible;
            }
        }
        private void creatNewPiece(int row, int column, SolidColorBrush color)
        {
            Ellipse ellip = new Ellipse();
            ellip.Name = "Ellipse " + row + "_" + column;
            ellip.Width = 40;
            ellip.Height = 40;
            ellip.Fill = color;
            ellip.SetValue(Grid.RowProperty, row);
            ellip.SetValue(Grid.ColumnProperty, column);
            board.Children.Add(ellip);
        }

        private void createWhiteEllipse(int row, int column, SolidColorBrush color)
        {
            Ellipse ellipWhite = new Ellipse();
            ellipWhite.Name = "whiteEllipse " + row + "_" + column;
            ellipWhite.Width = 40;
            ellipWhite.Height = 40;
            ellipWhite.Fill = color;
            ellipWhite.SetValue(Grid.RowProperty, row);
            ellipWhite.SetValue(Grid.ColumnProperty, column);
            board.Children.Add(ellipWhite);
        }
        private void createBlackEllipse(int row, int column, SolidColorBrush color)
        {
            Ellipse ellipBlack = new Ellipse();
            ellipBlack.Name = "blackEllipse " + row + "_" + column;
            ellipBlack.Width = 40;
            ellipBlack.Height = 40;
            ellipBlack.Fill = color;
            ellipBlack.SetValue(Grid.RowProperty, row);
            ellipBlack.SetValue(Grid.ColumnProperty, column);
            board.Children.Add(ellipBlack);
        }
        private void createPlayer1Ellipse()
        {
            ellipseP1 = new Ellipse();
            ellipseP1.Name = "player1Ellipse";
            ellipseP1.Width = 40;
            ellipseP1.Height = 40;
            ellipseP1.Margin = new Thickness(20);
            ellipseP1.Fill = new SolidColorBrush(Colors.White);
            ellipseP1.VerticalAlignment = VerticalAlignment.Center;
            player1SP.Children.Add(ellipseP1);
            ellipseP1.Visibility = Visibility.Collapsed;            
            ellipseP1.Tapped += EllipseP1_Tapped;
        }

        private void EllipseP1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Ellipse toChange = (Ellipse)FindName("Ellipse " + newPieceRow + "_" + newPieceColumn);
            Ellipse original = (Ellipse)sender;
            toChange.Name = "white"+"Ellipse " + newPieceRow + "_" + newPieceColumn;
            toChange.Fill = original.Fill;
            Debug.WriteLine(toChange.Name);
            ellipseP1.Visibility = Visibility.Collapsed;
            ellipseP2.Visibility = Visibility.Collapsed;
            white = 0;
            score.Text = "";
            countPieces("Player 1 Score : ", "white", white,grd);
        }

        private void countPieces(String player,String wOrbl,int wOrb,Grid g)
        {
            for (rowCount = 0; rowCount < 8; rowCount++)
            {
                for (colCount = 0; colCount < 8; colCount++)
                {
                    foreach (object obj in board.Children)
                    {
                        if (typeof(Ellipse) == obj.GetType())
                        {
                            Ellipse piece;
                            piece = (Ellipse)obj;
                            if (piece.Name.Contains(wOrbl))
                            {
                                if (((int)piece.GetValue(Grid.ColumnProperty) == colCount) && ((int)piece.GetValue(Grid.RowProperty) == rowCount))
                                {
                                    wOrb++;
                                }
                                TextBlock block = (TextBlock)FindName("playerScore");
                                g.Children.Remove(block);
                                playersScore(player + wOrb, HorizontalAlignment.Left,g);
                            }
                        }
                    }
                }
            }
        }

        private void createPlayer2Ellipse()
        {
            ellipseP2 = new Ellipse();
            ellipseP2.Name = "player2Ellipse";
            ellipseP2.Width = 40;
            ellipseP2.Height = 40;
            ellipseP2.Margin = new Thickness(20);
            ellipseP2.Fill = new SolidColorBrush(Colors.Black);
            ellipseP2.VerticalAlignment = VerticalAlignment.Bottom;
            player2SP.Children.Add(ellipseP2);
            ellipseP2.Visibility = Visibility.Collapsed;
            ellipseP2.Tapped += Ellipse2_Tapped;
        }

        private void Ellipse2_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Ellipse toChange = (Ellipse)FindName("Ellipse " + newPieceRow + "_" + newPieceColumn);
            Ellipse original = (Ellipse)sender;
            toChange.Name = "black" + "Ellipse " + newPieceRow + "_" + newPieceColumn;
            toChange.Fill = original.Fill;
            Debug.WriteLine(toChange.Name);
            ellipseP2.Visibility = Visibility.Collapsed;
            ellipseP1.Visibility = Visibility.Collapsed;
            black = 0;
            score.Text = "";
            countPieces("Player 2 Score : ", "black", black,grid);
        }
        private void playersScore(String player,HorizontalAlignment alli,Grid g)
        {            
            score = new TextBlock();
            score.Name = "playerScore";
            score.Text = player;
            score.Margin = new Thickness(5);
            score.HorizontalAlignment = alli;
            score.VerticalAlignment = VerticalAlignment.Center;
            g.Children.Add(score);
        }
    }
}
