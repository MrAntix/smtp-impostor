System.register(['./p-9fb714ae.system.js', './p-30389289.system.js', './p-aa2932a4.system.js'], function () {
    'use strict';
    var bootstrapLazy, patchBrowser, globalScripts;
    return {
        setters: [function (module) {
                bootstrapLazy = module.b;
            }, function (module) {
                patchBrowser = module.p;
                globalScripts = module.g;
            }, function () { }],
        execute: function () {
            patchBrowser().then(function (options) {
                globalScripts();
                return bootstrapLazy([["p-a6d85f08.system", [[1, "app-root", { "state": [32] }], [1, "smtp-host", { "value": [16], "showMessages": [516, "show-messages"], "showConfiguration": [516, "show-configuration"], "toggleState": [64], "searchMessages": [64] }], [1, "impostor-hub", { "socketProvider": [16], "url": [1], "status": [2], "connectAsync": [64], "sendAsync": [64], "disconnectAsync": [64] }], [1, "smtp-host-configuration", { "value": [16] }], [1, "app-input", { "placeholder": [513], "clearButton": [516, "clear-button"], "iconType": [513, "icon-type"], "value": [1] }], [1, "app-popup", { "isOpen": [1540, "is-open"], "modal": [4], "position": [1537], "shift": [1537], "toggle": [64] }], [1, "app-icon", { "type": [1], "rotate": [514], "flipHorizontal": [516, "flip-horizontal"], "flipVertical": [516, "flip-vertical"], "scale": [514] }]]]], options);
            });
        }
    };
});
