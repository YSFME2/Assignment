using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface ICacheService
    {
        T Get<T>(string key);
        bool Set<T>(string key, T value,DateTimeOffset expiration);
        void Delete<T>(string key);

    }
}
