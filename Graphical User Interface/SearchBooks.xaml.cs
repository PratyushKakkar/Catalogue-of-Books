using CatalogueOfBooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Graphical_User_Interface
{
     /// <summary>
     /// Interaction logic for SearchBooks.xaml
     /// </summary>
     public partial class SearchBooks : Window
     {
          public SearchBooks()
          {
               InitializeComponent();
          }

          private void Window_Loaded(object sender, RoutedEventArgs e)
          {
              BookInventory inventory = SharedInventory.inventory;
        }
    }
}
