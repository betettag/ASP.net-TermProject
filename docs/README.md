# ASP.net-TermProject
internet against humanity. voting and adding funny replies to a phrase

updated to core 3.1.2
used identity. 
    **(click the profile picture TOP RIGHT for a dropdown LOGOUT & OTHERS)**
    
validation when registering and adding a picture
admin has special dropdowns on profile picture (roles, users)

animation used for highscores
uploading a picture when registering is needed.

published with azure 
https://internetagainsthumanity.azurewebsites.net/

zap mitigations (out of context) added like:

```c#
 context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
 context.Response.Headers.Remove("Server");
 context.Response.Headers.Remove("X-Powered-By");
 context.Response.Headers.Remove("X-SourceFiles");
 context.Response.Headers.Add("Feature-Policy", "geolocation 'self'");
 context.Response.Headers.Add("X-Content-Type-Options", "nosniff");

    services.AddAntiforgery(options =>
    {
        options.SuppressXFrameOptionsHeader = true;
        // new API
        options.Cookie.Name = "AntiforgeryCookie";
        //options.Cookie.Domain = "https://internetagainsthumanity.azurewebsites.net/";
        options.Cookie.Path = "/";
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.SuppressXFrameOptionsHeader = true;
    });
    services.AddSession(options =>
    {
        // new API
        options.Cookie.Name = "SessionCookie";
        //options.Cookie.Domain = "https://internetagainsthumanity.azurewebsites.net/";
        options.Cookie.Path = "/";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

    services.Configure<CookiePolicyOptions>(options =>
    {
        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
        options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
    });

```
