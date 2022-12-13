using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO.Ports;

namespace ASCIIConverter
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            FileDialog file = new OpenFileDialog
            {
                Filter = "Images | *.png; *.jpg; *.JPEG; *.bmp"
            };

            while (true)
            {
                Console.WriteLine("Press Enter to start...");
                Console.ReadKey();

                if (file.ShowDialog() != DialogResult.OK)
                {
                    continue;
                }

                Bitmap image = new Bitmap(file.FileName);

                image = resizeBitmap(image);

                image = toBlackAndWhite(image);

                char[][] art = toASCII(image);

                foreach(var row in art)
                {
                    Console.WriteLine(row);
                }
                
                Console.ReadKey();
                Console.Clear();
            }
        }

        public static Bitmap toBlackAndWhite(Bitmap img)
        {
            Bitmap blackImage = img;
            for(int x = 0; x < blackImage.Width; x++)
            {
                for(int y = 0; y < blackImage.Height; y++)
                {
                    Color color = blackImage.GetPixel(x, y);
                    int avg = (color.R + color.G + color.B) / 3;
                    blackImage.SetPixel(x, y, Color.FromArgb(avg, avg, avg));
                }
            }
            return blackImage;
        }
        
        public static char[][] toASCII(Bitmap img)
        {
            char[][] art = new char[img.Height][];
            char[] palit = new char[] {' ', '.', '"', ',', ':', ';', '!', '~', '+', '-', 'x', 'm', 'o', '*', '#', 'W', '&', '8', '@' };
            for(int y = 0; y < img.Height; y++)
            {
                art[y] = new char[img.Width];
                for(int x = 0; x < img.Width; x++)
                {
                    float coof = 255 / (palit.Length - 1);
                    int index = Convert.ToInt32(img.GetPixel(x, y).R / coof);
                    art[y][x] = palit[index];
                }
            }

            return art;
        }

        public static Bitmap resizeBitmap(Bitmap img)
        {
            Bitmap resizedImg = img;

            int maxWidth = 250;
            double maxHeight = resizedImg.Height / 1.8 * maxWidth / resizedImg.Width;

            if(resizedImg.Width > maxWidth || resizedImg.Height > maxHeight)
            {
                resizedImg = new Bitmap(resizedImg, new Size(maxWidth, (int)maxHeight));
            }

            return resizedImg;
        }
    }
}
