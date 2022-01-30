using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sockman.SMLang
{
    public enum SMLToken
    {
        TextBlockBegin,
        TextBlock,
        TextBlockEnd,

        HexBlockBegin,
        HexBlock,
        HexBlockEnd,

        IDENTIFIER,
    }
}
