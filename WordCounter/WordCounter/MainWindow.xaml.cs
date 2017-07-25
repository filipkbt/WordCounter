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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WordCounter.ViewModel;
using WordCounter.Model;
using System.ComponentModel;

namespace WordCounter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PostViewModel postViewModel = new PostViewModel();
        CommentViewModel commentViewModel = new CommentViewModel();
        StatisticsViewModel statisticsViewModel = new StatisticsViewModel();
        public MainWindow()
        {
            InitializeComponent();
            usersListView.DataContext = new UserViewModel();
            postsListView.DataContext = postViewModel;
            postTextBlock.DataContext = postViewModel;
            commentsListView.DataContext = commentViewModel;
            commentTextBlock.DataContext = commentViewModel;      
            statisticsListView.DataContext = statisticsViewModel;

        }

    }
}
