using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelComponent
{

    public class Pixel : Panel
    {
        Form Form;
        public Pixel(Form form, int width, int height, Color color)
        {
            this.Form = form;
            this.Width = width;
            this.Height = height;
            this.BackColor = color;
        }

        internal void Place(int x, int y)
        {
            this.Location = new Point(x, y);
            Form.Controls.Add(this);
        }

        public void SetColor(Color color)
        {
            this.BackColor = color;
        }
    }

    public class Screen
    {
        public int RowCount { get { return Pixels.GetLength(1); } }
        public int ColumnCount { get { return Pixels.GetLength(0); } }

        public Pixel[,] GetRows(params int[] row_list)
        {
            Pixel[,] result = new Pixel[x_pixel_count, row_list.Length];

            for (int i = 0; i < row_list.Length; i++)
            {
                var newRow = GetRow(row_list[i]);

                for (int j = 0; j < newRow.Length; j++)
                {
                    result[j, i] = newRow[j];
                }
            }

            return result;
        }

        public Pixel[,] GetColumns(params int[] column_list)
        {
            Pixel[,] result = new Pixel[column_list.Length, y_pixel_count];

            for (int i = 0; i < column_list.Length; i++)
            {
                var newColumn = GetColumn(column_list[i]);

                for (int j = 0; j < newColumn.Length; j++)
                {
                    result[i, j] = newColumn[j];
                }
            }

            return result;
        }

        public Pixel[] GetRow(int row)
        {
            if (Math.Abs(row) > Pixels.GetLength(1))
            {
                throw new Exception($"Row {row} is not existing.");
            }
            else if (row < 0)
            {
                row = Pixels.GetLength(1) + row;
            }

            Pixel[] pixels = new Pixel[x_pixel_count];
            for (int i = 0; i < x_pixel_count; i++)
            {
                pixels[i] = Pixels[i, row];
            }
            return pixels;
        }

        public Pixel[] GetColumn(int column)
        {
            if (Math.Abs(column) > Pixels.GetLength(0))
            {
                throw new Exception($"Column {column} is not existing.");
            }
            else if (column < 0)
            {
                column = Pixels.GetLength(0) + column;
            }

            Pixel[] pixels = new Pixel[y_pixel_count];
            for (int i = 0; i < y_pixel_count; i++)
            {
                pixels[i] = Pixels[column, i];
            }
            return pixels;
        }

        /*
         00 10 20 30
         01 11 21 31
         02 12 22 32
         03 13 23 33
         */

        Form Form;
        public Pixel[,] Pixels;
        int x_pixel_count;
        int y_pixel_count;

        public Screen(int x_pixel_count, int y_pixel_count, int pixel_width, int pixel_height, Form form, Color color)
        {
            this.Form = form;
            Pixels = new Pixel[x_pixel_count, y_pixel_count];
            this.x_pixel_count = x_pixel_count;
            this.y_pixel_count = y_pixel_count;
            InstantiatePixels(pixel_width, pixel_height, color);
        }

        public void DrawGrids(bool draw = true)
        {
            for (int i = 0; i < Pixels.GetLength(0); i++)
            {
                for (int j = 0; j < Pixels.GetLength(1); j++)
                {
                    if (draw)
                        Pixels[i, j].BorderStyle = BorderStyle.FixedSingle;
                    else
                        Pixels[i, j].BorderStyle = BorderStyle.None;
                }
            }
        }

        internal void InstantiatePixels(int pixel_width, int pixel_height, Color first_color)
        {
            for (int i = 0; i < x_pixel_count; i++)
            {
                for (int j = 0; j < y_pixel_count; j++)
                {
                    Pixels[i, j] = new Pixel(form: Form, width: pixel_width, height: pixel_height, color: first_color);
                }
            }
        }

        internal void Place(int x, int y)
        {
            for (int i = 0; i < Pixels.GetLength(0); i++)
            {
                for (int j = 0; j < Pixels.GetLength(1); j++)
                {
                    Pixels[i, j].Place(x+i*Pixels[i, j].Width, y+j*Pixels[i, j].Height);
                }
            }
        }

        internal Pixel GetPixel(int x, int y)
        {
            if (Math.Abs(x) > Pixels.GetLength(0) || Math.Abs(y) > Pixels.GetLength(1))
            {
                throw new Exception($"Pixel ({x},{y}) is not existing.");
            }

            if (x < 0) x = Pixels.GetLength(0) + x;
            if (y < 0) y = Pixels.GetLength(1) + y;

            return Pixels[x, y];
        }

    }

    public class Utilities
    {
        public int MixColors(int from, int to, int ratio)
        {
            return from + (to - from) * ratio / 100;
        }
    }
}
