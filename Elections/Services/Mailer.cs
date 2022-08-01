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

		public string SendConfirmationEmailForCandidateAcceptance(Candidate c)
		{
			var body = $@"<h1>RBYA Elections!</h1>

<p>Thank you for <strong>accepting your nomination</strong> for the RBYA election. 
At this point, you will need to be <i>seconded</i> by somebody in the community and your 
pastor will be contacted so that we can get a referral from them.</p>

<p>As a reminder, this year's elections will be held in Chicago, IL at 
Romanian Baptist Church of Metropolitan Chicago (484 East Northwest Highway, Des Plaines, IL, 60016, United States) at 10:00 AM.</p>

<p><strong>We ask all that are nominated to be present for elections and have a 1-3 min introduction of themselves in front of the youth.</strong><br />
If you are <strong>unable</strong> to make it, we ask that you make a 1 minute video introducing yourself and explaining why you are wanting to serve!<br />
If you are sending in a video please send it <strong>via Google Drive</strong> to <a href='mailto:admin@rbya.org'>admin@rbya.org</a> by <strong>Thursday, September 2nd at 11:59 PM.</strong></p>

<p>Committing to RBYA committee is a commitment of time, effort, and finances. 
This is a ministry that is by the youth for the Romanian youth. It is completely voluntary. 
We do ask that you take this decision seriously serving the community and ultimately our Lord 
and Savior following in the commandments of sharing the Gospel. 
We look forward to all that this next year brings with you apart of it!

<p>We look forward to this exciting year working together for the praise and worship of our Lord and Savior, Jesus Christ.<br />
God Bless!<br />
Mark 16:15-16</p>

<p>Blessings,<br />
RBYA Elections Team</p>
";

			var message = CreateMail("RBYA Committee Nomination - Accepted!", body, c.Email);

			try
			{
				client.Send(message);
				return null;
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
<p>Congratulations, {c.Name}! You have been nominated to serve for the for the position of {c.Position} in the RBYA Committee in the 2021-2022 year!</p>
<p>In order for us to confirm your nomination, we've generated a unique page for you to visit on our website. Please visit there to either accept or decline the nomination.</p>
<p><a href=""{GetCandidateUrl(c)}"">Click Here To Accept or Decline the Nomination</a></p>
<p>Please visit <a href=""http://www.rbya.org/elections""> our elections page </a> for more information. Be sure to confirm your nomination ASAP!

<p>This year's elections will be held in Chicago, IL at Romanian Baptist Church of Metropolitan Chicago (484 East Northwest Highway, Des Plaines, IL, 60016, United States) at 10:00 AM.</p>

<p><strong>We ask all that are nominated to be present for elections and have a 1-3 min introduction of themselves in front of the youth.</strong><br />
If you are <strong>unable</strong> to make it, we ask that you make a 1 minute video introducing yourself and explaining why you are wanting to serve!<br />
If you are sending in a video please send it <strong>via Google Drive</strong> to <a href='mailto:admin@rbya.org'>admin@rbya.org</a> by <strong>Thursday, September 2nd at 11:59 PM.</strong></p>

<p>Committing to RBYA committee is a commitment of time, effort, and finances. This is a ministry that is by the youth for the Romanian youth. It is completely voluntary. We do ask that you take this decision seriously serving the community and ultimately our Lord and Savior following in the commandments of sharing the Gospel. We look forward to all that this next year brings with you apart of it!

<p>We look forward to this exciting year working together for the praise and worship of our Lord and Savior, Jesus Christ.<br />
God Bless!<br />
Mark 16:15-16</p>

<p>Blessings,<br />
RBYA Elections Team</p>
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
			return "http://rbya.vote/confirmation/" + c.Guid;
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
