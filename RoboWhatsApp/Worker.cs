using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace RoboWhatsApp
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private int countLoop;
        private string nameContact;
        private string message;
        private int timerLogin = 30;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1, stoppingToken);
                await ReceiveInfos();
                await SendSpanMessage();
            }
        }

        public async Task ReceiveInfos()
        {
            Console.WriteLine("Digite o nome do contato (É necessario que o mesmo não possua espaços):");
            nameContact = Console.ReadLine();
            Console.WriteLine("Digite a mensagem:");
            message = Console.ReadLine();
            Console.WriteLine("Digite a quantidade de envios:");
            countLoop = Convert.ToInt32(Console.ReadLine());
        }

        public async Task<bool> SendSpanMessage()
        {
            try
            {
                // Create a new ChromeDriver instance
                IWebDriver driver = new ChromeDriver();

                // Navigate to a web page
                _logger.LogInformation("01-Conectando com site", DateTimeOffset.Now);
                driver.Navigate().GoToUrl("https://web.whatsapp.com/");


                Console.WriteLine("Efetue o login pela nova aba do chrome aberta (Em até 30 segundos) em seguida aguarde.");
                //Tempo para logar no whatsapp
                while(timerLogin > 0)
                {
                    Console.WriteLine("Segundos para iniciar: " + timerLogin);
                    Thread.Sleep(1000);

                    timerLogin--;
                }


                // Click the search button
                IWebElement searchButton = driver.FindElement(By.CssSelector("[title='"+$"{nameContact}"+"']"));
                searchButton.Click();

                while (countLoop > 0)
                {
                    //// Find the search field and enter some text
                    IWebElement writeField = driver.FindElement(By.CssSelector("[data-testid='conversation-compose-box-input']"));
                    writeField.SendKeys($"{message}");

                    IWebElement sendButton = driver.FindElement(By.CssSelector("[data-testid='send']"));
                    sendButton.Click();

                    countLoop--;
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.LogInformation("Erro nas chamadas as: {time}", e.Message);
                throw;
            }
        }
    }
    
}
