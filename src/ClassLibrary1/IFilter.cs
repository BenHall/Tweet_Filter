using System.Collections.Generic;

namespace ClassLibrary1
{
    public interface IFilter
    {
        /// <summary>
        /// Tweets which are not allowed to be displayed
        /// </summary>
        List<Tweet> GetFilteredOutTweets(List<Tweet> tweets);

        bool TweetAllowed(Tweet tweet);
    }
}