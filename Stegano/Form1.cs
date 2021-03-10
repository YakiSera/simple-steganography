using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stegano
{
    public partial class Form1 : Form
    {
        private Bitmap image;
        private string message;

        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "png files (*.png)|*.png|bmp files (*.bmp)|*.bmp";
            saveFileDialog1.Filter = "png files (*.png)|*.png|bmp files (*.bmp)|*.bmp";
        }

        private void LoadImageViaDialogue()
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                image = new Bitmap(openFileDialog1.FileName);
                imagePreview.Image = new Bitmap(image);
            }
        }

        private void SaveImageViaDialogue()
        {
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                image.Save(saveFileDialog1.FileName);
            }
        }

        private void EncryptButton_Click(object sender, EventArgs e)
        {
            LoadImageViaDialogue();
            if (image != null)
            {
                message = messageBox.Text + '$';
                image = Steganography.Encrypt(image, message, 16);
                SaveImageViaDialogue();
            }
        }

        private void DecryptButton_Click(object sender, EventArgs e)
        {
            LoadImageViaDialogue();
            if (image != null)
            {
                message = Steganography.Decrypt(image, 16);
                messageBox.Text = message;
            }
        }

    }
}
