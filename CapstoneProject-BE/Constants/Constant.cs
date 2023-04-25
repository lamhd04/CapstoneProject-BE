using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;

namespace CapstoneProject_BE.Constants
{
    public static class Constant
    {
        public static readonly string ClientUrl= "https://imsd-capstoneproject-fe.vercel.app";
        public static readonly Regex validateGuidRegex = new Regex("^(?=.*?[A-Z])(?=.*?[0-9]).{8,32}$");
    }
}
