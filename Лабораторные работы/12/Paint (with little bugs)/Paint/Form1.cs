using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paint
{
    public partial class Form1 : Form
    {
        Graphics canvas, showCursor;
        Bitmap bitCanvas;
        static int paintBoxSizeX, paintBoxSizeY;
        Point prev, cur;
        Color color1, color2;
        ColorDialog colorDialog1, colorDialog2;
        OpenFileDialog openFileDialog;
        SaveFileDialog saveFileDialog;
        SolidBrush brush1, brush2;
        Pen pen1, pen2;
        Image image;
        ImageFormat imageFormat;
        bool brush = true;
        bool stamp = false;
        Bitmap bitStamp;
        int toolSize;
        int stampX;
        int stampY;
        int stampDeltaX;
        int stampDeltaY;
        string file = "";
        bool down;
        bool pipette = false;
        Point toDraw;
        bool hasChanged = false;
        public Form1()
        {
            InitializeComponent();
            toolSize = 10;
            paintBoxSizeX = pictureBox1.Width;
            paintBoxSizeY = pictureBox1.Height;
            bitCanvas = new Bitmap(paintBoxSizeX, paintBoxSizeY);
            for (int i = 0; i < paintBoxSizeX; i++)
            {
                for (int j = 0; j < paintBoxSizeY; j++)
                    bitCanvas.SetPixel(i, j, Color.White);        
            }
            canvas = Graphics.FromImage(bitCanvas);
            showCursor = pictureBox1.CreateGraphics();
            pictureBox1.Image = bitCanvas;
            colorDialog1 = new ColorDialog();
            colorDialog2 = new ColorDialog();
            openFileDialog = new OpenFileDialog();
            openFileDialog.AddExtension = true;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Multiselect = false;
            saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF";
            saveFileDialog.DefaultExt = ".png";
            saveFileDialog.AddExtension = true; 
            brush1 = new SolidBrush(toolStripLabel1.ForeColor);
            brush2 = new SolidBrush(toolStripLabel2.ForeColor);
            pen1 = new Pen(brush1, toolSize);
            pen2 = new Pen(brush2, toolSize);
            toolStripTextBox2.Text = paintBoxSizeX.ToString();
            toolStripTextBox3.Text = paintBoxSizeY.ToString();
            stampX = toolSize / 2;
            stampY = toolSize / 2;
        }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (brush)
            {
                if (e.Button == MouseButtons.Left)
                {
                    canvas.FillEllipse(brush1, e.X - toolSize / 2, e.Y - toolSize / 2, toolSize, toolSize);
                }
                if (e.Button == MouseButtons.Right)
                {
                    canvas.FillEllipse(brush2, e.X - toolSize / 2, e.Y - toolSize / 2, toolSize, toolSize);
                }
                pictureBox1.Refresh();
            }
            if (pipette)
            {
                if (e.Button == MouseButtons.Left)
                {
                    color1 = bitCanvas.GetPixel(e.X, e.Y);
                    toolStripLabel1.ForeColor = color1;
                    pen1.Color = color1; brush1.Color = color1;
                }
                if (e.Button == MouseButtons.Right)
                {
                    color2 = bitCanvas.GetPixel(e.X, e.Y);
                    toolStripLabel2.ForeColor = color2;
                    pen2.Color = color2; brush2.Color = color2;
                }
            }
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (down)
            {
                if (brush && !pipette)
                {
                    cur.X = e.X; cur.Y = e.Y;
                    if (e.Button == MouseButtons.Left)
                    {
                        canvas.FillEllipse(brush1, e.X - toolSize / 2, e.Y - toolSize / 2, toolSize, toolSize);
                        canvas.DrawLine(pen1, prev, cur);
                        canvas.FillEllipse(brush1, e.X - toolSize / 2, e.Y - toolSize / 2, toolSize, toolSize);
                    }
                    if (e.Button == MouseButtons.Right)
                    {
                        canvas.FillEllipse(brush2, e.X - toolSize / 2, e.Y - toolSize / 2, toolSize, toolSize);
                        canvas.DrawLine(pen2, prev, cur);
                        canvas.FillEllipse(brush2, e.X - toolSize / 2, e.Y - toolSize / 2, toolSize, toolSize);
                    }
                    prev = cur;
                    pictureBox1.Refresh();
                }
                if (pipette)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        color1 = bitCanvas.GetPixel(e.X, e.Y);
                        toolStripLabel1.ForeColor = color1;
                        pen1.Color = color1; brush1.Color = color1;
                    }
                    if (e.Button == MouseButtons.Right)
                    {
                        color2 = bitCanvas.GetPixel(e.X, e.Y);
                        toolStripLabel2.ForeColor = color2;
                        pen2.Color = color2; brush2.Color = color2;
                    }
                }
                if (stamp && !pipette)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        stampX = e.X;
                        stampY = e.Y;
                    }
                    if (e.Button == MouseButtons.Left)
                    {
                        int leftTopCornerX = stampX - toolSize / 2;
                        int leftTopCornerY = stampY - toolSize / 2;
                        int rightBottomCornerX = stampX + toolSize / 2;
                        int rightBottomCornerY = stampY + toolSize / 2;
                        if (leftTopCornerX < 0) leftTopCornerX = 0;
                        if (leftTopCornerY < 0) leftTopCornerY = 0;
                        if (rightBottomCornerX < 0) rightBottomCornerX = 0;
                        if (rightBottomCornerY < 0) rightBottomCornerY = 0;
                        if (rightBottomCornerX >= paintBoxSizeX) rightBottomCornerX = paintBoxSizeX - 1;
                        if (rightBottomCornerY >= paintBoxSizeY) rightBottomCornerY = paintBoxSizeY - 1;
                        if (leftTopCornerX >= paintBoxSizeX) leftTopCornerX = paintBoxSizeX - 1;
                        if (leftTopCornerY >= paintBoxSizeY) leftTopCornerY = paintBoxSizeY - 1;
                        int x = rightBottomCornerX - leftTopCornerX;
                        int y = rightBottomCornerY - leftTopCornerY;
                        if (x != 0 && y != 0)
                        {
                            bitStamp = bitCanvas.Clone(
                                new Rectangle(leftTopCornerX, leftTopCornerY, x, y), bitCanvas.PixelFormat);
                            toDraw.X = leftTopCornerX + stampDeltaX;
                            toDraw.Y = leftTopCornerY + stampDeltaY;
                            canvas.DrawImage(bitStamp, toDraw);
                        }
                        //src rectange:
                        //canvas.DrawRectangle(Pens.Black, leftTopCornerX, leftTopCornerY, x, y);
                        //dest rectangle:
                        //canvas.DrawRectangle(Pens.Black, leftTopCornerX + stampDeltaX, leftTopCornerY + stampDeltaY, x, y);
                        pictureBox1.Refresh();
                        stampX = e.X - stampDeltaX; stampY = e.Y - stampDeltaY;
                        if (stampX + toolSize <= 0) stampX = -toolSize;
                        if (stampY + toolSize <= 0) stampY = -toolSize;
                        if (stampX - toolSize >= paintBoxSizeX) stampX = paintBoxSizeX + toolSize;
                        if (stampY - toolSize >= paintBoxSizeY) stampY = paintBoxSizeY + toolSize;
                    }
                }
            }
            else
            {
                pictureBox1.Refresh();
                if (brush && !pipette)
                {
                    showCursor.FillEllipse(brush1, e.X - toolSize / 2, e.Y - toolSize / 2, toolSize, toolSize);
                }
                if (stamp && !pipette)
                {
                    int leftTopCornerX = stampX - toolSize / 2;
                    int leftTopCornerY = stampY - toolSize / 2;
                    int rightBottomCornerX = stampX + toolSize / 2;
                    int rightBottomCornerY = stampY + toolSize / 2;
                    if (leftTopCornerX < 0) leftTopCornerX = 0;
                    if (leftTopCornerY < 0) leftTopCornerY = 0;
                    if (rightBottomCornerX < 0) rightBottomCornerX = 0;
                    if (rightBottomCornerY < 0) rightBottomCornerY = 0;
                    if (rightBottomCornerX >= paintBoxSizeX) rightBottomCornerX = paintBoxSizeX - 1;
                    if (rightBottomCornerY >= paintBoxSizeY) rightBottomCornerY = paintBoxSizeY - 1;
                    if (leftTopCornerX >= paintBoxSizeX) leftTopCornerX = paintBoxSizeX - 1;
                    if (leftTopCornerY >= paintBoxSizeY) leftTopCornerY = paintBoxSizeY - 1;
                    int x = rightBottomCornerX - leftTopCornerX;
                    int y = rightBottomCornerY - leftTopCornerY;
                    Text = stampX.ToString() + " " + stampY.ToString() + " " + toolSize.ToString();
                    if (x != 0 && y != 0)
                    {
                        bitStamp = bitCanvas.Clone(
                            new Rectangle(leftTopCornerX, leftTopCornerY, x, y), bitCanvas.PixelFormat);
                        toDraw.X = e.X - toolSize / 2;
                        toDraw.Y = e.Y - toolSize / 2;
                        showCursor.DrawImage(bitStamp, toDraw);
                    }
                }
            }
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) || (e.Button == MouseButtons.Right))
            {
                down = true;
                prev.X = e.X;
                prev.Y = e.Y;
            }
            if (stamp && !pipette)
            {
                if (e.Button == MouseButtons.Right)
                {
                    stampX = e.X;
                    stampY = e.Y;
                    hasChanged = true;
                    //src rectangle
                    //canvas.DrawRectangle(Pens.Black, stampX - toolSize / 2, stampY - toolSize / 2, toolSize, toolSize);
                }
                if (e.Button == MouseButtons.Left)
                {
                    stampDeltaX = e.X - stampX;
                    stampDeltaY = e.Y - stampY;
                    int leftTopCornerX = stampX - toolSize / 2;
                    int leftTopCornerY = stampY - toolSize / 2;
                    int rightBottomCornerX = stampX + toolSize / 2;
                    int rightBottomCornerY = stampY + toolSize / 2;
                    if (leftTopCornerX < 0) leftTopCornerX = 0;
                    if (leftTopCornerY < 0) leftTopCornerY = 0;
                    if (rightBottomCornerX < 0) rightBottomCornerX = 0;
                    if (rightBottomCornerY < 0) rightBottomCornerY = 0;
                    if (rightBottomCornerX >= paintBoxSizeX) rightBottomCornerX = paintBoxSizeX - 1;
                    if (rightBottomCornerY >= paintBoxSizeY) rightBottomCornerY = paintBoxSizeY - 1;
                    if (leftTopCornerX >= paintBoxSizeX) leftTopCornerX = paintBoxSizeX - 1;
                    if (leftTopCornerY >= paintBoxSizeY) leftTopCornerY = paintBoxSizeY - 1;
                    int x = rightBottomCornerX - leftTopCornerX;
                    int y = rightBottomCornerY - leftTopCornerY;
                    if (x != 0 && y != 0)
                    {
                        bitStamp = bitCanvas.Clone(
                            new Rectangle(leftTopCornerX, leftTopCornerY, x, y), bitCanvas.PixelFormat);
                        toDraw.X = leftTopCornerX + stampDeltaX;
                        toDraw.Y = leftTopCornerY + stampDeltaY;
                        canvas.DrawImage(bitStamp, toDraw);
                    }
                    //src rectange:
                    //canvas.DrawRectangle(Pens.Black, leftTopCornerX, leftTopCornerY, x, y);
                    //dest rectangle:
                    //canvas.DrawRectangle(Pens.Black, leftTopCornerX + stampDeltaX, leftTopCornerY + stampDeltaY, x, y);
                    pictureBox1.Refresh();
                    stampX = e.X - stampDeltaX; stampY = e.Y - stampDeltaY;
                    if (stampX + toolSize <= 0) stampX = -toolSize;
                    if (stampY + toolSize <= 0) stampY = -toolSize;
                    if (stampX - toolSize >= paintBoxSizeX) stampX = paintBoxSizeX + toolSize;
                    if (stampY - toolSize >= paintBoxSizeY) stampY = paintBoxSizeY + toolSize;
                }
            }
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            down = false;
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            bitCanvas = new Bitmap(paintBoxSizeX,paintBoxSizeY);
            canvas = Graphics.FromImage(bitCanvas);
            pictureBox1.Width = bitCanvas.Size.Width;
            pictureBox1.Height = bitCanvas.Size.Height;
            paintBoxSizeX = pictureBox1.Width;
            paintBoxSizeY = pictureBox1.Height;
            pictureBox1.Image = bitCanvas;
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
            try
            {
                file = openFileDialog.FileName;
                image = Image.FromFile(file, true);
                bitCanvas = new Bitmap(image);
                canvas = Graphics.FromImage(bitCanvas);
                pictureBox1.Width = bitCanvas.Size.Width;
                pictureBox1.Height = bitCanvas.Size.Height;
                paintBoxSizeX = pictureBox1.Width;
                paintBoxSizeY = pictureBox1.Height;
                pictureBox1.Image = bitCanvas;
                image.Dispose();
            }
            catch (OutOfMemoryException ex)
            {
                MessageBox.Show("Поддерживаются только следующие файловые форматы:\nBMP, GIF, JPEG, PNG, TIFF",
                    "Bad file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ex.ToString();
            }
            catch (ArgumentException ex)
            {
                if (file != "")
                    MessageBox.Show("Файл имеет слишком большое разрешение или не существует",
                        "Bad file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ex.ToString();
            }
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                file = saveFileDialog.FileName;
                imageFormat = ImageFormat.Png;
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string ext = System.IO.Path.GetExtension(saveFileDialog.FileName);
                    switch (ext)
                    {
                        case ".jpg":
                            imageFormat = ImageFormat.Jpeg;
                            break;
                        case ".bmp":
                            imageFormat = ImageFormat.Bmp;
                            break;
                        case ".png":
                            imageFormat = ImageFormat.Png;
                            break;
                        case ".gif":
                            imageFormat = ImageFormat.Gif;
                            break;
                        case ".tiff":
                            imageFormat = ImageFormat.Tiff;
                            break;
                    }
                    pictureBox1.Image.Save(saveFileDialog.FileName, imageFormat);
                }   
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message, "Bad save", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            canvas.Clear(Color.White);
            pictureBox1.Refresh();
        }
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            color1 = colorDialog1.Color;
            toolStripLabel1.ForeColor = color1;
            pen1.Color = color1; brush1.Color = color1;
        }
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            colorDialog2.ShowDialog();
            color2 = colorDialog2.Color;
            toolStripLabel2.ForeColor = color2;
            pen2.Color = color2; brush2.Color = color2;
        }
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            pipette = !pipette;
            toolStripButton8.Checked = !toolStripButton8.Checked;
        }
        private void toolStripComboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = (char)Keys.None;
        }
        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            int newValue;
            if (Int32.TryParse(toolStripTextBox1.Text, out newValue))
            {
                if (newValue >= 1)
                {
                    toolStripTextBox1.Text = newValue.ToString();
                    toolSize = newValue;
                    pen1.Width = toolSize;
                    pen2.Width = toolSize;
                    if (!hasChanged)
                    {
                        stampX = toolSize / 2;
                        stampY = toolSize / 2;
                    }
                }
                else
                {
                    toolStripTextBox1.Text = toolSize.ToString();
                }
            }
        }
        private void toolStripTextBox2_TextChanged(object sender, EventArgs e)
        {
            int newValue;
            if (Int32.TryParse(toolStripTextBox2.Text, out newValue))
            {
                if (newValue >= 1 && newValue < 65536)
                {
                    toolStripTextBox2.Text = newValue.ToString();
                    paintBoxSizeX = newValue;
                }
                else
                {
                    toolStripTextBox2.Text = paintBoxSizeX.ToString();
                }
            }
        }
        private void toolStripTextBox3_TextChanged(object sender, EventArgs e)
        {
            int newValue;
            if (Int32.TryParse(toolStripTextBox3.Text, out newValue))
            {
                if (newValue >= 1 && newValue < 65536)
                {
                    toolStripTextBox3.Text = newValue.ToString();
                    paintBoxSizeY = newValue;
                }
                else
                {
                    toolStripTextBox3.Text = paintBoxSizeY.ToString();
                }
            }
        }

        private void toolStripComboBox1_TextChanged(object sender, EventArgs e)
        {
            if (toolStripComboBox1.SelectedItem.ToString().Equals("Кисть"))
            {
                brush = true;
                stamp = false;
            }
            else
            {
                brush = false;
                stamp = true;
            }
        }
    }
}
