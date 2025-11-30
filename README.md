# Mastodon BookmaRSS
A lightweight, stateless ASP.NET minimal API service that exposes your Mastodon bookmarks as an RSS 2.0 feed.
Designed for use with RSS readers such as FreshRSS, Miniflux, NetNewsWire, or any other reader that accepts a standard RSS URL.

## Features
- Exposes Mastodon bookmarks as an RSS 2.0 feed.
- Stateless design: no database or persistent storage required.
- Simple configuration via environment variables or url query parameters.

## Configuration
The service can be configured using the following environment variables:
- `MASTODON_INSTANCE_URL`: The base URL of your Mastodon instance (e.g., `https://mastodon.social`).
- `MASTODON_ACCESS_TOKEN`: Your Mastodon access token with the necessary permissions to read bookmarks.
- `BOOKMARKS_LIMIT`: (Optional) The maximum number of bookmarks to include in the RSS feed. Default is 20.

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

