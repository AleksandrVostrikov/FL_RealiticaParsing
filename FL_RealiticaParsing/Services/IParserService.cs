namespace FL_RealiticaParsing.Services
{
    public interface IParserService
    {
        Task<List<string>> ParseLinksAndAboutAuthorLinksAsync(string url);
        Task<int> ParseCount(string url);
    }
}
