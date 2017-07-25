using WordCounter.Model;
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
    class StatisticsViewModel : INotifyPropertyChanged
    {
        private static readonly char[] whiteSpaces = new char[] { '\r', '\n', ' ', ',', '.', '"' };
        #region Variable PostAndComments
        private string postAndComments = "";
        public string PostAndComments
        {
            get
            {
                return this.postAndComments;
            }

            set
            {
                this.postAndComments = value;
                NotifyPropertyChanged("PostAndComments");
            }
        }
        #endregion

        public Dictionary<string, int> WordsAndNumberOfOccurencies { get; set; }
        public static ObservableCollection<Statistics> TopOccurencyWords { get; set; }

        public StatisticsViewModel()
        {
            TopOccurencyWords = new ObservableCollection<Statistics>();
        }

       

        public static void MergePostAndComments(Post post, ObservableCollection<Comment> commentsConnectedWithSelectedPost)
        {
            string postAndComments = "";
            #region Merging post with comments body
            postAndComments += post.body + " ";
            foreach (Comment comment in commentsConnectedWithSelectedPost)
                postAndComments += comment.body + " ";
            #endregion
            CountTop10Words(postAndComments);
        }


        public static ObservableCollection<Statistics> CountTop10Words(string postAndComments)
        {
            Dictionary<string, int> WordsAndNumberOfOccurencies = new Dictionary<string, int>();
            ObservableCollection<Statistics> TopOccurencyWords = new ObservableCollection<Statistics>();

            string[] words = postAndComments.Split(whiteSpaces);

            #region Start counting words
            foreach (string word in words)
            {
                if (WordsAndNumberOfOccurencies.ContainsKey(word))
                {
                    WordsAndNumberOfOccurencies[word]++;
                }
                else
                {
                    WordsAndNumberOfOccurencies.Add(word, 1);
                }
            }
            #endregion

            WordsAndNumberOfOccurencies = returnTop10Results(WordsAndNumberOfOccurencies);
            TopOccurencyWords = CastDictionaryToObservableCollection(WordsAndNumberOfOccurencies);
            return TopOccurencyWords;
        }

        private static Dictionary<string, int> returnTop10Results(Dictionary<string, int> wordsAndNumberOfOccurencies)
        {

            Dictionary<string, int> top10Words = wordsAndNumberOfOccurencies.OrderByDescending(pair => pair.Value).Take(10)
               .ToDictionary(pair => pair.Key, pair => pair.Value);

            return top10Words;
        }

        private static ObservableCollection<Statistics> CastDictionaryToObservableCollection(Dictionary<string, int> dictionary)
        {
            TopOccurencyWords.Clear();

            #region Adding data from Dictionary to ObservableCollection
            foreach (var wordOccurency in dictionary)
            {
                Statistics addedStatisticAboutWord = new Statistics();
                addedStatisticAboutWord.Word = wordOccurency.Key;
                addedStatisticAboutWord.Count = wordOccurency.Value;
                TopOccurencyWords.Add(addedStatisticAboutWord);
            }
            #endregion

            return TopOccurencyWords;

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
