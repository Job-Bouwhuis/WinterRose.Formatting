namespace WinterRose.Formatting.Currency;

using System.Net.Http.Json;
using WinterRose.TaskLock;

internal static class CurrencyExchange
{
    private static HttpClient? HttpClient = new();
    private static readonly CachedTaskLock<CurrencyPair, decimal> ApiCache = new(TimeSpan.FromHours(8), absoluteTimeout: TimeSpan.FromDays(1));

    static CurrencyExchange()
    {
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
    }

    public static async Task<decimal> GetConversion(Currency from, Currency to)
    {
        if (from == to)
            return 1m;
        return await ApiCache.GetOrAddAsync(new CurrencyPair(from, to), c => DoConversion(from, to));
    }

    private static async Task<decimal> DoConversion(Currency from, Currency to)
    {
        string fromCode = from.CurrencyCode;
        string toCode = to.CurrencyCode;

        HttpClient ??= new();
        CurrencyResponse? response = await HttpClient.GetFromJsonAsync<CurrencyResponse>(
            $"https://api.frankfurter.app/latest?from={fromCode}&to={toCode}")
            ?? throw new InvalidOperationException("No response received from Frankfurter.");

        if (!response.Rates.TryGetValue(toCode, out decimal rate))
            throw new InvalidOperationException($"No conversion rate found from {fromCode} to {toCode}.");

        return rate;
    }

    private static void OnUnhandledException(object? sender, UnhandledExceptionEventArgs e)
    {
        HttpClient.Dispose();
        HttpClient = null;
    }

    private sealed class CurrencyResponse
    {
        public required Dictionary<string, decimal> Rates { get; init; }
    }
}