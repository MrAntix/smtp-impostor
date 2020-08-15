System.register(['./p-aa2932a4.system.js', './p-a6565e95.system.js'], function (exports) {
    'use strict';
    return {
        setters: [function (module) {
                var _setter = {};
                _setter.LogLevel = module.L;
                _setter.consoleLogMethodProvider = module.c;
                _setter.newId = module.n;
                exports(_setter);
            }, function (module) {
                var _setter = {};
                _setter.DEFAULT_SEARCH_HOST_MESSAGES_CRITERIA = module.D;
                _setter.Frag = module.F;
                _setter.HostStatus = module.p;
                _setter.HubStatus = module.H;
                _setter.Types = module.T;
                _setter.addHost = module.b;
                _setter.configureStore = module.c;
                _setter.deleteHostMessage = module.j;
                _setter.dispatch = module.d;
                _setter.getInitialState = module.v;
                _setter.hostIsRunning = module.q;
                _setter.hubSocketProvider = module.h;
                _setter.hubStatusDisplay = module.a;
                _setter.initWorkerState = module.i;
                _setter.loadHostMessage = module.k;
                _setter.loadWorkerState = module.l;
                _setter.openHost = module.o;
                _setter.removeHost = module.e;
                _setter.rootReducer = module.r;
                _setter.searchHostMessages = module.g;
                _setter.shutdownWorker = module.n;
                _setter.startHost = module.s;
                _setter.startupWorker = module.m;
                _setter.stopHost = module.f;
                _setter.toggleHostConfiguration = module.t;
                _setter.updateHost = module.u;
                exports(_setter);
            }],
        execute: function () {
            exports({
                deserializeDate: deserializeDate,
                serializeDate: serializeDate
            });
            function serializeDate(value) {
                return value.toISOString();
            }
            function deserializeDate(value) {
                return new Date(Date.parse(value));
            }
            var LocalStorageStore = /** @class */ (function () {
                function LocalStorageStore() {
                }
                LocalStorageStore.prototype.getAsync = function (key, deserialize) {
                    if (deserialize === void 0) { deserialize = JSON.parse; }
                    var value = window.localStorage.getItem(key);
                    return Promise.resolve(deserialize(value));
                };
                LocalStorageStore.prototype.putAsync = function (key, data, serialize) {
                    if (serialize === void 0) { serialize = JSON.stringify; }
                    window.localStorage.setItem(key, serialize(data));
                    return Promise.resolve();
                };
                LocalStorageStore.prototype.deleteAsync = function (key) {
                    window.localStorage.removeItem(key);
                    return Promise.resolve();
                };
                return LocalStorageStore;
            }());
            exports('LocalStorageStore', LocalStorageStore);
        }
    };
});
