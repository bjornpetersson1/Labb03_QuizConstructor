using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb03_GUI.API
{
    public static class TokenRespons
    {
        public static readonly Dictionary<int, string> TokenMessage = new()
        {
            { 0, "Success Returned results successfully."},
            { 1, "No Results Could not return results. The API doesn't have enough questions for your query. (Ex. Asking for 50 Questions in a Category that only has 20.)"},
            { 2, "Invalid Parameter Contains an invalid parameter. Arguements passed in aren't valid. (Ex. Amount = Five)"},
            { 3, "Token Not Found Session Token does not exist."},
            { 4, "Token Empty Session Token has returned all possible questions for the specified query. Resetting the Token is necessary."},
            { 5, "Rate Limit Too many requests have occurred. Each IP can only access the API once every 5 seconds." }
        };
    }
}
