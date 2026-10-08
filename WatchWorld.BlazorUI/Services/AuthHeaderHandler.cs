using System.Net;
using System.Net.Http.Headers;

namespace WatchWorld.BlazorUI.Services
{
    // Runs for EVERY request WatchWorldApiClient sends.
    // Attaches the key card (token) if the user is logged in.
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly CurrentUserState _currentUser;

        public AuthHeaderHandler(CurrentUserState currentUser)
        {
            _currentUser = currentUser;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _currentUser.Token;

            // 1. On the way OUT: attach the token, if we have one
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // 2. Pass the request on and wait for the API's answer
            var response = await base.SendAsync(request, cancellationToken);

            // 3. On the way BACK: a 401 even though we sent a token means the token
            //    is no longer valid (most likely expired), so log the user out
            if (response.StatusCode == HttpStatusCode.Unauthorized && !string.IsNullOrEmpty(token))
            {
                _currentUser.Clear();
            }

            return response;
        }
    }
}