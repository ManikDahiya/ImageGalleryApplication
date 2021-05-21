using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using C1.Win.C1Tile;
using C1.C1Pdf;
using System.Linq;
using System.IO;

/*
Assignment - Image Gallery Application
Date       - 17-05-2021
   Shows images, exports images to PDF and saves images to local disk
*/

namespace ImageGalleryApplication
{
    public partial class ImageGallery : Form
    {
        DataFetcher datafetch = new DataFetcher();
        List<ImageItem> imagesList;
        int checkedItems = 0;

        Panel panel1 = new Panel();
        Panel panel2 = new Panel();
        
        TextBox textBox = new TextBox();

        PictureBox pictureBoxSearch = new PictureBox();
        PictureBox pictureBoxExport = new PictureBox();
        PictureBox pictureBoxSave = new PictureBox();
        
        Label labelExport = new Label();
        Label labelSave = new Label();
        
        C1TileControl tile = new C1TileControl();
        
        StatusStrip statusStrip = new StatusStrip();
        
        Tile tile1 = new Tile();
        Tile tile2 = new Tile();
        Tile tile3 = new Tile();
        
        
        ImageElement imageElement = new ImageElement();
        TextElement textElement = new TextElement();
        
        Group _group = new Group();
        
        ToolStripProgressBar tSPB = new ToolStripProgressBar();
        
        SplitContainer splitContainer = new SplitContainer();
        
        TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
        
        C1PdfDocument imagePdfDocument = new C1PdfDocument();

        AutoCompleteStringCollection data = new AutoCompleteStringCollection();


        public ImageGallery()
        {
            InitializeComponent();

            //Customizing the form
            
            MaximumSize = new Size(810, 810);
            MaximizeBox = false;
            ShowIcon = false;
            Size = new Size(780, 700);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Image Gallery";
            BackColor = Color.White;

            //Splitting the table
           
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Margin = new Padding(2);
            splitContainer.SplitterDistance = 40;
            splitContainer.FixedPanel = FixedPanel.Panel1;
            splitContainer.IsSplitterFixed = true;
            splitContainer.Orientation = Orientation.Horizontal;
            splitContainer.Panel1.Controls.Add(tableLayoutPanel);
            splitContainer.Panel2.Controls.Add(pictureBoxExport);
            splitContainer.Panel2.Controls.Add(labelExport);
            splitContainer.Panel2.Controls.Add(labelSave);
            splitContainer.Panel2.Controls.Add(pictureBoxSave);
            splitContainer.Panel2.Controls.Add(tile);
            splitContainer.Panel2.Controls.Add(statusStrip);
            Controls.Add(splitContainer);


            //Creating Search Box

            textBox.Name = "_searchBox";
            textBox.BorderStyle = BorderStyle.None;
            textBox.Dock = DockStyle.Fill;
            textBox.Location = new Point(16, 9);
            textBox.Size = new Size(244, 16);
            textBox.Text = "Search Image";
            textBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right);
            textBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            
            //Creating Search Button

            pictureBoxSearch.Name = "_search";
            pictureBoxSearch.Dock = DockStyle.Left;
            pictureBoxSearch.Location = new Point(0, 0);
            pictureBoxSearch.Margin = new Padding(0, 0, 0, 0);
            pictureBoxSearch.Size = new Size(40, 16);
            pictureBoxSearch.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxSearch.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right);
            pictureBoxSearch.Image = Properties.Resources.Search;
            pictureBoxSearch.Click += new EventHandler(OnSearchClick);

            //Creating Export Button

            pictureBoxExport.Name = "_exportImage";
            pictureBoxExport.Location = new Point(29, 1);
            pictureBoxExport.Size = new Size(75, 25);
            pictureBoxExport.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxExport.Visible = false;
            pictureBoxExport.Image = Properties.Resources.ExportToPDF;
            pictureBoxExport.Click += new EventHandler(OnExportClick);
            pictureBoxExport.Paint += new PaintEventHandler(OnExportImagePaint);

            //Creating Export to PDF label

            labelExport.Name = "_exportLabel";
            labelExport.Text = "Export to PDF";
            labelExport.Location = new Point(29, 28);
            labelExport.Size = new Size(75, 15);
            labelExport.Visible = false;

            //Creating Save to PC button

            pictureBoxSave.Name = "_saveImage";
            pictureBoxSave.Location = new Point(120, 1);
            pictureBoxSave.Size = new Size(85, 28);
            pictureBoxSave.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxSave.Image = Properties.Resources.SaveToPC;
            pictureBoxSave.Visible = false;
            pictureBoxSave.Click += new EventHandler(OnSaveClick);
            pictureBoxSave.Paint += new PaintEventHandler(OnSaveImagePaint);

            //Creating Save to PC Label

            labelSave.Name = "_saveLabel";
            labelSave.Text = "Save to PC";
            labelSave.Location = new Point(129, 28);
            labelSave.Size = new Size(75, 15);
            labelSave.Visible = false;

            //Creating Table Layout
            
            tableLayoutPanel.ColumnCount = 3;
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.Location = new Point(0, 0);
            tableLayoutPanel.RowCount = 1;
            tableLayoutPanel.Size = new Size(800, 40);
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37.50F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37.50F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel.Controls.Add(panel1, 1, 0);
            tableLayoutPanel.Controls.Add(panel2, 2, 0);
            
            //Creating Panels
            
            panel1.Location = new Point(477, 0);
            panel1.Size = new Size(287, 40);
            panel1.Dock = DockStyle.Fill;
            panel1.Paint += new PaintEventHandler(OnSearchPanelPaint);
            panel1.Controls.Add(textBox);
            panel2.Size = new Size(40, 16);
            panel2.Location = new Point(479, 12);
            panel2.Margin = new Padding(2, 12, 45, 12);
            panel2.TabIndex = 1;
            panel2.Controls.Add(pictureBoxSearch);

            //Customizing Tiles

            tile1.BackColor = Color.LightCoral;
            tile1.Name = "tile1";
            tile1.Text = "Tile 1";

            tile2.BackColor = Color.Teal;
            tile2.Name = "tile2";
            tile2.Text = "Tile 2";

            tile3.BackColor = Color.SteelBlue;
            tile3.Name = "tile3";
            tile3.Text = "Tile 3";

            //Grouping Tiles

            _group.Name = "_group";
            _group.Text = "Results";
            _group.Tiles.Add(tile1);
            _group.Tiles.Add(tile2);
            _group.Tiles.Add(tile3);
            _group.Visible = false;

            //Customizing Tiles
            
            tile.Name = "_imageTileControl";
            tile.AllowChecking = true;
            tile.AllowRearranging = true;
            tile.CellHeight = 78;
            tile.CellSpacing = 11;
            tile.CellWidth = 78;
            tile.Dock = DockStyle.Fill;
            tile.Size = new Size(764, 718);
            tile.SurfacePadding = new Padding(12, 4, 12, 4);
            tile.SwipeDistance = 20;
            tile.SwipeRearrangeDistance = 98;
            tile.Groups.Add(_group);
            tile.Location = new Point(0, 0);
            tile.Orientation = LayoutOrientation.Vertical;
            tile.TileChecked += new System.EventHandler<C1.Win.C1Tile.TileEventArgs>(OnTileChecked);
            tile.TileUnchecked += new System.EventHandler<C1.Win.C1Tile.TileEventArgs>(OnTileUnchecked);
            tile.Paint += new PaintEventHandler(OnTileControlPaint);

            //Customizing Status Bar

            statusStrip.Dock = DockStyle.Bottom;
            statusStrip.Visible = false;
            tSPB.Style = ProgressBarStyle.Marquee;
            statusStrip.Items.Add(tSPB);
        }
        
        //Implementation of Search Method
        
        private async void OnSearchClick(object sender, EventArgs e)
        {
            //autocompleting words
            
            data.Add(textBox.Text);
            textBox.AutoCompleteCustomSource = data;
            
            statusStrip.Visible = true;
            imagesList = await datafetch.GetImageData(textBox.Text);

            //Checking meaningful words

            if (!imagesList.Any())
            {
                MessageBox.Show("Image not Found");
            }
            
            AddTiles(imagesList);
            statusStrip.Visible = false;
          
        }

        //Adding tiles to the panel
        
        private void AddTiles(List<ImageItem> imageList)
        {
            tile.Groups[0].Tiles.Clear();
            _group.Visible = true;
            foreach (var imageitem in imageList)
            {
                Tile tile_ = new Tile();
                tile_.HorizontalSize = 2;
                tile_.VerticalSize = 2;
                tile.Groups[0].Tiles.Add(tile_);
                Image img = Image.FromStream(new MemoryStream(imageitem.Base64));
                Template tl = new Template();
                ImageElement imageElement = new ImageElement();
                imageElement.ImageLayout = ForeImageLayout.Stretch;
                tl.Elements.Add(imageElement);
                tile_.Template = tl;
                tile_.Image = img;
            }
        }

        //Adding other customizations to the panels
        
        private void OnSearchPanelPaint(object sender, PaintEventArgs e)
        {
            Rectangle r = textBox.Bounds;
            r.Inflate(3, 3);
            Pen p = new Pen(Color.LightGray);
            e.Graphics.DrawRectangle(p, r);
        }

        //Implementation of Export to PDF Method
        
        private void OnExportClick(object sender, EventArgs e)
        {
            List<Image> images = new List<Image>();
            foreach (Tile tile_ in tile.Groups[0].Tiles)
            {
                if (tile_.Checked)
                {
                    images.Add(tile_.Image);
                }
            }
            ConvertToPdf(images);
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.DefaultExt = "pdf";
            saveFile.Filter = "PDF files (*.pdf)|*.pdf*";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                imagePdfDocument.Save(saveFile.FileName);
            }
        }

        //Converting list of images to PDF
        
        private void ConvertToPdf(List<Image> images)
        {
            RectangleF rect = imagePdfDocument.PageRectangle;
            bool firstPage = true;
            foreach (var selectedimg in images)
            {
                if (!firstPage)
                {
                    imagePdfDocument.NewPage();
                }
                firstPage = false;
                rect.Inflate(-72, -72);
                imagePdfDocument.DrawImage(selectedimg, rect);
            }
        }

        //Adding customizations to Export panel
        
        private void OnExportImagePaint(object sender, PaintEventArgs e)
        {
            Rectangle r = new Rectangle(pictureBoxExport.Location.X, pictureBoxExport.Location.Y, pictureBoxExport.Width, pictureBoxExport.Height);
            r.X -= 29;
            r.Y -= 1;
            r.Width--;
            r.Height--;
            Pen p = new Pen(Color.White);
            e.Graphics.DrawRectangle(p, r);
            e.Graphics.DrawLine(p, new Point(0, 43), new Point(Width, 43));
        }

        //Implementation of Saving to Local Disk Method
        
        private void OnSaveClick(object sender, EventArgs e)
        {
            List<Image> images1 = new List<Image>();
            foreach (Tile tile_ in tile.Groups[0].Tiles)
            {
                if (tile_.Checked)
                {
                    images1.Add(tile_.Image);
                }
            }

            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.DefaultExt = "jpg";
            saveFile.Filter = "JPG files (*.jpg)|*.jpg*";
            foreach (var img in images1)
            {
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    img.Save(saveFile.FileName);
                }
            }

        }

        //Adding customizations to Save panel
        
        private void OnSaveImagePaint(object sender, PaintEventArgs e)
        {
            Rectangle r = new Rectangle(pictureBoxSave.Location.X, pictureBoxSave.Location.Y, pictureBoxSave.Width, pictureBoxSave.Height);
            r.X -= 29;
            r.Y -= 1;
            r.Width--;
            r.Height--;
            Pen p = new Pen(Color.LightGray);
            e.Graphics.DrawRectangle(p, r);
            e.Graphics.DrawLine(p, new Point(0, 43), new Point(Width, 43));
        }

        //Implementation of tile checking method
        
        private void OnTileChecked(object sender, TileEventArgs e)
        {
            checkedItems++;
            pictureBoxExport.Visible = true;
            pictureBoxSave.Visible = true;
            labelExport.Visible = true;
            labelSave.Visible = true;
        }

        //Implementation of tile unchecking method
        
        private void OnTileUnchecked(object sender, TileEventArgs e)
        {
            checkedItems--;
            pictureBoxExport.Visible = checkedItems > 0;
            pictureBoxSave.Visible = checkedItems > 0;
            labelExport.Visible = checkedItems > 0;
            labelSave.Visible = checkedItems > 0;
        }

        //Adding customizations to tiles
        
        private void OnTileControlPaint(object sender, PaintEventArgs e)
        {
            Pen p = new Pen(Color.LightGray);
            e.Graphics.DrawLine(p, 0, 43, 800, 43);
        }
    }
}
