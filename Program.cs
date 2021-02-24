using System;
using System.IO;
using System.Net;
using System.Text;
using CapMonsterCloud; //  Install-Package CapMonsterCloud 
using CapMonsterCloud.Models.CaptchaTasks;
using CapMonsterCloud.Models.CaptchaTasksResults;

namespace CapMonsterDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // excluded by .gitignore, but contains the CapMonster key
            var secret = File.ReadAllText("secret.txt");
            var start = DateTime.Now;
            var client = new CapMonsterClient(secret);
            var captchaTask = new RecaptchaV3TaskProxyless
            {
                WebsiteUrl = "https://lessons.zennolab.com/captchas/recaptcha/v3.php?level=beta",
                WebsiteKey = "6Le0xVgUAAAAAIt20XEB4rVhYOODgTl00d8juDob",
                MinScore = 0.3,
                PageAction = "myverify"
            };
            // Create the task and get the task id
            var taskId = client.CreateTaskAsync(captchaTask).Result;
            Console.WriteLine("Created task id : " + taskId);
            //var solution = client.GetTaskResultAsync<RecaptchaV3TaskProxylessResult>(taskId).Result;
            // Recaptcha response to be used in the form
            //var recaptchaResponse = solution.GRecaptchaResponse;
            var recaptchaResponse = "bad";
            
            Console.WriteLine("Solution : " + recaptchaResponse);
            var web = new WebClient {Encoding = Encoding.UTF8};
            web.Headers.Add("content-type","application/x-www-form-urlencoded");
            var result = web.UploadString("https://lessons.zennolab.com/captchas/recaptcha/v3_verify.php?level=beta", "token=" + recaptchaResponse);
            var idxStart = result.IndexOf("<pre>", StringComparison.Ordinal);
            var idxEnd = result.IndexOf("</pre>", StringComparison.Ordinal);
            var jsonResult = result.Substring(idxStart, idxEnd - idxStart);
            Console.WriteLine(jsonResult);
            var end = DateTime.Now;
            var duration = end-start;
            Console.WriteLine(duration.TotalSeconds);
        }
    }
}
