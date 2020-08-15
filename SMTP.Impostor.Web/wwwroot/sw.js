/**
 * Welcome to your Workbox-powered service worker!
 *
 * You'll need to register this file in your web app and you should
 * disable HTTP caching for this file too.
 * See https://goo.gl/nhQhGp
 *
 * The rest of the code is auto-generated. Please don't update this file
 * directly; instead, make changes to your Workbox build configuration
 * and re-run your build process.
 * See https://goo.gl/2aRDsh
 */

importScripts("https://storage.googleapis.com/workbox-cdn/releases/4.3.1/workbox-sw.js");

self.addEventListener('message', (event) => {
  if (event.data && event.data.type === 'SKIP_WAITING') {
    self.skipWaiting();
  }
});

/**
 * The workboxSW.precacheAndRoute() method efficiently caches and responds to
 * requests for URLs in the manifest.
 * See https://goo.gl/S9QRab
 */
self.__precacheManifest = [
  {
    "url": "index.html",
    "revision": "4c5ece985840f4e14aebf39a208a6e2e"
  },
  {
    "url": "assets/styles/host.css",
    "revision": "fedf24f639cd157bdab855bfd046d727"
  },
  {
    "url": "assets/styles/init.css",
    "revision": "02f11575295eb739916e3c08e7615d40"
  },
  {
    "url": "build/index.esm.js",
    "revision": "0ce3e028d8cdda59ba2dbac123aec6e1"
  },
  {
    "url": "build/p-10a20875.css"
  },
  {
    "url": "build/p-2463c9e4.js"
  },
  {
    "url": "build/p-514b3755.js"
  },
  {
    "url": "build/p-ae551da3.js"
  },
  {
    "url": "build/p-b55f584c.entry.js"
  },
  {
    "url": "build/p-bbb3d3bd.js"
  },
  {
    "url": "build/p-c95129be.js"
  },
  {
    "url": "build/p-d99e8c9b.js"
  },
  {
    "url": "build/p-df055c62.js"
  },
  {
    "url": "manifest.json",
    "revision": "8d9b6350283f4bd949c3003e31195d7c"
  }
].concat(self.__precacheManifest || []);
workbox.precaching.precacheAndRoute(self.__precacheManifest, {});
