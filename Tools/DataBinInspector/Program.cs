using System;
using System.Data.SQLite;

class Program
{
    const string Password = "JkejXP5_fG2vN-jlFVME";
    static void Main()
    {
        var builder = new SQLiteConnectionStringBuilder
        {
            DataSource = @"C:\Users\Texy6\Desktop\newhcb\CopilotBuddy\datadb\data.bin",
            Password = Password,
            ReadOnly = true
        };
        using var conn = new SQLiteConnection(builder.ConnectionString);
        conn.Open();
        
        // Flag breakdown � what types of NPCs are stored
        // ClassTrainer=32, ProfessionTrainer=64, Vendor=128, AmmoVendor=256, FoodVendor=512,
        // ReagentVendor=2048, AnyVendor=3968, Repair=4096, Flightmaster=8192

        var queries = new[]
        {
            ("ClassTrainer (flag&32)",     "SELECT COUNT(*) FROM npcs WHERE flag & 32"),
            ("ProfTrainer (flag&64)",       "SELECT COUNT(*) FROM npcs WHERE flag & 64"),
            ("Vendor (flag&128)",           "SELECT COUNT(*) FROM npcs WHERE flag & 128"),
            ("FoodVendor (flag&512)",       "SELECT COUNT(*) FROM npcs WHERE flag & 512"),
            ("ReagentVendor (flag&2048)",   "SELECT COUNT(*) FROM npcs WHERE flag & 2048"),
            ("Repair (flag&4096)",          "SELECT COUNT(*) FROM npcs WHERE flag & 4096"),
            ("Flightmaster (flag&8192)",    "SELECT COUNT(*) FROM npcs WHERE flag & 8192"),
            ("Innkeeper (flag&65536)",      "SELECT COUNT(*) FROM npcs WHERE flag & 65536"),
            ("TOTAL",                       "SELECT COUNT(*) FROM npcs"),
        };

        foreach (var (label, sql) in queries)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            Console.WriteLine($"  {label,-30}: {cmd.ExecuteScalar()}");
        }

        // trainer_class distribution
        Console.WriteLine("\n=== trainer_class distribution (trainers only) ===");
        using var tcCmd = conn.CreateCommand();
        tcCmd.CommandText = "SELECT trainer_class, COUNT(*) FROM npcs WHERE flag & 96 GROUP BY trainer_class ORDER BY trainer_class";
        using var tcR = tcCmd.ExecuteReader();
        while (tcR.Read()) Console.WriteLine($"  trainer_class={tcR[0]}: {tcR[1]}");
    }
}
