// Copyright 2016 John L Swartzentruber
namespace DebateEmulator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class Teams
    {
        private readonly Random rng = new Random();

        public IList<DebateTeam> AllTeams { get; }
        public IList<IList<DebateTeam>> TeamsByRecord { get; }

        public Teams()
        {
            this.AllTeams = new List<DebateTeam>();
            this.TeamsByRecord = new List<IList<DebateTeam>>();
        }

        /// <summary>
        /// Resets the TeamsByRecord object. The collection for each record is randomized.
        /// </summary>
        public void AddNewRound()
        {
            int roundNumber = this.TeamsByRecord.Count + 1;

            this.TeamsByRecord.Clear();
            for (int i = 0; i < roundNumber; ++i)
            {
                var recordList = new List<DebateTeam>();
                var withThisRecord = this.AllTeams.Where(t => t.WinCount == i).ToList();

                while (withThisRecord.Any())
                {
                    int index = this.rng.Next(withThisRecord.Count);
                    var team = withThisRecord[index];
                    recordList.Add(team);
                    withThisRecord.Remove(team);
                }

                this.TeamsByRecord.Add(recordList);
            }
        }

        public void Competition(DebateTeam team1, DebateTeam team2, bool randomize = false)
        {
            var rand = this.rng.NextDouble();
            var val = (rand * team1.Ability) / team2.Ability;
            if (val >= 0.5)
            {
                team1.WinCount++;
                team2.LossCount++;
            } else
            {
                team2.WinCount++;
                team1.LossCount++;
            }
        }
    }
}
