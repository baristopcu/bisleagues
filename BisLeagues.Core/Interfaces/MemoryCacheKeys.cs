namespace BisLeagues.Core.Interfaces
{
    public static class MemoryCacheKeys
    {
        public const string HomePageKey = "HomePageViewModel-Cache";
        public const string CompanyNameCacheKey = "CompanySettings-Name";
        public const string CompanyAddressCacheKey = "CompanySettings-Address";
        public const string CompanyPhoneCacheKey = "CompanySettings-Phone";
        public const string CompanyEmailCacheKey = "CompanySettings-Email";
        public const string TeamDetailPastMatchesByIdCacheKey = "TeamDetail-PastMatches-Id-{0}";
        public const string ExchangeTableCacheKey = "ExchangeTable-PageNumber-{0}-PageSize-{1}";
        public const string ExchangeTableTotalCountCacheKey = "ExchangeTableTotalCount-PageNumber-{0}-PageSize-{1}";
        public const string GoalKingTableCacheKey = "GoalKingTable-PageNumber-{0}-PageSize-{1}";
        public const string GoalKingTableTotalCountCacheKey = "GoalKingTableTotalCount-PageNumber-{0}-PageSize-{1}";
        public const string NewsOfPastMatchesCacheKey = "NewsOfPastMatches-PageNumber-{0}-PageSize-{1}";
        public const string NewsOfPastMatchesTotalCountCacheKey = "NewsOfPastMatchesTotalCount-PageNumber-{0}-PageSize-{1}";


    }
}