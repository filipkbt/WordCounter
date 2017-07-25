using WordCounter.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
namespace WordCounter.ViewModel
{
    class UserViewModel : INotifyPropertyChanged
    {
        
        public List<User> usersList { get; set; }
        private User selectedUser;

        public UserViewModel()
        {
            usersList = downloadDataAboutUsers();

        }
       
        public User SelectedUser
        {
            get
            {
                return this.selectedUser;
            }

            set
            {
                if (value != this.selectedUser)
                {
                    this.selectedUser = value;
                    NotifyPropertyChanged("SelectedUser");                    
                    PostViewModel.ActualizeData(selectedUser.id, usersList);
                    CommentViewModel.commentsConnectedWithSelectedPostList.Clear();
                }
            }

        }

        
        public List<User> downloadDataAboutUsers()
        {
            List<User> usersList = new List<User>();

            var client = new RestClient
            {
                BaseUrl = new Uri("https://jsonplaceholder.typicode.com")
            };
            var request = new RestRequest("users", Method.GET);
            var Response = client.Execute<List<User>>(request);
            var ResponseData = Response.Data;

            foreach (User user in ResponseData)
                usersList.Add(user);
            return usersList;

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
