using System;
using System.Drawing;
using System.Windows.Forms;
using CursorRemote.Library;

using Emgu.CV;
using Emgu.CV.Structure;

using DirectShowLib;
using Emgu.CV.Face;

namespace EcoVR
{
    public partial class Form1 : Form
    {
        // Позиции лица
        private int positionFaceX = 0;
        private int positionFaceY = 0;
        private int newPositionFaceX = 0;
        private int newPositionFaceY = 0;

        // Значение смещения положения мыши
        private int mouseOffset = 5;

        // Значения границ смещения лица
        private int offsetX = 30;
        private int offsetY = 25;

        private int startTimer = 100;

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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (webCams.Length == 0)
                {
                    throw new Exception("Нет доступных камер!");
                }
                else if (toolStripComboBox1.SelectedItem == null)
                {
                    throw new Exception("Необходимо выбрать камеру!");
                }
                else if (capture != null)
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

                foreach (var face in faces)
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        using (var pen = new Pen(Color.Yellow, 3))
                        {
                            graphics.DrawRectangle(pen, face);
                            graphics.DrawEllipse(pen, face.X + face.Width / 2, face.Y + face.Height / 2, 10, 10);
                        }
                    }
                    // Проверяем есть ли начальные значения положения лица
                    if (positionFaceX == 0 && positionFaceY == 0)
                    {
                        positionFaceX = face.X + face.Height / 2;
                        positionFaceY = face.Y;
                        api.MoveRelative(0.4, 0.4);
                    }
                    else
                    {
                        newPositionFaceX = face.X + face.Height / 2;
                        newPositionFaceY = face.Y;
                        if (newPositionFaceX > positionFaceX + offsetX)
                        {
                            api.Move(api.X + mouseOffset, api.Y);
                            startTimer = 100;
                        }
                        if (newPositionFaceX < positionFaceX - offsetX)
                        {
                            api.Move(api.X - mouseOffset, api.Y);
                            startTimer = 100;
                        }
                        if (newPositionFaceY > positionFaceY + offsetY)
                        {
                            api.Move(api.X, api.Y + mouseOffset);
                            startTimer = 100;
                        }
                        if (newPositionFaceY < positionFaceY - offsetY)
                        {
                            api.Move(api.X, api.Y - mouseOffset);
                            startTimer = 100;
                        }
                    }
                    Console.WriteLine(startTimer--);
                    if (startTimer == 0)
                    {
                        api.SetMouseLeftClick();
                    }
                    Console.WriteLine($"X = {newPositionFaceX}");
                    Console.WriteLine($"Y = {newPositionFaceY}");
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
