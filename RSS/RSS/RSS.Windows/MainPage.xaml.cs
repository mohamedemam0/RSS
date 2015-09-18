using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Syndication;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace RSS
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {                
        public MainPage()
        {
            this.InitializeComponent();
            
        }
                     
        private async void ItemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SyndicationClient client = new SyndicationClient();
            Uri feedUri = new Uri("http://mohammedemam.wordpress.com/feed/");
            var feed = await client.RetrieveFeedAsync(feedUri);
            FeedItem f = new FeedItem();
            f.Link =  feed.Items[ItemListView.SelectedIndex].Links[0].Uri;
            web.Navigate(f.Link);
            web.Visibility = Visibility.Visible;
        }

        private async void k_Click_1(object sender, RoutedEventArgs e)
        {
            SyndicationClient client = new SyndicationClient();
            Uri feedUri = new Uri("http://mohammedemam.wordpress.com/feed/");
            var feed = await client.RetrieveFeedAsync(feedUri);

            FeedData feedData = new FeedData();

            foreach (SyndicationItem item in feed.Items)
            {
                FeedItem feedItem = new FeedItem();
                feedItem.Title = item.Title.Text;
                feedItem.PubDate = item.PublishedDate.DateTime;
                feedItem.Author = item.Authors[0].Name;
                // Handle the differences between RSS and Atom feeds.
                if (feed.SourceFormat == SyndicationFormat.Atom10)
                {
                    feedItem.Content = item.Content.Text;
                    feedItem.Link = new Uri("http://mohammedemam.wordpress.com/feed/" + item.Id);
                }
                else if (feed.SourceFormat == SyndicationFormat.Rss20)
                {
                    feedItem.Content = item.Summary.Text;
                    feedItem.Link = item.Links[0].Uri;
                }
                feedData.Items.Add(feedItem);
            }
            ItemListView.DataContext = feedData.Items;
        }   
    }
}
