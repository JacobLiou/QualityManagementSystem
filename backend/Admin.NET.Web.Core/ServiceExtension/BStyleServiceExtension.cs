using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Web.Core
{
    /// <summary>
    /// B格
    /// </summary>
    internal static class BStyleServiceExtension
    {
        public static void AddBStyle(this IServiceCollection services, Action<BStyleServiceBuilder> configure)
        {
            var builder = new BStyleServiceBuilder(services);
            configure(builder);
        }
    }
    internal class BStyleServiceBuilder
    {
        private IServiceCollection serviceCollection;

        public BStyleServiceBuilder(IServiceCollection services)
        {
            serviceCollection = services;
        }

        public void UseDefault()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(@"  ___      _           _         _   _  _____ _____ 
 / _ \    | |         (_)       | \ | ||  ___|_   _|
/ /_\ \ __| |_ __ ___  _ _ __   |  \| || |__   | |  
|  _  |/ _` | '_ ` _ \| | '_ \  | . ` ||  __|  | |  
| | | | (_| | | | | | | | | | |_| |\  || |___  | |  
\_| |_/\__,_|_| |_| |_|_|_| |_(_)_| \_/\____/  \_/  ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"

gitee: https://gitee.com/zuohuaijun/Admin.NET");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"期待您的PR，让.net更好！

");
        }

        public void UseOther()
        {
            System.Console.WriteLine(@"另一个BStyle");
        }
    }
}
