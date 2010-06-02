using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using ClassLibrary1;

namespace ConsoleApplication1
{
    [Export(typeof(IFilter))]
    public class DefaultFilter : IFilter
    {
        public object GetFilteredOutTweets(List<Tweet> tweets)
        {
            return tweets.Where(TweetAllowed).ToList();
        }

        public bool TweetAllowed(Tweet tweet)
        {
            return !string.IsNullOrEmpty(tweet.User) && !string.IsNullOrEmpty(tweet.Text);
        }
    }
}