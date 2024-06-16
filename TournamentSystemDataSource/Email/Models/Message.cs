﻿using MimeKit;

namespace TournamentSystemDataSource.Email.Models
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public Message(IEnumerable<string> to, string subject, string content)
        {
            To = [.. to.Select(x => new MailboxAddress("", x))];
            Subject = subject;
            Content = content;
        }
    }
}
