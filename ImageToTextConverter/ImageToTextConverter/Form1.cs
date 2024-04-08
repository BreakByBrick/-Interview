using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ImageToTextConverter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Bitmap bmp;
        int picture_width, picture_height;

        bool isPictureChoosed = false;

        //кнопка выбора изображения
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files (*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Image image = Image.FromFile(dialog.FileName);
                picture_width = image.Width;
                picture_height = image.Height;
                bmp = new Bitmap(image, picture_width, picture_height);

                isPictureChoosed = true;
                textBox1.Text = dialog.FileName;

                textBox3.Text = textBox3.Text + "Выбрано изображение - " + dialog.FileName + "." + Environment.NewLine;
                textBox3.Text = textBox3.Text + "Ширина - " + Convert.ToString(bmp.Width)+ " пикселей." + Environment.NewLine;
                textBox3.Text = textBox3.Text + "Высота - " + Convert.ToString(bmp.Height) + " пикселей." + Environment.NewLine;
                textBox3.Text = textBox3.Text + Environment.NewLine;
            }
        }

        //кнопка конвертирования
        private void button2_Click(object sender, EventArgs e)
        {
            if (isPictureChoosed == false)
            {
                textBox3.Text = textBox3.Text + "Выберите изображение" + Environment.NewLine;
                textBox3.Text = textBox3.Text + Environment.NewLine;
            }
            else if  (textBox5.Text == "")
            {
                textBox3.Text = textBox3.Text + "Введите значение параметра 'Масштаб конвертирования'." + Environment.NewLine;
                textBox3.Text = textBox3.Text + Environment.NewLine;
            }
            else if (Convert.ToInt32(textBox5.Text) == 0)
            {
                textBox3.Text = textBox3.Text + "Неверно указано значение параметра 'Масштаб конвертирования'. минимальное значение - 1." + Environment.NewLine;
                textBox3.Text = textBox3.Text + Environment.NewLine;
            }
            else if ((Convert.ToInt32(textBox5.Text) >bmp.Width / 2)||(Convert.ToInt32(textBox5.Text) > bmp.Height / 2))
            {
                textBox3.Text = textBox3.Text + "Слишком большое значение параметра 'Масштаб конвертирования' " + Environment.NewLine;
                textBox3.Text = textBox3.Text + Environment.NewLine;
            }
            else if (textBox6.Text == "")
            {
                textBox3.Text = textBox3.Text + "Параметр символы должен содержать хотя бы один символ." + Environment.NewLine;
                textBox3.Text = textBox3.Text + Environment.NewLine;
            }
            else
            {
                textBox3.Text = textBox3.Text + "Конвертирование...." + Environment.NewLine;
                textBox3.Text = textBox3.Text + Environment.NewLine;

                int pixels_for_symbol_width = Convert.ToInt32(Convert.ToDouble(Convert.ToInt32(textBox5.Text)) / (Convert.ToDouble(bmp.Height) / (Convert.ToDouble(bmp.Width) / 0.85)));
                int pixels_for_symbol_height = Convert.ToInt32(textBox5.Text);

                int number_of_symbols = textBox6.TextLength;
                string [] symbols = new string[number_of_symbols];

                for (int i = 0; i < number_of_symbols; i++)
                {
                    symbols[i] = Convert.ToString(textBox6.Text[i]);
                }

                SaveFileDialog savedialog = new SaveFileDialog();
                savedialog.Title = "Сохранить картинку";
                savedialog.OverwritePrompt = true;
                savedialog.CheckPathExists = true;
                savedialog.Filter = "Text File(*.TXT)|*.TXT";
                if (savedialog.ShowDialog() == DialogResult.OK)
                {
                    textBox2.Text = savedialog.FileName;
                    StreamWriter fin = new StreamWriter(savedialog.FileName, false, Encoding.Default);

                    for (int i = 0; i < bmp.Height / pixels_for_symbol_height; i++)
                    {
                        for (int j = 0; j < bmp.Width / pixels_for_symbol_width; j++)
                        {
                            int avg_color_value = 0;
                            for (int l = 0; l < pixels_for_symbol_width; l++)
                            {
                                for (int p = 0; p < pixels_for_symbol_height; p++)
                                {
                                    int RED_bright = bmp.GetPixel(j * pixels_for_symbol_width + l, i * pixels_for_symbol_height + p).R;
                                    int GREEN_bright = bmp.GetPixel(j * pixels_for_symbol_width + l, i * pixels_for_symbol_height + p).G;
                                    int BLUE_bright = bmp.GetPixel(j * pixels_for_symbol_width + l, i * pixels_for_symbol_height + p).B;
                                    if ((l == 0) && (p == 0))
                                        avg_color_value = (RED_bright + GREEN_bright + BLUE_bright) / 3;
                                    else
                                        avg_color_value = (avg_color_value + (RED_bright + GREEN_bright + BLUE_bright) / 3) / 2;
                                }
                            }


                            int symbol_color = 255 / number_of_symbols;
                            int k = 1;
                            while (symbol_color * k < avg_color_value)
                            {
                                k++;
                            }
                            k--;
                            k = Convert.ToInt32(Convert.ToDouble(k) * (Convert.ToDouble(symbols.Length) / Convert.ToDouble(number_of_symbols)));
                            while (k > symbols.Length - 1)
                                k--;
                            fin.Write("{0,2}", symbols[k]);
                        }
                        fin.Write("\r\n");
                    }
                    fin.Close();
                }

                textBox3.Text = textBox3.Text + "Изображение успешно конвертировано в - " + savedialog.FileName + "." + Environment.NewLine;
                textBox3.Text = textBox3.Text + Environment.NewLine;
            }
        }
    }
}
