using Microsoft.AspNetCore.DataProtection;

namespace MusicFree
{
    public class CustomDataProtector : IDataProtector
    {
        public IDataProtector CreateProtector(string purpose)
        {
            return new CustomDataProtector();
        }

        public byte[] Protect(byte[] plaintext)
        {
            return plaintext;
        }

        public byte[] Unprotect(byte[] protectedData)
        {
            return protectedData;
        }
    }
}
