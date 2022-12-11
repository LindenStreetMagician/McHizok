using McHizok.Entities.Exceptions;
using McHizok.Entities.Extensions;
using McHizok.Entities.Models;
using McHizok.Services.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Reflection;
using System.Text.Json;

namespace McHizok.Services;

public class ApplePieService : IApplePieService
{
    public async Task<Coupon> GetApplePieCoupon(string blockCode)
    {
        var form = ReadFormConfig();

        var chromeOptions = new ChromeOptions();
        chromeOptions.AddArguments("--headless");

        using var webDriver = new ChromeDriver(chromeOptions);

        var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(60));

        webDriver.Url = form.FormUrl;

        wait.Until(webDriver => webDriver.GetElementAndScrollTo(By.XPath(form.PromoCodeInputXPath))).SendKeys(blockCode);

        webDriver.FindElement(By.XPath(form.SendPromoCodeButtonXPath)).Click();

        await Task.Delay(500);

        EnsureBlockCodeIsValid(webDriver, By.XPath(form.CodeInvalidBlockXPath));

        var element = wait.Until(webDriver => webDriver.GetElementAndScrollTo(By.XPath(form.FirstAnswerXPath)));
        await Task.Delay(2000);
        element.Click();

        foreach (var question in form.Questions)
        {
            if (question.IsRandomAnswer)
            {
                var randomAnswer = question.Answers.GetRandomElement();
                webDriver.GetElementAndScrollTo(By.XPath(randomAnswer.XPath)).Click();

                continue;
            }

            foreach (var answer in question.Answers)
            {
                var answerElement = webDriver.GetElementAndScrollTo(By.XPath(answer.XPath));

                switch (answer.Action)
                {
                    case Actions.Click:
                        answerElement.Click();
                        break;
                    case Actions.Text:
                        answerElement.SendKeys(answer.Text.GetRandomElement());
                        break;
                }
            }
        }

        webDriver.GetElementAndScrollTo(By.XPath(form.SubmitButtonXPath)).Click();

        var elementToScreen = wait.Until(webDriver => webDriver.GetElementAndScrollTo(By.XPath(form.CouponXPath)));

        var elementScreenshot = (elementToScreen as ITakesScreenshot)!.GetScreenshot();
        var fileName = $"expiration {DateTime.Now.AddDays(7).ToString("yyyy-M-dd--HH--mm--ss")}.jpeg";

        var couponBytes = Convert.FromBase64String(elementScreenshot.AsBase64EncodedString);

        webDriver.Quit();

        return new Coupon { Content = couponBytes, FileName = fileName };
    }

    private Form ReadFormConfig()
    {
        var dirPath = Assembly.GetExecutingAssembly().Location;
        dirPath = Path.GetDirectoryName(dirPath);
        var dir = Path.Join(dirPath, "\\Resources\\form.json");
        var jsonString = File.ReadAllText(dir);
        return JsonSerializer.Deserialize<Form>(jsonString)!;
    }

    private void EnsureBlockCodeIsValid(ChromeDriver webDriver, By invalidBlockXPath)
    {
        try
        {
            var isBlockCodeInvalidDivPresent = webDriver.FindElement(invalidBlockXPath).Enabled;

            if (isBlockCodeInvalidDivPresent)
                throw new BlockCodeInvalidBadRequestException("The given code is either invalid or expired.");
        }
        catch (NoSuchElementException)
        {
            Console.WriteLine("minden fasza.");
        }
    }
}