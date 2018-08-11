using Elections.Models;
using Elections.Options;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;

namespace Elections.Services
{
	public class Mailer
	{
		private readonly GmailOptions _gmailOptions;
		private readonly SmtpClient client;

		public Mailer(IOptions<GmailOptions> options)
		{
			_gmailOptions = options.Value;

			client = new SmtpClient
			{
				UseDefaultCredentials = false,
				Host = "smtp.gmail.com",
				Port = 587,
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				Credentials = new NetworkCredential(_gmailOptions.Email, _gmailOptions.Password),
				Timeout = 20000
			};
		}

		public string SendConfirmationMail(string recipient, string code)
		{
			var body = $@"<h1>RBYA Elections!</h1><p>Thank you for signing up to vote for the RBYA elections. 
Your code is {code} if the link below doesn't work feel free to enter on <a href=""http://rbya.azurewebsites.net"">the website</a>.</p>
<h3><a href=""http://rbya.azurewebsites.net/login?code={code}"">Click Here to Vote!</a>";

			var message = CreateMail("Voting Confirmation", body, recipient);

			try
			{
				client.Send(message);
				return "Please check your email for a secret code from the RBYA. Enter that onto the main page in order to log in or click the link in the email to access your voting.";
			}
			catch (Exception e)
			{
				return "Error! " + e.Message;
			}
			finally
			{
				message.Dispose();
			}
		}

		public string SendCandidateConfirmation(Candidate c)
		{
			var body = $@"
<p>Congratulations, {c.Name}! You have been nominated to serve for the for the position of {c.Position} in the RBYA Committee!</p>
<p>In order for us to confirm your nomination, we've generated a unique page for you to visit on our website. Please visit there to either accept or decline the nomination.</p>
<p><a href=""{GetCandidateUrl(c)}"">Click Here To Accept or Decline the Nomination</a></p>
<p>Please visit <a href=""http://www.rbya.org/elections""> our elections page </a> for more information. Be sure to confirm your nomination ASAP!
<p> Thanks, <br>The RBYA Committee</p>
";

			var mail = CreateMail("RBYA Committee Nomination", body, c.Email);

			try
			{
				client.Send(mail);
				return $"A confirmation email has been sent to {c.Name} <{c.Email}>";
			}
			catch (Exception e)
			{
				return "Error! " + e.Message;
			}
			finally
			{
				mail.Dispose();
			}
		}

		private string GetCandidateUrl(Candidate c)
		{
			return "http://rbya.vote/candidates/confirmation/" + c.Guid;
		}

		public MailMessage CreateMail(string subject, string body, string recipient)
		{
			var message = new MailMessage
			{
				From = new MailAddress(_gmailOptions.Email),
				Subject = subject,
				Body = body,
				IsBodyHtml = true
			};
			message.To.Add(recipient);
			return message;
		}
	}
}
