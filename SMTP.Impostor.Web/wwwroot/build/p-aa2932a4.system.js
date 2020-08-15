System.register([], function (exports) {
    'use strict';
    return {
        execute: function () {
            exports({
                L: void 0,
                c: consoleLogMethodProvider,
                n: newId
            });
            var LogLevel;
            (function (LogLevel) {
                LogLevel[LogLevel["debug"] = 0] = "debug";
                LogLevel[LogLevel["info"] = 1] = "info";
                LogLevel[LogLevel["warn"] = 2] = "warn";
                LogLevel[LogLevel["error"] = 3] = "error";
            })(LogLevel || (LogLevel = exports('L', {})));
            function consoleLogMethodProvider(level) {
                return function (args) { return console[LogLevel[level]].apply(console, args); };
            }
            function newId() {
                return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                    var r = (Math.random() * 16) | 0;
                    var v = c === 'x' ? r : (r & 0x3) | 0x8;
                    return v.toString(16);
                });
            }
        }
    };
});
