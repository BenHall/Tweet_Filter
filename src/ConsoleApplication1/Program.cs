using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using ClassLibrary1;
using Newtonsoft.Json.Linq;

namespace ConsoleApplication1
{
    public class Program
    {
        [Import(typeof (IFilter))]
        private readonly IList<IFilter> filters;

        public Program()
        {
            filters = new List<IFilter>();
        }

        private static void Main()
        {
            Console.WriteLine("Starting...");
            var p = new Program();
            LoadMef(p);

            p.Start();

            Console.ReadLine();
        }

        private static void LoadMef(Program p)
        {
            var catalogs = new AssemblyCatalog(typeof(Program).Assembly);
            
            new Composer(p, ".", catalogs).Compose();
        }

        private void Start()
        {
            List<Tweet> tweets = LoadTweets("Tweets.json");

            foreach (Tweet tweet in tweets)
            {
                if (IsTweetAllowed(tweet))
                    Console.WriteLine("@{0}\t{1}", tweet.User, GetFirstChars(tweet.Text, 50));
                else
                    Console.WriteLine("Tweet from @{0} blocked", tweet.User);
            }
        }

        private bool IsTweetAllowed(Tweet tweet)
        {
            bool allowed = true;
            foreach (IFilter filter in filters)
            {
                allowed = filter.TweetAllowed(tweet);
                if(!allowed)
                    break;
            }

            return allowed;
        }

        private string GetFirstChars(string text, int length)
        {
            if (text.Length <= length)
                return text;

            return text.Substring(0, length);
        }

        private List<Tweet> LoadTweets(string source)
        {
            string json = File.ReadAllText(source);
            return Parse(json);
        }

        private List<Tweet> Parse(string json)
        {
            List<Tweet> tweets = new List<Tweet>();

            JObject jObject = JObject.Parse(json);
            JArray results = jObject["results"].Value<JArray>();

            foreach (JToken jToken in results)
            {
                tweets.Add(new Tweet
                               {
                                   User = jToken["from_user"].Value<string>(), 
                                   Text = jToken["text"].Value<string>()
                               });
            }
            return tweets;
        }
    }
}