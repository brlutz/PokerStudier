# PokerStudier
Hand History Analyzer for PokerStars

Goal of this project is to build a simple range analyser and results tracker for Pokerstars hand history. You're more than welcome to contribute, open an issue if you have any questions.

I'm going to keep this database free for the moment and try to do any reads from text files with totall processing each time. I may move to a "precalculated" idempotent calculator if processing times exceed 1 minute. If total recalculation moves to more than 10 minutes, I'm going to set up a more robust datbase solution to make sure data is persisted better than in text files. 

# Currently Implimented
* Hands being read in and rough parsing

# Initial Scope

* Range Analyser for Hero (position agnostic)
* Winnings Tracker for Hero (overall aka Green Line)

# Near Future scope
* Range Analyser for Hero (position aware)
* Winnings tracker for Hero (non-showdown aka Red Line)
* Winnings tracker for Hero (showdown only aka Blue Line)

# Medium Range Scope
* Ability for PFR and other stats

# Long Term Plan
* Create a web database to store and upload results and stats
* Enable accounts
* Think poker tracker/sharkscope/HHresults type of database
