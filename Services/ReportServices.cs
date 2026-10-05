using Npgsql;

namespace cpwp_pr_1.Services;

public class ReportService
{
    public void DailyReport(DateTime date)
    {
        using var conn = Db.GetConnection();

        using (var cmd = new NpgsqlCommand(
            @"SELECT type, COALESCE(SUM(quantity),0)
              FROM operations
              WHERE date::date = @d
              GROUP BY type", conn))
        {
            cmd.Parameters.AddWithValue("d", date.Date);
            int income = 0, outcome = 0;
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                var t = r.GetString(0);
                var sum = r.GetInt32(1);
                if (t == "in") income = sum;
                else outcome = sum;
            }
            Console.WriteLine($"\nДата: {date:dd.MM.yyyy}");
            Console.WriteLine($"Приход: {income}");
            Console.WriteLine($"Расход: {outcome}");
        }

        Console.WriteLine("\nТовары ниже минимального остатка:");
        using (var cmd = new NpgsqlCommand(
            "SELECT name, quantity, min_quantity FROM products WHERE quantity < min_quantity ORDER BY name", conn))
        using (var r = cmd.ExecuteReader())
        {
            bool any = false;
            while (r.Read())
            {
                any = true;
                Console.WriteLine($"- {r.GetString(0)} | {r.GetInt32(1)} | {r.GetInt32(2)}");
            }
            if (!any) Console.WriteLine("(нет)");
        }
    }
}