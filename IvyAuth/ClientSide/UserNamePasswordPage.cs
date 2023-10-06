using Microsoft.AspNetCore.Mvc;

namespace IvyAuth
{
    public static class UserNamePasswordPage
    {
        public static ContentResult GetContent(string client_id, string state, string code_challenge, string scopes, string redirect_uri)
        {
            var content =
$@"<!DOCTYPE html> 
<html>
    <head>
    IvyTech Login
    </head>
    <body>
      Username: <input autocomplete='username' onClick='onChangeUsername(this.value)'/>
      Password: <input autocomplete='current-password' type='password' onClick='onChangePassword(this.value)'/>
      <button onclick='onLogin()'>Login</button>
    </body>
    <script>
      var username = """";
      var password = """";

      function onChangeUsername(x) {{
        username = x;
      }}
      function onChangePassword(x) {{
        password = x;
      }}

      async function onLogin() {{

        console.log('onLogin');
        const res = await fetch('/Code', {{
          method: 'POST',
          headers: {{
            'Content-Type': 'application/x-www-form-urlencoded'
          }},
          body: new URLSearchParams({{
            'client_id': '{client_id}',
            'scopes': '{scopes}',
            'username': username,
            'password': password,
            'code_challenge': '{code_challenge}'
        }})}});

        if (res.ok) {{
          const code = await res.text();
          window.location = `{redirect_uri}?code=${{code}}&state={state}`;
        }} else {{
          //todo show error
        }}
      }}
    </script>
</html>";

            return new ContentResult()
            {
                Content = content,
                StatusCode = 200,
                ContentType = "text/html"
            };
        }
    }
}
