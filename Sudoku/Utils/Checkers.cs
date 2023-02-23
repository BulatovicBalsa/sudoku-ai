using Sudoku.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Sudoku.Utils
{
    internal class Checkers
    {
        private bool tried = false;
        public bool IsPlayable(Fields fields)
        {
            //if (SolvedFieldsCount(fields) < 17) return false;
            if (!AreRowsValid(fields)) return false;
            if (!AreColumnsValid(fields)) return false;
            if (!AreBlocksValid(fields)) return false;
            return true;
        }

        public int SolvedFieldsCount(Fields fields)
        {
            return 81 - fields.UnsolvedFields;
        }

        public void InitGame(Fields fields)
        {
            int i = 0;
            foreach (Field item in fields.Arr)
            {
                if (item.Value != 0)
                {
                    fields.Buttons[i].Foreground = Brushes.Red;
                    item.Solved = true;
                    fields.UnsolvedFields--;
                }
                i++;
            }
        }

        public Field[] GetRow(Fields fields, int row)
        {
            Field[] fields1 = new Field[9];
            for (int i = row * 9; i < row * 9 + 9; i++)
            {
                fields1[i - row * 9] = fields.Arr[i];
            }
            return fields1;
        }

        public Field[] GetColumn(Fields fields, int column)
        {
            Field[] fields1 = new Field[9];
            for (int i = column; i < 81; i += 9)
            {
                fields1[(i - column) / 9] = fields.Arr[i];
            }
            return fields1;
        }

        public Field[] GetBlock(Fields fields, int block)
        {
            Field[] fields1 = new Field[9];
            int i = block / 3;
            int j = block % 3;
            int x = (i * 27) + j * 3;
            int counter = 0;

            for (i = x; i < x + 3; i++)
            {
                for (j = 0; j < 3; j++)
                {
                    fields1[counter++] = fields.Arr[i + 9 * j];
                }
            }
            return fields1;
        }

        public bool IsRowColumnBlockValid(Field[] arr)
        {
            HashSet<int> set = new HashSet<int>();
            foreach (var item in arr)
            {
                if (set.Contains(item.Value) && item.Value != 0)
                {
                    return false;
                }
                else
                {
                    set.Add(item.Value);
                }
            }
            return true;
        }

        public bool AreRowsValid(Fields fields)
        {
            for (int i = 0; i < 9; i++)
            {
                Field[] row = GetRow(fields, i);
                if (!IsRowColumnBlockValid(row))
                {
                    return false;
                }
                Array.Clear(row);
            }
            return true;
        }

        public bool AreColumnsValid(Fields fields)
        {
            for (int i = 0; i < 9; i++)
            {
                Field[] row = GetColumn(fields, i);
                if (!IsRowColumnBlockValid(row))
                {
                    return false;
                }
            }
            return true;
        }
        public bool AreBlocksValid(Fields fields)
        {
            for (int i = 0; i < 9; i++)
            {
                Field[] row = GetBlock(fields, i);
                if (!IsRowColumnBlockValid(row))
                {
                    return false;
                }
            }
            return true;
        }

        public int GetBlockIndex(int index)
        {
            int i = index / 9;
            int j = index % 9;

            return i - i % 3 + j / 3;
        }

        public void SolveSudoku(Fields fields)
        {
            int before = fields.UnsolvedFields;

            SetCandidates(fields);
            
            for (int i = 0; i < fields.Arr.Length; i++)
            {
                if (!fields.Arr[i].Solved)
                    CheckAllArray(i, fields);
            }
            if (before == fields.UnsolvedFields)
            {   
                if (tried)
                {
                    tried = false;
                    int firstUnsolved = FindUnsolved(fields);
                    SolveUnsolvedField(firstUnsolved, fields);
                    return;
                }

                SetCandidates(fields);
                for (int i = 0; i < 9; i++)
                {
                    Field[] block = GetBlock(fields, i);
                    AllCandsOneRowColumn(block, fields);
                    tried = true;
                }
            }
        }

        private void SetCandidates(Fields fields)
        {
            for (int i = 0; i < fields.Arr.Length; i++)
            {
                if (fields.Arr[i].Solved)
                    continue;

                int blockIndex = GetBlockIndex(i);
                int row = i / 9;
                int column = i % 9;

                RemoveCandidates(GetRow(fields, row), fields.Arr[i]);
                RemoveCandidates(GetColumn(fields, column), fields.Arr[i]);
                RemoveCandidates(GetBlock(fields, blockIndex), fields.Arr[i]);

                var f = fields.Arr[i];
                if (f.Candidates.Count == 1)
                {
                    f.Value = f.Candidates.First();
                    fields.Buttons[i].Content = f.Value;
                    f.Solved = true;
                    fields.UnsolvedFields--;
                }
            }
        }

        private void AllCandsOneRowColumn(Field[] block, Fields f)
        {
            Field[] block2 = new Field[0];
            for (int i = 1; i < 10; i++)
            {
                int row = -1;
                int column = -1;
                List<Field> list = new List<Field>();
                for (int j = 0; j < block.Length; j++)
                {
                    if (block[j].Solved) continue;
                    if (!block[j].Candidates.Contains((ushort)i)) continue;
                    Point p = GetPoint(block[j], f);
                    if (row == -1) row = (int)p.X;
                    else if (row != (int)p.X) row = -2;
                    if (column == -1) column = (int)p.Y;
                    else if (column != (int)p.Y) column = -2;
                    list.Add(block[j]);

                }
                if (row < 0 && column < 0) continue;
                if (list.Count == 1)
                {
                    list[0] = f.Arr[row * 9 + column];
                    list[0].Value = (ushort)i;
                    f.Buttons[row*9 + column].Content = list[0].Value;
                    list[0].Solved = true;
                    f.UnsolvedFields--;
                }

                if (row >= 0) block2 = GetRow(f, row);
                if (column >= 0) block2 = GetColumn(f, column);
                for (int j = 0; j < block2.Length; j++)
                {
                    if (list.Contains(block2[j])) continue;
                    block2[j].Candidates.Remove((ushort)i);
                }
            }
        }

        private Point GetPoint(Field field, Fields f)
        {
            Point p = new Point(-1, -1);
            for (int i = 0; i < f.Arr.Length; i++)
            {
                if (f.Arr[i] == field)
                {
                    p.X = (int) i / 9;
                    p.Y = i % 9;
                    return p;
                }
            }
            return p;
        }

        public void RemoveCandidates(Field[] fields, Field f)
        {
            foreach (var item in fields)
            {
                if (item.Value != 0)
                    f.Candidates.Remove(item.Value);
            }
        }

        public void CheckRow(int b, Fields fields)
        {
            Field[] row = GetRow(fields, b / 9);
            CheckArray(b, fields, row);
        }

        public void CheckColumn(int b, Fields fields)
        {
            Field[] column = GetColumn(fields, b % 9);
            CheckArray(b, fields, column);
        }

        public void CheckBlock(int b, Fields fields)
        {
            Field[] block = GetBlock(fields, GetBlockIndex(b));
            CheckArray(b, fields, block);
        }

        public void CheckArray(int b, Fields fields, Field[] block)
        {
            if (fields.Arr[b].Solved) return;
            for (int i = 1; i < 10; i++)
            {
                bool fillable = true;
                if (!fields.Arr[b].Candidates.Contains((ushort)i))
                    continue;

                foreach (var item in block)
                {
                    if (item == fields.Arr[b]) continue;
                    if (item.Value == i) { fillable = false; break; }
                    if (item.Solved) continue;
                    if (item.Candidates.Contains((ushort) i))
                    {
                        fillable = false;
                        break;
                    }
                }
                if (fillable)
                {
                    var f = fields.Arr[b];
                    f.Value = (ushort) i;
                    fields.Buttons[b].Content = f.Value;
                    f.Solved = true;
                    fields.UnsolvedFields--;
                    return;
                }
            }
        }

        public void CheckAllArray(int b, Fields fields)
        {
            CheckBlock(b, fields);
            CheckRow(b, fields);
            CheckColumn(b, fields);
        }

        public int FindUnsolved(Fields fields)
        {
            for (int i = 0; i < fields.Arr.Length; i++)
            {
                if (!fields.Arr[i].Solved)
                {
                    return i;
                }
            }
            return -1;
        }

        public void SolveUnsolvedField(int b, Fields fields)
        {
            var f = fields.Arr[b];
            f.Value = f.Candidates.First();
            fields.Buttons[b].Content = f.Value;
            f.Solved = true;
            fields.UnsolvedFields--;
            return;
        }

        public bool GameOver(Fields fields)
        {
            return fields.UnsolvedFields == 0;
        }
    }
}
