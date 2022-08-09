[assembly: FunctionsStartup(typeof(Startup))]

namespace QueryApi;

public class Startup : FunctionsStartup
{
    private const string RedisConnectionStringConfigKey = "RedisConnectionString";

    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services
            .AddScoped((provider) =>
            {
                var config = provider.GetRequiredService<IConfiguration>();
                var redisConnectionString = config[RedisConnectionStringConfigKey];
                ArgumentNullException.ThrowIfNull(redisConnectionString);

                var connectionProvider = new RedisConnectionProvider(redisConnectionString);
                return connectionProvider.Connection;
            })
            .AddScoped<ISuggestionService<ArticleDocument>, ArticleSuggestionService>()
            .AddScoped<IArticleQueryService, ArticleQueryService>()
            .AddScoped<ISpellCheckService, SpellCheckService>();
    }
}