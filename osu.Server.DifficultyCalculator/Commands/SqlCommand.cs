// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using Dapper;
using McMaster.Extensions.CommandLineUtils;

namespace osu.Server.DifficultyCalculator.Commands
{
    [Command("sql", Description = "Calculates the difficulty of beatmaps based on a custom filter.")]
    public class SqlCommand : CalculatorCommand
    {
        [Argument(0, "sql", Description = "The SQL clause to match against (on `osu_beatmaps`)")]
        public string Sql { get; set; }

        protected override IEnumerable<int> GetBeatmaps()
        {
            using (var conn = Database.GetSlaveConnection())
            {
                var condition = CombineSqlConditions(
                    Sql,
                    "`deleted_at` IS NULL"
                );

                return conn.Query<int>($"SELECT `beatmap_id` FROM `osu_beatmaps` {condition}");
            }
        }
    }
}
