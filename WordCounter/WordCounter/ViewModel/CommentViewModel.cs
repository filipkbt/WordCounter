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

namespace WordCounter.ViewModel
{
    class CommentViewModel : INotifyPropertyChanged
    {
        #region Variable SelectedComment
        private Comment selectedComment;
        public Comment SelectedComment
        {
            get
            {
                return this.selectedComment;
            }

            set
            {
                if (value != this.selectedComment)
                {
                    this.selectedComment = value;
                    NotifyPropertyChanged("SelectedComment");
                    SelectedCommentBody = SelectedComment.body;
                }
            }

        }
        #endregion
        #region Variable SelectedCommentBody
        private string selectedCommentBody;

        public string SelectedCommentBody
        {
            get
            {
                return this.selectedCommentBody;
            }

            set
            {

                this.selectedCommentBody = value;
                NotifyPropertyChanged("SelectedCommentBody");

            }
        }
        #endregion
        private static ObservableCollection<Comment> allComments;
        public static ObservableCollection<Comment> commentsConnectedWithSelectedPostList { get; set; }

        public CommentViewModel()
        {
            allComments = DownloadAllComments();
            commentsConnectedWithSelectedPostList = new ObservableCollection<Comment>();
        }

        private ObservableCollection<Comment> DownloadAllComments()
        {
            ObservableCollection<Comment> allComments = new ObservableCollection<Comment>();
            #region Downloading all comments from JsonPlaceHolder
            var client = new RestClient
            {
                BaseUrl = new Uri("https://jsonplaceholder.typicode.com")
            };
            var request = new RestRequest("comments", Method.GET);
            var Response = client.Execute<List<Comment>>(request);
            var ResponseData = Response.Data;
            #endregion
            #region Adding comments to ObservableCollection
            foreach (Comment comment in ResponseData)
                allComments.Add(comment);
            #endregion
            return allComments;
        }

        public static ObservableCollection<Comment> ActualizeData(int selectedPostId, ObservableCollection<Post> allPosts)
        {
            commentsConnectedWithSelectedPostList.Clear();
            #region commentsConnectedWithSelectedPostQuery implementation
            var commentsConnectedWithSelectedPostQuery = from post in allPosts
                                                         join comment in allComments on post.id equals comment.postId
                                                         where comment.postId == selectedPostId
                                                         select new
                                                         {

                                                             postId = comment.postId,
                                                             id = comment.id,
                                                             name = comment.name,
                                                             email = comment.email,
                                                             body = comment.body
                                                         };

            #endregion

            #region Adding comments from query to list
            foreach (var comment in commentsConnectedWithSelectedPostQuery)
            {
                Comment addedComment = new Comment();
                addedComment.postId = comment.postId;
                addedComment.id = comment.id;
                addedComment.name = comment.name;
                addedComment.email = comment.email;
                addedComment.body = comment.body;
                commentsConnectedWithSelectedPostList.Add(addedComment);
            }
            #endregion
            return commentsConnectedWithSelectedPostList;
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion

    }
}
