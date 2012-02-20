using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BoxCreator
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    /// <summary>
    /// Gap between wall and 
    /// </summary>
    public const int EdgeGap = 8;
    /// <summary>
    /// Gap between box walls 
    /// </summary>
    public const int WallGap = 10;
    /// <summary>
    /// Gap between box and cover (if box is created with cover)
    /// </summary>
    public const int CoverGap = 15;

    public MainWindow()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Handles the Click event of the Preview control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
    private void Preview_Click(object sender, RoutedEventArgs e)
    {
      ThreeDPreviewWindow preview = new ThreeDPreviewWindow();
      preview.ShowDialog();
    }

    /// <summary>
    /// Gets the parsed dimension.
    /// </summary>
    /// <param name="text">The text in control.</param>
    /// <param name="dimension">The dimension description.</param>
    /// <param name="result">The parsed result.</param>
    /// <returns>True if parsing was posible; otherwise false</returns>
    private bool GetParsedDimension(string text, string dimension, out int result)
    {
      if (!int.TryParse(text, out result))
      {
        MessageBox.Show(dimension + " nie jest liczbą.");
        return false;
      }
      if (result <= 0)
      {
        MessageBox.Show(dimension+" musi byc wieksza od zera");
        return false;
      }
      return true;
    }

    /// <summary>
    /// Gets the scale which should be used to display the walls.
    /// </summary>
    /// <param name="width">The width of box.</param>
    /// <param name="length">The lenght of box.</param>
    /// <param name="height">The height of box.</param>
    /// <param name="coverHeight">Height of the cover (if cover used > 0; ohterwise 0).</param>
    /// <param name="tableSize">Size of the table where box will be shown.</param>
    /// <returns>
    /// Scale calculated.
    /// </returns>
    /// <remarks>
    /// To calculte is used <see cref="EdgeGap"/>, <see cref="WallGap"/> and <see cref="CoverGap"/>.
    /// </remarks>
    private  double GetScale(int width, int length, int height, int coverHeight, int tableSize = 550)
    {
      //scale which will be return
      double scale = 1;
      //gaps which apear on length
      int gapsLength = 2 * EdgeGap + 2 * WallGap;
      //gaps which apear on width
      int gapsWidth = 3 * WallGap + 2 * EdgeGap;
      //length of expanded box (only box) 
      int boxExpandedLength = 2 * height + length;
      //width of expanded box (only box) 
      int boxExpandedWidth = 2 * height + 2 * width;
      
      if (coverHeight > 0)
      {//with coverHeight
        boxExpandedWidth = 2 * height + 2 * width + 2 * coverHeight;
        gapsWidth = 4*WallGap + 2*EdgeGap + CoverGap;
      }

      //scale should be used only for box not for gap!!!
      if (gapsLength + boxExpandedLength > gapsWidth + boxExpandedWidth)
      {
        if (gapsLength + boxExpandedLength > tableSize)
        {
          scale = (double)(boxExpandedLength)/(tableSize - gapsLength);
        }
        else
        {
          scale = (double)(tableSize - gapsLength) / (boxExpandedLength);
        }

      }
      else
      {
        if (gapsWidth + boxExpandedWidth > tableSize - gapsWidth)
        {
          scale = (double)(boxExpandedWidth) / (tableSize - gapsWidth);
        }
        else
        {
          scale = (double)(tableSize - gapsWidth) / (boxExpandedWidth);
        }

      }
 
      return scale;
    }

    /// <summary>
    /// Handles the Click event of the Reset control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
    /// <remarks>This funciton will rebuild the box.</remarks>
    private void Reset_Click(object sender, RoutedEventArgs e)
    {
      int width = 0;
      int length = 0;
      int height = 0;
      int coverHeight = 0;
      int tableSize = 550;

      if (!GetParsedDimension(Width.Text, Width.Name, out width))
        return;
      if (!GetParsedDimension(Length.Text, Length.Name, out length))
        return;
      if (!GetParsedDimension(Heigth.Text, Heigth.Name, out height))
        return;
      if (cbWithCover.IsChecked.HasValue && 
        cbWithCover.IsChecked.Value &&
        !GetParsedDimension(CoverHeight.Text, CoverHeight.Name, out coverHeight))
        return;
      if (coverHeight > height)
      {
        MessageBox.Show("Wysokosc pokrywki nie moze byc wieksza niz wysokosc pudelka.");
        return;
      }

      //czyscimy stolik
      Table.Children.Clear();

      //liczymy skale do wyswietlania
      double scale = GetScale(width, length, height, coverHeight, tableSize);

      
      int lengthToDisplay = (int)(2 * EdgeGap + 2 * WallGap + (2 * height + length) * scale);

      int widthToDisplay = (int)(2 * EdgeGap + 3 * WallGap + (2 * height + 2 * width) * scale);

      if (coverHeight > 0)
      {//with coverHeight
        widthToDisplay = (int)(4 * WallGap + 2 * EdgeGap + CoverGap + (2 * height + 2 * width) * scale);
      }

      int leftShift =  (tableSize - lengthToDisplay) / 2;

      int topShift = (tableSize - widthToDisplay) / 2;

      //dodajemy gore      
      AddBoxWall(length * scale, width * scale, topShift + EdgeGap, leftShift + EdgeGap + WallGap + height * scale, Colors.Yellow);

      //dodajemy przednia sciane
      AddBoxWall(length * scale, height * scale, topShift + EdgeGap + WallGap + width * scale, leftShift + EdgeGap + WallGap + height * scale, Colors.Yellow);

      //dodajemy dol      
      AddBoxWall(length * scale, width * scale, topShift + EdgeGap + 2 * WallGap + (width + height) * scale, leftShift + EdgeGap + WallGap + height * scale, Colors.Yellow);

      //dodajemy przednia sciane
      AddBoxWall(length * scale, height * scale, topShift + EdgeGap + 3 * WallGap + (width * 2 + height) * scale, leftShift + EdgeGap + WallGap + height * scale, Colors.Yellow);

      //dodajemy lewa sciane
      AddBoxWall(height * scale, width * scale, topShift + EdgeGap + 2 * WallGap + (width + height) * scale, leftShift + EdgeGap, Colors.Yellow);

      //dodajemy prawa sciane
      AddBoxWall(height * scale, width * scale, topShift + EdgeGap + 2 * WallGap + (width + height) * scale, leftShift + EdgeGap +  (height + length) * scale + 2* WallGap, Colors.Yellow);
    }

    private void AddBoxWall(double width, double height, double top, double left, Color color)
    {
      //dodajemy dol
      Canvas canvas = new Canvas();
      canvas.Height = height;
      canvas.Width = width;
      canvas.Background = new SolidColorBrush(color);
      Canvas.SetLeft(canvas, left);
      Canvas.SetTop(canvas, top);
      canvas.MouseLeftButtonDown += boxWall_MouseLeftButtonDown;
      Table.Children.Add(canvas);   
    }
    

    void boxWall_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      SketchWindow sketch = new SketchWindow();
      sketch.BoxWall = (Canvas)sender;
      sketch.ShowDialog();
    }


  }
}
