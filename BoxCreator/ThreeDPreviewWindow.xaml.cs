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
using System.Windows.Shapes;

namespace BoxCreator
{
  /// <summary>
  /// Interaction logic for _3DPreviewWindow.xaml
  /// </summary>
  public partial class ThreeDPreviewWindow : Window
  {
    public ThreeDPreviewWindow()
    {
      InitializeComponent();
    }

    // http://www.codeguru.pl/baza-wiedzy/tworzenie-i-manipulacja-grafika-3d-w-wpf-cz-i---modelowanie-przestrzeni-3d,2374
    private Box _boxToDisplay3D;
    public Box BoxToDisplay3D
    {
      get { return _boxToDisplay3D; }
      set
      {
        _boxToDisplay3D = value;
        if (_boxToDisplay3D.IsRebuild)
        {
          //_boxToDisplay3D.UpWall.CopyToWall(wallUp, true);
          //_boxToDisplay3D.BottomWall.CopyToWall(wallBottom, true);
          //_boxToDisplay3D.FrontWall.CopyToWall(wallFront, true);
          //_boxToDisplay3D.BackWall.CopyToWall(wallBack, true);
          //_boxToDisplay3D.LeftWall.CopyToWall(wallLeft, true);
          //_boxToDisplay3D.RightWall.CopyToWall(wallRight, true);
        }
      }
    }
  }
}
