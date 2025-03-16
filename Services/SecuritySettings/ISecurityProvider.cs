using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.SecuritySettings
{
    public interface ISecurityProvider
    {
        public bool GetRule(string rule);
        public bool UpdateRule(string ruleName, bool state);
    }
}
