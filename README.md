# Mastodon BookmaRSS
A lightweight, stateless ASP.NET minimal API service that exposes Mastodon post bookmarks as an RSS 2.0 feed.

## Example Usecase
The tool is used to facilitate reading Mastodon bookmarks in a RSS reader (e.g. FreshRSS) in an Environment were the RSS-Reader acts as a newsroom-tool. 
Bookmarked posts can easily be redistributed to other media channels, queued for a later reposting (intead of direct reposting), archived for reference, ...

## Features
- Exposes Mastodon bookmarks as a RSS 2.0 feed.
- Stateless design: no database or persistent storage required.
- Simple configuration via environment variables or url query parameters.

## Configuration
The service can be configured using the following environment variables (bear security warnings below in: mind):
- `MASTODON_INSTANCE_URL`: (Optional) The base URL of your Mastodon instance (defaults to, `mastodon.social`).
- `MASTODON_ACCESS_TOKEN`: (Optional, use url query parameter if missing) Your Mastodon access token with the necessary permissions to read bookmarks.
- `BOOKMARKS_LIMIT`: (Optional) The maximum number of bookmarks to include in the RSS feed. Default is 20.
- `REDIRECT_URL`: (Optional) The URL to redirect to when accessing the root path (`/`). Defaults to `/conf`'
- ~~`HTTP_USER`': (Optional) Username for HTTP Basic Authentication to secure access to `/feed`~~.
- ~~`HTTP_PASSWORD`: (Optional) Password for HTTP Basic Authentication to secure access to `/feed`.~~

- Alternatively, you can provide these parameters via URL query parameters (overriding the environment variables):
- `instance`: The base URL of your Mastodon instance.
- `token`: Your Mastodon access token.
- `limit`: The maximum number of bookmarks to include in the RSS feed.

## Usage
- Deploy the Mastodon BookmaRSS service to your preferred hosting environment (simple Dockerfile included).
- Generate a Mastodon access token (`READ:bookmarks` scope) from your Mastodon account settings.
- Access the service via a URL structured as follows
  ```
  https://your-service-url.com/feed?instance=https://mastodon.social&token=YOUR_ACCESS_TOKEN&limit=20
  ```
- Or set the required environment variables and access the service via:
   ```
    https://your-service-url.com/feed
    ```
3. Add the generated RSS feed URL to your RSS reader of choice.
4. Enjoy reading your Mastodon bookmarks in your RSS reader!

*) Use the form at `https://your-service-url.com/conf` to generate the feed URL to paste in your RSS-Client.

**Important:** Ensure that your Mastodon access token has the necessary permissions to read bookmarks.

**__Security Warning:__** Be cautious when sharing the generated feed URL, as it contains your access token. I recommend using a unique token for this purpose over a permissive token..

**__Security Warning:__** Avoid using this service on public or shared hosting environments without proper security 
(at least Http-Auth) measures. Otherwise, when setting `MASTODON_ACCESS_TOKEN` this exposes all Bookmarks publicly.

## Roadmap
Additional features and improvements planned for future releases:
- Implement HTTP Basic Authentication to secure access to the `/feed` endpoint.
- Add support for custom RSS feed titles and descriptions.
- Enhance error handling and logging for better diagnostics.
- Provide Docker Compose configuration for easier deployment.

## License and Acknowledgements

The project logic is very inspired by the original mastodon-bookmark-rss service created by untitaker at 
https://github.com/untitaker/mastodon-bookmark-rss
The upstream project is licensed under the MIT License, which permits reuse, modification, and redistribution.
No source code from the original project has been copied verbatim; this is an independent reimplementation with 
enhancements in ASP.NET.

The Copyright notice for the upstream project goes as follows:
`Copyright (c) 2023 untitaker`

This rewrite is also licensed under the MIT License, as detailed below.

```
Copyright (c) 2025 Hinnerk Weiler – https://aufmboot.com

MIT License
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the “Software”), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
```

