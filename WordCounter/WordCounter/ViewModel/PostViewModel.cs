using WordCounter.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WordCounter.ViewModel
{
    class PostViewModel : INotifyPropertyChanged
    {
        #region variable SelectedPost
        private Post _selectedPost;
        public Post SelectedPost
        {
            get
            {
                return this._selectedPost;
            }

            set
            {
                if (value != this._selectedPost)
                {
                    this._selectedPost = value;
                    NotifyPropertyChanged("SelectedPost");
                    SelectedPostBody = SelectedPost.body;
                    CommentViewModel.ActualizeData(_selectedPost.id, allPosts);
                    StatisticsViewModel.MergePostAndComments(SelectedPost, CommentViewModel.commentsConnectedWithSelectedPostList);

                }
            }

        }
        #endregion
        #region Variable SelectedPostBody
        private string selectedPostBody;

        public string SelectedPostBody
        {
            get
            {
                return this.selectedPostBody;
            }

            set
            {
                this.selectedPostBody = value;
                NotifyPropertyChanged("SelectedPostBody");
            }
        }
        #endregion

        public static ObservableCollection<Post> postsConnectedWithSelectedUserList { get; set; }

        private static ObservableCollection<Post> allPosts;

        public PostViewModel()
        {
            allPosts = downloadAllPosts();
            postsConnectedWithSelectedUserList = new ObservableCollection<Post>();

        }


        public ObservableCollection<Post> downloadAllPosts()
        {
            #region Downloading all posts from JsonPlaceHolder
            ObservableCollection<Post> allPosts = new ObservableCollection<Post>();

            var client = new RestClient
            {
                BaseUrl = new Uri("https://jsonplaceholder.typicode.com")
            };
            var request = new RestRequest("posts", Method.GET);
            var Response = client.Execute<List<Post>>(request);
            var ResponseData = Response.Data;
            #endregion
            #region Adding posts to ObservableCollection
            foreach (Post post in ResponseData)
                allPosts.Add(post);
            #endregion
            return allPosts;

        }



        public static ObservableCollection<Post> ActualizeData(int selectedUserId, List<User> usersList)
        {

            postsConnectedWithSelectedUserList.Clear();

            #region postsConnectedWithSelectedUserQuery implementation
            var postsConnectedWithSelectedUserQuery = from post in allPosts
                                                      join user in usersList on post.userId equals user.id
                                                      where user.id == selectedUserId
                                                      select new
                                                      {

                                                          userId = post.userId,
                                                          id = post.id,
                                                          title = post.title,
                                                          body = post.body
                                                      };

            #endregion

            #region Adding posts to ObservableCollection
            foreach (var post in postsConnectedWithSelectedUserQuery)
            {
                Post addedPost = new Post();
                addedPost.userId = post.userId;
                addedPost.id = post.id;
                addedPost.title = post.title;
                addedPost.body = post.body;
                postsConnectedWithSelectedUserList.Add(addedPost);
            }
            #endregion
            return postsConnectedWithSelectedUserList;
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }

            #endregion
        }
    }
}