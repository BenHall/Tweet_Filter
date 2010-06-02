using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using ClassLibrary1;

namespace ConsoleApplication1
{
    [Export(typeof(IFilter))]
    public class DefaultFilter : IFilter
    {
        public List<Tweet> GetFilteredOutTweets(List<Tweet> tweets)
        {
            return new List<Tweet>();
        }

        public bool TweetAllowed(Tweet tweet)
        {
            return true;
        }
    }
}