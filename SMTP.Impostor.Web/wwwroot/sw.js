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
    "revision": "1c9d69e0b6428d8d3bdf65db33d85837"
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
    "revision": "f06a6d10afcd9bb896338e053337ca3c"
  },
  {
    "url": "build/p-10a20875.css"
  },
  {
    "url": "build/p-24fc4ba9.js"
  },
  {
    "url": "build/p-3b0b096b.js"
  },
  {
    "url": "build/p-46109683.js"
  },
  {
    "url": "build/p-4cbdf50e.js"
  },
  {
    "url": "build/p-823f2846.entry.js"
  },
  {
    "url": "build/p-dd3d73d6.js"
  },
  {
    "url": "build/p-df055c62.js"
  },
  {
    "url": "build/p-ebb4d602.js"
  },
  {
    "url": "manifest.json",
    "revision": "8d9b6350283f4bd949c3003e31195d7c"
  }
].concat(self.__precacheManifest || []);
workbox.precaching.precacheAndRoute(self.__precacheManifest, {});
