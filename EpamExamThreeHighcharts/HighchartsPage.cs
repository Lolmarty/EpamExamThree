using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.PageObjects;

namespace EpamExamThreeHighcharts
{
    class HighchartsPage
    {
        public const string PageUrl = @"http://www.highcharts.com/component/content/article/2-news/146-highcharts-5th-anniversary";

        public const string PathTagName = "path";
        public const string IframeXPath = "//*[@id=\"hs-component\"]/div/div/p[2]/iframe";
        public const string LegendClassName = "highcharts-legend-item";
        public const string SeriesClassName = "highcharts-series";

        public const string EmployeeAmountRegex = @"(\d{1,2}) employee";

        public const string EmployeeKeyword = "employee";
        public const string JoinKeyword = "join";
        public const string LeftKeyword = "left";

        public HighchartsPage(IWebDriver driver)
        {
            PageFactory.InitElements(driver, this);
        }
        
        [FindsBy(How = How.ClassName, Using = @"highcharts-tooltip")]
        public IWebElement TooltipElement { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#highcharts-0 > svg > g.highcharts-series-group")]
        public IWebElement RootElement { get; set; }

        [FindsBy(How = How.ClassName, Using = "highcharts-legend-item")]
        public IWebElement LegendItems { get; set; }

        [FindsBy(How = How.Id, Using = "container")]
        public IWebElement Container { get; set; }

        public void HideNonEmployeeCharts()
        {
            foreach (
                IWebElement item in
                    Container.FindElements(By.ClassName(HighchartsPage.LegendClassName)))
            {
                string tag = item.FindElement(By.TagName("text")).Text;
                if (!tag.Contains(HighchartsPage.EmployeeKeyword)) item.Click();
            }
        }

        public IWebElement LocateSeries()
        {
            IWebElement seriesElement = Container.FindElement(By.ClassName(HighchartsPage.SeriesClassName));
            foreach (
                IWebElement item in
                    Container.FindElements(By.ClassName(HighchartsPage.SeriesClassName)))
            {
                if (item.GetAttribute("visibility") != "hidden" &&
                    item.ContainsElement(By.TagName("path")))
                {
                    seriesElement = item;
                    break;
                }
            }
            return seriesElement;
        }

    }
}
