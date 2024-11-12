// namespace ManufacturingManager.Web.Services
// {
//     public class GraphHelper
//     {
//         private static GraphServiceClient graphClient;
//         public static void Initialize(IAuthenticationProvider authProvider)
//         {
//             graphClient = new GraphServiceClient(authProvider);
//         }
//
//         public static async Task<IEnumerable<Message>> GetMailAsync(string user)
//         {
//
//             var resultPage = await graphClient.Me.Messages.Request()
//                 .Select(e => new
//                 {
//                     e.Subject,
//                     e.Sender,
//                     e.Body,
//                     e.SentDateTime
//                 })
//                 .Top(2)
//                 //.OrderBy("sentDateTime DESC")
//                 .GetAsync();
//
//             //var resultPage = await graphClient.Users[user].Messages.Request()
//             //    .Select(e => new
//             //    {
//             //        e.Subject,
//             //        e.Sender,
//             //        e.Body,
//             //        e.SentDateTime
//             //    })
//             //    .Top(2)
//             //    //.OrderBy("sentDateTime DESC")
//             //    .GetAsync();
//
//             return resultPage.CurrentPage;
//         }
//
//         public static async void DeleteMessage(string user, string messageID)
//         {
//             await graphClient.Users[user].Messages[messageID].Request()
//                 .DeleteAsync();
//         }
//
//         public static async void SendMessage(string user, string subject, string body, IEnumerable<string> recipientEmails)
//         {
//             List<Recipient> recipients = new List<Recipient>();
//             foreach (string recipientEmail in recipientEmails)
//             {
//                 recipients.Add(new Recipient
//                 {
//                     EmailAddress = new EmailAddress
//                     {
//                         Address = recipientEmail
//                     }
//                 });
//             }
//             var message = new Message
//             {
//                 Subject = subject,
//                 Body = new ItemBody
//                 {
//                     ContentType = BodyType.Text,
//                     Content = body,
//                 },
//                 ToRecipients = recipients,
//             };
//             await graphClient.Users[user].SendMail(message, false)
//                 .Request()
//                 .PostAsync();
//         }
//     }
// }