using System;
using System.Threading.Tasks;
using PuppeteerSharp;

class Program
{
    public static async Task Main(string[] args)
    {
        var browser = await Puppeteer.LaunchAsync(new LaunchOptions 
        { 
            Headless = false,
            ExecutablePath = @"C:\Users\{YourUserName}\Downloads\chrome-win\chrome.exe", //Add your username in here 
            Args = new[] { "--user-data-dir=C:\\Users\\{YourUserName}\\AppData\\Local\\Chromium\\User Data" } //Add your username in here 
        });

        var page = await browser.NewPageAsync();
        await page.GoToAsync("https://web.whatsapp.com/");
        
        await Task.Delay(15000); 
        await SendMessage(page, "Contact name you want to send msg.", "Msg that you want to send in WhatsApp");

        await browser.CloseAsync();
    }

    public static async Task SendMessage(IPage page, string contactName, string message)
    {
        var searchBox = await page.QuerySelectorAsync("div[contenteditable='true'][data-tab='3']");
        if (searchBox != null)
        {
            await searchBox.ClickAsync();
            await searchBox.TypeAsync(contactName);
            await Task.Delay(2000);

            var contactElement = await page.QuerySelectorAsync($"span[title*='{contactName}']");
            if (contactElement != null)
            {
                Console.WriteLine($"Contact found: {contactName}");
                await contactElement.ClickAsync(); 
                await Task.Delay(2000);

                await page.Keyboard.TypeAsync(message);
                await page.Keyboard.PressAsync("Enter"); 
                await Task.Delay(3000); 
            }
            else
            {
                Console.WriteLine($"Contact not found: {contactName}");
            }
        }
        else
        {
            Console.WriteLine("Element for contact search was not found");
        }
    }
}
