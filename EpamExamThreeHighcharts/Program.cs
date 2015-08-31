using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Interactions.Internal;
using OpenQA.Selenium.Support.UI;

namespace EpamExamThreeHighcharts
{
    class Program
    {
        private const int Step = 1;
        private IWebDriver driver;

        private static void Main(string[] args)
        {
            
        }

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl(HighchartsPage.PageUrl);

            driver.SwitchTo().Frame(driver.FindElement(By.XPath(HighchartsPage.IframeXPath)));
        }

        [Test]
        public void TestChartsTooltip()
        {
            HighchartsPage page = new HighchartsPage(driver);
            page.HideNonEmployeeCharts();
            IWebElement seriesElement = page.LocateSeries();
            List<Point> graph = GeneratePointsFromSeries(seriesElement);

            List<string> tooltip = GenerateTooltipMessages(driver, graph, page);

            ProcessTooltipMessages(tooltip);
        }

        [TearDown]
        public void TearDown()
        {
            driver.Close();
        }

        private static List<Point> GeneratePointsFromSeries(IWebElement seriesElement)
        {
            string path =
                seriesElement.FindElements(
                    By.TagName(HighchartsPage.PathTagName))[1].GetAttribute(
                        "d");

            string[][] coordinates =
                path.Split('L')
                    .Select(line => line.Trim().Split(' ').ToArray())
                    .ToArray();
            List<Point> graph = new List<Point>();
            
            double x = double.Parse(coordinates[0][1], CultureInfo.InvariantCulture);
            double y = double.Parse(coordinates[0][2], CultureInfo.InvariantCulture);
            graph.Add(new Point((int) x, (int) y));
            for (int i = 1; i < coordinates.Length - 1; i++)
            {
                x = double.Parse(coordinates[i][0], CultureInfo.InvariantCulture);
                y = double.Parse(coordinates[i][3], CultureInfo.InvariantCulture);
                graph.Add(new Point((int) x, (int) y));
            }
            return graph;
        }

        private static List<string> GenerateTooltipMessages(IWebDriver driver, List<Point> graph,
            HighchartsPage page)
        {
            Actions action = new Actions(driver);
            
            
            action.MoveToElement(page.RootElement, 0, 0)
                .MoveByOffset(graph[0].X, graph[0].Y)
                .Build()
                .Perform();
            List<string> tooltip = new List<string>();
            //tooltip.Add("");
            for (int point = 1; point < graph.Count; point++)
            {
                for (int i = 0; i <= graph[point].X - graph[point - 1].X; i+=Step)
                {
                    action.MoveByOffset(Step, 0).Build().Perform();
                    if (tooltip.LastOrDefault() != page.TooltipElement.Text &&
                        page.TooltipElement.Text.Contains(HighchartsPage.EmployeeKeyword))
                    {
                        tooltip.Add(page.TooltipElement.Text);
                    }
                }
                action.MoveByOffset(0, graph[point].Y - graph[point - 1].Y)
                    .Build()
                    .Perform();
            }
            //tooltip.RemoveAt(0);
            return tooltip;
        }

        private static void ProcessTooltipMessages(List<string> tooltip)
        {
            int employee_amount = 0;
            int previous_employee_amount = 0;
            foreach (string message in tooltip)
            {
                Regex capturer = new Regex(HighchartsPage.EmployeeAmountRegex);

                Match match = capturer.Match(message);

                if (match.Success)
                {
                    employee_amount =
                        int.Parse(match.Groups[1].Captures[0].Value);
                }
                if (message.Contains(HighchartsPage.JoinKeyword))
                {
                    Assert.IsTrue((previous_employee_amount < employee_amount));
                }
                if (message.Contains(HighchartsPage.LeftKeyword))
                {
                    Assert.IsTrue((previous_employee_amount > employee_amount));
                }
                previous_employee_amount = employee_amount;
            }
        }
    }
}
