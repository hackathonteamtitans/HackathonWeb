using HackathonWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using System.Globalization;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using static Microsoft.AspNetCore.Http.HttpContext;

namespace HackathonWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "Google Sheets API .NET Quickstart";
        public static List<InterviewData> InterviewDatas { get; set; }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            GetCalendarData();
            return View(InterviewDatas);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public UserCredential GetCredential()
        {
            UserCredential credential;


            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }
            return credential;
        }

        public void GetCalendarData()
        {
            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = GetCredential(),
                ApplicationName = ApplicationName,
            });

            // Define request parameters.
            String spreadsheetId = "1_T-8hgakOdeWE0xCMCYwNvDuSGvPWPb16jPic2ZvXhA";
            String range = "Drive!A2:K";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);

            // Prints the names and majors of students in a sample spreadsheet:
            // https://docs.google.com/spreadsheets/d/1_T-8hgakOdeWE0xCMCYwNvDuSGvPWPb16jPic2ZvXhA/edit
            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;
            List<InterviewData> data = new List<InterviewData>();
            CultureInfo culture = new CultureInfo("en-US");
            if (values != null && values.Count > 0)
            {   
                foreach (var row in values)
                {
                    // Print columns
                    data.Add(new InterviewData
                    {
                        SrNo = row[0] != null ? Convert.ToInt16(row[0]) : 0,
                        Name = row[1] != null ? row[1].ToString() : string.Empty,
                        SkillSet = row[2] != null ? row[2].ToString().Split(",").ToList() : null,
                        Source = row[3] != null ? row[3].ToString() : string.Empty,
                        Show = row[4] != null && row[4].ToString() == "Yes" ? Show.Yes : Show.No,
                        Contact = row[5] != null ? row[5].ToString() : string.Empty,
                        Email = row[6] != null ? row[6].ToString() : string.Empty,
                        Experience = row[7] != null ? Convert.ToDecimal(row[7]) : 0,
                        NoticePeriod = row[8] != null ? Convert.ToInt16(row[8]) : 0,
                        InterviewTime = row[9] != null ? Convert.ToDateTime(row[9], culture) : null,
                        ResumeLink = row[10] != null ? row[10].ToString() : string.Empty,
                    });
                }

                InterviewDatas = data;
            }
            else
            {

                InterviewDatas = null;
            }
        }


        public IActionResult SendInvites()
        {
            try
            {
                foreach (var record in InterviewDatas)
                {

                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return View(InterviewDatas);
        }

        //public void SendCalendarInvites(InterviewData interviewData)
        //{   
        //    // Create Google Calendar API service.
        //    var service = new CalendarService(new BaseClientService.Initializer()
        //    {
        //        HttpClientInitializer = GetCredential(),
        //        ApplicationName = ApplicationName,
        //    });


        //    var ev = new Event();
        //    EventDateTime start = new EventDateTime();
        //    start.DateTime = interviewData.InterviewTime;

        //    EventDateTime end = new EventDateTime();
        //    end.DateTime = interviewData.InterviewTime.Value.AddHours(1);

        //    ev.Start = start;
        //    ev.End = end;
        //    ev.Summary = "";
        //    ev.Description = "Interview";

        //    ev.Attachments.Add();

        //    var calendarId = "primary";
        //    Event recurringEvent = service.Events.Insert(ev, calendarId).Execute();
        //}
    }
}
