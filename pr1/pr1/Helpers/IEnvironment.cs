using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace pr1.Helpers
{
    public interface IEnvironment
    {
        void SetStatusBarColor(Color color, bool darkStatusBarTint);
    }
}