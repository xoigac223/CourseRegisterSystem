using System.Threading.Tasks;
using CourseRegisterSystem.Data;

namespace CourseRegisterSystem.Services;

public interface ISendMailService
{
    Task SendMail(MailContent mailContent);
    Task SendEmailAsync(string email, string subject, string htmlMessage);
}