using Npgsql;

namespace cpwp_pr_1.Services;

public class OperationService
{
    public void Income(int productId, int qty) => Move(productId, qty, "in");
    public void Outcome(int productId, int qty) => Move(productId, qty, "out");

    private void Move(int productId, int qty, string type)
    {
        if (qty <= 0)
        {
            Console.WriteLine("Количество должно быть > 0.");
            return;
        }

        using var conn = Db.GetConnection();
        using var tx = conn.BeginTransaction();
        try
        {
            using (var check = new NpgsqlCommand(
                "SELECT quantity FROM products WHERE id=@id FOR UPDATE", conn, tx))
            {
                check.Parameters.AddWithValue("id", productId);
                var res = check.ExecuteScalar();
                if (res == null)
                {
                    Console.WriteLine("Товар не найден.");
                    tx.Rollback();
                    return;
                }
                var current = (int)res;
                if (type == "out" && current < qty)
                {
                    Console.WriteLine($"Недостаточно товара. Остаток: {current}");
                    tx.Rollback();
                    return;
                }
            }

            using (var upd = new NpgsqlCommand(
                type == "in"
                    ? "UPDATE products SET quantity = quantity + @q WHERE id=@id"
                    : "UPDATE products SET quantity = quantity - @q WHERE id=@id", conn, tx))
            {
                upd.Parameters.AddWithValue("q", qty);
                upd.Parameters.AddWithValue("id", productId);
                upd.ExecuteNonQuery();
            }

            using (var ins = new NpgsqlCommand(
                "INSERT INTO operations (product_id, type, quantity) VALUES (@p,@t,@q)", conn, tx))
            {
                ins.Parameters.AddWithValue("p", productId);
                ins.Parameters.AddWithValue("t", type);
                ins.Parameters.AddWithValue("q", qty);
                ins.ExecuteNonQuery();
            }

            tx.Commit();
            Console.WriteLine(type == "in" ? "Приход оформлен." : "Расход оформлен.");
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }
}