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

            try
            {
                using (var conn = new SqlConnection(cs))
                {
                    conn.Open();

                    // 1. Thêm cột SmtpEmail nếu chưa có
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
IF OBJECT_ID(N'dbo.ConfigSites', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.ConfigSites', N'SmtpEmail') IS NULL
    ALTER TABLE dbo.ConfigSites ADD SmtpEmail NVARCHAR(100) NULL;";
                        cmd.ExecuteNonQuery();
                    }

                    // 2. Thêm cột SmtpPassword nếu chưa có
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
IF OBJECT_ID(N'dbo.ConfigSites', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.ConfigSites', N'SmtpPassword') IS NULL
    ALTER TABLE dbo.ConfigSites ADD SmtpPassword NVARCHAR(200) NULL;";
                        cmd.ExecuteNonQuery();
                    }

                    // 3. Gỡ SMTP cũ không còn dùng (dùng sp_executesql để chỉ compile khi cột đã tồn tại)
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
IF OBJECT_ID(N'dbo.ConfigSites', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.ConfigSites', N'SmtpEmail') IS NOT NULL
    EXEC sp_executesql N'UPDATE dbo.ConfigSites SET SmtpEmail = NULL, SmtpPassword = NULL WHERE SmtpEmail = @legacy',
                       N'@legacy NVARCHAR(100)', @legacy = @legacy;";
                        cmd.Parameters.AddWithValue("@legacy", RemovedLegacySmtp);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                // Tránh lỗi ngoại lệ làm gián đoạn ứng dụng
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

            try
            {
                using (var conn = new SqlConnection(cs))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        // Giữ mật khẩu cũ nếu form để trống
                        cmd.CommandText = @"
IF OBJECT_ID(N'dbo.ConfigSites', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.ConfigSites', N'SmtpEmail') IS NOT NULL
BEGIN
    UPDATE TOP (1) dbo.ConfigSites
    SET SmtpEmail = @e,
        SmtpPassword = CASE
            WHEN @p IS NULL OR LTRIM(RTRIM(@p)) = N'' THEN SmtpPassword
            ELSE @p
        END
END";
                        cmd.Parameters.AddWithValue("@e", (object)email ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@p", string.IsNullOrWhiteSpace(password) ? (object)DBNull.Value : password);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                // Tránh throw làm crash trang admin nếu kết nối hoặc bảng có vấn đề
            }
        }
    }
}
