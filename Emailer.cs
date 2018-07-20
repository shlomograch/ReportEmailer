using System;
using System.Net.Mail;
using System.Linq;

using AutomationHelpers.Utilities.Emailer;
using AutomationHelpers.Utilities.Helpers;
using AutomationHelpers.Utilities.TestRun;

namespace Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class GetLastRanEmail
    {
        /// <summary>
        /// Private, read-only variable for storing the email list model.
        /// </summary>
        private readonly EmailListModel _EmailList = new EmailListModel();

        /// <summary>
        /// Private, read-only variable for storing the email distribution model.
        /// </summary>
        private readonly EmailDistributorFlatModel _EmailListDistributor = new EmailDistributorFlatModel();

        /// <summary>
        /// The Email module that handles grabbing TestRun data from the database and then sending that data via Email from AutomationOutput@Nelnet.net.
        /// </summary>
        public void SendEmail()
        {
            var emailDistro = _EmailList.EmailLists.ToList();

            var newEmailDist =
                    _EmailListDistributor.EmailDistributorFlats
                        .ToList()
                        .Where(appName => appName.ApplicationName == PropertiesCollection.AllTestRuns[0].ApplicationName)
                        .ToList();

            #region Format and Send Email

            #region Set the Email From, To Section and SMTP
            MailMessage mail = new MailMessage();
            MailAddress fromMail = new MailAddress("AutomationOutput@nelnet.net");
            mail.From = fromMail;
            foreach (var item in newEmailDist)
            {
                mail.To.Add(item.Email);
            }

            //mail.To.Add("Shlomo.Grach@nelnet.net");
            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "Exchcasprd01v.us.nelnet.biz";
            #endregion Set the Email From, To Section and SMTP

            #region Format Elapsed Time
            //Todo: not getting correct end time.

            var elapsedTime =
                Convert.ToInt32(
                (DateTime.Now -
                  PropertiesCollection.AllTestRuns.FirstOrDefault().StartDateTime).TotalSeconds);
            int elapsedMinutes = elapsedTime / 60;
            string elapsedTimeString = String.Empty;
            if (elapsedMinutes > 0)
            {
                if (elapsedMinutes == 1)
                {
                    elapsedTimeString = elapsedMinutes + " Minute";
                }
                else
                {
                    elapsedTimeString = elapsedMinutes + " Minutes";
                }
                elapsedTime -= (elapsedMinutes * 60);
                elapsedTimeString += " and " + elapsedTime + " Seconds";
            }
            else
            {
                elapsedTimeString = elapsedTime + " Seconds";
            }
            #endregion Format Elapsed Time

            #region Format the Email Body

            mail.IsBodyHtml = true;
            mail.Body = "<div style='font-family:Calibri;'>";
            //mail.Body += "<img src='https://cdn2.iconfinder.com/data/icons/font-awesome/1792/flask-128.png' style='width:128px;height:128px;'>";

            mail.Body += "<b>Automation Test Report:</b></br>";
            mail.Body += "  Tests Executed By: " + PropertiesCollection.AllTestRuns.FirstOrDefault().TestRunExecuter + "</br>";

            mail.Body += "  Start Time: " + $"{PropertiesCollection.AllTestRuns.FirstOrDefault().StartDateTime}</br>";
            mail.Body += "  End Time: " + $"{PropertiesCollection.AllTestRuns.LastOrDefault().EndDateTime}</br>";
            mail.Body += "  Total Elapsed Time: " + elapsedTimeString + " Elapsed</br>";
            mail.Body += "  Environment: " + PropertiesCollection.AllTestRuns[0].Environment + "</br>";

            if (PropertiesCollection.AllTestRuns[0].BrowserVersion != null)
            {
                mail.Body += "  Browser: " + $"{PropertiesCollection.AllTestRuns[0].BrowserVersion}</br>";
            }
            mail.Body += "</br>";

            mail.Body += "<b>Individual Tests in the run:</b></br>";
            bool overallStatusPassed = true;

            foreach (var run in PropertiesCollection.AllTestRuns.Where(run => run.TestResult == NewTestRun.TestResultTypes.Fail))
            {
                overallStatusPassed = false;
            }

            foreach (var item in PropertiesCollection.AllTestRuns)
            {
                mail.Body += $"  {item.TestName} - {item.TestResult} </br>";
            }

            mail.Body += "------</br>";
            mail.Body += "To see full results please <a href='http://autodash.nelnet.net/EmailView/Index/" + $"{PropertiesCollection.AllTestRuns.FirstOrDefault().TestRunGuid}'>Click Here</a>";
            mail.Body += "</div>";

            #endregion Format the Email Body

            #region Format the Subject Line
            var overallResult = "PASS";
            if (!overallStatusPassed)
            {
                overallResult = "FAIL";
            }
            mail.Subject = $"Automated Report for {PropertiesCollection.AllTestRuns[0].ApplicationName} finished with a total result of {overallResult}";
            #endregion Format the Subject Line

            client.Send(mail);

            #endregion Format and Send Email
        }
    }
}
