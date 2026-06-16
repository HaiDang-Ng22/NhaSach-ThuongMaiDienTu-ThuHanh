using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Net.Mail;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using BookStoreOnline.Models;

public class UserServiceFacade
{
    private NhaSachEntities3 db = new NhaSachEntities3();
    private const string SecretKey = "your_new_very_long_secret_key_at_least_32_characters!";

    public string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }

    public string GenerateAccessToken(KHACHHANG user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(SecretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Ten),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("TrangThai", user.TrangThai.ToString()),
                new Claim("NgayTao", user.NgayTao?.ToString("yyyy-MM-dd HH:mm:ss"))
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }

    public KHACHHANG SignUp(KHACHHANG cus, string rePass, out string message)
    {
        if (db.KHACHHANGs.Any(c => c.Email == cus.Email))
        {
            message = "Email đã được sử dụng";
            return null;
        }

        if (cus.MatKhau.Length < 6)
        {
            message = "Mật khẩu phải có ít nhất 6 ký tự";
            return null;
        }

        if (cus.MatKhau != rePass)
        {
            message = "Mật khẩu xác nhận không khớp";
            return null;
        }

        cus.TrangThai = true;
        cus.NgayTao = DateTime.Now;
        cus.MatKhau = HashPassword(cus.MatKhau);

        string accessToken = GenerateAccessToken(cus);
        string refreshToken = GenerateRefreshToken();

        cus.AccessToken = accessToken;
        cus.RefreshToken = refreshToken;
        cus.TokenExpiration = DateTime.UtcNow.AddDays(7);

        db.KHACHHANGs.Add(cus);
        db.SaveChanges();

        message = "Đăng ký thành công";
        return cus;
    }

    public KHACHHANG Login(string email, string password, out string message)
    {
        var hashedPassword = HashPassword(password);
        var account = db.KHACHHANGs.FirstOrDefault(k => k.Email == email && k.MatKhau == hashedPassword);

        if (account == null)
        {
            message = "Email hoặc mật khẩu không chính xác";
            return null;
        }

        if (!account.TrangThai)
        {
            message = "Tài khoản đã bị khóa";
            return null;
        }

        account.AccessToken = GenerateAccessToken(account);
        account.RefreshToken = GenerateRefreshToken();
        account.TokenExpiration = DateTime.UtcNow.AddDays(7);
        db.SaveChanges();

        message = "Đăng nhập thành công";
        return account;
    }

    public bool ForgotPassword(string email, out string message)
    {
        var user = db.KHACHHANGs.FirstOrDefault(u => u.Email == email);
        if (user == null)
        {
            message = "Không tìm thấy tài khoản với email này.";
            return false;
        }

        user.ResetPasswordToken = Guid.NewGuid().ToString();
        user.ResetTokenExpiration = DateTime.UtcNow.AddHours(1);
        db.SaveChanges();

        var resetLink = $"https://yourwebsite.com/User/ResetPassword?token={user.ResetPasswordToken}";
        SendPasswordResetEmail(user.Email, resetLink);

        message = "Đã gửi email khôi phục mật khẩu.";
        return true;
    }

    private void SendPasswordResetEmail(string email, string resetLink)
    {
        var mail = new MailMessage();
        mail.From = new MailAddress("manhhoang8th4@gmail.com");
        mail.To.Add(email);
        mail.Subject = "Khôi phục mật khẩu";
        mail.Body = $"Nhấp vào <a href='{resetLink}'>đây</a> để đặt lại mật khẩu.";
        mail.IsBodyHtml = true;

        var smtpClient = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential("manhhoang8th4@gmail.com", "your_smtp_password"),
            EnableSsl = true
        };
        smtpClient.Send(mail);
    }
}
