/// <summary>
/// Copyright 2016 John L. Swartzentruber
/// </summary>

namespace DebateEmulator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    class DebateEmulatorProgram
    {
        static Random rng = new Random();

        static void Main(string[] args)
        {
            int loopCount = 5000;
            int teamCount = 166;
            int roundCount = 6;
            int totalMisplaced = 0;
            int totalRunoff = 0;
            for (int loopCt = 0; loopCt < loopCount; ++loopCt)
            {
                var teams = new Teams();
                for (int i = 0; i < teamCount; ++i)
                {
                    double u1 = rng.NextDouble(); //these are uniform(0,1) random doubles
                    double u2 = rng.NextDouble();
                    double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
                    teams.AllTeams.Add(new DebateTeam(randStdNormal));
                }

                teams.TeamsByRecord.Clear();
                for (int round = 0; round < roundCount; ++round)
                {
                    teams.AddNewRound();
                    SameRecord(teams);
                }

                teams.AddNewRound();

                var cutOff = teams.AllTeams.OrderByDescending(t => t.Ability).Take(32).Last().Ability;

                totalMisplaced += MisplacedFinishers(teams, cutOff);
                totalRunoff += MadeRunnoff(teams, cutOff);
            }

            Console.WriteLine("Average misplaced = {0}", 1.0 * totalMisplaced / loopCount);
            Console.WriteLine("Average break = {0}", 1.0 * totalRunoff / loopCount);
        }

        static void SameRecord(Teams teams)
        {
            var combined = new List<DebateTeam>();

            for (int i = teams.TeamsByRecord.Count - 1; i >= 0; --i)
            {
                combined.AddRange(teams.TeamsByRecord[i]);
            }

            for (int i = 0; i < combined.Count; i += 2)
            {
                teams.Competition(combined[i], combined[i + 1]);
            }
        }

        static void OppositeRecords(Teams teams)
        {
            var combined = new List<DebateTeam>();

            for (int i = teams.TeamsByRecord.Count - 1; i >= 0; --i)
            {
                combined.AddRange(teams.TeamsByRecord[i]);
            }

            for (int i = 0; i < combined.Count; i += 2)
            {
                teams.Competition(combined[i], combined[combined.Count - 1 - i]);
            }
        }

        static int MisplacedFinishers(Teams teams, double cutOff)
        {
            int elimCount = 0;
            int level;
            for (level = teams.TeamsByRecord.Count - 1; level >= 0 && elimCount < 32; --level)
            {
                elimCount += teams.TeamsByRecord[level].Count;
            }

            int misplaced = 0;
            for (int i = 0; i < level; ++i)
            {
                misplaced += teams.TeamsByRecord[i].Count(t => t.Ability >= cutOff);
            }

            return misplaced;
        }

        static int MadeRunnoff(Teams teams, double cutOff)
        {
            int elimCount = 0;
            int level;
            for (level = teams.TeamsByRecord.Count - 1; level >= 0 && elimCount < 32; --level)
            {
                elimCount += teams.TeamsByRecord[level].Count;
            }

            return elimCount;
        }
    }
}
