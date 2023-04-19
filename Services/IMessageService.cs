namespace rp_ef_maria.Services;

public interface IMessageService
{
    public bool SendEmail(string from, string to, string subject, string body);
}

