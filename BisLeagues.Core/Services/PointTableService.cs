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
    public class PointTableService : IPointTableService
    {
        private readonly IMatchRepository _matchRepository;
        private readonly ISeasonRepository _seasonRepository;
        private readonly IResultRepository _resultRepository;
        private readonly IPointRepository _pointRepository;
        private readonly IPointTableRowRepository _pointTableRowRepository;
        private readonly ITeamRepository _teamRepository;

        public PointTableService(IMatchRepository matchRepository, ISeasonRepository seasonRepository, IResultRepository resultRepository, IPointRepository pointRepository, IPointTableRowRepository pointTableRowRepository, ITeamRepository teamRepository)
        {
            _seasonRepository = seasonRepository;
            _matchRepository = matchRepository;
            _resultRepository = resultRepository;
            _pointRepository = pointRepository;
            _pointTableRowRepository = pointTableRowRepository;
            _teamRepository = teamRepository;
        }

        public async Task<bool> CreatePointTablesForActiveSeasons()
        {
            //add scope here
            try
            {
                var seasons = _seasonRepository.GetActiveSeasons();
                foreach (var season in seasons)
                {
                    int seasonId = season.Id;
                    var oldPointTable = _pointTableRowRepository.GetPointTableRowsBySeasonId(seasonId);
                    _pointTableRowRepository.RemoveRange(oldPointTable);

                    List<Point> pointsOfSeason = _pointRepository.Find(x => x.SeasonId == seasonId).ToList();
                    List<int> homeTeams = pointsOfSeason.Select(x => x.Result.Match.Home.Id).ToList();
                    List<int> awayTeams = pointsOfSeason.Select(x => x.Result.Match.Away.Id).ToList();
                    List<int> teamIdsOfSeason = homeTeams.Union(awayTeams).ToList();
                    List<PointTableRow> pointTable = new List<PointTableRow>();
                    foreach (var team in teamIdsOfSeason)
                    {
                        int average = 0, matchCount = 0, winCount = 0, loseCount = 0, totalPoint = 0;
                        List<Point> homeWins = pointsOfSeason.Where(x => x.Result.Match.Home.Id == team && x.HomePoint > x.AwayPoint).ToList();
                        List<Point> awayWins = pointsOfSeason.Where(x => x.Result.Match.Away.Id == team && x.AwayPoint > x.HomePoint).ToList();
                        List<Point> draws = pointsOfSeason.Where(x => (x.Result.Match.Away.Id == team || x.Result.Match.Home.Id == team) && x.AwayPoint == x.HomePoint).ToList();
                        List<Point> homeLoses = pointsOfSeason.Where(x => x.Result.Match.Home.Id == team && x.HomePoint < x.AwayPoint).ToList();
                        List<Point> awayLoses = pointsOfSeason.Where(x => x.Result.Match.Away.Id == team && x.AwayPoint < x.HomePoint).ToList();
                        foreach (var point in homeWins)
                        {
                            totalPoint += point.HomePoint;
                            average += point.Result.HomeScore;
                            average -= point.Result.AwayScore;
                            winCount += 1;
                            matchCount++;
                        }
                        foreach (var point in awayWins)
                        {
                            totalPoint += point.AwayPoint;
                            average += point.Result.AwayScore;
                            average -= point.Result.HomeScore;
                            winCount += 1;
                            matchCount++;
                        }
                        foreach (var point in draws)
                        {
                            totalPoint += point.HomePoint == point.AwayPoint ? point.HomePoint : 1;
                            matchCount++;
                        }
                        foreach (var point in homeLoses)
                        {
                            totalPoint += point.HomePoint; // -'li değer gelecek zaten
                            average += point.Result.HomeScore;
                            average -= point.Result.AwayScore;
                            loseCount += 1;
                            matchCount++;
                        }
                        foreach (var point in awayLoses)
                        {
                            totalPoint += point.AwayPoint; // -'li değer gelecek zaten
                            average += point.Result.AwayScore;
                            average -= point.Result.HomeScore;
                            loseCount += 1;
                            matchCount++;
                        }

                        var teamToRow = _teamRepository.Get(team);
                        pointTable.Add(new PointTableRow() { SeasonId = seasonId, Team = teamToRow, Average = average, MatchCount = matchCount, WinCount = winCount, LoseCount = loseCount, Point = totalPoint });
                    }
                    pointTable = pointTable.OrderByDescending(x => x.Point).ThenByDescending(x => x.Average).ThenByDescending(x => x.MatchCount).ThenBy(x => x.Team.Name).ToList();

                    _pointTableRowRepository.AddRange(pointTable);
                }
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }

        }
    }
}
