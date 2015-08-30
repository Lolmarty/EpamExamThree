using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace EpamExamThreeHighcharts
{
    public static class Utilities
    {

        public static bool ContainsElement(this IWebElement element, By by)
        {
            try
            {
                element.FindElement(by);
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
            return true;
        }
    }
}
