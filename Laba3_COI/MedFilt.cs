using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba3_COI
{
    public partial class MedFilt : Form
    {
        Form1 main;

        int height, width;

        byte[, ,] OriginalImageByte,
            AlteredImageByte;
        
        public MedFilt()
        {
            InitializeComponent();
        }

        private void MedFilt_Load(object sender, EventArgs e)
        {
            main = this.Owner as Form1;
            height = main.getHeight();
            width = main.getWidth();
            OriginalImageByte = main.getOriginalImageByte();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            int SelectedApert = HowRadioClick();
            switch (SelectedApert)
            {
                case 2:
                    AlteredImageByte = SecondApertFilter(OriginalImageByte, SelectedApert);
                    break;
                case 4:
                    AlteredImageByte = FourthAndSixthApertFilter(OriginalImageByte, SelectedApert);
                    break;
                case 5:
                    AlteredImageByte = FifthApertFilter(OriginalImageByte, SelectedApert);
                    break;
                case 6:
                    AlteredImageByte = FourthAndSixthApertFilter(OriginalImageByte, SelectedApert);
                    break;
            };
            main.setAlteredImageByte(AlteredImageByte);
            main.setPictureBox2(AlteredImageByte);
            this.Close();

        }

        private int HowRadioClick()
        {
            if (radioButton2.Checked) return 2;
            if (radioButton4.Checked) return 4;
            if (radioButton5.Checked) return 5;
            return 6;
        }

        private byte[, ,] SecondApertFilter(byte[, ,] byteMass, int select)
        {
            byte[, ,] res = new byte[3, height, width];
            int i, j;
            if (select == 1) { i = 0; j = 1; }
            else { i = 1; j = 0; }
            for (int y = i; y < height - i; y++)
                for (int x = j; x < width - j; x++)
                {
                    res[0, y, x] = CulcSecondFilters(byteMass, y, x, 0);
                    res[1, y, x] = CulcSecondFilters(byteMass, y, x, 1);
                    res[2, y, x] = CulcSecondFilters(byteMass, y, x, 2);
                }
            return res;
        }

        private byte[, ,] FifthApertFilter(byte[, ,] byteMass, int select)
        {
            byte[, ,] res = new byte[3, height, width];
            int shift = (select - 1) / 2;
            for(int y=shift;y<height-shift;y++)
                for (int x = shift; x < width - shift; x++)
                {
                    res[0, y, x] = CulcFifthFilters(byteMass, y, x, 0);
                    res[1, y, x] = CulcFifthFilters(byteMass, y, x, 1);
                    res[2, y, x] = CulcFifthFilters(byteMass, y, x, 2);
                }
            return res;
        }

        private byte[, ,] FourthAndSixthApertFilter(byte[, ,] byteMass, int select)
        {
            byte[,,] res = new byte[3,height,width];
            int shift = select/2 -1;
            for (int y = shift; y < height - shift; y++)
                for (int x = shift; x < width - shift; x++)
                {
                    res[0, y, x] = CulcFourthAndSixthFilters(byteMass, select, y, x, 0);
                    res[1, y, x] = CulcFourthAndSixthFilters(byteMass, select, y, x, 1);
                    res[2, y, x] = CulcFourthAndSixthFilters(byteMass, select, y, x, 2);
                }
            return res;
        }

        private byte CulcSecondFilters(byte[, ,] byteMass, int y, int x, int chennal)
        {
            byte[] filter = new byte[3];
            for (int j = -1; j < 2; j++)
                filter[1 + j] = byteMass[chennal, y + j, x];
            return filter.OrderBy(q => q).ToArray()[1];
        }

        private byte CulcFifthFilters(byte[, ,] byteMass, int y, int x, int chennal)
        {
            int scaleApert = 13;
            List<byte> filter = new List<byte>(scaleApert);
            int shift = 2;
            for (int i = -shift; i < shift + 1; i++)
                for (int j = Math.Abs(i); j < 5 - Math.Abs(i); j++)
                {
                    filter.Add(byteMass[chennal, y + i, x + j - shift]);
                }
            return filter.OrderBy(q => q).ToArray()[(scaleApert-1)/2];
        }

        private byte CulcFourthAndSixthFilters(byte[, ,] byteMass, int WhatisFilter, int y, int x, int chennal)
        {
            int scale = (WhatisFilter - 1) * (WhatisFilter - 1);
            int shift = WhatisFilter/2 -1;
            List<byte> filter = new List<byte>(scale);
            for (int i = -shift; i < shift + 1; i++)
                for (int j = -shift; j < shift + 1; j++)
                {
                    filter.Add(byteMass[chennal, y + i, x + j]);
                }
            return filter.OrderBy(q => q).ToArray()[scale / 2 - 1];
        }
    }
}
