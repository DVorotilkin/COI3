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

namespace Laba3_COI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //--
        #region Переменные

        int width, height;//Размеры изображения

        //------Для оригинального изображения
        Bitmap OriginalImage;
        byte[, ,] OriginalImageByte;

        //------Для изменённого изображения
        byte[, ,] AlteredImageByte;

        //-----Для сохранения оригинального изображения при добавлении шума
        Bitmap BackupImage;
        byte[, ,] BackupImageByte;
        
        bool FormsSuccessfullFlag = false;//Нажал ли пользователь "Применить" в дочерней форме
        bool IsNoiseImageByte = false;//Зашумлено ли изображение
        bool IsNoiseImage;//Отображено зашумлённое изображение или нет

        #endregion

        //--
        #region Работа меню программы

        //----Файл
        #region Меню "Файл"

        private void загрузитьИзображениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    посмотретьИсходноеИзображениеToolStripMenuItem.Text = "Посмотреть исходное изображение";
                    посмотретьИсходноеИзображениеToolStripMenuItem.Enabled = false;
                    IsNoiseImageByte = false;
                    OriginalImage = (Bitmap)Bitmap.FromFile(openFileDialog1.FileName);
                    EnabledMeny(true);
                    width = OriginalImage.Width;
                    height = OriginalImage.Height;
                    DownConsole.Text = "Изображение успешно загружено.";
                    OriginalImageByte = toBytes(OriginalImage);
                    pictureBox1.Image = OriginalImage;
                    pictureBox2.Image = null;
                    label1.Text = "Количество пикселей: " + (height * width).ToString();
                    OpenHaracteristicMenu(false);
                }
                catch (Exception openE)
                {
                    DownConsole.Text = "Невозможно загрузить изображение: " + openE.Message;
                }
            }
        }
        //------Пункт меню  загрузки изображений

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //------Пункт меню выхода из программы

        #endregion

        //----Фильтрация
        #region Меню "Фильтрация"
        private void добавитьАддитивныйШумToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (IsNoiseImageByte)
            {
                OriginalImage = BackupImage;
                OriginalImageByte = BackupImageByte;
            }
            Noise N = new Noise();
            N.Owner = this;
            N.ShowDialog();
            if (FormsSuccessfullFlag)
            {
                BackupImage = (Bitmap)OriginalImage.Clone();
                OriginalImage = N.getNoiseImage();
                IsNoiseImage = true;
                pictureBox1.Image = OriginalImage;
                посмотретьИсходноеИзображениеToolStripMenuItem.Text = "Посмотреть исходное изображение";
                посмотретьИсходноеИзображениеToolStripMenuItem.Enabled = true;
                удалитьШумToolStripMenuItem.Enabled = true;
                IsNoiseImageByte = true;
            }
            FormsSuccessfullFlag = false;
            OpenHaracteristicMenu(false);
        }
        //------Пункт меню Добавить шум

        private void удалитьШумToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OriginalImageByte = BackupImageByte;
            OriginalImage = BackupImage;
            pictureBox1.Image = OriginalImage;
            pictureBox2.Image = null;
            удалитьШумToolStripMenuItem.Enabled = false;
            посмотретьИсходноеИзображениеToolStripMenuItem.Text = "Посмотреть исходное изображение";
            посмотретьИсходноеИзображениеToolStripMenuItem.Enabled = false;
            IsNoiseImageByte = false;
            OpenHaracteristicMenu(false);
        }
        //------Пункт меню Удалить шум

        private void медианнаяФильтрацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MedFilt MF = new MedFilt();
            MF.Owner = this;
            MF.Show();
            OpenHaracteristicMenu(true);
        }
        //------Пункт меню Медианная фильтрация

        private void эффективностьМетодаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Kriterii k = new Kriterii();
            k.Owner = this;
            k.Show();
        }
        //------Пункт меню Эффективность метода

        private void посмотретьИсходноеИзображениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsNoiseImage)
            {
                pictureBox1.Image = BackupImage;
                посмотретьИсходноеИзображениеToolStripMenuItem.Text = "Посмотреть зашумлённое изображение";
                IsNoiseImage = false;
            }
            else
            {
                pictureBox1.Image = OriginalImage;
                посмотретьИсходноеИзображениеToolStripMenuItem.Text = "Посмотреть исходное изображение";
                IsNoiseImage = true;
            }
        }
        //------Пункт меню Посмотреть исходное изображение

        #endregion

        #endregion
        //--
        #region Специальные и служебные методы

        //----Методы get
        #region Методы get

        public int getHeight()
        {
            return height;
        }
        //------Метод возвращающий высоту изображения

        public int getWidth()
        {
            return width;
        }
        //------Метод возвращающий ширину изображения

        public byte[, ,] getOriginalImageByte()
        {
            return OriginalImageByte;
        }
        //------Метод возращающий массив байтов оригинального изображения

        public byte[, ,] getAlteredImageByte()
        {
            return AlteredImageByte;
        }
        //------Метод возвращающий массив байтов изменённого изображения

        public byte[, ,] getBackupImageByte()
        {
            return BackupImageByte;
        }
        //------Метод возвращающий массив байтов изображения в Бэкапе

        public Bitmap getOriginalImage()
        {
            return OriginalImage;
        }
        //------Метод возарщающий орагинальне изображение

        public bool getIsNoisedImageByte()
        {
            return IsNoiseImageByte;
        }
        //------Метод возращающий логическое значение является ли изображение зашумлённым

        #endregion

        //----Методы set
        #region Методы set

        public void setPictureBox2(byte[, ,] byteMass)
        {
            pictureBox2.Image = toBitmap(byteMass);
        }
        //------Метод заменющий пикчерБокс2 на приходящий массив байт

        public void setAlteredImageByte(byte[,,] byteMass)
        {
            AlteredImageByte = (byte[,,])byteMass.Clone();
        }
        //------Метод заменющий массив изменённого изображения на приходящий массив байт

        #endregion

        //----Методы изменения bool переменных
        #region Методы изменения bool переменных

        private void EnabledMeny(bool flag)
        {
            фильтрацияToolStripMenuItem.Enabled = flag;
        }
        //------Метод определяет активны ли меню обработки изображения

        public void isSeccessfullyForm(bool flag)
        {
            FormsSuccessfullFlag = flag;
        }
        //------Метод определяет, нажал ли пользователь кнопку "Применить" в дочерней форме

        private void OpenHaracteristicMenu(bool flag)
        {
            эффективностьМетодаToolStripMenuItem.Enabled = flag;
        }
        //------Метод определяет активен ли пункт меню "Эффективность метода"

        #endregion

        //----Методы конвертации данны
        #region Методы конвертации данны

        public byte[, ,] toBytes(Bitmap bmp)
        {
            byte[, ,] res = new byte[3, height, width];
            for (int y = 0; y < height; ++y)
                for (int x = 0; x < width; ++x)
                {
                    Color color = bmp.GetPixel(x, y);
                    res[0, y, x] = color.R;
                    res[1, y, x] = color.G;
                    res[2, y, x] = color.B;
                }
            return res;
        }
        //------Метод конвертирует изображение в массив байт

        public Bitmap toBitmap(byte[, ,] byteMass)
        {
            Bitmap res = new Bitmap(width, height);
            for (int y = 0; y < height; ++y)
                for (int x = 0; x < width; ++x)
                {
                    Color color = Color.FromArgb(byteMass[0, y, x],
                        byteMass[1, y, x],
                        byteMass[2, y, x]);
                    res.SetPixel(x, y, color);
                }
            return res;
        }
        //------Метод конвертирует массив байт в изображение

        public byte toByte(int pixel)
        {
            if (pixel <= 0)
                return 0;
            else if (pixel > 255)
                return 255;
            else
                return (byte)pixel;
        }
        //------Метод ковертирует значение типа int в значение типа byte

        #endregion

        //----Служебные методы
        #region Служебные методы

        public void ApplyNoise(byte[, ,] noiseImage)
        {
            BackupImageByte = OriginalImageByte;
            OriginalImageByte = noiseImage;
            pictureBox2.Image = null;
        }
        //------Метод применяет к изображению шум
        #endregion

        #endregion             
    }
}