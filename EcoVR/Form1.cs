using System;
using System.Drawing;
using System.Windows.Forms;
using CursorRemote.Library;

using Emgu.CV;
using Emgu.CV.Structure;

using DirectShowLib;

namespace EcoVR
{
    public partial class Form1 : Form
    {
        private CursorApi api = new CursorApi();
        
        //Файл весов распознавания по лицу
        private static CascadeClassifier classifier = new CascadeClassifier("haarcascade_frontalface_default.xml");

        private VideoCapture capture = null;

        private DsDevice[] webCams = null;

        private int selectedCameraId = 0;
        public Form1()
        {
            InitializeComponent();
        }

        // загрузка формы
        private void Form1_Load(object sender, EventArgs e)
        {
            webCams = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

            for (int i = 0; i < webCams.Length; i++)
            {
                toolStripComboBox1.Items.Add(webCams[i].Name);
            }
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedCameraId = toolStripComboBox1.SelectedIndex;
        }

        // смотреть
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if(webCams.Length == 0)
                {
                    throw new Exception("Нет доступных камер!");
                }
                else if(toolStripComboBox1.SelectedItem == null)
                {
                    throw new Exception("Необходимо выбрать камеру!");
                }
                else if(capture != null)
                {
                    capture.Start();
                }
                else
                {
                    capture = new VideoCapture(selectedCameraId);

                    capture.ImageGrabbed += Capture_ImageGrabbed;

                    capture.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Метод создания изображения и отлавливания лиц
        private void Capture_ImageGrabbed(object sender, EventArgs e)
        {
            try
            {
                Mat m = new Mat();

                capture.Retrieve(m);

                Bitmap bitmap = m.ToImage<Bgr, byte>().Flip(Emgu.CV.CvEnum.FlipType.Horizontal).Bitmap;

                Image<Bgr, byte> grayImage = m.ToImage<Bgr, byte>().Flip(Emgu.CV.CvEnum.FlipType.Horizontal);

                Rectangle[] faces = classifier.DetectMultiScale(grayImage, 1.4);

                int width = bitmap.Width;
                int height = bitmap.Height;

                if (faces.Length > 0)
                {
                    var face = faces[0];
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        using (var pen = new Pen(Color.Yellow, 3))
                        {                          
                            int x = face.X + (face.Width) / 2;
                            int y = face.Y + (face.Height) / 2;

                            graphics.DrawEllipse(pen, x, y, 10, 10);
                            
                            double targetX = mouseSensHor.Value * (double)face.X / (double)width;
                            double targetY = mouseSensVert.Value * (double)face.Y / (double)Height;

                            api.MoveRelative(targetX, targetY);
                            this.Text = $"{targetX} {targetY}";
                        }
                    }
                }

                pictureBox1.Image = bitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (capture != null)
                {
                    capture.Pause();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (capture != null)
                {
                    capture.Pause();

                    capture.Dispose();

                    capture = null;

                    pictureBox1.Image.Dispose();

                    pictureBox1.Image = null;
                    
                    selectedCameraId = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            capture.Stop();
            capture.Dispose();
        }
    }
}
