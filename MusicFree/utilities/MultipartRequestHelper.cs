using System;
using System.IO;
using Microsoft.Net.Http.Headers;
namespace MusicFree.utilities
{
    public class MultipartRequestHelper
    {
       
            // Content-Type: multipart/form-data; boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq"
            // The spec at https://tools.ietf.org/html/rfc2046#section-5.1 states that 70 characters is a reasonable limit.
            public static string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit)
            {
                var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).Value;

                if (string.IsNullOrWhiteSpace(boundary))
                {
                    throw new InvalidDataException("Missing content-type boundary.");
                }

                if (boundary.Length > lengthLimit)
                {
                    throw new InvalidDataException(
                        $"Multipart boundary length limit {lengthLimit} exceeded.");
                }

                return boundary;
            }
        public static string GetName(string input)
        {
            //17

            var string_with = input.Split('=')[1].Split(';')[0];

            return string_with.Substring(1, string_with.Length-2) ;
        }
        public static string[] AuthorStringSplit(string input)
        {
            var string_1 = input.Split('[')[1].Split(']')[0];
            var strings = string_1.Split(',');
            return strings;
        }
        }
}
