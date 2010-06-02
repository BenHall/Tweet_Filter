require 'ClassLibrary1'
include ClassLibrary1

class FilterBenHall < PartDefinition
	include IFilter
	export IFilter
	
	def initialize
	  puts "Filtering out Ben Hall"
	end
	
	def get_filtered_out_tweets(tweets)
	  tweets.select {|t| tweet_allowed(t)}
	end
	
	def tweet_allowed(tweet)
	  tweet.user != "Ben_Hall"
	end

end