namespace PictureBoxAndScrollbarTest
{
    public partial class Form1 : Form
    {
        private int _pictureBoxSize;
        private Random _rng;
        private Point _pictureBoxStartingPoint;

        public Form1()
        {
            InitializeComponent();

            _rng = new Random();
            _pictureBoxSize = groupBox1.Height / 2;

            _pictureBoxStartingPoint = new Point(((groupBox1.Width / 2) - (_pictureBoxSize / 2)) - _pictureBoxSize, (groupBox1.Height / 2) - (_pictureBoxSize / 2));

            for(int i = 0; i < 30; i++)
            {
                PictureBox toAdd = new PictureBox();
                toAdd.BackColor = Color.FromArgb(_rng.Next(256), _rng.Next(256), _rng.Next(256));
                toAdd.Hide();
                toAdd.Width = _pictureBoxSize;
                toAdd.Height = _pictureBoxSize;
                toAdd.Location = _pictureBoxStartingPoint;

                groupBox1.Controls.Add(toAdd);

            }

            hScrollBar1.Maximum = groupBox1.Controls.Count - 3;

            // show the first 3 picture boxes
            groupBox1.Controls[0].Show();
            groupBox1.Controls[1].Location = new Point(_pictureBoxStartingPoint.X + _pictureBoxSize, _pictureBoxStartingPoint.Y);
            groupBox1.Controls[1].Show();
            groupBox1.Controls[2].Location = new Point(_pictureBoxStartingPoint.X + _pictureBoxSize * 2, _pictureBoxStartingPoint.Y);
            groupBox1.Controls[2].Show();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            foreach(Control control in groupBox1.Controls)
            {
                control.Hide();
            }

            for(int i = 0; i < 3; i++)
            {
                groupBox1.Controls[i + hScrollBar1.Value].Location = new Point(_pictureBoxStartingPoint.X + _pictureBoxSize * i, _pictureBoxStartingPoint.Y);
                groupBox1.Controls[i + hScrollBar1.Value].Show();
            }
            
        }
    }
}