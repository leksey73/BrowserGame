using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BG_BLL
{

    public static class FileLoggerFactoryExtensions
    {
            public static ILoggerFactory AddFile(this ILoggerFactory factory,
                                            string filePath)
            {
                factory.AddProvider(new FileLoggerProvider(filePath));
                return factory;
            }
        }
    } 