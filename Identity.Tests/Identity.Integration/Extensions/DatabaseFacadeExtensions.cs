using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Identity.Integration.Extensions;

public static class DatabaseFacadeExtensions
{
    public static void DropTables(this DatabaseFacade db)
    {
        const string sql =
            """
            DO $$
            DECLARE r RECORD;
            BEGIN
                FOR r IN (SELECT tablename FROM pg_tables WHERE schemaname = current_schema()) LOOP
                    EXECUTE 'DROP TABLE IF EXISTS ' || quote_ident(r.tablename) || ' CASCADE';
                END LOOP;
            END $$;
            """;

        db.ExecuteSqlRaw(sql);
    }
}