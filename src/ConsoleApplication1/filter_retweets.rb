require 'ClassLibrary1'
include ClassLibrary1

class FilterRetweets < PartDefinition
	include IFilter
	export IFilter
	
	def initialize
	  puts "Filtering out RTs" 
	end
	
	def get_filtered_out_tweets(tweets)
	  tweets.select {|t| tweet_allowed(t)}
	end
	
	def tweet_allowed(tweet)
	  tweet.text.match(/^RT /).nil?
	end
end