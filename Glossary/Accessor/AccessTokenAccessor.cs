using System;
using System.Threading;

namespace Accessor
{
    public class AccessTokenAccessor : IAccessTokenAccessor
    {
        private readonly AsyncLocal<string> _localAccessToken = new AsyncLocal<string>();

        public AccessTokenAccessor()
        {
            // resolve stuff to help determining 
        }

        public string AccessToken
        {
            get
            {
                if (_localAccessToken.Value == null)
                {
                    // do some logic to get and store the result of it
                    _localAccessToken.Value = Guid.NewGuid().ToString();
                }

                return _localAccessToken.Value;
            }
        }
    }
}
