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
    public MainWindow()
    {
      InitializeComponent();
    }

    private void Preview_Click(object sender, RoutedEventArgs e)
    {
      ThreeDPreviewWindow preview = new ThreeDPreviewWindow();
      preview.ShowDialog();
    }

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

    private  double GetScale(int width, int lenght, int height, int tableSize = 550)
    {
      double scale;
      int tableLength = 2 * height + lenght;
      int tableWidth = 2 * height + 2 * width;
      int max = Math.Max(tableLength, tableWidth);

      if (max > 550)
      {
        scale = ((double)max / 550);
      }
      else
      {
        scale = ((double)550 / max);
      }
      return scale;
    }

    private void Reset_Click(object sender, RoutedEventArgs e)
    {
      int width;
      int length;
      int height;

      if (!GetParsedDimension(Width.Text, Width.Name, out width))
        return;
      if (!GetParsedDimension(Length.Text, Length.Name, out length))
        return;
      if (!GetParsedDimension(Heigth.Text, Heigth.Name, out height))
        return;

      //czyscimy stolik
      Table.Children.Clear();

      //liczymy skale do wyswietlania
      double scale = GetScale(width, length, height);

      //dodajemy gore
      Canvas top = new Canvas();
      top.Height = width*scale;
      top.Width = length*scale;
      top.Background = new SolidColorBrush(Colors.Red);
      Canvas.SetLeft(top, 12 + height*scale);
      Canvas.SetTop(top, 10);
      top.MouseLeftButtonDown += boxWall_MouseLeftButtonDown;
      Table.Children.Add(top);

      //dodajemy tyl
      Canvas back = new Canvas();
      back.Height = height*scale;
      back.Width = length*scale;
      back.Background = new SolidColorBrush(Colors.Brown);
      Canvas.SetLeft(back, 12 + height*scale);
      Canvas.SetTop(back, 20 + top.Height);
      Table.Children.Add(back);

      //dodajemy dol      
      AddBoxWall(length * scale, width * scale, 30 + (width + height) * scale, 12 + height * scale, Colors.Yellow);
    }

    private void AddBoxWall(double width, double height, double  top, double left, Color color)
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
