using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Saper
{
    public class Board
    {
        private List<List<Field>> fields;
        private Grid gameGrid;
        private int width;
        private int height;
        private int difficulty;
        private game CurrentGame;
        private int numberOfFlags;


        public Board(int width, int height, Grid grid, int dif, game g)
        {
            gameGrid = grid;
            this.width = width;
            this.height = height;
            this.difficulty = dif;
            CurrentGame = g;
            fields = new List<List<Field>>();
            AdjustingGameGrid();
            CreateButtons();

        }
        public int GetWidth()
        {
            return width;
        }
        public int GetHeight()
        {
            return height;
        }
        public void SetFlagNumber(int n)
        {
            numberOfFlags = n;
        }
        

        private void AdjustingGameGrid()
        {
            gameGrid.Children.Clear();
            if (gameGrid.Height / height > gameGrid.Width / width)
                gameGrid.Height = height * gameGrid.Width / width;
            else
                gameGrid.Width = width * gameGrid.Height / height;
            for (int x = 0; x < width; x++)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(1, GridUnitType.Star);
                gameGrid.ColumnDefinitions.Add(col);

            }
            for (int y = 0; y < height; y++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(1, GridUnitType.Star);
                gameGrid.RowDefinitions.Add(row);
            }
        }
        
        private void CreateButtons()
        {
            for (int x = 0; x < width; x++)
            {
                fields.Add(new List<Field>());
                for (int y = 0; y < height; y++)
                {
                    fields[x].Add(new Field(this));
                    gameGrid.Children.Add(fields[x][y].GetImage());
                    Grid.SetRow(fields[x][y].GetImage(), y);
                    Grid.SetColumn(fields[x][y].GetImage(), x);
                    fields[x][y].GetImage().MouseLeftButtonDown += GenerateBombs;
                }
            }
        }
        private void GenerateBombs(object sender, RoutedEventArgs e)
        {
            int clickedX = -1, clickedY = -1, numberOfBombs, divident;
            Random rand = new Random();
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    if (fields[x][y].GetImage() == sender)
                    {
                        clickedX = x;
                        clickedY = y;
                    }
            if (clickedX < 0 || clickedY < 0)
            {
                return;
            }
            List<List<int>> freeFields = new List<List<int>>();
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    if (!((x == clickedX || x == clickedX - 1 || x == clickedX + 1) && (y == clickedY || y == clickedY - 1 || y == clickedY + 1)))
                        freeFields.Add(new List<int> { x, y });
                }
            switch (difficulty)
            {
                case 1:
                    divident = 5;
                    break;
                case 2:
                    divident = 4;
                    break;
                case 3:
                    divident = 3;
                    break;
                default:
                    divident = 5;
                    break;

            }
            numberOfBombs = (width * height) / divident;
            if (numberOfBombs > freeFields.Count)
                numberOfBombs = freeFields.Count;
            //tworzenie bomb
            for (int i = 0; i < numberOfBombs; i++)
            {
                int currentField = rand.Next() % freeFields.Count;
                List<int> currentCoordinates = freeFields[currentField];
                freeFields.RemoveAt(currentField);
                fields[currentCoordinates[0]][currentCoordinates[1]].SetValue(-1);
            }
            //ustawianie liczby bomb w sasiedztwie
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (fields[x][y].GetValue() == -1)
                        continue;
                    fields[x][y].SetValue(0);
                    for (int a = x - 1; a <= x + 1; a++)
                    {
                        if (a < 0 || a == width)
                            continue;
                        for (int b = y - 1; b <= y + 1; b++)
                        {
                            if (b < 0 || b == height)
                                continue;
                            if (fields[a][b].GetValue() == -1)
                            {
                                fields[x][y].SetValue(fields[x][y].GetValue() + 1);
                            }
                        }
                    }
                }
            }
            //ustawianie innej funkcji
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    fields[x][y].GetImage().MouseLeftButtonDown -= GenerateBombs;
                    fields[x][y].GetImage().MouseLeftButtonDown += showValues;
                    fields[x][y].GetImage().MouseRightButtonDown += PlaceFlag;
                }
            }
            //showValues();
            showField(clickedX, clickedY, false);
            CurrentGame.start = DateTime.Now;
            CurrentGame.timer.Start();
        }

        private void showValues(object sender, RoutedEventArgs e)
        {
            int clickedX = -1, clickedY = -1;
            Random rand = new Random();
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    if (fields[x][y].GetImage() == sender)
                    {
                        clickedX = x;
                        clickedY = y;
                    }
            showField(clickedX, clickedY, true);
        }
        private void showField(int x, int y, bool primary)
        {
            if (fields[x][y].GetValue() == -1 && fields[x][y].GetState() == 0)
            {  //Game over
                fields[x][y].Show();
            }
            if (fields[x][y].GetValue() == 0 && fields[x][y].GetState() == 0)
            {
                fields[x][y].Show();
                for (int i = x - 1; i <= x + 1; i++)
                    for (int j = y - 1; j <= y + 1; j++)
                        if (i != -1 && i != width && j != -1 && j != height)
                            showField(i, j, false);
            }
            if (fields[x][y].GetValue() > 0 && fields[x][y].GetState() == 0)
            {
                fields[x][y].Show();
            }
            if (fields[x][y].GetValue() > 0 && fields[x][y].GetState() == 1 && primary && CheckNOFlags(x, y))
            {
                for (int i = x - 1; i <= x + 1; i++)
                    for (int j = y - 1; j <= y + 1; j++)
                        if (i != -1 && i != width && j != -1 && j != height && fields[i][j].GetState() != 2)
                            showField(i, j, false);
            }

        }
        private void PlaceFlag(object sender, RoutedEventArgs e)
        {
            int clickedX = 0, clickedY = 0;
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    if (fields[x][y].GetImage() == sender)
                    {
                        clickedX = x;
                        clickedY = y;
                    }
            numberOfFlags += fields[clickedX][clickedY].ToggleFlag();
        }
        private bool CheckNOFlags(int x, int y)
        {
            int NOFlags = 0;
            for (int i = x - 1; i <= x + 1; i++)
                for (int j = y - 1; j <= y + 1; j++)
                    if (i != -1 && i != width && j != -1 && j != height)
                        if (fields[i][j].GetState() == 2)
                            NOFlags++;
            return NOFlags == fields[x][y].GetValue();
        }
    }

}
