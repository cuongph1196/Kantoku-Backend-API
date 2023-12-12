namespace QLKTX.Class.Helper
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }
        // refresh token time to live (in days), inactive tokens are
        // automatically deleted from the database after this time
        public int RefreshTokenTTL { get; set; }
        public int RefreshTokenCookie { get; set; }
        public int ResetTokenExpires { get; set; }
        public int TokenExpires { get; set; }
        public int WebTokenExpires { get; set; }

    }
}
