using AForge.Imaging.Filters;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;


namespace ImageDisplayApp
{
    public partial class Form1 : Form
    {
        private Bitmap? img;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            var file = openFileDialog1.FileName;
            List<Bitmap> filteredImgs = new List<Bitmap>();

 /*           IFilter[] filters = new IFilter[]
            {
                new Grayscale(0.2125, 0.7154, 0.0721),
                new GaussianBlur(),
                new SobelEdgeDetector(),
                new Threshold(100) 
            };*/

            Threshold filter_ = new Threshold(100);

            int numberOfThreads = 4;

            if (file != null)
            {
                img = new Bitmap(file);
                pictureBox1.Image = img;

                List<Bitmap> listImgs = new List<Bitmap>();
                
                for (int i = 0; i < numberOfThreads; i++)
                {
                    listImgs.Add(new Bitmap((Bitmap)img.Clone()));
                }

                //img = ToGrayscale(img);

                Parallel.ForEach(listImgs, img =>
                {

                    for (int i = 0;)
                  
                    filteredImgs.Add(img);
                });
            }

            pictureBox2.Image = filteredImgs[0];
            pictureBox3.Image = filteredImgs[1];
            pictureBox3.Image = filteredImgs[2];
            pictureBox4.Image = filteredImgs[3];
        }

     /*   Bitmap ToGrayscale(Bitmap source)
        {
            Parallel.For(0, source.Height, y =>
            {
                for (int x = 0; x < source.Width; x++)
                {
                    Color current = source.GetPixel(x, y);
                    int avg = (current.R + current.B + current.G) / 3;
                    Color output = Color.FromArgb(avg, avg, avg);
                    source.SetPixel(x, y, output);
                }
            });

            return source;
        }*/
    }
}
