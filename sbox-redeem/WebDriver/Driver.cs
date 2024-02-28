using System.Reflection.Metadata.Ecma335;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V119.DOM;
using OpenQA.Selenium.Firefox;

namespace sbox_redeem.WebDriver;

public class Driver
{
    private IWebDriver driver;
    private IWebElement redeemButton;
    private IWebElement keyInput;
    public Driver()
    {
        FirefoxOptions options = new FirefoxOptions();
        options.Profile = new FirefoxProfileManager().GetProfile("default");
        options.BinaryLocation = @"firefox";
        options.AddArgument("--headless");
        driver = new FirefoxDriver("geckodriver", options);
        driver.Navigate().GoToUrl("https://asset.party/~invitecode");
        Console.WriteLine("Input your sessionguid cookie:");
        string sessionguid = Console.ReadLine();
        if(sessionguid == null) throw new ArgumentNullException("sessionguid");
        driver.Manage().Cookies.AddCookie(new Cookie("sessionguid", sessionguid));
        driver.Navigate().Refresh();
        //Task.Factory.StartNew(() => TryRedeem("1234"));
    }

    public void GetNewSessionGuid(string cookie)
    {
        driver.Manage().Cookies.DeleteCookieNamed("sessionguid");
        driver.Manage().Cookies.AddCookie(new Cookie("sessionguid", cookie));
    }
    
    public async Task<string> TryRedeem(string key)
    {
        //await Task.Delay(TimeSpan.FromSeconds(1));
        redeemButton = driver.FindElements(By.ClassName("button")).First(e => e.Text.ToLower().Equals("redeem"));
        // there should only be one
        keyInput = driver.FindElement(By.TagName("input"));
        keyInput.Clear();
        keyInput.SendKeys(key);
        redeemButton.Click();
        await Task.Delay(TimeSpan.FromSeconds(1));
        string resultText = driver.FindElement(By.ClassName("user-invite-code")).FindElement(By.XPath(".//div[2]/div/div")).Text;
        Console.WriteLine(resultText);
        return resultText;
    }
}