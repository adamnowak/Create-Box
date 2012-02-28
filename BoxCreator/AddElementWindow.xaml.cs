using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace BoxCreator
{
  /// <summary>
  /// Interaction logic for AddElementWindow.xaml
  /// </summary>
  public partial class AddElementWindow : Window
  {
    public AddElementWindow()
    {
      InitializeComponent();
    }

    public FrameworkElement Element { get; private set; }

    private void BrowseClick(object sender, RoutedEventArgs e)
    {
       OpenFileDialog ofd = new OpenFileDialog();
       ofd.Multiselect = false;
       bool? b = ofd.ShowDialog();
       if (b != null && b.Value == true)
       {
         txtBoxImagePath.Text = ofd.FileName;
       }
    }

    private void OkClick(object sender, RoutedEventArgs e)
    {
      if (tabContItemSelector.SelectedItem == tabItemText)
      {
        TextBlock tb = new TextBlock();
        tb.Text = txtBoxTextToInsert.Text;
        Element = tb;
      }
      if (tabContItemSelector.SelectedItem == tabItemImage)
      {
        Image img = new Image();
        ImageSourceConverter converter = new ImageSourceConverter();
        img.Source = (ImageSource)converter.ConvertFromString(txtBoxImagePath.Text);
        img.Width = 60;
        img.Height = 30;
        img.Tag = txtBoxImagePath.Text;
        Element = img;
      }
      Close();
    }

    private void CancelClick(object sender, RoutedEventArgs e)
    {
      Element = null;
      Close();
    }
  }
}
