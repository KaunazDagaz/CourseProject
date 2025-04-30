using CourseProject.Entities;
using CourseProject.Models;
using CourseProject.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CourseProject.Services
{
    public class SearchService : ISearchService
    {
        private readonly ApplicationDbContext dbContext;

        public SearchService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<TemplateGalleryViewModel>> SearchTemplatesAsync(
            string query,
            bool includePrivate = false,
            string? userId = null,
            int limit = 50,
            double similarityThreshold = 0.1)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<TemplateGalleryViewModel>();
            var connection = dbContext.Database.GetDbConnection() as NpgsqlConnection;
            if (connection == null)
                return new List<TemplateGalleryViewModel>();
            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();
            using (var cmd = new NpgsqlCommand("CREATE EXTENSION IF NOT EXISTS pg_trgm;", connection))
            {
                await cmd.ExecuteNonQueryAsync();
            }
            var results = new List<TemplateGalleryViewModelWithScore>();
            results.AddRange(await SearchWithQueryAsync(connection, GetTemplateSql(), query, includePrivate, userId, limit, similarityThreshold));
            results.AddRange(await SearchWithQueryAsync(connection, GetQuestionSql(), query, includePrivate, userId, limit, similarityThreshold));
            results.AddRange(await SearchWithQueryAsync(connection, GetQuestionOptionSql(), query, includePrivate, userId, limit, similarityThreshold));
            results.AddRange(await SearchWithQueryAsync(connection, GetCommentSql(), query, includePrivate, userId, limit, similarityThreshold));
            results.AddRange(await SearchWithQueryAsync(connection, GetTagSql(), query, includePrivate, userId, limit, similarityThreshold));
            return results
                .GroupBy(t => t.Id)
                .Select(g => g.OrderByDescending(r => r.Score).First())
                .OrderByDescending(t => t.Score)
                .Take(limit)
                .ToList();
        }

        private async Task<List<TemplateGalleryViewModelWithScore>> SearchWithQueryAsync(
            NpgsqlConnection connection,
            string sql,
            string query,
            bool includePrivate,
            string? userId,
            int limit,
            double similarityThreshold)
        {
            var results = new List<TemplateGalleryViewModelWithScore>();
            using (var cmd = new NpgsqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("query", query);
                cmd.Parameters.AddWithValue("includePrivate", includePrivate);
                cmd.Parameters.AddWithValue("userId", userId ?? string.Empty);
                cmd.Parameters.AddWithValue("threshold", similarityThreshold);
                cmd.Parameters.AddWithValue("limit", limit);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        results.Add(new TemplateGalleryViewModelWithScore
                        {
                            Id = reader.GetGuid(0),
                            Title = reader.GetString(1),
                            Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                            AuthorName = reader.GetString(3),
                            Image = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Score = reader.GetDouble(5)
                        });
                    }
                }
            }
            return results;
        }

        private string GetTemplateSql() => @"
            SELECT t.""Id"", t.""Title"", t.""Description"", u.""Name"", t.""Image"",
                   greatest(
                       similarity(t.""Title"", @query),
                       similarity(COALESCE(t.""Description"", ''), @query),
                       similarity(t.""Topic"", @query)
                   ) as score
            FROM ""Templates"" t
            JOIN ""AspNetUsers"" u ON t.""AuthorId"" = u.""Id""
            WHERE (
                similarity(t.""Title"", @query) > @threshold OR
                similarity(COALESCE(t.""Description"", ''), @query) > @threshold OR
                similarity(t.""Topic"", @query) > @threshold
            )
            AND (t.""IsPublic"" = true OR (@includePrivate AND t.""AuthorId"" = @userId))
            ORDER BY score DESC
            LIMIT @limit";

        private string GetQuestionSql() => @"
            SELECT t.""Id"", t.""Title"", t.""Description"", u.""Name"", t.""Image"",
                   greatest(
                       similarity(q.""Title"", @query),
                       similarity(COALESCE(q.""Description"", ''), @query)
                   ) as score
            FROM ""Questions"" q
            JOIN ""Forms"" f ON q.""FormId"" = f.""Id""
            JOIN ""Templates"" t ON f.""TemplateId"" = t.""Id""
            JOIN ""AspNetUsers"" u ON t.""AuthorId"" = u.""Id""
            WHERE (
                similarity(q.""Title"", @query) > @threshold OR
                similarity(COALESCE(q.""Description"", ''), @query) > @threshold
            )
            AND (t.""IsPublic"" = true OR (@includePrivate AND t.""AuthorId"" = @userId))
            ORDER BY score DESC
            LIMIT @limit";

        private string GetQuestionOptionSql() => @"
            SELECT t.""Id"", t.""Title"", t.""Description"", u.""Name"", t.""Image"",
                   similarity(qo.""Text"", @query) as score
            FROM ""QuestionOptions"" qo
            JOIN ""Questions"" q ON qo.""QuestionId"" = q.""Id""
            JOIN ""Forms"" f ON q.""FormId"" = f.""Id""
            JOIN ""Templates"" t ON f.""TemplateId"" = t.""Id""
            JOIN ""AspNetUsers"" u ON t.""AuthorId"" = u.""Id""
            WHERE similarity(qo.""Text"", @query) > @threshold
            AND (t.""IsPublic"" = true OR (@includePrivate AND t.""AuthorId"" = @userId))
            ORDER BY score DESC
            LIMIT @limit";

        private string GetCommentSql() => @"
            SELECT t.""Id"", t.""Title"", t.""Description"", u.""Name"", t.""Image"",
                   similarity(c.""Content"", @query) as score
            FROM ""Comments"" c
            JOIN ""Templates"" t ON c.""TemplateId"" = t.""Id""
            JOIN ""AspNetUsers"" u ON t.""AuthorId"" = u.""Id""
            WHERE similarity(c.""Content"", @query) > @threshold
            AND (t.""IsPublic"" = true OR (@includePrivate AND t.""AuthorId"" = @userId))
            ORDER BY score DESC
            LIMIT @limit";

        private string GetTagSql() => @"
            SELECT t.""Id"", t.""Title"", t.""Description"", u.""Name"", t.""Image"",
                   similarity(tag.""Name"", @query) as score
            FROM ""Tags"" tag
            JOIN ""TemplateTags"" tt ON tag.""Id"" = tt.""TagId""
            JOIN ""Templates"" t ON tt.""TemplateId"" = t.""Id""
            JOIN ""AspNetUsers"" u ON t.""AuthorId"" = u.""Id""
            WHERE similarity(tag.""Name"", @query) > @threshold
            AND (t.""IsPublic"" = true OR (@includePrivate AND t.""AuthorId"" = @userId))
            ORDER BY score DESC
            LIMIT @limit";

        internal class TemplateGalleryViewModelWithScore : TemplateGalleryViewModel
        {
            public double Score { get; set; }
        }
    }
}