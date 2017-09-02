﻿using Elections.Options;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;

namespace Elections.Services
{
	public class Mailer
	{
		private readonly GmailOptions _gmailOptions;

		public Mailer(IOptions<GmailOptions> options)
		{
			_gmailOptions = options.Value;
		}

		public string SendConfirmationMail(string recipient, string code)
		{
			MailMessage message = new MailMessage
			{
				From = new MailAddress(_gmailOptions.Email),
				Subject = "Voting Confirmation",
				Body = $"Thank you for signing up to vote for the RBYA elections. Your code is {code}"
			};
			message.To.Add(recipient);


			SmtpClient client = new SmtpClient
			{
				UseDefaultCredentials = false,
				Host = "smtp.gmail.com",
				Port = 587,
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				Credentials = new NetworkCredential(_gmailOptions.Email, _gmailOptions.Password),
				Timeout = 20000
			};

			try
			{
				client.Send(message);
				return "Please check your email for a secret code from the RBYA. Enter that onto the main page in order to log in.";
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
	}
}