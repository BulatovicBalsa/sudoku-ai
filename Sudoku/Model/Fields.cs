﻿using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

namespace Sudoku.Model
{
    internal class Fields
    {
        const int _MOV = 5;

        private Field[] arr = new Field[81];
        public Field[] Arr { get { return arr; } }

        private short size = 40;
        public short Size { get { return size; } }

        private Button[] buttons = new Button[81];

        public Button[] Buttons { get { return buttons; } set { buttons = value; } }

        public Fields()
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = new Field();
            }
        }

        public void Draw(Canvas canMain)
        {
            int[,] res = new int[,] {
              { 5, 3, 4, 6, 7, 8, 9, 1, 2},
              { 6, 7, 2, 1, 9, 5, 3, 4, 8 },
              { 1, 9, 8, 3, 4, 2, 5, 6, 7 },
              { 8, 5, 9, 7, 6, 1, 4, 2, 3 },
              { 4, 2, 6, 8, 5, 3, 7, 9, 1 },
              { 7, 1, 3, 9, 2, 4, 8, 5, 6 },
              { 9, 6, 1, 5, 3, 7, 2, 8, 4 },
              { 2, 8, 7, 4, 1, 9, 6, 3, 5 },
              { 3, 4, 5, 2, 8, 6, 1, 7, 9} 
            };
            int currentHeight = -size + _MOV;
            int paddingLeft = _MOV;
            Button btn;
            for (int i = 0; i < arr.Length; i++)
            {
                btn = new Button();
                buttons[i] = btn;

                arr[i].Value = 0; //(ushort) res[i / 9, i % 9];
                buttons[i].Content = ""; // res[i / 9, i % 9];
                buttons[i].FontSize = 22;
                buttons[i].FontWeight = FontWeights.Medium;
                if (Paint(i)) buttons[i].Background = Brushes.Yellow;

                if (arr[i].Value != 0) { buttons[i].Content = arr[i].Value.ToString(); }

                buttons[i].Height = size;
                buttons[i].Width = size;

                if (i % 9 == 0) { currentHeight += size; }

                canMain.Children.Add(buttons[i]);

                Canvas.SetLeft(buttons[i], paddingLeft + (i % 9) * size);
                Canvas.SetTop(buttons[i], currentHeight);

                buttons[i].Click += new RoutedEventHandler((sender, e) => {
                    bool left = false;
                    if (Mouse.LeftButton == MouseButtonState.Released)
                        left = true;
                    int t = getPosition(Mouse.GetPosition(canMain).X, Mouse.GetPosition(canMain).Y);
                    FieldPressed(t, left);
                });
                buttons[i].MouseDown += new MouseButtonEventHandler((sender, e) => {
                    int t = getPosition(Mouse.GetPosition(canMain).X, Mouse.GetPosition(canMain).Y);
                    FieldPressed(t, false);
                });
            }
        }
        private int getPosition(double x, double y)
        {
            int i = 0;
            int j = 0;
            i = (int) x - _MOV;
            i = i / size;
            j = ((int) y - _MOV) / size;
            return 9 * j + i;
        }
        public void FieldPressed(int i, bool leftClick)
        {
            if (arr[i].Solved) { return; }
            if (leftClick) { arr[i].Value++; changeButtonValue(i); return; }
            else { arr[i].Value--; changeButtonValue(i); return; }
        }

        public void changeButtonValue(int i)
        {
            if (arr[i].Value == 65535) arr[i].Value = 9;
            if (arr[i].Value > 9) arr[i].Value = 0;
            if (arr[i].Value == 0) buttons[i].Content = "";
            else buttons[i].Content = arr[i].Value.ToString();
        }

        private bool Paint(int x)
        {
            int i = x % 9;
            int j = x / 9;
            if (i > 2 && i < 6)
            {
                if (j < 3 || j > 5)
                    return true;
            }
            if (i < 3 || i > 5)
            {
                if (j > 2 && j < 6) return true;
            }
            return false;
        }

        public void UnsolveAll()
        {
            int i = 0;
            foreach (var item in arr)
            {
                buttons[i].Foreground = Brushes.Black;
                item.Solved = false;
                i++;
            }
        }
    }
}
