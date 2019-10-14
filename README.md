# PokerStudier
Hand History Analyzer for PokerStars

Goal of this project is to build a simple range analyser and results tracker for Pokerstars hand history. You're more than welcome to contribute, open an issue if you have any questions.

I'm going to keep this database free for the moment and try to do any reads from text files with totall processing each time. I may move to a "precalculated" idempotent calculator if processing times exceed 1 minute. If total recalculation moves to more than 10 minutes, I'm going to set up a more robust datbase solution to make sure data is persisted better than in text files. 

# Currently Implimented
* Hands being read in and rough parsing
* Range Analyser for Hero with hero position filtering
* Hand reviewer for hand types
* Actions parsing for hero
* Filter scaffold for future hero actions filtering

![Example Image](https://raw.githubusercontent.com/brlutz/PokerStudier/master/example.JPG "Example")

# Currently being implimented
* Really big refactor happening in "EnemyActions" branch. This should provide similar functionality to existing stuff, but will provide a more generic approach to parsing hands that should allow the ability to see ranges/hands for any enemy. This is a pretty big rewite and is currently ongoing. Hope to have that sorted by 10/20ish.

# Near Future scope
*  Winnings Tracker for Hero (overall aka Green Line)
* Winnings tracker for Hero (non-showdown aka Red Line)
* Winnings tracker for Hero (showdown only aka Blue Line)

# Medium Range Scope
* Ability for PFR and other stats

# Long Term Plan
* Create a web database to store and upload results and stats
* Enable accounts
* Think poker tracker/sharkscope/HHresults type of database
