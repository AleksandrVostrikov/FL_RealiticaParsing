namespace FL_RealiticaParsing.Services
{
    public interface IParserService
    {
        Task<List<string>> ParseLinksAndAboutAuthorLinksAsync(string url);
    }
}
