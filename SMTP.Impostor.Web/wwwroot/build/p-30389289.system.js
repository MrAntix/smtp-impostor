var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
var __spreadArrays = (this && this.__spreadArrays) || function () {
    for (var s = 0, i = 0, il = arguments.length; i < il; i++) s += arguments[i].length;
    for (var r = Array(s), k = 0, i = 0; i < il; i++)
        for (var a = arguments[i], j = 0, jl = a.length; j < jl; j++, k++)
            r[k] = a[j];
    return r;
};
System.register(['./p-9fb714ae.system.js', './p-aa2932a4.system.js'], function (exports, module) {
    'use strict';
    var CSS, plt, win, promiseResolve, doc, NAMESPACE, Context, LogLevel, consoleLogMethodProvider;
    return {
        setters: [function (module) {
                CSS = module.C;
                plt = module.p;
                win = module.w;
                promiseResolve = module.a;
                doc = module.d;
                NAMESPACE = module.N;
                Context = module.c;
            }, function (module) {
                LogLevel = module.L;
                consoleLogMethodProvider = module.c;
            }],
        execute: function () {
            var _this = this;
            /*
             Stencil Client Patch v1.16.0 | MIT Licensed | https://stenciljs.com
             */
            var noop = function () {
                /* noop*/
            };
            var IS_DENO_ENV = typeof Deno !== 'undefined';
            var IS_NODE_ENV = !IS_DENO_ENV &&
                typeof global !== 'undefined' &&
                typeof require === 'function' &&
                !!global.process &&
                typeof __filename === 'string' &&
                (!global.origin || typeof global.origin !== 'string');
            var IS_DENO_WINDOWS_ENV = IS_DENO_ENV && Deno.build.os === 'windows';
            var getCurrentDirectory = IS_NODE_ENV ? process.cwd : IS_DENO_ENV ? Deno.cwd : function () { return '/'; };
            var exit = IS_NODE_ENV ? process.exit : IS_DENO_ENV ? Deno.exit : noop;
            var getDynamicImportFunction = function (namespace) { return "__sc_import_" + namespace.replace(/\s|-/g, '_'); };
            var patchEsm = exports('a', function () {
                // NOTE!! This fn cannot use async/await!
                // @ts-ignore
                if (!(CSS && CSS.supports && CSS.supports('color', 'var(--c)'))) {
                    // @ts-ignore
                    return module.import(/* webpackChunkName: "polyfills-css-shim" */ './p-dd650c87.system.js').then(function () {
                        if ((plt.$cssShim$ = win.__cssshim)) {
                            return plt.$cssShim$.i();
                        }
                        else {
                            // for better minification
                            return 0;
                        }
                    });
                }
                return promiseResolve();
            });
            var patchBrowser = exports('p', function () {
                {
                    // shim css vars
                    plt.$cssShim$ = win.__cssshim;
                }
                // @ts-ignore
                var scriptElm = Array.from(doc.querySelectorAll('script')).find(function (s) { return new RegExp("/" + NAMESPACE + "(\\.esm)?\\.js($|\\?|#)").test(s.src) || s.getAttribute('data-stencil-namespace') === NAMESPACE; });
                var opts = scriptElm['data-opts'] || {};
                if ('onbeforeload' in scriptElm && !history.scrollRestoration /* IS_ESM_BUILD */) {
                    // Safari < v11 support: This IF is true if it's Safari below v11.
                    // This fn cannot use async/await since Safari didn't support it until v11,
                    // however, Safari 10 did support modules. Safari 10 also didn't support "nomodule",
                    // so both the ESM file and nomodule file would get downloaded. Only Safari
                    // has 'onbeforeload' in the script, and "history.scrollRestoration" was added
                    // to Safari in v11. Return a noop then() so the async/await ESM code doesn't continue.
                    // IS_ESM_BUILD is replaced at build time so this check doesn't happen in systemjs builds.
                    return {
                        then: function () {
                            /* promise noop */
                        },
                    };
                }
                {
                    opts.resourcesUrl = new URL('.', new URL(scriptElm.getAttribute('data-resources-url') || scriptElm.src, win.location.href)).href;
                    {
                        patchDynamicImport(opts.resourcesUrl, scriptElm);
                    }
                    if (!win.customElements) {
                        // module support, but no custom elements support (Old Edge)
                        // @ts-ignore
                        return module.import(/* webpackChunkName: "polyfills-dom" */ './p-03f91178.system.js').then(function () { return opts; });
                    }
                }
                return promiseResolve(opts);
            });
            var patchDynamicImport = function (base, orgScriptElm) {
                var importFunctionName = getDynamicImportFunction(NAMESPACE);
                try {
                    // test if this browser supports dynamic imports
                    // There is a caching issue in V8, that breaks using import() in Function
                    // By generating a random string, we can workaround it
                    // Check https://bugs.chromium.org/p/chromium/issues/detail?id=990810 for more info
                    win[importFunctionName] = new Function('w', "return import(w);//" + Math.random());
                }
                catch (e) {
                    // this shim is specifically for browsers that do support "esm" imports
                    // however, they do NOT support "dynamic" imports
                    // basically this code is for old Edge, v18 and below
                    var moduleMap_1 = new Map();
                    win[importFunctionName] = function (src) {
                        var url = new URL(src, base).href;
                        var mod = moduleMap_1.get(url);
                        if (!mod) {
                            var script_1 = doc.createElement('script');
                            script_1.type = 'module';
                            script_1.crossOrigin = orgScriptElm.crossOrigin;
                            script_1.src = URL.createObjectURL(new Blob(["import * as m from '" + url + "'; window." + importFunctionName + ".m = m;"], { type: 'application/javascript' }));
                            mod = new Promise(function (resolve) {
                                script_1.onload = function () {
                                    resolve(win[importFunctionName].m);
                                    script_1.remove();
                                };
                            });
                            moduleMap_1.set(url, mod);
                            doc.head.appendChild(script_1);
                        }
                        return mod;
                    };
                }
            };
            Context.store = (function () {
                var _store;
                var setStore = function (store) {
                    _store = store;
                };
                var getState = function () {
                    return _store && _store.getState();
                };
                var getStore = function () {
                    return _store;
                };
                var mapDispatchToProps = function (component, props) {
                    Object.keys(props).forEach(function (actionName) {
                        var action = props[actionName];
                        Object.defineProperty(component, actionName, {
                            get: function () { return function () {
                                var args = [];
                                for (var _i = 0; _i < arguments.length; _i++) {
                                    args[_i] = arguments[_i];
                                }
                                return _store.dispatch(action.apply(void 0, args));
                            }; },
                            configurable: true,
                            enumerable: true,
                        });
                    });
                };
                var mapStateToProps = function (component, mapState) {
                    var _mapStateToProps = function (_component, _mapState) {
                        var mergeProps = mapState(_store.getState());
                        Object.keys(mergeProps).forEach(function (newPropName) {
                            var newPropValue = mergeProps[newPropName];
                            component[newPropName] = newPropValue;
                        });
                    };
                    var unsubscribe = _store.subscribe(function () { return _mapStateToProps(); });
                    _mapStateToProps();
                    return unsubscribe;
                };
                return {
                    getStore: getStore,
                    setStore: setStore,
                    getState: getState,
                    mapDispatchToProps: mapDispatchToProps,
                    mapStateToProps: mapStateToProps,
                };
            })();
            var appGlobalScript = function () { return __awaiter(_this, void 0, void 0, function () {
                return __generator(this, function (_a) {
                    globalThis.logScope = { default: LogLevel.debug };
                    globalThis.loggerMethodProvider = consoleLogMethodProvider;
                    globalThis.getLogger = function (scope) {
                        var getMethod = function (methodLevel) {
                            var method = globalThis.loggerMethodProvider(methodLevel);
                            return function () {
                                var args = [];
                                for (var _i = 0; _i < arguments.length; _i++) {
                                    args[_i] = arguments[_i];
                                }
                                var scopeLevel = Reflect.has(globalThis.logScope, scope)
                                    ? globalThis.logScope[scope]
                                    : globalThis.logScope.default;
                                if (methodLevel >= scopeLevel)
                                    method(__spreadArrays(["[" + LogLevel[methodLevel] + "] " + scope], args));
                            };
                        };
                        return {
                            debug: getMethod(LogLevel.debug),
                            info: getMethod(LogLevel.info),
                            warn: getMethod(LogLevel.warn),
                            error: getMethod(LogLevel.error)
                        };
                    };
                    return [2 /*return*/];
                });
            }); };
            var globalScripts = exports('g', function () {
                appGlobalScript();
            });
        }
    };
});
