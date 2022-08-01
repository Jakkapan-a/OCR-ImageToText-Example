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
using Tesseract;

namespace ImageToText
{
    public partial class Main : Form
    {
        string part;
        public Main()
        {
            InitializeComponent();
        }

        private void MenuItemLoadImage_Click(object sender, EventArgs e)
        {
            rtbOcrResult.Clear();

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select an image file";
            ofd.Filter = "Image Files(*.jpg;*.jpeg;*.png;*.bmp;*.gif)|*.png; *.jpg; *.bmp; *.gif";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                part = System.IO.Path.GetFullPath(ofd.FileName);
                PictureBox.Image = Image.FromFile(part);
                PictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                StatusLabel.Text = part+ " loaded";
            }
        }

        private void MenuItemExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {


                if (PictureBox.Image == null)
                {
                    MessageBox.Show("Please load an image first");
                    return;
                }
                else
                {
                    StatusLabel.Text = "OCR in progress...";
                    rtbOcrResult.Clear();
                    using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                    {
                        using (var img = Pix.LoadFromFile(part))
                        {
                            using (var page = engine.Process(img))
                            {
                                var text = page.GetText();
                                rtbOcrResult.Text = text;
                            }
                        }
                    }
                    StatusLabel.Text = "OCR completed";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                StatusLabel.Text = "OCR failed";
            }
        }
    }
}
