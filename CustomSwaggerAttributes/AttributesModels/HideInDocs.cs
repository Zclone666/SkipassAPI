using System;
using System.Collections.Generic;
using System.Text;

namespace CustomSwaggerAttributes.AttributesModels
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class HideInDocsAttribute : Attribute
    {
    }
}
