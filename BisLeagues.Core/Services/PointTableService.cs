using BisLeagues.Core.Interfaces;
using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Models;
using BisLeagues.Core.ServiceModels;
using BisLeagues.Core.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Core.Services
{
    public class PointTableService : PointTableRow, IPointTableService
    {
        private readonly IMatchRepository _matchRepository;
        private readonly ISeasonRepository _seasonRepository;
        private readonly IResultRepository _resultRepository;

        public PointTableService(IMatchRepository matchRepository, ISeasonRepository seasonRepository, IResultRepository resultRepository)
        {
            _seasonRepository = seasonRepository;
            _matchRepository = matchRepository;
            _resultRepository = resultRepository;
        }

        public List<PointTableRow> GetPointTableBySeasonId(int seasonId)
        {
            List<Match> matchesOfSeason = _matchRepository.GetMatchesBySeasonId(seasonId).ToList();
            List<Result> resultsOfMatches = _resultRepository.GetResultsOfMatches(matchesOfSeason).ToList();
            List<Team> homeTeams = matchesOfSeason.Select(x => x.Home).ToList();
            List<Team> awayTeams = matchesOfSeason.Select(x => x.Away).ToList();
            List<Team> teamsOfSeason = homeTeams.Union(awayTeams).ToList();
            List<PointTableRow> pointTable = new List<PointTableRow>();
            foreach (var team in teamsOfSeason)
            {
                int average = 0, matchCount = 0, winCount = 0, loseCount = 0, point = 0;
                List<Result> homeWins = resultsOfMatches.Where(x => x.Match.Home == team && x.HomeScore > x.AwayScore).ToList();
                List<Result> awayWins = resultsOfMatches.Where(x => x.Match.Away == team && x.AwayScore > x.HomeScore).ToList();
                List<Result> draws = resultsOfMatches.Where(x => (x.Match.Away == team || x.Match.Home == team) && x.AwayScore == x.HomeScore).ToList();
                List<Result> homeLoses = resultsOfMatches.Where(x => x.Match.Home == team && x.HomeScore < x.AwayScore).ToList();
                List<Result> awayLoses = resultsOfMatches.Where(x => x.Match.Away == team && x.AwayScore < x.HomeScore).ToList();
                foreach (var result in homeWins)
                {
                    point += 3;
                    average += result.HomeScore;
                    average -= result.AwayScore;
                    matchCount++;
                }
                foreach (var result in awayWins)
                {
                    point += 3;
                    average += result.AwayScore;
                    average -= result.HomeScore;
                    matchCount++;
                }
                foreach (var result in draws)
                {
                    point += 1;
                    matchCount++;
                }
                foreach (var result in homeLoses)
                {
                    point -= 3;
                    average += result.HomeScore;
                    average -= result.AwayScore;
                    matchCount++;
                }
                foreach (var result in awayLoses)
                {
                    point -= 3;
                    average += result.AwayScore;
                    average -= result.HomeScore;
                    matchCount++;
                }
                pointTable.Add(new PointTableRow() { Team = team, Average = average, MatchCount = matchCount, WinCount = winCount, LoseCount = loseCount, Point = point });
            }
            pointTable = pointTable.OrderByDescending(x => x.Point).ThenByDescending(x => x.Average).ThenByDescending(x => x.MatchCount).ThenBy(x => x.Team.Name).ToList();
            return pointTable;
        }
    }
}
