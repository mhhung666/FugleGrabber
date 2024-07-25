using System.Data;
using Dapper;
using FugleGrabber.Database;
using FugleGrabber.Model;

namespace FugleGrabber.Service;

public class TickerService
{
    private readonly DatabaseHelper _databaseHelper;
    private readonly IDbConnection _dbConnection;

    public TickerService(DatabaseHelper databaseHelper)
    {
        _databaseHelper = databaseHelper;
    }

    public async Task<IEnumerable<Ticker>> GetAllTickersAsync()
    {
        using (IDbConnection connection = _databaseHelper.CreateConnection())
        {
            string query = "SELECT * FROM tickers";
            return await connection.QueryAsync<Ticker>(query);
        }
    }

    public async Task<Ticker> GetTickerBySymbolAsync(string symbol)
    {
        using (IDbConnection connection = _databaseHelper.CreateConnection())
        {
            string query = "SELECT * FROM tickers WHERE symbol = @Symbol";
            return await connection.QuerySingleOrDefaultAsync<Ticker>(query, new { Symbol = symbol });
        }
    }
    
    public async Task InsertOrUpdateTickersAsync(IEnumerable<Ticker> tickers)
    {
        var query = @"
            INSERT INTO tickers (symbol, name, type, exchange, update_time) 
            VALUES (@Symbol, @Name, @Type, @Exchange, @UpdateTime)
            ON DUPLICATE KEY UPDATE
                name = VALUES(name),
                type = VALUES(type),
                exchange = VALUES(exchange),
                update_time = VALUES(update_time);";

        using (IDbConnection connection = _databaseHelper.CreateConnection())
        {
            await connection.ExecuteAsync(query, tickers);
        }
    }
}