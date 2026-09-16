using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace hailinh.Utils
{
    /// <summary>
    /// SMTP gửi mail — lưu trên ConfigSites (cột ngoài EF model để khỏi migration snapshot).
    /// Cấu hình tại MMS → Thông tin chung.
    /// </summary>
    public static class SmtpSettings
    {
        const string RemovedLegacySmtp = "kythuatluatankhang@gmail.com";

        public static void EnsureColumns()
        {
            var cs = WebConfigurationManager.ConnectionStrings["DataEntities"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(cs)) return;

            using (var conn = new SqlConnection(cs))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
IF OBJECT_ID(N'dbo.ConfigSites', N'U') IS NOT NULL
BEGIN
    IF COL_LENGTH(N'dbo.ConfigSites', N'SmtpEmail') IS NULL
        ALTER TABLE dbo.ConfigSites ADD SmtpEmail NVARCHAR(100) NULL;
    IF COL_LENGTH(N'dbo.ConfigSites', N'SmtpPassword') IS NULL
        ALTER TABLE dbo.ConfigSites ADD SmtpPassword NVARCHAR(200) NULL;

    -- Gỡ SMTP cũ không còn dùng
    IF COL_LENGTH(N'dbo.ConfigSites', N'SmtpEmail') IS NOT NULL
        UPDATE dbo.ConfigSites
        SET SmtpEmail = NULL, SmtpPassword = NULL
        WHERE SmtpEmail = @legacy;
END";
                    cmd.Parameters.AddWithValue("@legacy", RemovedLegacySmtp);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void Get(out string email, out string password)
        {
            email = null;
            password = null;
            var cs = WebConfigurationManager.ConnectionStrings["DataEntities"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(cs)) return;

            try
            {
                using (var conn = new SqlConnection(cs))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT TOP 1 SmtpEmail, SmtpPassword FROM dbo.ConfigSites";
                        using (var r = cmd.ExecuteReader())
                        {
                            if (r.Read())
                            {
                                email = r["SmtpEmail"] as string;
                                password = r["SmtpPassword"] as string;
                            }
                        }
                    }
                }
            }
            catch
            {
                // cột chưa có
            }

            if (string.Equals(email, RemovedLegacySmtp, StringComparison.OrdinalIgnoreCase))
            {
                email = null;
                password = null;
            }

            // Fallback tùy chọn từ appSettings (nếu còn khai báo)
            if (string.IsNullOrWhiteSpace(email))
                email = ConfigurationManager.AppSettings["email"];
            if (string.IsNullOrWhiteSpace(password))
                password = ConfigurationManager.AppSettings["password"];

            if (string.Equals(email, RemovedLegacySmtp, StringComparison.OrdinalIgnoreCase))
            {
                email = null;
                password = null;
            }
        }

        public static void Save(string email, string password)
        {
            var cs = WebConfigurationManager.ConnectionStrings["DataEntities"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(cs)) return;

            EnsureColumns();

            using (var conn = new SqlConnection(cs))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // Giữ mật khẩu cũ nếu form để trống
                    cmd.CommandText = @"
UPDATE TOP (1) dbo.ConfigSites
SET SmtpEmail = @e,
    SmtpPassword = CASE
        WHEN @p IS NULL OR LTRIM(RTRIM(@p)) = N'' THEN SmtpPassword
        ELSE @p
    END";
                    cmd.Parameters.AddWithValue("@e", (object)email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@p", string.IsNullOrWhiteSpace(password) ? (object)DBNull.Value : password);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
