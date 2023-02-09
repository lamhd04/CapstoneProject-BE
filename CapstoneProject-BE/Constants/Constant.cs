using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;

namespace CapstoneProject_BE.Constants
{
    public static class Constant
    {
        public static readonly string ClientUrl= "http://localhost:3000";
        public static readonly Regex validateGuidRegex = new Regex("^(?=.*?[A-Z])(?=.*?[0-9]).{6,32}$");
    }
}
