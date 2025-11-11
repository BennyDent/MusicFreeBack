using Microsoft.AspNetCore.DataProtection;

namespace MusicFree
{
    public class DataProtectorProvider : IDataProtectionProvider
    {
        private IDataProtectionProvider _root;                                                       
                                                   
       public DataProtectorProvider(IDataProtectionProvider root)
      {                                                                 
        _root = root;                                                   
      }                                                                                 
                                                                                        
      public IDataProtector CreateProtector(string purpose)
      {                                                                
        return _root.CreateProtector("oidc");
      }                    
    }
}
