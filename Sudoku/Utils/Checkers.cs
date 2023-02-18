using Sudoku.Model;
using System;
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
            int i = 0;
            foreach (Field item in fields.Arr)
            {
                if (item.Solved) i++;
            }
            return i;
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

        public void SetCandidates(Fields fields)
        {
            for (int i = 0; i < fields.Arr.Length; i++)
            {
                if (fields.Arr[i].Solved)
                    continue;

                int block = GetBlockIndex(i);
                int row = i / 9;
                int column = i % 9;

                RemoveCandidates(GetRow(fields, row), fields.Arr[i]);
                RemoveCandidates(GetColumn(fields, column), fields.Arr[i]);
                RemoveCandidates(GetBlock(fields, block), fields.Arr[i]);

                var f = fields.Arr[i];
                if (f.Candidates.Count == 1)
                {
                    f.Value = f.Candidates.First();
                    fields.Buttons[i].Content = f.Value;
                    f.Solved = true;
                }
            }
            for (int i = 0; i < fields.Arr.Length; i++)
            {
                if (!fields.Arr[i].Solved)
                    CheckBlock(i, fields);
            }

        }

        public void RemoveCandidates(Field[] fields, Field f)
        {
            foreach (var item in fields)
            {
                if (item.Value != 0)
                    f.Candidates.Remove(item.Value);
            }
        }

        public void CheckBlock(int b, Fields fields)
        {
            Field[] block = GetBlock(fields, GetBlockIndex(b));
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
                    return;
                }
            }
        }
    }
}
