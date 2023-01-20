using McHizok.Entities.DataTransferObjects;
using McHizok.Entities.Exceptions;
using McHizok.Entities.Extensions;
using McHizok.Entities.Models.InputForm;
using McHizok.Services.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace McHizok.Services;

public class ApplePieService : IApplePieService
{
    public async Task<CouponDto> GetApplePieCouponAsync(string blockCode)
    {
        IsBlockCodeFormatValid(blockCode);

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
        var couponCode = wait.Until(webDriver => webDriver.GetElementAndScrollTo(By.XPath(form.CouponCodeXPath))).Text;
        var expiresAt = DateTime.Now.AddDays(7);

        var elementScreenshot = (elementToScreen as ITakesScreenshot)!.GetScreenshot();
        var fileName = $"{couponCode} expires {expiresAt:MM-dd}.jpeg";

        var base64CouponContent = elementScreenshot.AsBase64EncodedString;

        webDriver.Quit();

        return new CouponDto(Guid.Empty, base64CouponContent, fileName, expiresAt, couponCode, string.Empty);
    }

    private Form ReadFormConfig()
    {
        var dirPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var formJsonPath = Path.Join(dirPath, "Services\\Resources\\form.json");

        return JsonSerializer.Deserialize<Form>(File.ReadAllText(formJsonPath))!;
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
        }
    }

    private void IsBlockCodeFormatValid(string blockCode)
    {
        var validBlockCodeLength = 12;

        if (blockCode.Length != validBlockCodeLength)
            throw new BlockCodeInvalidBadRequestException($"The block code length must be {validBlockCodeLength} long.");

        var validBlockCodeFormat = new Regex("^[a-zA-Z0-9]{12}$");

        if (!validBlockCodeFormat.IsMatch(blockCode))
            throw new BlockCodeInvalidBadRequestException($"The block code must be consist of letters and numbers");

    }
}