using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Services.Models;
using tileType = Services.Enums.TileEnum.TileType;
using Services.Switch;



// TO DO: 
// 3# change tile design according to its properties: IsRevealed, HasFlag, Number
// 4# do all the button styles the styles with the <Style> in the xaml file
// 5# to optimize the code, use the TileModel class to store the button and the tile properties
// 6# to optimize the code, create Two boards innitially, one for the answers and one for the UI, and then update the UI board according to the answers board
// 6# maintain consistency using camalCase;
// 7# rename variables to be more descriptive
// 8# The first click should have zero chance of being a bomb
// 9# No "Game Over" message
// 10# No "You Win" message or check if won
// 11# No "Flag" feature
// 12# Settings are not fully implemented
// 13# The hotbar is unfunctional
// 14# 

namespace MineSweeperDemo
{
    public partial class GameTable : Page
    {
        private TileModel[,]? gameBoardAnswer;
        private const int mineQuantity = 100;
        private const int tableSize = 20;
        const int TileSize = 30;

        public GameTable()
        {
            InitializeComponent();
            InitializeGame(tableSize);
        }

        private async void InitializeGame(int tableSize)
        {
            gameBoardAnswer = new TileModel[tableSize, tableSize];
            for (int i = 0; i < tableSize; i++)
            {
                for (int j = 0; j < tableSize; j++)
                {
                    gameBoardAnswer[i, j] = new TileModel(tileType.Empty);
                }
            }

            Task task1 = Task.Run(() => PlaceMines());
            //Task task2 = Task.Run(() => CreateUIElements());
            CreateUIElements();
            await task1;
            Task task3 = Task.Run(() => CalculateNumbers());
            //await Task.WhenAll(task3, task2);
            await task3;
            CalculateNumbers();
        }

        private void PlaceMines()
        {
            Random random = new Random();
            int minesPlaced = 0;

            while (minesPlaced < mineQuantity)
            {
                int row = random.Next(0, tableSize);
                int col = random.Next(0, tableSize);

                if (gameBoardAnswer != null && gameBoardAnswer[row, col].Type != tileType.Bomb)
                {
                    gameBoardAnswer[row, col] = new TileModel(tileType.Bomb);
                    minesPlaced++;
                }
            }
        }

        //private void CalculateNumbers()
        //{
        //    for (int i = 0; i < tableSize; i++)
        //    {
        //        for (int j = 0; j < tableSize; j++)
        //        {
        //            if (gameBoardAnswer[i, j].Type == tileType.Empty)
        //            {
        //                int count = 0;
        //                if (i - 1 > 0 && gameBoardAnswer[i - 1, j].Type == tileType.Bomb) count++;                                    // <-
        //                if (i - 1 > 0 && j - 1 > 0 && gameBoardAnswer[i - 1, j - 1].Type == tileType.Bomb) count++;                   // ^<-
        //                if (j - 1 > 0 && gameBoardAnswer[i, j - 1].Type == tileType.Bomb) count++;                                    // ^
        //                if (i + 1 < tableSize && j > 0 && gameBoardAnswer[i + 1, j - 1].Type == tileType.Bomb) count++;               // ->^
        //                if (i + 1 < tableSize && gameBoardAnswer[i + 1, j].Type == tileType.Bomb) count++;                            // ->
        //                if (i + 1 < tableSize && j + 1 < tableSize && gameBoardAnswer[i + 1, j + 1].Type == tileType.Bomb) count++;   // ->v
        //                if (j + 1 < tableSize && gameBoardAnswer[i, j + 1].Type == tileType.Bomb) count++;                            // v
        //                if (i - 1 > 0 && j + 1 < tableSize && gameBoardAnswer[i - 1, j + 1].Type == tileType.Bomb) count++;           // v<-

        //                if (count > 0 && count < 9)
        //                {
        //                    gameBoardAnswer[i, j].Type = (tileType)count;
        //                }
        //                else
        //                {
        //                    throw new Exception($"Tile number is out of [1,8] range: Calculations for mines near a tile got a value of {count}");
        //                }
        //            }
        //        }
        //    }
        //}

        private void CalculateNumbers()
        {
            for (int i = 0; i < tableSize; i++)
            {
                for (int j = 0; j < tableSize; j++)
                {
                    if (gameBoardAnswer![i, j].Type == tileType.Empty)
                    {
                        int count = CountBombsAroundTile(i, j) - 1;

                        if (count != -1)
                        {
                            if (count < 8)
                            {
                                gameBoardAnswer[i, j].Type = (tileType)count;
                            }
                            else
                            {
                                throw new Exception($"Tile number is out of [1,8] range: Calculations for mines near a tile got a value of {count}");
                            }
                        }
                    }
                }
            }
        }

        private int CountBombsAroundTile(int x, int y)
        {
            int count = 0;
            for (int row = x - 1; row <= x + 1; row++)
            {
                for (int col = y - 1; col <= y + 1; col++)
                {
                    if (row >= 0 && row < tableSize && col >= 0 && col < tableSize && !(row == x && col == y))
                    {
                        if (gameBoardAnswer![row, col].Type == tileType.Bomb)
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }


        private void CreateUIElements()
        {
            for (int i = 0; i < tableSize; i++)
            {
                gameGrid.RowDefinitions.Add(new RowDefinition());
                for (int j = 0; j < tableSize; j++)
                {
                    gameGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    Button tileButton = new()
                    {
                        Content = "",
                        Width = TileSize,
                        Height = TileSize,
                    };
                    tileButton.Click += Tile_Click;

                    gameGrid.Children.Add(tileButton);
                    Grid.SetRow(tileButton, i);
                    Grid.SetColumn(tileButton, j);
                }
            }
        }

        private void Tile_Click(object sender, RoutedEventArgs e)
        {
            Button TileClicked = (Button)sender;
            int Row = Grid.GetRow(TileClicked);
            int Column = Grid.GetColumn(TileClicked);

            if (gameBoardAnswer![Row, Column].HasFlag)
                return;

            int TileClickedType = (int)gameBoardAnswer![Row, Column].Type + 1;
            gameBoardAnswer![Row, Column].IsRevealed = true;

            if (TileClickedType < 9)
            {
                InsertTileClicked(Row, Column, TileClickedType.ToString());
            }
            else if (TileClickedType == 9) // (int)tileType.Empty + 1
            {
                InsertTileClicked(Row, Column, "");
                UncoverAroundEmptyTile(Row, Column);
            }
            else if (TileClickedType == 10) // (int)tileType.Bomb + 1
            {
                InsertTileClicked(Row, Column, "X");
                GameOver();
            }
        }

        private void UncoverAroundEmptyTile(int x, int y)
        {
            for (int row = x - 1; row <= x + 1 && row >= 0 && row < tableSize; row++)
            {
                for (int col = y - 1; col <= y + 1 && col >= 0 && col < tableSize; col++)
                {

                    if (!gameBoardAnswer![row, col].IsRevealed) // "&& row != x" - not needed, because the middle tile is already revealed
                    {
                        gameBoardAnswer![row, col].IsRevealed = true;

                        if (gameBoardAnswer![row, col].Type == tileType.Empty)
                        {
                            InsertTileClicked(row, col, "");
                            UncoverAroundEmptyTile(row, col);
                        }
                        else
                        {
                            InsertTileClicked(row, col, ((int)gameBoardAnswer![row, col].Type + 1).ToString());
                        }
                    }
                }

            }
        }

        private void InsertTileClicked(int x, int y, String text)
        {
            SolidColorBrush ForeColor = new SolidColorBrush(TileTextColor.GetTileTextColor(text));

            Border TextBlockTile = new Border
            {
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1),
                Child = new TextBlock
                {
                    Text = text,
                    TextAlignment = TextAlignment.Center,
                    FontSize = 16,
                    Foreground = ForeColor,
                    FontWeight = FontWeights.Bold,
                    Background = Brushes.White,
                    Width = TileSize - 2,
                    Height = TileSize - 2,

                }
            };

            gameGrid.Children.Remove((Button)gameGrid.Children.Cast<UIElement>().First(e => Grid.GetRow(e) == x && Grid.GetColumn(e) == y));
            gameGrid.Children.Add(TextBlockTile);
            Grid.SetRow(TextBlockTile, x);
            Grid.SetColumn(TextBlockTile, y);
        }


        /// this method checks for content of a button? is this more optimized than getting grid row and column (ask gpt)?
        private void Tile_RightClick(object sender, RoutedEventArgs e)
        {
            Button tileClicked = (Button)sender;
            int row = Grid.GetRow(tileClicked);
            int column = Grid.GetColumn(tileClicked);
            if (tileClicked.Content.ToString() == "")
            {
                tileClicked.Content = "F";
                gameBoardAnswer![row, column].HasFlag = true;
            }
            else
            {
                tileClicked.Content = "";
                gameBoardAnswer![row, column].HasFlag = false;
            }
        }

        private void GameOver()
        {
            for (int i = 0; i < tableSize; i++)
            {
                for (int j = 0; j < tableSize; j++)
                {
                    if (!gameBoardAnswer![i, j].IsRevealed && gameBoardAnswer![i, j].Type == tileType.Bomb)
                    {
                        
                        InsertTileClicked(i, j, "X");
                    }
                }
            }
        }
    }
}
